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
    public enum Campus { Launceston, Hobart, All};
    public class Staff
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public Campus Campus { get; set; }
        public Category Category { get; set; }
        public string Phone { get; set; }
        public string Room { get; set; }
        public string Email { get; set; }
        public List<Consultation> WorkTime { get; set; }
        public List<Unit> Unit { get; set; }
        public List<Class> Class { get; set; }
        public string Image { get; set; }
        public List<ActivityGrid> ActivityGrid { get; set; }
        public string Availability
        {
            get
            {
                if (WorkTime != null || Class != null)
                {
                    // replace this datetime with the hardcode datetime when testing
                    DateTime now = DateTime.Now;

                    //Below is a hardcode DateTime for testing availbity function if needed
                    //DateTime now = new DateTime(2020, 10, 12, 13, 00, 0);

                    var ConsultationOverlapping = from Consultation work in WorkTime
                                                  where work.Overlaps(now)
                                                  select work;

                    var ClassOverlapping = from Class work in Class
                                           where work.Overlaps(now)
                                           select work;

                    if (ConsultationOverlapping.Count() > 0)
                    {
                        return "Consulting";
                    }
                    else if (ClassOverlapping.Count() > 0)
                    {
                        foreach (Class work in ClassOverlapping)
                        {
                            return "Teaching" + " " + work.UnitCode + " in " + work.Room;
                        }
                    }
                }
                return "Free";
            }
        }

        public override string ToString()
        {
            return FamilyName + ", " + GivenName + " (" + Title + ")";
        }
    }
}
