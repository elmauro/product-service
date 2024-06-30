using FluentValidation;
using MC.ProductService.API.ClientModels;

namespace MC.ProductService.API.Validators
{
    public class ProductValidator : AbstractValidator<ProductRequest>
    {
        public const string ProductNameValidator = "Invalid product name provided, please request a product name again.";
        public const string ProductDescriptionValidator = "Invalid product id provided, please request a product id again.";

        public ProductValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty().WithMessage(ProductNameValidator);

            RuleFor(product => product.Description)
                .NotEmpty().WithMessage(ProductDescriptionValidator);
        }
    }
}
