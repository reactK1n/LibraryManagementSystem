using AutoMapper;
using LibraryManagementSystem.Domain.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Domain.Utilities
{
    public static class Paginator
    {
        public static async Task<PaginatedResult<TDestination>> PaginationAsync<TSource, TDestination>(
            this IQueryable<TSource> queryable, int pageSize, int pageNumber, IMapper mapper)
            where TSource : class
            where TDestination : class
        {
            var totalItems = await queryable.CountAsync();

            // Normalize page size and number
            pageSize = (pageSize > 10 || pageSize < 1) ? 10 : pageSize;
            pageNumber = pageNumber > 0 ? pageNumber : 1;

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedItems = mapper.Map<List<TDestination>>(items);

            return new PaginatedResult<TDestination>
            {
                Items = mappedItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }
    }
}
