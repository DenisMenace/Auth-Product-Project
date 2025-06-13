using Domain.Entities;
using MediatR;

namespace Application.Products.Queries.GetAllProductsQuery
{
    public sealed record GetAllProductsQuery() : IRequest<List<Product>>;
}
