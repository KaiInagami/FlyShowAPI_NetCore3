using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class User
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? Gender { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        public DateTime? Birthday { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public int? Priority { get; set; }

        [NotMapped]
        public string Actor { get; set; }

        [NotMapped]
        public string Sex { get; set; }
    }
}
