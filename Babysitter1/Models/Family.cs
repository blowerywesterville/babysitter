using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Babysitter1.Models
{
    public class Family
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string HomePhone { get; set; }
        public string ContactPhone { get; set; }
        [Display(Name="Email")]
        public string ContactEmail { get; set; }
        public string Bedtime { get; set; }
        public bool PartialPay { get; set; }
        public bool Cash { get; set; }
        public virtual List<Kid> Kids { get; set; }
    }
}
