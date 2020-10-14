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
                e.ActivityGrid = ActivityGridGenerate(e.WorkTime);
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
        public List<ActivityGrid> ActivityGridGenerate(List<Consultation> consultations)
        {
            List<ActivityGrid> Rowdata = new List<ActivityGrid>();

            DateTime datetime = new DateTime(2020, 10, 12, 9, 00, 0);


            var ConsultationOverlapping = from Consultation work in consultations
                                          where work.Overlaps(datetime)
                                          select work;

            String[] day = { "white", "white", "white", "white", "white", "white" };


            for (int hour = 9; hour < 18; hour++)
            {
                for (int Day = 1; Day < 6; Day++)
                {
                    if (ConsultationOverlapping.Count() > 0)
                        day[Day] = "green";
                    datetime = datetime.AddDays(1);
                }

                Rowdata.Add(new ActivityGrid 
                { 
                    Time = hour.ToString() + " - " + (hour+1).ToString(), Mon = day[1], Tue = day[2], Wed = day[3], Thu = day[4], Fri = day[5] 
                });
                day[1] = "white";
                day[2] = "white";
                day[3] = "white";
                day[4] = "white";
                day[5] = "white";
                datetime = datetime.AddDays(-5);
                datetime = datetime.AddHours(1);
                }

            return Rowdata;
        }
    }
}
