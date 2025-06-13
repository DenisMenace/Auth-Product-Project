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
            if (string.IsNullOrWhiteSpace(request.Colour))
            {
                _logger.LogWarning("Colour was empty.");
                throw new ArgumentException("Colour must not be null or empty");
            }

            List<Product> products = await _unitOfWork.ProductRepository.Get().Where(p => p.Colour == request.Colour).ToListAsync(); 

            return products;
        }
    }
}
