using FluentValidation;

namespace Application.Products.Queries.GetProductByColourQuery
{
    public sealed class GetProductByColourValidator : AbstractValidator<GetProductByColourQuery>
    {
        public GetProductByColourValidator()
        {
            RuleFor(x => x.Colour).NotEmpty();
        }
    }
}
