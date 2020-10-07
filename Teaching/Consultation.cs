using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS_KIT506.Teaching 
{
    public class Consultation
    {
        public int StaffID { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public bool Overlaps(DateTime sometime)
        {
            //TimeSpan objects overload the comparison operators, so we can treat them as if they were plain numbers in the checks below
            //A longer version of the second test would be sometime.TimeOfDay.CompareTo(Start) >= 0
            return sometime.DayOfWeek == Day &&
                sometime.TimeOfDay >= Start &&
                sometime.TimeOfDay < End;
        }

        public override string ToString()
        {
            return Day + " " + Start + "--" + End;

            //This alternative uses the Format method of string, with the
            //format hh:mm to eliminate the seconds component from the time
            //return string.Format("{0} {1:hh':'mm}--{2:hh':'mm}", Day, Start, End);
        }
    }
}
