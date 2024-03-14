using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Model;

namespace WebShopCore.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasData(new Role
            {
                RoleId = (int)SysEnum.DefaultRole.Admin,
                Name = SysEnum.DefaultRole.Admin.ToString()
            });
            builder.HasData(new Role
            {
                RoleId = (int)SysEnum.DefaultRole.EndUser,
                Name = SysEnum.DefaultRole.EndUser.ToString()
            });
        }
    }
}
