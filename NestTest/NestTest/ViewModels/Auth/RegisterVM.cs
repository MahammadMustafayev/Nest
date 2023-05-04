using System.ComponentModel.DataAnnotations;

namespace NestTest.ViewModels.Auth
{
    public class RegisterVM
    {
        [Required,MaxLength(50)]
        public string FirstName { get; set; }
        [Required,MaxLength(100)]
        public string LastName { get; set; }
        [Required,MaxLength(50)]
        public string UserName { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
