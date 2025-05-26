using Managing.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Managing.Services
{
    public class ProductionOrderService
    {
        private readonly ApiService _apiService;

        public ProductionOrderService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<ProductionOrder>> GetProductionOrdersAsync()
        {
            return await _apiService.GetProductionOrdersAsync();
        }

        public async Task<ProductionOrder?> GetProductionOrderByIdAsync(int id)
        {
            return await _apiService.GetProductionOrderByIdAsync(id);
        }

        public async Task<ProductionOrder?> CreateProductionOrderAsync(string productSKU, int quantity)
        {
            var order = new ProductionOrder
            {
                ProductSKU = productSKU,
                Quantity = quantity,
                CreatedAt = DateTime.Now,
                Status = "Created"
            };

            return await _apiService.CreateProductionOrderAsync(order);
        }

        public async Task<bool> UpdateProductionOrderAsync(ProductionOrder order)
        {
            return await _apiService.UpdateProductionOrderAsync(order.Id, order);
        }

        public async Task<bool> DeleteProductionOrderAsync(int id)
        {
            return await _apiService.DeleteProductionOrderAsync(id);
        }

        public async Task<ProductionOrder?> CompleteProductionOrderAsync(int id)
        {
            return await _apiService.CompleteProductionOrderAsync(id);
        }

        public async Task<Dictionary<string, int>> GetProductionOrderStatusSummaryAsync()
        {
            return await _apiService.GetProductionOrderStatusSummaryAsync();
        }
    }
}