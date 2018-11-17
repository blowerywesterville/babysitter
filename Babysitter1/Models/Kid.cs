using System.ComponentModel.DataAnnotations;

namespace Babysitter1.Models
{
    public class Kid
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bedtime { get; set; }
        public int? Age { get; set; }
        public string Note { get; set; }
        public int? FamilyId { get; set; }
        public virtual Family Family { get; set; }
    }
}
