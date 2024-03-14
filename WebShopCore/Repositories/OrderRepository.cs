using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Interfaces;
using WebShopCore.Model;

namespace WebShopCore.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(WebShopDbContext context) : base(context)
        {

        }
        public Order GetById(long id)
        {
            var data = _context.orders.Find(id);
            return data;
        }
    }
}
