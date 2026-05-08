using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Vui long nhap email de dang ky")]
        [EmailAddress(ErrorMessage = "email khong hop le")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui long nhap password de dang ky")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
