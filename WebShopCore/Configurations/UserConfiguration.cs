using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Model;

namespace WebShopCore.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.ToTable("User");
            modelBuilder.Property(u => u.CartId).IsRequired(false); // Cho phép null

            modelBuilder.HasData(new User
            {
                UserId = 1,
                UserName ="admin1",
                Email = "mailtouestest1@gmail.com".ToLower(),
                Address = "123/456",
                CreatedAt = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddTicks(0),
                UpdatedAt = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddTicks(0),
                IsActive = true,
                Avatar = "",
                CartId = null,
                ConfirmEmail = true,
                IsDeleted = false,
                RoleId = (int)SysEnum.DefaultRole.Admin,
                Name = "Admin 1",
                Password = "123456".HasPassword(),
                Phone = "0123456789",
            });
            modelBuilder.HasData(new User
            {
                UserId = 2,
                UserName = "admin2",
                Email = "mailtouestest1@gmail.com".ToLower(),
                Address = "123/456",
                CreatedAt = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddTicks(0),
                UpdatedAt = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddTicks(0),
                IsActive = true,
                Avatar = "",
                CartId = null,
                ConfirmEmail = true,
                IsDeleted = false,
                RoleId = (int)SysEnum.DefaultRole.Admin,
                Name = "Admin 2",
                Password = "123456".HasPassword(),
                Phone = "0123456789",
            });

        }
    }
}
