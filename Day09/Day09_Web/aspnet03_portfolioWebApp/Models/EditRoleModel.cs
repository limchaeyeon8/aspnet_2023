using Microsoft.Build.Framework;
using System.ComponentModel;

namespace aspnet02_boardapp.Models
{
    public class EditRoleModel
    {
        [DisplayName("권한아이디")]
        public string Id { get; set; }

        [DisplayName("권한 이름")]
        [Required]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }

        public EditRoleModel() 
        {
            Users = new List<string>();
        }
    }
}
