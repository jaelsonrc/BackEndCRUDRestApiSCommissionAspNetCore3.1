using Microsoft.EntityFrameworkCore;
using ServiceCommission.Repositories.Interfaces;
using ServiceCommission.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCommission.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected RepositoryContext _context { get; set; }
        public Repository(RepositoryContext context)
        {
            _context = context;
        }
        public IQueryable<T> FindAll()
        {
            return _context.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
                .Where(expression).AsNoTracking();
        }
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IQueryable<T> List(IQueryParameters parameters)
        {
            var query = FindAll();
            var queryParam = ApplyParams(query, parameters);
            return queryParam;
        }

        protected virtual IQueryable<T> ApplyParams(IQueryable<T> query, IQueryParameters parameters)
        {
            ApplySort(ref query, parameters.OrderBy);
            var queryParam = query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                             .Take(parameters.PageSize);
            return queryParam;
        }

        private void ApplySort(ref IQueryable<T> queryLinq, string orderByQueryString)
        {

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return;
            }

            if (!queryLinq.Any())
                return;

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? 1 : 0;

                if (sortingOrder == 0) queryLinq = queryLinq.OrderBy(s => s.GetType().GetProperty(objectProperty.Name.ToString()).GetValue(s));
                else queryLinq = queryLinq.OrderByDescending(s => s.GetType().GetProperty(objectProperty.Name.ToString()).GetValue(s));
            }


        }


    }
}
