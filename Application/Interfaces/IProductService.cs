using Application.DTOs;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDTO> CreateAsync(Guid ownerUserId, CreateProductDTO dto, CancellationToken ct);
        Task<ProductDTO?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken ct);
        Task<bool> UpdateAsync(Guid id, Guid ownerUserId, UpdateProductDTO dto, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, Guid ownerUserId, CancellationToken ct);
        Task<PagedResult<ProductDTO>> SearchAsync(ProductQuery query, Guid ownerUserId, CancellationToken ct);
    }

    public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);
}
