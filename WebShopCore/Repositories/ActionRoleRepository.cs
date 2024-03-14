using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Interfaces;
using WebShopCore.Model;

namespace WebShopCore.Repositories
{
    internal class ActionRoleRepository : GenericRepository<ActionRole>, IActionRoleRepository
    {
        public ActionRoleRepository(WebShopDbContext context) : base(context)
        {

        }
    }
}
