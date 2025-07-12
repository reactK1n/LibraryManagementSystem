namespace LibraryManagementSystem.Domain.Dtos
{
    public class ApiResponse
    {
        public bool Status { get; set; } = false;
        public string? Message { get; set; }
        public object? Data { get; set; }

    }

    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
