using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Repositories;

namespace WebShopCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();
        void RollBack();
        void BeginTransaction();
        void CommitTransaction();

        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IActionCtrlRepository ActionCtrlRepository { get; }
        IActionRoleRepository ActionRoleRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IImageRepository ImageRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        ICouponRepository CouponRepository { get; }
        ICouponHistoryRepository CouponHistoryRepository { get; }
        ICartRepository CartRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        ICartHistoryRepository CartHistoryRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
    }
}
