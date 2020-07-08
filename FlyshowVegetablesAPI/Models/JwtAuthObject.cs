using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyshowVegetablesAPI.Models
{
    public class JwtAuthObject
    {
        public string Account { get; set; }
        public string CurrentTime { get; set; }
        public string ExpireTime { get; set; }
    }
}
