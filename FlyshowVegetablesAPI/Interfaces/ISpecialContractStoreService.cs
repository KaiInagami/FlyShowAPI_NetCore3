using Infrastructure.Entities;
using System.Collections.Generic;

namespace FlyshowVegetablesAPI.Interfaces
{
    public interface ISpecialContractStoreService
    {
        List<SpecialContractStore> GetDatas();

        bool DeleteData(int ID);

        int CreateDatas(SpecialContractStore model);
    }
}
