using ServiceCommission.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Repositories.Interfaces
{
    public interface ICommissionRepository : IRepository<Commission>
    {
        Commission GetByOrder(int order);
        IQueryable<Commission> FindByDescriptionOrOrderOrSituation(string description,int order, int situation);
        bool IsOrder(int orderRandom);
    }
}
