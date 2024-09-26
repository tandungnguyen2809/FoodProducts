using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class FoodProductService
    {
        private readonly HttpClient _client;
        private readonly ILogger<FoodProductService> _logger;

        public FoodProductService(HttpClient client, ILogger<FoodProductService> logger)
        {
            _client = client;
            _logger = logger;
        }

        // Lấy danh sách sản phẩm
        public async Task<IEnumerable<FoodProduct>> GetProductsAsync()
        {
            try
            {
                var response = await _client.GetAsync("/api/FoodProducts");
                if (response.IsSuccessStatusCode)
                {
                    // Đảm bảo rằng dữ liệu trả về không null
                    return await response.Content.ReadFromJsonAsync<IEnumerable<FoodProduct>>() ?? new List<FoodProduct>();
                }
                else
                {
                    _logger.LogError($"Failed to fetch products. Status Code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching products: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
            }

            // Trả về danh sách rỗng nếu có lỗi
            return Enumerable.Empty<FoodProduct>();
        }

        // Lấy sản phẩm theo ID
        public async Task<FoodProduct?> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"/api/FoodProducts/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FoodProduct>();
                }
                else
                {
                    _logger.LogError($"Failed to fetch product with ID: {id}. Status Code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching product with ID {id}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
            }

            // Trả về null nếu không tìm thấy hoặc có lỗi
            return null;
        }

        // Thêm mới sản phẩm
        public async Task<FoodProduct?> CreateProductAsync(FoodProduct product)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("/api/FoodProducts", product);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FoodProduct>();
                }
                else
                {
                    _logger.LogError($"Failed to create product. Status Code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
            }

            // Trả về null nếu có lỗi
            return null;
        }

        // Cập nhật sản phẩm
        public async Task<bool> UpdateProductAsync(int id, FoodProduct product)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"/api/FoodProducts/{id}", product);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    _logger.LogError($"Failed to update product with ID: {id}. Status Code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error updating product with ID {id}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
            }

            // Trả về false nếu không cập nhật thành công
            return false;
        }

        // Xóa sản phẩm
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"/api/FoodProducts/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    _logger.LogError($"Failed to delete product with ID: {id}. Status Code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error deleting product with ID {id}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
            }

            // Trả về false nếu không xóa thành công
            return false;
        }
    }
}
