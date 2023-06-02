using Microsoft.Build.Framework;


namespace aspnet02_boardapp.Models
{
    public class CreateRoleModel
    {
        [Required]

        public string RoleName { get; set; }  // Admin, User, Manager
    }
}
