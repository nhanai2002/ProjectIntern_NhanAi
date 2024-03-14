using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Interfaces;
using WebShopCore.Model;

namespace WebShopCore.Repositories
{
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(WebShopDbContext context) : base(context)
        {
        }
    }
}
