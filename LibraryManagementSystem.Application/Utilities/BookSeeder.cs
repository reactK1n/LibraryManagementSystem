using System.Text.Json;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.IRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using static System.Formats.Asn1.AsnWriter;

namespace LibraryManagementSystem.Application.Utilities
{
    public static class BookSeeder
    {

        public static async Task UseSeedInitializer(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await SeedBooks(app.ApplicationServices, unitOfWork);
        }

        /// <summary>
        /// Seeds books into the database if none exist.
        /// </summary>
        /// <param name="unitOfWork">Unit of Work to access repositories.</param>
        /// <returns></returns>
        public static async Task SeedBooks(IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
        {
            try
            {
                var bookContext = unitOfWork.BookRepository.GetContext();

                if (!bookContext.Any())
                {
                    var json = await File.ReadAllTextAsync("SeedData/books.json");
                   // var books = JsonSerializer.Deserialize<List<Book>>(json);
                    var books = JsonSerializer.Deserialize<List<Book>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    books.ForEach(b => b.PublishedDate = DateTime.UtcNow);

                    if (books is { Count: > 0 })
                    {
                        await bookContext.AddRangeAsync(books);
                        await unitOfWork.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new($"An error occurred while seeding book data. {ex}");
            }
        }
    }
}
