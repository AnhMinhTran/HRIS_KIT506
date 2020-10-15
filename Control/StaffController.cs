using HRIS_KIT506.Database;
using HRIS_KIT506.Teaching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS_KIT506.Control
{

    class StaffController
    {
        // Staff master list
        private List<Staff> Staff;

        public List<Staff> Workers { get { return Staff; } set { } }

        private ObservableCollection<Staff> ViewableStaff;
        public ObservableCollection<Staff> ViewableStaffDetail;
        public ObservableCollection<Staff> VisibleWorkers { get { return ViewableStaff; } set { } }
        public StaffController()
        {
            Staff = DbAdapter.LoadAllStaff();
            ViewableStaff = new ObservableCollection<Staff>(Staff);

            foreach (Staff e in Staff)
            {
                e.WorkTime = DbAdapter.LoadConsultationItems(e.ID);
                e.Unit = DbAdapter.LoadStaffUnit(e.ID);
                e.Class = DbAdapter.LoadStaffClass(e.ID);
                e.ActivityGrid = ActivityGridGenerate(e.WorkTime, e.Class);
            }
        }
        public ObservableCollection<Staff> GetViewableList()
        {
            return VisibleWorkers;
        }

        // for filter function in staff list view
        public void Filter(string category)
        {
            var selected = from Staff e in Staff
                           where category == "All" || e.Category.ToString() == category
                           select e;
            ViewableStaff.Clear();

            //Converts the result of the LINQ expression to a List and then calls viewableStaff.Add with each element of that list in turn
            selected.ToList().ForEach(ViewableStaff.Add);
        }

        // For Serach function in staff list view
        public void Search(string search)
        {
            var name = from Staff e in Staff
                       where search == "" || e.FamilyName.ToLower().Contains(search.ToLower()) || e.GivenName.ToLower().Contains(search.ToLower())
                       select e;

            ViewableStaff.Clear();
            name.ToList().ForEach(ViewableStaff.Add);
        }

        // For displaying staff detail when user click on staff id in class timetable
        public void DisplayStaffDetail(int id)
        {
            ViewableStaffDetail = new ObservableCollection<Staff>(Staff);

            var selected = from Staff e in Staff
                       where id == e.ID
                       select e;

            ViewableStaffDetail.Clear();
            selected.ToList().ForEach(ViewableStaffDetail.Add);
        }

        // For loading rowdata in the staff activity grid
        public List<ActivityGrid> ActivityGridGenerate(List<Consultation> consultations, List<Class> classes)
        {
            List<ActivityGrid> Rowdata = new List<ActivityGrid>();

            // The datetime must be a Monday at 09:00 am
            // there should be a better way to define the datetime instead of a specific date
            DateTime datetime = new DateTime(2020, 10, 12, 9, 00, 0);

            // LINQ to select the overlaps of class and consultation with a specific datetime
            var ConsultationOverlapping = from Consultation work in consultations
                                          where work.Overlaps(datetime)
                                          select work;

            var ClassOverlapping = from Class work in classes
                                   where work.Overlaps(datetime)
                                   select work;

            String[] day = { "white", "white", "white", "white", "white" };

            // a for loop to set color in each row in the activity grid
            for (int hour = 9; hour < 18; hour++)
            {
                for (int Day = 0; Day < 5; Day++)
                {
                    if (ConsultationOverlapping.Count() > 0)
                    {
                        day[Day] = "green";
                    }
                    else if (ClassOverlapping.Count() > 0)
                    { 
                        day[Day] = "blue";
                    }
                    datetime = datetime.AddDays(1);
                }

                Rowdata.Add(new ActivityGrid 
                { 
                    Time = hour.ToString() + " - " + (hour+1).ToString(), Mon = day[0], Tue = day[1], Wed = day[2], Thu = day[3], Fri = day[4] 
                });

                for (int i = 0; i < 5; i++)
                {
                    day[i] = "white";
                }

                datetime = datetime.AddDays(-5);
                datetime = datetime.AddHours(1);
                }

            return Rowdata;
        }
    }
}
