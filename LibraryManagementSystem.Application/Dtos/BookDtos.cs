using static LibraryManagementSystem.Application.Utilities.Validators.BookValidation;

namespace LibraryManagementSystem.Application.Dtos
{
    public class BookDtos
    {
        public class CreateRequest
        {
            public required string Title { get; set; }
            public required string Author { get; set; }

            [Isbn13Only]
            public required string ISBN { get; set; }
            public required DateTime PublishedDate { get; set; }
        }

        public class BookResponse
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string ISBN { get; set; }
            public DateTime PublishedDate { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? ModifiedAt { get; set; }
        }

        public class UpdateRequest
        {
            public string? Title { get; set; }
            public string? Author { get; set; }

            [Isbn13Only]
            public string? ISBN { get; set; }
            public DateTime? PublishedDate { get; set; }
        }

        public class BookFilterRequest
        {
            public string? Search { get; set; }
            public int PageSize { get; set; } = 10;
            public int PageNumber { get; set; } = 1;
        }
    }
}
