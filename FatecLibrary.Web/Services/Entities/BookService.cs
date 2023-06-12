using FatecLibrary.Web.Models.Entities;
using FatecLibrary.Web.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FatecLibrary.Web.Services.Entities;
public class BookService : IBookService
{

    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndpoint = "/api/book/";
    private readonly JsonSerializerOptions _options;
    private BookViewModel _bookVM;
    private IEnumerable<BookViewModel> booksViewModel;

    public BookService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<BookViewModel>> GetAllBooks(string token)
    {
        var client = _clientFactory.CreateClient("BookAPI");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.GetAsync(apiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                booksViewModel = await JsonSerializer.
                    DeserializeAsync<IEnumerable<BookViewModel>>(apiResponse, _options);
            }
            else
                return null;
        }
        return booksViewModel;
    }

    public async Task<BookViewModel> FindBookById(int id,string token)
    {
        var client = _clientFactory.CreateClient("BookAPI");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _bookVM = await JsonSerializer.
                    DeserializeAsync<BookViewModel>(apiResponse, _options);
            }
            else
                return null;
        }
        return _bookVM;
    }

    public async Task<BookViewModel> CreateBook(BookViewModel bookVM, string token)
    {
        var client = _clientFactory.CreateClient("BookAPI");
        PutTokenInHeaderAuthorization(token, client);

        StringContent content = new StringContent(JsonSerializer.Serialize(bookVM),
                Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _bookVM = await JsonSerializer.
                    DeserializeAsync<BookViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return _bookVM;
    }

    public async Task<BookViewModel> UpdateBook(BookViewModel bookVM, string token)
    {
        var client = _clientFactory.CreateClient("BookAPI");
        PutTokenInHeaderAuthorization(token, client);

        BookViewModel bookUpdate = new BookViewModel();

        using (var response = await client.PutAsJsonAsync(apiEndpoint, bookVM))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                bookUpdate = await JsonSerializer.
                    DeserializeAsync<BookViewModel>(apiResponse, _options);
            }
            else
                return null;
        }

        return bookUpdate;
    }

    public async Task<bool> DeleteBookById(int id, string token)
    {
        var client = _clientFactory.CreateClient("BookAPI");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.DeleteAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode) return true;
        }
        return false;
    }

    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
