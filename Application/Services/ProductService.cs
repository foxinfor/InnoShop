using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo) => _repo = repo;

        public async Task<ProductDTO> CreateAsync(Guid ownerUserId, CreateProductDTO dto, CancellationToken ct)
        {
            var product = new Product(dto.Name, dto.Description, dto.Price, dto.IsAvailable, ownerUserId);
            await _repo.AddAsync(product, ct);
            return Map(product);
        }

        public async Task<ProductDTO?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken ct)
        {
            var p = await _repo.GetByIdAsync(id, ownerUserId, ct);
            return p is null ? null : Map(p);
        }

        public async Task<bool> UpdateAsync(Guid id, Guid ownerUserId, UpdateProductDTO dto, CancellationToken ct)
        {
            var p = await _repo.GetByIdAsync(id, ownerUserId, ct);
            if (p is null) return false; 
            p.Update(dto.Name, dto.Description, dto.Price, dto.IsAvailable);
            await _repo.UpdateAsync(p, ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid ownerUserId, CancellationToken ct)
        {
            var p = await _repo.GetByIdAsync(id, ownerUserId, ct);
            if (p is null) return false;
            await _repo.DeleteAsync(p, ct);
            return true;
        }

        public async Task<PagedResult<ProductDTO>> SearchAsync(ProductQuery q, Guid ownerUserId, CancellationToken ct)
        {
            var (items, total) = await _repo.SearchAsync(q, ownerUserId, ct);
            return new PagedResult<ProductDTO>(items.Select(Map).ToList(), total, q.Page, q.PageSize);
        }

        private static ProductDTO Map(Product p) =>
            new(p.Id, p.Name, p.Description, p.Price, p.IsAvailable, p.OwnerUserId, p.CreatedAt, p.UpdatedAt);
    }

}
