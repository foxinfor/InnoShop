namespace Application.DTOs
{
    public record CreateProductDTO(string Name, string Description, int Price, bool IsAvailable);
    public record UpdateProductDTO(string Name, string Description, int Price, bool IsAvailable);
    public record ProductDTO(Guid Id, string Name, string Description, int Price, bool IsAvailable,
                             Guid OwnerUserId, DateTime CreatedAt, DateTime? UpdatedAt);
}