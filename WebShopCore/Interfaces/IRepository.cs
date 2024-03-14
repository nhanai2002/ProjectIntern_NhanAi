using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;

namespace WebShopCore.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T e);
        Task AddAsync(T e);
        void Update(T e);
        void Delete(T e);
        T GetById(int id);
        void DeleteById(int id);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        IQueryable<T> BuildQuery(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();

    }
}
