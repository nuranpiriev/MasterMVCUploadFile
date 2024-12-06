namespace MastersMVC2.DTO.ServiceDTO
{
    public class CreateServiceDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
    public class UpdateServiceDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool isActive { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
    }
}
