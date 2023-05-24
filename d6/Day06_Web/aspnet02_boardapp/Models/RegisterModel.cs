using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aspnet02_boardapp.Models
{
    public class RegisterModel
    {
        // 회원가입할 때 데이터 받는 부분

        [Required(ErrorMessage = "이메일주소는 필수입니다!")]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [DisplayName("핸드폰번호")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "비밀번호는 필수입니다!")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "비밀번호확인은 필수입니다!")]
        [DataType(DataType.Password)]
        [DisplayName("Password Check")]
        [Compare("Password", ErrorMessage = "*패스워드가 일치하지 않습니다! 다시 입력하세요")]
        public string ConfirmPassword { get; set; }
    }
}
