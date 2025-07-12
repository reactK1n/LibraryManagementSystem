using LibraryManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Application.Dtos.BookDtos;

namespace LibraryManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Creates a new book record.
        /// </summary>
        /// <param name="request">The book creation request object.</param>
        /// <returns>ApiResponse indicating success or failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequest request)
        {
            var result = await _bookService.CreateAsync(request);
            return StatusCode(result.Status ? 200 : 400, result);
        }

        /// <summary>
        /// Retrieves a list of books with optional search and pagination.
        /// </summary>
        /// <param name="request">Filter and pagination options.</param>
        /// <returns>Paginated list of books in an ApiResponse.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BookFilterRequest request)
        {
            var result = await _bookService.GetAllAsync(request);
            return StatusCode(result.Status ? 200 : 400, result);
        }

        /// <summary>
        /// Retrieves a book by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>Book details in an ApiResponse.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetByIdAsync(id);
            return StatusCode(result.Status ? 200 : 400, result);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">ID of the book to update.</param>
        /// <param name="request">Updated book data.</param>
        /// <returns>ApiResponse indicating success or failure.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRequest request)
        {
            var result = await _bookService.UpdateAsync(id, request);
            return StatusCode(result.Status ? 200 : 400, result);
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <param name="id">ID of the book to delete.</param>
        /// <returns>ApiResponse indicating success or failure.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteAsync(id);
            return StatusCode(result.Status ? 200 : 400, result);
        }
    }
}
