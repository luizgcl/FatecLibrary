using FatecLibrary.Web.Models;
using FatecLibrary.Web.Models.Entities;
using FatecLibrary.Web.Models.Error;
using FatecLibrary.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FatecLibrary.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;

        public HomeController(ILogger<HomeController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }


        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooks(string.Empty);
            books = books.OrderByDescending(x => x.Price).ToList();
            if (books is null) return View("Error");
            return View(books);
        }

        [HttpGet]
        public async Task<ActionResult<BookViewModel>> BooksDetails(int id)
        {
            var result = await _bookService.FindBookById(id, string.Empty);
            if (result is null) return View("Error");
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token"); // obtendo o token e salvando ele no contexto
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}