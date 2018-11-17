using System;

namespace Babysitter1.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string FamilyName { get; set; }
        public DateTime JobDate { get; set; }
        public int TotalHours { get; set; }
        public int EarlyHours { get; set; }
        public int EveningHours { get; set; }
        public int LateHours { get; set; }
        public decimal Amount { get; set; }
    }
}
