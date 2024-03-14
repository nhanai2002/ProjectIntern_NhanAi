using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Interfaces;
using WebShopCore.Model;

namespace WebShopCore.Repositories
{
    public class CartHistoryRepository : GenericRepository<CartHistory>, ICartHistoryRepository
    {
        public CartHistoryRepository(WebShopDbContext context) : base(context)
        {

        }
    }
}
