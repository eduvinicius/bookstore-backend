using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Bookstore.Api.DTOs.External;
using Bookstore.App.Services.Interfaces;

namespace Bookstore.App.Services.External
{
    public class GoogleBooksService(HttpClient httpClient, IConfiguration configuration) : IGoogleBooksService
    {

        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;

        public async Task<IEnumerable<GoogleBookDto>> SearchAsync(string query, int maxResults = 10, int startIndex = 0)
        {
            var apiKey = _configuration["GoogleBooks:ApiKey"];
            var baseUrl = _configuration["GoogleBooks:BaseUrl"];

            var url =
            $"{baseUrl}volumes?q={Uri.EscapeDataString(query)}&startIndex={startIndex}&maxResults={maxResults}";

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);

            var items = document.RootElement.GetProperty("items");

            var result = new List<GoogleBookDto>();

            foreach (var item in items.EnumerateArray())
            {
                var volumeInfo = item.GetProperty("volumeInfo");

                result.Add(new GoogleBookDto
                {
                    Id = item.GetProperty("id").GetString()!,
                    Title = volumeInfo.GetProperty("title").GetString()!,
                    Authors = volumeInfo.TryGetProperty("authors", out var authors)
                        ? authors.EnumerateArray().Select(a => a.GetString()!).ToList()
                        : [],
                    Description = volumeInfo.TryGetProperty("description", out var desc)
                        ? desc.GetString()
                        : null,
                    Thumbnail = volumeInfo.TryGetProperty("imageLinks", out var images)
                        && images.TryGetProperty("thumbnail", out var thumb)
                        ? thumb.GetString()
                        : null,
                    PublishedYear = volumeInfo.TryGetProperty("publishedDate", out var date)
                        && int.TryParse(date.GetString()?.Substring(0, 4), out var year)
                        ? year
                        : null,
                    PageCount = volumeInfo.TryGetProperty("pageCount", out var pageCount)
                        ? pageCount.GetInt32()
                        : null,
                    Language = volumeInfo.TryGetProperty("language", out var lang)
                        ? lang.GetString()
                        : null
                });
            }

            return result;
        }

        public async Task<GoogleBookDto?> GetByIdAsync(string googleBookId)
        {
            var apiKey = _configuration["GoogleBooks:ApiKey"];
            var baseUrl = _configuration["GoogleBooks:BaseUrl"];

            var url = $"{baseUrl}volumes/{googleBookId}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);
            var volumeInfo = document.RootElement.GetProperty("volumeInfo");

            return new GoogleBookDto
            {
                Id = googleBookId,
                Title = volumeInfo.GetProperty("title").GetString()!,
                Authors = volumeInfo.TryGetProperty("authors", out var authors)
                    ? authors.EnumerateArray().Select(a => a.GetString()!).ToList()
                    : [],
                Description = volumeInfo.TryGetProperty("description", out var desc)
                    ? desc.GetString()
                    : null,
                Thumbnail = volumeInfo.TryGetProperty("imageLinks", out var images)
                        && images.TryGetProperty("thumbnail", out var thumb)
                        ? thumb.GetString()
                        : null,
                PublishedYear = volumeInfo.TryGetProperty("publishedDate", out var date)
                    && int.TryParse(date.GetString()?.Substring(0, 4), out var year)
                    ? year
                    : null,
                PageCount = volumeInfo.TryGetProperty("pageCount", out var pageCount)
                        ? pageCount.GetInt32()
                        : null,
                Language = volumeInfo.TryGetProperty("language", out var lang)
                        ? lang.GetString()
                        : null
            };
        }

    }
}
