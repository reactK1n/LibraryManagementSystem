using AutoMapper;
using LibraryManagementSystem.Application.Dtos;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Application.Utilities;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.IRepository;
using Microsoft.Extensions.Logging;
using static LibraryManagementSystem.Application.Dtos.BookDtos;

namespace LibraryManagementSystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new book and saves it to the database.
        /// </summary>
        public async Task<ApiResponse> CreateAsync(CreateRequest dto)
        {
            // Map incoming DTO to Book entity
            var book = _mapper.Map<Book>(dto);

            // Insert new book record and commit to database
            await _unitOfWork.BookRepository.InsertAsync(book);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Book created: Title = {book.Title}, Author = {book.Author}");

            // Map saved Book entity to response DTO
            var result = _mapper.Map<BookResponse>(book);
            return new ApiResponse
            {
                Status = true,
                Message = "Book created successfully",
                Data = result
            };
        }

        /// <summary>
        /// Retrieves all books with optional search and pagination.
        /// </summary>
        public async Task<ApiResponse> GetAllAsync(BookFilterRequest request)
        {
            var query = _unitOfWork.BookRepository.GetContext().AsQueryable();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(search) ||
                    b.Author.ToLower().Contains(search)
                );
            }

            // Apply pagination and map to response
            var paginated = await query.PaginationAsync<Book, BookResponse>(
                request.PageSize, request.PageNumber, _mapper);

            _logger.LogInformation($"Books fetched with search: '{request.Search}', Page: {request.PageNumber}");

            return new ApiResponse
            {
                Status = true,
                Message = "Books retrieved successfully",
                Data = paginated
            };
        }

        /// <summary>
        /// Retrieves a book by its unique ID.
        /// </summary>
        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book not found: ID = {id}");
                return new ApiResponse { Message = "Book not found" };
            }

            _logger.LogInformation($"Book retrieved: ID = {id}");

            var result = _mapper.Map<BookResponse>(book);
            return new ApiResponse
            {
                Status = true,
                Message = "Book retrieved successfully",
                Data = result
            };
        }

        /// <summary>
        /// Updates an existing book with new values.
        /// </summary>
        public async Task<ApiResponse> UpdateAsync(int id, UpdateRequest dto)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book not found for update: ID = {id}");
                return new ApiResponse { Message = "Book not found" };
            }

            // Map updated values to existing book entity
            _mapper.Map(dto, book);

            _unitOfWork.BookRepository.Update(book);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Book updated: ID = {id}");

            var result = _mapper.Map<BookResponse>(book);
            return new ApiResponse
            {
                Status = true,
                Message = "Book updated successfully",
                Data = result
            };
        }

        /// <summary>
        /// Deletes a book from the system by ID.
        /// </summary>
        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book not found for deletion: ID = {id}");
                return new ApiResponse { Message = "Book not found" };
            }

            _unitOfWork.BookRepository.Delete(book);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Book deleted: ID = {id}");

            return new ApiResponse
            {
                Status = true,
                Message = "Book deleted successfully"
            };
        }
    }
}
