using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebShopCore.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly WebShopDbContext _context;
        public GenericRepository(WebShopDbContext context)
        {
            _context = context;
        }

        public virtual void Add(T e)
        {
            _context.Set<T>().Add(e);
        }

        public virtual async Task AddAsync(T e)
        {
            await _context.Set<T>().AddAsync(e);
        }

        public virtual IQueryable<T> BuildQuery(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> set = _context.Set<T>();
            return set.Where(predicate);
        }

        public virtual void Delete(T e)
        {
            _context.Set<T>().Remove(e);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> set = _context.Set<T>();
            return set.FirstOrDefault(predicate);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public virtual void Update(T e)
        {
            _context.Set<T>().Update(e);
        }

        public virtual T GetById(int id)
        {
            var data = _context.Set<T>().Find(id);
            return data;
        }
        public virtual void DeleteById(int id)
        {
            var data = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(data);
        }
    }
}
