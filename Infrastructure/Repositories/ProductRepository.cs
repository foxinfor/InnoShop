using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) => _db = db;

        public Task<Product?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken ct)
            => _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.OwnerUserId == ownerUserId, ct);

        public async Task AddAsync(Product product, CancellationToken ct)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Product product, CancellationToken ct)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Product product, CancellationToken ct)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<(IReadOnlyList<Product> Items, int Total)> SearchAsync(ProductQuery q, Guid ownerUserId, CancellationToken ct)
        {
            var query = _db.Products.AsQueryable();

            query = query.Where(p => p.OwnerUserId == ownerUserId && p.IsAvailable);

            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var s = q.Search.ToLower();
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{s}%")
                                      || EF.Functions.Like(p.Description, $"%{s}%"));
            }
            if (q.MinPrice is not null) query = query.Where(p => p.Price >= q.MinPrice);
            if (q.MaxPrice is not null) query = query.Where(p => p.Price <= q.MaxPrice);
            if (q.IsAvailable is not null) query = query.Where(p => p.IsAvailable == q.IsAvailable);
            if (q.CreatedFrom is not null) query = query.Where(p => p.CreatedAt >= q.CreatedFrom);
            if (q.CreatedTo is not null) query = query.Where(p => p.CreatedAt <= q.CreatedTo);

            query = q.SortBy switch
            {
                "Name" => (q.Desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name)),
                "Price" => (q.Desc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price)),
                _ => (q.Desc ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt)),
            };

            var total = await query.CountAsync(ct);
            var items = await query.Skip((q.Page - 1) * q.PageSize).Take(q.PageSize).ToListAsync(ct);
            return (items, total);
        }

        public async Task<IReadOnlyList<Product>> GetAllByOwnerAsync(Guid ownerUserId, CancellationToken ct)
        {
            return await _db.Products
                .Where(p => p.OwnerUserId == ownerUserId)
                .ToListAsync(ct);
        }
    }

}
