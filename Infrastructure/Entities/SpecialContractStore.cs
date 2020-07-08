using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class SpecialContractStore
    {
        [Required]
        public int ID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }
    }
}
