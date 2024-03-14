using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.Repositories;

namespace WebShopCore.Interfaces
{
    public interface ICartHistoryRepository : IRepository<CartHistory>
    {
    }
}
