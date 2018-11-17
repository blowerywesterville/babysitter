using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Babysitter1.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        public int? FamilyId { get; set; }
        public DateTime JobDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool Paid { get; set; }
        public virtual List<Kid> Kids { get; set; }
        public virtual Family Family { get; set; }
    }
}
/*
string startTime = "7:00 AM";
string endTime = "2:00 PM";

string startTime = "7:00";
string endTime = "14:00";

TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));

Console.WriteLine(duration);
Console.ReadKey();
Outputs: 07:00:00.

DateTime startTime = Convert.ToDateTime(textBox1.Text);
DateTime endtime = Convert.ToDateTime(textBox2.Text);

TimeSpan duration = startTime - endtime;
*/