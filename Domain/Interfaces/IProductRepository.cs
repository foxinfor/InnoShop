using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(Product product, CancellationToken ct);
        Task UpdateAsync(Product product, CancellationToken ct);
        Task DeleteAsync(Product product, CancellationToken ct);

        Task<(IReadOnlyList<Product> Items, int Total)> SearchAsync(ProductQuery query, CancellationToken ct);
    }

    public record ProductQuery(
    string? Search,          
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? IsAvailable,
    DateTime? CreatedFrom,
    DateTime? CreatedTo,
    int Page = 1,
    int PageSize = 20,
    string? SortBy = "CreatedAt",
    bool Desc = true
);
}
