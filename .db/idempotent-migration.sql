CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240630172212_InitialCreate') THEN
    CREATE TABLE "Products" (
        "ProductId" uuid NOT NULL,
        "Name" text NOT NULL,
        "Status" integer NOT NULL,
        "Stock" integer NOT NULL,
        "Description" text NOT NULL,
        "Price" integer NOT NULL,
        "CreatedBy" text NOT NULL,
        "LastUpdatedBy" text NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        "LastUpdatedAt" timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        CONSTRAINT "PK_Products" PRIMARY KEY ("ProductId")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240630172212_InitialCreate') THEN
    CREATE INDEX "IX_Products_CreatedAt" ON "Products" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240630172212_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240630172212_InitialCreate', '8.0.6');
    END IF;
END $EF$;
COMMIT;

