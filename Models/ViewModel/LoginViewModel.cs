using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui long nhap email de dang nhap")]
        [EmailAddress(ErrorMessage = "email khong hop le")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui long nhap password de dang nhap")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
