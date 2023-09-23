using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using SampleWebAPI.Models;
using SampleWebAPI.Services;

namespace SampleWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _bookService;
        public BooksController(BooksService bookService) =>
            _bookService = bookService;

        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _bookService.GetBooksAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _bookService.GetBookAsync(id);
            if(book is null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpPost]
        public async Task<IActionResult>Post(Book newBook)
        {
            await _bookService.CreateAsync(newBook);
            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult>Update(string id,Book updatedBook)
        {
            var book = await _bookService.GetBookAsync(id);
            if(book is null)
            {
                return NotFound();
            }
            updatedBook.Id = book.Id;
            await _bookService.UpdateAsync(id, updatedBook);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _bookService.GetBookAsync(id);
            if(book is null)
            {
                return NotFound();
            }
            await _bookService.RemoveAsync(id);
            return NoContent();
        }
    }
}
