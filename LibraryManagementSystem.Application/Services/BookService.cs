using AutoMapper;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Application.Dtos;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.IRepository;
using LibraryManagementSystem.Application.Utilities;
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
            var book = _mapper.Map<Book>(dto); //map incoming request dto to the type book

            await _unitOfWork.BookRepository.InsertAsync(book);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Book created: {book}");

            var result = _mapper.Map<BookResponse>(book); //map type book to the outgoing response
            return new ApiResponse
            {
                Status = true,
                Message = "Book created successfully",
                Data = result
            };
        }

        /// <summary>
        /// Retrieves all books with optional filtering and pagination.
        /// </summary>
        public async Task<ApiResponse> GetAllAsync(BookFilterRequest request)
        {
            var query = _unitOfWork.BookRepository.GetContext().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search)) //if search is not null or empty, return all books
            {
                var search = request.Search.ToLower();

                query = query.Where(b =>
                    b.Title.ToLower().Contains(search) ||
                    b.Author.ToLower().Contains(search)
                );
            }

            var paginated = await query.PaginationAsync<Book, BookResponse>(
                request.PageSize, request.PageNumber, _mapper); //paginate result

            _logger.LogInformation($"Books fetched with search: '{request.Search}', Page {request.PageNumber}");

            return new ApiResponse
            {
                Status = true,
                Message = "Books retrieved successfully",
                Data = paginated
            };
        }

        /// <summary>
        /// Retrieves a specific book by ID.
        /// </summary>
        public async Task<ApiResponse> GetByIdAsync(long id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book not found: ID {id}");

                return new ApiResponse{ Message = "Book not found" };
            }

            _logger.LogInformation($"Book retrieved: ID {id}");

            var result = _mapper.Map<BookResponse>(book);
            return new ApiResponse
            {
                Status = true,
                Message = "Book retrieved successfully",
                Data = result
            };
        }

        /// <summary>
        /// Updates an existing book by ID.
        /// </summary>
        public async Task<ApiResponse> UpdateAsync(long id, UpdateRequest dto)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book not found for update: ID {id}");

                return new ApiResponse { Message = "Book not found" };
            }

            _mapper.Map(dto, book);
            _unitOfWork.BookRepository.Update(book);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Book updated: ID {id}");

            var result = _mapper.Map<BookResponse>(book);
            return new ApiResponse
            {
                Status = true,
                Message = "Book updated successfully",
                Data = result
            };
        }

        /// <summary>
        /// Deletes a book by ID.
        /// </summary>
        public async Task<ApiResponse> DeleteAsync(long id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book not found for deletion: ID {id}");

                return new ApiResponse { Message = "Book not found" };
            }

            _unitOfWork.BookRepository.Delete(book);
            await _unitOfWork.SaveChangesAsync(); 

            _logger.LogInformation($"Book deleted: ID {id}");

            return new ApiResponse
            {
                Status = true,
                Message = "Book deleted successfully"
            };
        }
    }
}
