using System.Text.Json;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.IRepository;

namespace LibraryManagementSystem.Application.Utilities
{
    public static class BookSeeder
    {
        /// <summary>
        /// Seeds books into the database if none exist.
        /// </summary>
        /// <param name="unitOfWork">Unit of Work to access repositories.</param>
        /// <returns></returns>
        public static async Task SeedBooksAsync(IUnitOfWork unitOfWork)
        {
            try
            {
                var bookContext = unitOfWork.BookRepository.GetContext();

                if (!bookContext.Any())
                {
                    var json = await File.ReadAllTextAsync("SeedData/books.json");
                    var books = JsonSerializer.Deserialize<List<Book>>(json);

                    if (books is { Count: > 0 })
                    {
                        await bookContext.AddRangeAsync(books);
                        await unitOfWork.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new("An error occurred while seeding book data.");
            }
        }
    }
}
