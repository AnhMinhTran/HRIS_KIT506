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
        private List<Unit> Unit;
        public List<Unit> Courses { get { return Unit; } set { } }

        private ObservableCollection<Unit> ViewableUnit;
        public ObservableCollection<Unit> VisibleCourses { get { return ViewableUnit; } set { } }
        public UnitController()
        {
            Unit = DbAdapter.LoadAllUnit();
            ViewableUnit = new ObservableCollection<Unit>(Unit); //this list we will modify later

            //Part of step 2.3.2 from Week 9 tutorial
            foreach (Unit e in Unit)
            {
                e.ClassList = DbAdapter.LoadClasses(e.Code);
            }
        }

        public ObservableCollection<Unit> GetViewableList()
        {
            return VisibleCourses;
        }
    }
}
