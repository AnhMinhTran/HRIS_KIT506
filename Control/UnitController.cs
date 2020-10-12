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
    class UnitController
    {
        // Unit master list
        private List<Unit> Unit;
        public List<Unit> Courses { get { return Unit; } set { } }

        private ObservableCollection<Unit> ViewableUnit;

        public ObservableCollection<Class> ViewableClass;
        public ObservableCollection<Class> ViewableFilteredClass;
        public ObservableCollection<Unit> VisibleCourses { get { return ViewableUnit; } set { } }
        public UnitController()
        {
            Unit = DbAdapter.LoadAllUnit();
            ViewableUnit = new ObservableCollection<Unit>(Unit); 

            foreach (Unit e in Unit)
            {
                e.Class = DbAdapter.LoadClasses(e.Code);
            }
        }

        public ObservableCollection<Unit> GetViewableList()
        {
            return VisibleCourses;
        }

        // For Serach function in unit list view
        public void Search(string search)
        {
            var unit = from Unit e in Unit
                       where search == "" || e.Title.ToLower().Contains(search.ToLower()) || e.Code.ToLower().Contains(search.ToLower())
                       select e;

            ViewableUnit.Clear();
            unit.ToList().ForEach(ViewableUnit.Add);
        }

        // For display class timetable when user click on teaching class in class timetable
        public void DisplayClass(string unitCode)
        {
            ViewableClass = new ObservableCollection<Class>(DbAdapter.LoadClasses(unitCode));
        }

        // For filter function in class timetable
        public void Filter(string campus)
        {
            var selected = from Class e in ViewableClass
                           where campus == "All" || e.Campus.ToString() == campus
                           select e;

            ViewableFilteredClass = new ObservableCollection<Class>(selected);
        }
    }
}
