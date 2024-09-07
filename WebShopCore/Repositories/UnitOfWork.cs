using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Interfaces;

namespace WebShopCore.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebShopDbContext _context;

        private IUserRepository _UserRepository;
        private IRoleRepository _RoleRepository;
        private IActionCtrlRepository _ActionCtrlRepository;
        private IActionRoleRepository _ActionRoleRepository;
        private IProductRepository _ProductRepository;
        private IProductCategoryRepository _ProductCategoryRepository;
        private ICategoryRepository _CategoryRepository;
        private IImageRepository _ImageRepository;
        private IProductImageRepository _ProductImageRepository;
        private ICouponRepository _CouponRepository;
        private ICouponHistoryRepository _CouponHistoryRepository;
        private ICartRepository _CartRepository;
        private ICartItemRepository _CartItemRepository;
        private ICartHistoryRepository _CartHistoryRepository;
        private IOrderRepository _OrderRepository;
        private IOrderItemRepository _OrderItemRepository;
        private IFeedbackRepository _FeedbackRepository;
        private IProductFeedBackRepository _ProductFeedBackRepository;
        private IHubConnectionRepository _HubConnectionRepository;
        private INotificationRepository _NotificationRepository;
        private IUserNotiRepository _UserNotiRepository;

        public UnitOfWork(WebShopDbContext context)
        {
            _context = context;
        }
        public IUserRepository UserRepository => _UserRepository ??= new UserRepository(_context);
        public IRoleRepository RoleRepository => _RoleRepository ??= new RoleRepository(_context);
        public IActionCtrlRepository ActionCtrlRepository => _ActionCtrlRepository ??= new ActionCtrlRepository(_context);
        public IActionRoleRepository ActionRoleRepository => _ActionRoleRepository ??= new ActionRoleRepository(_context);
        public IProductRepository ProductRepository => _ProductRepository ??= new ProductRepository(_context);
        public IProductCategoryRepository ProductCategoryRepository => _ProductCategoryRepository ??= new ProductCategoryRepository(_context);
        public ICategoryRepository CategoryRepository => _CategoryRepository ??= new CategoryRepository(_context);
        public IImageRepository ImageRepository => _ImageRepository ??= new ImageRepository(_context);
        public IProductImageRepository ProductImageRepository => _ProductImageRepository ??= new ProductImageRepository(_context);
        public ICouponRepository CouponRepository => _CouponRepository ??= new CouponRepository(_context);
        public ICouponHistoryRepository CouponHistoryRepository => _CouponHistoryRepository ??= new CouponHistoryRepository(_context);
        public ICartRepository CartRepository => _CartRepository ??= new CartRepository(_context);
        public ICartItemRepository CartItemRepository => _CartItemRepository ??= new CartItemRepository(_context);
        public ICartHistoryRepository CartHistoryRepository => _CartHistoryRepository ??= new CartHistoryRepository(_context);
        public IOrderRepository OrderRepository => _OrderRepository ??= new OrderRepository(_context);
        public IOrderItemRepository OrderItemRepository => _OrderItemRepository ??= new OrderItemRepository(_context);
        public IFeedbackRepository FeedbackRepository => _FeedbackRepository ??= new FeedbackRepository(_context);
        public IProductFeedBackRepository ProductFeedBackRepository => _ProductFeedBackRepository ??= new ProductFeedBackRepository(_context);
        public IHubConnectionRepository HubConnectionRepository => _HubConnectionRepository ??= new HubConnectionRepository(_context);
        public INotificationRepository NotificationRepository => _NotificationRepository ??= new NotificationRepository(_context);
        public IUserNotiRepository UserNotiRepository => _UserNotiRepository ??= new UserNotiRepository(_context);

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }
        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }
        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }
        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
