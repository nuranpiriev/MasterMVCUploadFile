using Microsoft.AspNetCore.Identity;

namespace MastersMVC2.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class BaseEntity
    {
        public int Id { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class Master : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public int ExperienceYear { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }

    public class Order : BaseEntity
    {
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientEmail { get; set; }
        public string Problem { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public int MasterId { get; set; }
        public Master? Master { get; set; }
    }

    public class Service : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }

        public ICollection<Master>? Masters { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
