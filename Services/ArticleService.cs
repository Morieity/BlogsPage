using ArticleAPI.DTOs;
using System.Text.Json;

namespace ArticleAPI.Services
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetArticlesAsync();
        Task<ArticleDto?> GetArticleAsync(int id);
        Task<ArticleDto?> CreateArticleAsync(CreateArticleDto createArticleDto);
        Task<bool> UpdateArticleAsync(int id, UpdateArticleDto updateArticleDto);
        Task<bool> DeleteArticleAsync(int id);
    }

    public class ArticleService : IArticleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ArticleService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArticleService(HttpClient httpClient, ILogger<ArticleService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private string GetBaseUrl()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                var request = context.Request;
                return $"{request.Scheme}://{request.Host}";
            }
            // fallback for when there's no HTTP context (shouldn't happen in web requests)
            return "http://localhost:5000";
        }

        public async Task<List<ArticleDto>> GetArticlesAsync()
        {
            try
            {
                var baseUrl = GetBaseUrl();
                var response = await _httpClient.GetAsync($"{baseUrl}/api/articles");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var articles = JsonSerializer.Deserialize<List<ArticleDto>>(jsonContent, _jsonOptions);
                    return articles ?? new List<ArticleDto>();
                }
                else
                {
                    _logger.LogError("获取文章列表失败，状态码: {StatusCode}", response.StatusCode);
                    return new List<ArticleDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取文章列表时发生异常");
                return new List<ArticleDto>();
            }
        }

        public async Task<ArticleDto?> GetArticleAsync(int id)
        {
            try
            {
                var baseUrl = GetBaseUrl();
                var response = await _httpClient.GetAsync($"{baseUrl}/api/articles/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ArticleDto>(jsonContent, _jsonOptions);
                }
                else
                {
                    _logger.LogError("获取文章失败，ID: {Id}, 状态码: {StatusCode}", id, response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取文章时发生异常，ID: {Id}", id);
                return null;
            }
        }

        public async Task<ArticleDto?> CreateArticleAsync(CreateArticleDto createArticleDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(createArticleDto);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                var baseUrl = GetBaseUrl();
                var response = await _httpClient.PostAsync($"{baseUrl}/api/articles", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ArticleDto>(responseContent, _jsonOptions);
                }
                else
                {
                    _logger.LogError("创建文章失败，状态码: {StatusCode}", response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建文章时发生异常");
                return null;
            }
        }

        public async Task<bool> UpdateArticleAsync(int id, UpdateArticleDto updateArticleDto)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(updateArticleDto);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                var baseUrl = GetBaseUrl();
                var response = await _httpClient.PutAsync($"{baseUrl}/api/articles/{id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    _logger.LogError("更新文章失败，ID: {Id}, 状态码: {StatusCode}", id, response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新文章时发生异常，ID: {Id}", id);
                return false;
            }
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            try
            {
                var baseUrl = GetBaseUrl();
                var response = await _httpClient.DeleteAsync($"{baseUrl}/api/articles/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    _logger.LogError("删除文章失败，ID: {Id}, 状态码: {StatusCode}", id, response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除文章时发生异常，ID: {Id}", id);
                return false;
            }
        }
    }
}
