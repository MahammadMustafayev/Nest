using System.ComponentModel.DataAnnotations;

namespace NestTest.ViewModels.Auth
{
    public class LoginVM
    {
       
        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
