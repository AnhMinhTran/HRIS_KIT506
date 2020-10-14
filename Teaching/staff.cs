using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HRIS_KIT506.Teaching
{
    public enum Category { Academic, Casual, Admin, Technical, All};
    public enum Campus { Launceston, Hobart};
    public class Staff
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public Campus Campus { get; set; }
        public Category Category { get; set; }
        public string Phone { get; set; }
        public string Room { get; set; }
        public string Email { get; set; }
        public List<Consultation> Consultations { get; set; }
        public List<Class> Class { get; set; }
        public string Image { get; set; }
        public bool BusyNow
        {
            get
            {
                if (Consultations != null)
                {
                    DateTime now = DateTime.Now;
                    var overlapping = from Consultation work in Consultations
                                      where work.Overlaps(now)
                                      select work;
                    return overlapping.Count() > 0;

                    //which could be rewritten as a single expression:
                    //return (from RosterItem work in WorkTimes
                    //        where work.Overlaps(now)
                    //        select work).Count() > 0;
                }
                return false;
            }
        }

        public override string ToString()
        {
            //For the purposes of this week's demonstration this returns only the name
            return Name;
        }
    }
}
