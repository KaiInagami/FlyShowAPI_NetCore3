using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Interfaces
{
    public interface ISpecialContractStoreRepository
    {
        IQueryable<SpecialContractStore> GetAll();

        bool DeleteByID(int ID);

        int CreateDatas(SpecialContractStore model);
    }
}
