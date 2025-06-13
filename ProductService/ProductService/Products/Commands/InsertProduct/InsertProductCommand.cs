using MediatR;

namespace Application.Products.Commands.InsertProduct
{
    public sealed record InsertProductCommand(string Name, string Colour) : IRequest<string>;
}
