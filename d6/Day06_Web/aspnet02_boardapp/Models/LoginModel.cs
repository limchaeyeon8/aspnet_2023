using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace aspnet02_boardapp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "이메일주소는 필수입니다!")]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "비밀번호는 필수입니다!")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("이메일 기억")]
        public bool RememberMe { get; set; }


    }
}
