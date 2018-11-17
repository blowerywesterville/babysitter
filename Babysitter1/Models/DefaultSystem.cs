using System;

namespace Babysitter1.Models
{
    public class DefaultSystem
    {
        public int Id { get; set; }
        public string MinTime { get; set; }
        public string MaxTime { get; set; }
        public decimal Standard { get; set; }
        public decimal Evening { get; set; }
        public decimal LateNite { get; set; }
    }
}
