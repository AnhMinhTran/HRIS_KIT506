using HRIS_KIT506.Control;
using HRIS_KIT506.Teaching;
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
    public partial class MainWindow : Window
    {
        private const string STAFF_LIST_KEY = "StaffList";
        private const string UNIT_LIST_KEY = "UnitList";
        private Control.StaffController StaffController;
        private Control.UnitController UnitController;
        public MainWindow()
        {
            InitializeComponent();
            StaffController = (Control.StaffController)(Application.Current.FindResource(STAFF_LIST_KEY) as ObjectDataProvider).ObjectInstance;
            UnitController = (Control.UnitController)(Application.Current.FindResource(UNIT_LIST_KEY) as ObjectDataProvider).ObjectInstance;
        }

        // function for displaying details of the selected staff in staff list view
        private void StaffListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                StaffDetailsPanel.DataContext = e.AddedItems[0];
            }
        }

        // fucntion for searching staff by name in staff list view
        private void NameSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var search = NameSearch.Text.ToString();
                StaffController.Search(search);
            }
        }

        // fucntion for filtering staff by category in staff list view
        private void StaffComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selected = e.AddedItems[0].ToString();
                StaffController.Filter(selected);
            }
        }

        // function for selecting staff teaching unit in staff detail view
        private void TeachingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                // bring user to staff tab page
                TabControl.SelectedIndex = 1;

                // cancel previous selected unit to avoid user distraction
                unitListBox.SelectedItem = null;

                // display unit code in unit detail header
                unitDetailHeader.DataContext = e.AddedItems[0];

                // get class detail of selected unit
                Unit selectedUnit = TeachingListBox.SelectedItem as Unit;
                string selectedUnitCode = selectedUnit.Code;
                UnitController.DisplayClass(selectedUnitCode);
                
                // update class datagrid
                classDataGrid.ItemsSource = UnitController.ViewableClass;

                TeachingListBox.SelectedItem = null;
                ClassComboBox.SelectedItem = null;
            }
        }

        // function for displaying class timetable of the selected unit in unit list view
        private void unitListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                // display unit code in unit detail header
                unitDetailHeader.DataContext = e.AddedItems[0];

                // send unitcode to the DisplayClass function in unit controller
                Unit selectedUnit = unitListBox.SelectedItem as Unit;
                string selectedUnitCode = selectedUnit.Code;
                UnitController.DisplayClass(selectedUnitCode);

                // update class datagrid
                classDataGrid.ItemsSource = UnitController.ViewableClass;

                ClassComboBox.SelectedItem = null;
            }
        }

        // fucntion for searching unit by code/name in staff list view
        private void UnitSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var search = UnitSearch.Text.ToString();
                UnitController.Search(search);
            }
        }

        // fucntion for filtering class by campus in class timetable
        private void ClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (classDataGrid.Items.Count > 0)
            {
                if (e.AddedItems.Count > 0)
                {
                    var selected = e.AddedItems[0].ToString();
                    UnitController.Filter(selected);

                    // update timetable
                    classDataGrid.ItemsSource = UnitController.ViewableFilteredClass;
                }
            }
        }

        // function for displaying details of the selected staff in class timetable view
        private void Staff_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            // bring user to unit tab page
            TabControl.SelectedIndex = 0;

            // send staff id to the DisplayStaffDetail function in staff controller
            TextBlock textblock = (TextBlock)sender;
            int id = Int32.Parse(textblock.Tag.ToString());

            StaffController.DisplayStaffDetail(id);

            // update staffdetailview with the selected staff detail
            StaffDetailsPanel.DataContext = StaffController.ViewableStaffDetail;

        }

    }

}
