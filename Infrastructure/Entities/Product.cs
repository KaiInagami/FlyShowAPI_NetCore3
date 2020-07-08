using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Product
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string SerialNo { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Image { get; set; }

        public int? Price { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        public int? Inventory { get; set; }

        public string Remark { get; set; }

        [StringLength(50)]
        public string Area { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? IsInStock { get; set; }

        public int? Type { get; set; }

        public string YouTubeUrl { get; set; }
    }
}
