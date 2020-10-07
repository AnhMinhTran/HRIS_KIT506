using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS_KIT506.Teaching
{
    public enum Type { Lecture, Practical, Tutorial, Workshop };
    public class Class
    {
        public string UnitCode { get; set; }
        public Campus Campus { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public Type Type { get; set; }
        public string Room { get; set; }
        public int StaffID { get; set; }

        public override string ToString()
        {
            return UnitCode;
        }
    }
}
