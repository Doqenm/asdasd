using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Managing.Models;

namespace Managing.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private string? _authToken;

        public ApiService(string baseUrl = "https://localhost:5001")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetAuthToken(string token)
        {
            _authToken = token;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        #region Auth

        public async Task<User?> LoginAsync(string username, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", new { Username = username, Password = password });
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonDocument>();
                    var success = result?.RootElement.GetProperty("success").GetBoolean() ?? false;
                    
                    if (success)
                    {
                        var token = result?.RootElement.GetProperty("token").GetString();
                        if (!string.IsNullOrEmpty(token))
                        {
                            SetAuthToken(token);
                        }

                        var user = JsonSerializer.Deserialize<User>(
                            result?.RootElement.GetProperty("user").GetRawText() ?? "{}",
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        
                        return user;
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region Stock

        public async Task<List<StockItem>> GetStockAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/stock");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<StockItem>>() ?? new List<StockItem>();
                }
                
                return new List<StockItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get stock error: {ex.Message}");
                return new List<StockItem>();
            }
        }

        public async Task<bool> AddStockAsync(string sku, int quantity, string? note = null)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/stock/add", new { SKU = sku, Quantity = quantity, Note = note });
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add stock error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ConsumeStockAsync(string sku, int quantity, string? note = null)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/stock/consume", new { SKU = sku, Quantity = quantity, Note = note });
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Consume stock error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteStockAsync(string sku, string? note = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/stock/{sku}");
                if (!string.IsNullOrEmpty(note))
                {
                    var uriBuilder = new UriBuilder($"{_baseUrl}/api/stock/{sku}");
                    uriBuilder.Query = $"note={Uri.EscapeDataString(note)}";
                    request = new HttpRequestMessage(HttpMethod.Delete, uriBuilder.Uri);
                }
                
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete stock error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<StockHistory>> GetStockHistoryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/stock/history");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<StockHistory>>() ?? new List<StockHistory>();
                }
                
                return new List<StockHistory>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get stock history error: {ex.Message}");
                return new List<StockHistory>();
            }
        }

        #endregion

        #region Products

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/products");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();
                }
                
                return new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get products error: {ex.Message}");
                return new List<Product>();
            }
        }

        public async Task<Product?> GetProductBySKUAsync(string sku)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/products/{sku}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Product>();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get product error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddOrUpdateProductAsync(Product product)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/products", product);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add/update product error: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Predictions

        public async Task<Dictionary<string, int>?> CalculateRequirementsAsync(Dictionary<string, int> productQuantities)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/prediction/calculate-requirements", productQuantities);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Calculate requirements error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> SavePredictionAsync(ProductionPrediction prediction)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/prediction/save", prediction);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save prediction error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ProductionPrediction>> GetPredictionsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/prediction");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<ProductionPrediction>>() ?? new List<ProductionPrediction>();
                }
                
                return new List<ProductionPrediction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get predictions error: {ex.Message}");
                return new List<ProductionPrediction>();
            }
        }

        #endregion

        #region MaterialReceipts

        public async Task<List<MaterialReceipt>> GetMaterialReceiptsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/materialreceipts");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<MaterialReceipt>>() ?? new List<MaterialReceipt>();
                }
                return new List<MaterialReceipt>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get material receipts error: {ex.Message}");
                return new List<MaterialReceipt>();
            }
        }

        public async Task<MaterialReceipt?> GetMaterialReceiptByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/materialreceipts/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<MaterialReceipt>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get material receipt by id error: {ex.Message}");
                return null;
            }
        }

        public async Task<MaterialReceipt?> CreateMaterialReceiptAsync(MaterialReceipt receipt)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/materialreceipts", receipt);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<MaterialReceipt>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create material receipt error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateMaterialReceiptAsync(int id, MaterialReceipt receipt)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/materialreceipts/{id}", receipt);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update material receipt error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteMaterialReceiptAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/materialreceipts/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete material receipt error: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region ProductionOrders

        public async Task<List<ProductionOrder>> GetProductionOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/productionorders");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<ProductionOrder>>() ?? new List<ProductionOrder>();
                }
                return new List<ProductionOrder>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get production orders error: {ex.Message}");
                return new List<ProductionOrder>();
            }
        }

        public async Task<ProductionOrder?> GetProductionOrderByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/productionorders/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProductionOrder>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get production order by id error: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductionOrder?> CreateProductionOrderAsync(ProductionOrder order)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/productionorders", order);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProductionOrder>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create production order error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateProductionOrderAsync(int id, ProductionOrder order)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/productionorders/{id}", order);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update production order error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductionOrderAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/productionorders/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete production order error: {ex.Message}");
                return false;
            }
        }
        
        public async Task<ProductionOrder?> CompleteProductionOrderAsync(int id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"/api/productionorders/{id}/complete", null);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProductionOrder>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Complete production order error: {ex.Message}");
                return null;
            }
        }

        public async Task<Dictionary<string, int>> GetProductionOrderStatusSummaryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/productionorders/status-summary");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Dictionary<string, int>>() ?? new Dictionary<string, int>();
                }
                return new Dictionary<string, int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get production order status summary error: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }

        #endregion

        #region InventoryItems

        public async Task<List<InventoryItem>> GetInventoryItemsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/inventoryitems");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<InventoryItem>>() ?? new List<InventoryItem>();
                }
                return new List<InventoryItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get inventory items error: {ex.Message}");
                return new List<InventoryItem>();
            }
        }

        public async Task<InventoryItem?> GetInventoryItemByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/inventoryitems/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<InventoryItem>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get inventory item by id error: {ex.Message}");
                return null;
            }
        }

        public async Task<InventoryItem?> CreateInventoryItemAsync(InventoryItem item)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/inventoryitems", item);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<InventoryItem>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create inventory item error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateInventoryItemAsync(int id, InventoryItem item)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/inventoryitems/{id}", item);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update inventory item error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteInventoryItemAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/inventoryitems/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete inventory item error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<InventoryItem>> SearchInventoryItemsAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/inventoryitems/search/{query}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<InventoryItem>>() ?? new List<InventoryItem>();
                }
                return new List<InventoryItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search inventory items error: {ex.Message}");
                return new List<InventoryItem>();
            }
        }

        #endregion
    }
}