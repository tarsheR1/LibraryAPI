using BusinessLogicLayer.Dto.Requests.Author;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAll(CancellationToken cancellationToken)
        {
            var authors = await _authorService.GetAllAsync(cancellationToken);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetByIdAsync(id, cancellationToken);

            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult<Author>> Create(CreateAuthorDto author, CancellationToken cancellationToken)
        {
            var created = await _authorService.CreateAsync(author, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Author>> Update(Guid id, UpdateAuthorDto author, CancellationToken cancellationToken)
        {
            var updated = await _authorService.UpdateAsync(id, author, cancellationToken);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _authorService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}