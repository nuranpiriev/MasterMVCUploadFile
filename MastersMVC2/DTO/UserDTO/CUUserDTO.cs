using System.ComponentModel.DataAnnotations;

namespace MastersMVC2.DTO.UserDTO
{
    public class CreateUserDto
    {
        [Required]
        [Display(Prompt = "Firstname")]
        public string FirstName { get; set; }

        [Required]
        [Display(Prompt = "Lastname")]
        public string LastName { get; set; }

        [Required]
        [Display(Prompt = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Prompt = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Prompt = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Prompt = "Repeat Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
    public class LoginUserDto
    {
        [Required]
        [Display(Prompt = "Email or Username")]
        public string EmailOrUserName { get; set; }

        [Required]
        [Display(Prompt = "Password")]
        public int Password { get; set; }
    }
}
