using ServiceCommission.Enums;
using ServiceCommission.Models;
using ServiceCommission.Repositories.Interfaces;
using ServiceCommission.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Repositories
{
    public class CommissionRepository : Repository<Commission>, ICommissionRepository
    {

        public CommissionRepository(RepositoryContext context):base(context)
        {
                
        }

        public IQueryable<Commission> FindByDescriptionOrOrderOrSituation(string description, int order, int situation)
        {
            var query = FindAll();
            if (!string.IsNullOrEmpty(description))
                query = query.Where(w => w.Description.Contains(description));

            if (order > 999)
                query = query.Where(w => w.Order == order);

            if(situation > 0)
                query = query.Where(w => w.Situation == (SituationEnum)situation);

            return query.OrderByDescending(o => o.UpdateAt).Take(100);
        }

        public Commission GetByOrder(int order)
        {
            return FindAll().FirstOrDefault(w => w.Order == order);
        }

        public bool IsOrder(int orderRandom)
        {
            return FindAll().Count(w => w.Order == orderRandom) > 0;
        }

        public override IQueryable<Commission> List(IQueryParameters parameters)
        {
            return base.List(parameters).OrderByDescending(o => o.UpdateAt);
        }
    }

}
