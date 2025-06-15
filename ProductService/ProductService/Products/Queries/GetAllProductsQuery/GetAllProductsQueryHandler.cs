﻿using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetAllProductsQuery
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;
        public GetAllProductsQueryHandler(
            IUnitOfWork unitOfWork,
            ILogger<GetAllProductsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllProductsQuery.");

            List<Product> products = await _unitOfWork.ProductRepository.GetAllAsync();

            _logger.LogInformation("Retrieved {Count} products from the database.", products.Count);

            if (products.Count == 0)
            {
                _logger.LogWarning("No products found in the database.");
            }

            return products;
        }
    }
}
