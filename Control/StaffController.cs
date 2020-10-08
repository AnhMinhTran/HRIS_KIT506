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

        private List<Staff> Staff;
        public List<Staff> Workers { get { return Staff; } set { } }

        private ObservableCollection<Staff> ViewableStaff;
        public ObservableCollection<Staff> VisibleWorkers { get { return ViewableStaff; } set { } }
        public StaffController()
        {
            Staff = DbAdapter.LoadAllStaff();
            ViewableStaff = new ObservableCollection<Staff>(Staff);

            foreach (Staff e in Staff)
            {
                e.WorkTime = DbAdapter.LoadConsultationItems(e.ID);
                e.Class = DbAdapter.LoadStaffClasses(e.ID);
            }
        }
        public ObservableCollection<Staff> GetViewableList()
        {
            return VisibleWorkers;
        }

        public void Filter(Category category)
        {
            var selected = from Staff e in Staff
                           where category == Category.All || e.Category == category
                           select e;
            ViewableStaff.Clear();
            //Converts the result of the LINQ expression to a List and then calls viewableStaff.Add with each element of that list in turn
            selected.ToList().ForEach(ViewableStaff.Add);
        }
    }
}
