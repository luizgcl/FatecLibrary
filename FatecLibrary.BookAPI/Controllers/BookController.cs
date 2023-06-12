using FatecLibrary.BookAPI.DTO.Entities;
using FatecLibrary.BookAPI.Services.Entities;
using FatecLibrary.BookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace FatecLibrary.BookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : Controller
{

    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublishingDTO>>> Get()
    {
        var booksDTO = await _bookService.GetAll();
        if (booksDTO == null) return NotFound("Books not found!");
        return Ok(booksDTO);
    }

    [HttpGet("{id}", Name = "GetBook")]
    public async Task<ActionResult<BookDTO>> Get(int id)
    {
        var bookDTO = await _bookService.GetById(id);
        if (bookDTO == null) return NotFound("Book not found");
        return Ok(bookDTO);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] BookDTO bookDTO)
    {
        if (bookDTO is null) return BadRequest("Data Invalid");
        await _bookService.Create(bookDTO);
        return new CreatedAtRouteResult("GetBook", new { id = bookDTO.Id }, bookDTO);
    }

    [HttpPut()]
    public async Task<ActionResult> Put([FromBody] BookDTO bookDTO)
    {
        if (bookDTO is null) return BadRequest("Data invalid");
        await _bookService.Update(bookDTO);
        return Ok(bookDTO);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BookDTO>> Delete(int id)
    {
        var bookDTO = await _bookService.GetById(id);
        if (bookDTO == null) return NotFound("Product not found");
        await _bookService.Remove(id);
        return Ok(bookDTO);
    }
}
