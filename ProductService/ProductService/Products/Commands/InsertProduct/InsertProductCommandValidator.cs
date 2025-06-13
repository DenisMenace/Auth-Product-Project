using FluentValidation;

namespace Application.Products.Commands.InsertProduct
{
    public sealed class InsertProductCommandValidator : AbstractValidator<InsertProductCommand>
    {
        public InsertProductCommandValidator() 
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Colour).NotEmpty();
        }
    }
}
