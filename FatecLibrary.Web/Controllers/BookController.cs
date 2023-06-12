using FatecLibrary.Web.Models.Entities;
using FatecLibrary.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FatecLibrary.Web.Roles;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;

namespace FatecLibrary.Web.Controllers;

[Authorize(Roles = Role.Admin)]
public class BookController : Controller
{

    private readonly IBookService _bookService;
    private readonly IPublishingService _publisingService;

    public BookController(IBookService bookService, IPublishingService publisingService)
    {
        _bookService = bookService;
        _publisingService = publisingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookViewModel>>> Index()
    {
        var result = await _bookService.GetAllBooks(await GetAccessToken());
        if (result is null) return View("Error");
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> CreateBook()
    {
        ViewBag.PublishingId = new SelectList(
                await _publisingService.GetAllPublishers(await GetAccessToken()), "Id", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook(BookViewModel bookViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _bookService.CreateBook(bookViewModel, await GetAccessToken());

            if (result != null) return RedirectToAction(nameof(Index));
        }
        else
        {
            ViewBag.PublishingId = new SelectList(
                 await _publisingService.GetAllPublishers(await GetAccessToken()), "Id", "Name");
            // return BadRequest("Erro");
        }
        return View(bookViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateBook(int id)
    {
        ViewBag.PublishingId = new 
            SelectList(await _publisingService.GetAllPublishers(await GetAccessToken()), "Id", "Name");

        var result = await _bookService.FindBookById(id, await GetAccessToken());

        if (result is null) return View("Error");

        return View(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBook(BookViewModel bookViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _bookService.UpdateBook(bookViewModel, await GetAccessToken());

            if (result is not null) return RedirectToAction(nameof(Index));

        }
        else 
            return BadRequest("Error");
        
        return View(bookViewModel);
    }

    [HttpGet]
    public async Task<ActionResult<BookViewModel>> DeleteBook(int id)
    {
        var result = await _bookService.FindBookById(id, await GetAccessToken());

        if (result is null) return View("Error");

        return View(result);
    }

    [HttpPost(), ActionName("DeleteBook")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _bookService.DeleteBookById(id, await GetAccessToken());

        if (!result) return View("Error");

        return RedirectToAction("Index");
    }
    private async Task<string> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }

}
