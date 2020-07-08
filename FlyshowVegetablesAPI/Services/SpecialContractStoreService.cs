using FlyshowVegetablesAPI.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FlyshowVegetablesAPI.Services
{
    public class SpecialContractStoreService : ISpecialContractStoreService
    {
        private ISpecialContractStoreRepository _specialContractStoreRepository;

        public SpecialContractStoreService(ISpecialContractStoreRepository specialContractStoreRepository)
        {
            _specialContractStoreRepository = specialContractStoreRepository;
        }

        /// <summary>
        /// Get All SpecialContractStore
        /// </summary>
        /// <returns></returns>
        public List<SpecialContractStore> GetDatas()
        {
           return _specialContractStoreRepository.GetAll().ToList();
        }

        /// <summary>
        /// Create SpecialContractStore Datas
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateDatas(SpecialContractStore model)
        {
            return _specialContractStoreRepository.CreateDatas(model);
        }

        /// <summary>
        /// Delete SpecialContractStore By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteData(int ID)
        {
            return _specialContractStoreRepository.DeleteByID(ID);
        }
    }
}
