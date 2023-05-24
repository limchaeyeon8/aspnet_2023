using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aspnet02_boardapp.Models
{
    public class RegisterModel
    {
        // 회원가입할 때 데이터 받는 부분

        [Required]
        [EmailAddress]
        [DisplayName("---Email Address---")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("---Password---")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("---Password Check---")]
        [Compare("Password", ErrorMessage = "***패스워드가 일치하지 않습니다! 다시 입력하세요***")]
        public string ConfirmPassword { get; set; }
    }
}
