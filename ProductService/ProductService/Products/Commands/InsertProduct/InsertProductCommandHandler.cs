using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.InsertProduct
{
    public class InsertProductCommandHandler : IRequestHandler<InsertProductCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InsertProductCommandHandler> _logger;

        public InsertProductCommandHandler(IUnitOfWork unitOfWork, ILogger<InsertProductCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<string> Handle(InsertProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Colour = request.Colour
            };

            _unitOfWork.ProductRepository.Add(product);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                _logger.LogError("Product creation failed for {Name}. No changes were saved to the database.", request.Name);
                throw new InvalidOperationException("Failed to create product. No changes were saved to the database.");
            }

            _logger.LogInformation("Product {Name} successfully added to the database.", request.Name);

            return "Product successfully created.";

        }
    }
}
