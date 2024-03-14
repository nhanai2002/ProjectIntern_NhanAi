using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{

    public class BaseUser : BaseClass
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(50, ErrorMessage = "Tài khoản không vượt quá 50 kí tự")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(50, ErrorMessage = "Mật khẩu không vượt quá 50 kí tự")]
        public string Password { get; set; }
        public Guid UserGuid { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên không dài quá 100 kí tự")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Email { get; set; }
        public string? TokenEmail { get; set; }
        public bool ConfirmEmail { get; set; } = false;
        public string? Avatar { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(20)]
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? JoinedAt { get; set; }

        public int RoleId { get; set; }
        public int? CartId { get; set; }
    }

    public class User : BaseUser
    {
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }
    }
}
