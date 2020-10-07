using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRIS_KIT506
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string STAFF_LIST_KEY = "StaffList";
        private const string UNIT_LIST_KEY = "UnitList";
        private StaffController StaffController;
        private UnitController UnitController;
        public MainWindow()
        {
            InitializeComponent();
            StaffController = (StaffController)(Application.Current.FindResource(STAFF_LIST_KEY) as ObjectDataProvider).ObjectInstance;
            UnitController = (UnitController)(Application.Current.FindResource(UNIT_LIST_KEY) as ObjectDataProvider).ObjectInstance;
        }

        private void unitListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                unitDetailsPanel.DataContext = e.AddedItems[0];
            }
        }

        private void sampleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
               
                staffDetailsPanel.DataContext = e.AddedItems[0];
            }
        }

        public string SelectedCategory { get => SelectedCategory; set => SelectedCategory = value; }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count >0)
            {
              
                MessageBox.Show("Dropdown selected " + e.AddedItems[0]);
                var selected = e.AddedItems[0].ToString();
                foreach (Staff staff in StaffController.Workers)
                {
                    if (staff.Category.ToString().Equals(selected))
                    {
                        sampleListBox.Items.Add(staff);
                    }
                }

            }
        }
    }
}
