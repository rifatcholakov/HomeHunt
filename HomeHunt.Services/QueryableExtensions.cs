using HomeHunt.Service.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeHunt.Services
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResponseData<T>> ToPagedAsync<T>(this IQueryable<T> query, int page, int pageSize)
        {
            int totalCount = await query.CountAsync();
            int safePageSize = Math.Max(1, pageSize);
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)safePageSize));
            int safePage = Math.Min(Math.Max(1, page), totalPages);

            List<T> items = await query
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToListAsync();

            return new PagedResponseData<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = safePage,
                TotalPages = totalPages
            };
        }
    }
}
