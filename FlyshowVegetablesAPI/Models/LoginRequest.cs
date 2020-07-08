using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlyshowVegetablesAPI.Models.Request
{
    public class LoginRequest
    {
        /// <summary>
        /// Login User's Account 
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// Login User's Password
        /// </summary>
        [Required]
        [StringLength(16, ErrorMessage = "Password Length Invalid")]
        public string Password { get; set; }
    }
}
