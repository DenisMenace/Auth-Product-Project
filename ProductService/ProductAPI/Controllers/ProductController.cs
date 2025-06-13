using Application.Products.Commands.InsertProduct;
using Application.Products.Queries.GetAllProductsQuery;
using Application.Products.Queries.GetProductByColourQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("products")]
    [Authorize] 
    public class ProductController : ControllerBase
    {
        private readonly ISender _mediator;

        public ProductController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] InsertProductRequest request)
        {
            var command = new InsertProductCommand(request.Name, request.Colour);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllProducts), result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpGet("colour/{colour}")]
        public async Task<IActionResult> GetProductsByColour(string colour)
        {
            var products = await _mediator.Send(new GetProductByColourQuery(colour));
            return Ok(products);
        }
    }
}
