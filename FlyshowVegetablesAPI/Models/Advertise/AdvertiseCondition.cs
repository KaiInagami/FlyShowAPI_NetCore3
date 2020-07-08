using System;

namespace FlyshowVegetablesAPI.Models.Request.Advertise
{
    public class AdvertiseCondition
    {
        public int ID { get; set; }

        public string ResourceUrl { get; set; }

        public bool ShowInTimeData { get; set; }
    }
}
