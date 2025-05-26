using Managing.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Managing.Services
{
    public class MaterialReceiptService
    {
        private readonly ApiService _apiService;

        public MaterialReceiptService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<MaterialReceipt>> GetMaterialReceiptsAsync()
        {
            return await _apiService.GetMaterialReceiptsAsync();
        }

        public async Task<MaterialReceipt?> GetMaterialReceiptByIdAsync(int id)
        {
            return await _apiService.GetMaterialReceiptByIdAsync(id);
        }

        public async Task<MaterialReceipt?> CreateMaterialReceiptAsync(string supplier, string lotNumber, int quantity)
        {
            var receipt = new MaterialReceipt
            {
                Supplier = supplier,
                LotNumber = lotNumber,
                Quantity = quantity,
                ReceivedAt = DateTime.Now
            };

            return await _apiService.CreateMaterialReceiptAsync(receipt);
        }

        public async Task<bool> UpdateMaterialReceiptAsync(MaterialReceipt receipt)
        {
            return await _apiService.UpdateMaterialReceiptAsync(receipt.Id, receipt);
        }

        public async Task<bool> DeleteMaterialReceiptAsync(int id)
        {
            return await _apiService.DeleteMaterialReceiptAsync(id);
        }
    }
}