using FlyshowVegetablesAPI.Models.Request.Advertise;
using Infrastructure.Entities;
using System.Collections.Generic;

namespace FlyshowVegetablesAPI.Interfaces
{
    public interface IAdvertiseService
    {
        List<Advertise> SearchAdvertises(AdvertiseCondition condition);

        int CreateAdvertise(Advertise advertise);

        bool DeleteAdvertise(int ID);
    }
}
