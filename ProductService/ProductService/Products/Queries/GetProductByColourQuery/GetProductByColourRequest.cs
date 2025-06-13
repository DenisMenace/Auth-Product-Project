using System.ComponentModel.DataAnnotations;

namespace Application.Products.Queries.GetProductByColourQuery
{
    public record GetProductByColourRequest(
    [Required] string Colour);
}
