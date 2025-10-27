using BusinessLogicLayer.Dto.Requests.Book;
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
        public async Task<ActionResult<List<Book>>> GetAll(CancellationToken cancellationToken)
        {
            var books = await _bookService.GetAllAsync(cancellationToken);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(id, cancellationToken);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<Book>>> GetByAuthor(Guid authorId, CancellationToken cancellationToken)
        {
            var books = await _bookService.GetByAuthorIdAsync(authorId, cancellationToken);
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Create(CreateBookDto book, CancellationToken cancellationToken)
        {
            var created = await _bookService.CreateAsync(book, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);  
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> Update(Guid id, UpdateBookDto book, CancellationToken cancellationToken)
        {
            var updated = await _bookService.UpdateAsync(id, book, cancellationToken);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _bookService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}