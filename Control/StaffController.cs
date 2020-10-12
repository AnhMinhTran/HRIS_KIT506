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

    }
}
