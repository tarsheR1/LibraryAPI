using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAll()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(Guid id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<Book>>> GetByAuthor(Guid authorId)
        {
            var books = await _bookService.GetByAuthorIdAsync(authorId);
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Create(Book book)
        {
            var created = await _bookService.CreateAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);  
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> Update(Guid id, Book book)
        {
            if (id != book.Id)
                return BadRequest("ID mismatch");


            var updated = await _bookService.UpdateAsync(book);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}