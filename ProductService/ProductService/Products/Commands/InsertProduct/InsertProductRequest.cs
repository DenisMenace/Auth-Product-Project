using System.ComponentModel.DataAnnotations;

namespace Application.Products.Commands.InsertProduct
{
    public record InsertProductRequest(
    [Required] string Name,
    [Required] string Colour);
}
