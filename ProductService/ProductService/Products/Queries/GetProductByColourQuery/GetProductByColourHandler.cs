using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProductByColourQuery
{
    public class GetProductByColourQueryHandler : IRequestHandler<GetProductByColourQuery, List<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetProductByColourQueryHandler> _logger;
        public GetProductByColourQueryHandler(
            IUnitOfWork unitOfWork,
            ILogger<GetProductByColourQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<Product>> Handle(GetProductByColourQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetProductByColourQuery for Colour: {Colour}", request.Colour);

            List<Product> products = await _unitOfWork.ProductRepository.GetByColourAsync(request.Colour);

            _logger.LogInformation("Retrieved {Count} products for Colour: {Colour}", products.Count, request.Colour);

            return products;
        }
    }
}
