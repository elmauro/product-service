
#------------------------------------------------------------------------------------------------------#
# communication options for Makefile
# https://www.gnu.org/software/make/manual/html_node/Options_002fRecursion.html
# MacOS interprets this flag as part of the executing command instead of an option to the Makefile itself
# MAKEFLAGS lets us load flags to (this) Makefile. Avoids breaking MacOS

MAKEFLAGS += --no-print-directory
#------------------------------------------------------------------------------------------------------#

os_name := $(shell uname -s)
user_secrets_id := CB317898-F186-43D4-9FB3-5D266822C543
db_container_name := my-account-postgres

# If nonce network is missing, create it. We use variable assignment here for circle ci
nonce_network := $(shell docker network ls | grep nonce-services)
ifeq ("$(nonce_network)", "")
throwaway := $(shell docker network create nonce-services)
endif

# If you have your PAT as an en_var, (such as with your /etc/bash.bashrc or equivilant) 
# it can be exported. This removes the need to create a .env file, but you can use one
# still if you prefer, or have trouble with this env_var
export $(PAT)
export user_secrets_id
export db_container_name

#------------------------------------------------------------------------------------------------------#

help:                            ## Shows this help.
	@fgrep -h "##" $(MAKEFILE_LIST) | fgrep -v fgrep | sed -e 's/\\$$//' | sed -e 's/##//'

setup:                           ## Put Idempotent developer workspace setup here!
	@make env  
	@make tools 
	@make db.up

env:                             ## Creates the project .env with a line for your PAT
	@touch .env 
	$(shell echo PAT=$(PAT) > .env)

user.secrets:               ## Creates an empty user secrets file in the proper location. Don't run this more than once! It will override what you have.
	@mkdir -p ~/.microsoft/usersecrets/$(user_secrets_id)/ && touch ~/.microsoft/usersecrets/$(user_secrets_id)/secrets.json
	$(shell echo "{}" > ~/.microsoft/usersecrets/$(user_secrets_id)/secrets.json)

#------------------------------------------------------------------------------------------------------#

local.start:                     ## Builds and starts the service using localhost.
	dotnet run --project "./src/MyAccount.Web/MyAccount.Web.csproj"

local.test:                      ## Runs tests on the host machine
	dotnet test

docker.start:                    ## Builds and starts the MyAccount service in detatched mode
	docker-compose -f compose.yml up --build -d product

docker.run:                    ## Builds and starts the MyAccount service in attached mode
	docker-compose -f compose.yml up --build product

docker.stop:                     ## Stops the MyAccount service
	docker-compose -f compose.yml stop my-account

docker.down:                     ## Removes only containers and networks defined in this compose file
	docker-compose -f compose.yml down --rmi all

docker.exec:                     ## Opens a shell into the running MyAccount container 
	docker-compose -f compose.yml exec my-account sh

docker.test:                     ## Builds and runs tests from within docker
	docker-compose -f compose.yml up --build --abort-on-container-exit my-account-testrunner

docker.prune:                    ## Prunes docker system and volume
	docker system prune -a -f

docker.nuke:                     ## Removes the service, all dependencies, volumes, and orphans. 
	@make docker.prune
	docker-compose -f compose.yml down --rmi all --remove-orphans -v
	docker volume prune -f

#------------------------------------------------------------------------------------------------------#	

package.patch:
	git checkout develop && git fetch --prune && git pull
	./versioning.sh -v "patch"

package.minor:
	git checkout develop && git fetch --prune && git pull
	./versioning.sh -v "minor"

package.major:
	git checkout develop && git fetch --prune && git pull
	./versioning.sh -v "major"

git.init:
	git init
	git add .
	git commit -m "first commit"
	git branch -M develop
	#git remote add origin git@github.com:bitstopco/my-account.git
	#git push -u origin main

#-----------------------------------------------------------------------------------------------------#

ngrok.run:                    ## Port forwarding utility that allows webhooks to be picked up from a developer workspace. Remove if you do not need webhooks.
	ngrok http -hostname='bitstop.ngrok.io' https://localhost:httpsPort

tools:        ## Installs dotnet tools based on .config/dotnet-tools.json in this repo.
	@dotnet tool restore  

#------------------------------------------------------------------------------------------------------#	

db.up:                           ## Ups the db, applies migrations, and seeds required data. Requires no dependencies besides docker.
	@docker-compose -f compose.yml up --build -d $(db_container_name)
	@if [ $(os_name) = "Linux" ]; then \
		timeout 90s bash -c "until docker exec ${db_container_name} pg_isready ; do sleep 5 ; done"; \
	fi
	@if [ $(os_name) != "Linux" ]; then \
		gtimeout 90s bash -c "until docker exec ${db_container_name} pg_isready ; do sleep 5 ; done"; \
	fi

	docker exec -it $(db_container_name) psql -U my-account -d my-account -f /scripts/idempotent-migration.sql

db.migration:             ## Create a new migration
	@echo "Enter a name for this migration"; \
	read MIGRATION_NAME; \
	echo "Creating" "$$MIGRATION_NAME" . . .; \
	dotnet tool restore ; \
	dotnet dotnet-ef migrations add "$$MIGRATION_NAME" --project src/MyAccount.Web --output-dir Database/Migrations
	@make db.script 

db.update:                ## Update DB to latest migration using dotnet cli. This requires locally installed tools. Intended for app devs. Everyone else should use db.up
	@dotnet tool restore
	@dotnet dotnet-ef database update --project src/MyAccount.Web

db.remove-migration:      ## Unapply and remove the last migration, useful for tweaking migrations in progress
	@dotnet tool restore
	@dotnet dotnet-ef migrations remove --force --project src/MyAccount.Web

db.script:            ## Generates an idempotent sql migration script
	@dotnet tool restore
	dotnet ef migrations script --idempotent --project src/MyAccount.Web --output .db/idempotent-migration.sql

db.script.rollback:      ## Create a sql script to rollback in the event there is an issue with a deployment.
	@make db.migration.list
	@echo "Enter name of migration for rollback starting point"; \
	read ROLLBACK_FROM; \
	echo "Enter name of migration for rollback ending point"; \
	read ROLLBACK_TO; \
	echo "Creating rollback from" "$$ROLLBACK_FROM" to "$$ROLLBACK_TO" . . .; \
	dotnet dotnet-ef migrations script "$$ROLLBACK_FROM" "$$ROLLBACK_TO" --project src/MyAccount.Web --output ".db/rollbacks/rollback_""$$ROLLBACK_FROM""_to_""$$ROLLBACK_TO"".sql"

db.migration.list:       ## List Db Migrations
	@dotnet dotnet-ef migrations list --project src/MyAccount.Web --no-build --json