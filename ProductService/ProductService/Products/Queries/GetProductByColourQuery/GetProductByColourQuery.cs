using Domain.Entities;
using MediatR;

namespace Application.Products.Queries.GetProductByColourQuery
{
    public sealed record GetProductByColourQuery(string Colour) : IRequest<List<Product>>;
}
