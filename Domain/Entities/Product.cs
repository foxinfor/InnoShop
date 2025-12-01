namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }
        public bool IsAvailable { get; private set; }
        public Guid OwnerUserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public Product() { }

        public Product(string name, string description, int price, bool isAvailable, Guid ownerUserId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            IsAvailable = isAvailable;
            OwnerUserId = ownerUserId;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string description, int price, bool isAvailable)
        {
            Name = name;
            Description = description;
            Price = price;
            IsAvailable = isAvailable;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;
        }
    }
}
