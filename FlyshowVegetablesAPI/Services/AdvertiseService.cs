using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.Models.Request.Advertise;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyshowVegetablesAPI.Services
{
    public class AdvertiseService : IAdvertiseService
    {
        private IAdvertiseRepository _advertiseRepository;

        public AdvertiseService(IAdvertiseRepository advertiseRepository)
        {
            _advertiseRepository = advertiseRepository;
        }

        public List<Advertise> SearchAdvertises(AdvertiseCondition condition)
        {
            var advertise = _advertiseRepository.GetAll();

            if (condition.ID > 0)
            {
                advertise = advertise.Where(model => model.ID.Equals(condition.ID));
            }

            if (!string.IsNullOrEmpty(condition.ResourceUrl))
            {
                advertise = advertise.Where(model => model.ResourceUrl.Equals(condition.ResourceUrl));
            }

            if (condition.ShowInTimeData)
            {
                DateTime nowTime = DateTime.Now;
                advertise = advertise.Where(model => model.StartDate >= nowTime && model.EndDate <= nowTime);
            }

            return advertise.ToList();
        }

        public int CreateAdvertise(Advertise advertise)
        {
            return _advertiseRepository.CreateAdvertise(advertise);
        }

        public bool DeleteAdvertise(int ID)
        {
            return _advertiseRepository.DeleteAdvertise(ID);
        }
    }
}
