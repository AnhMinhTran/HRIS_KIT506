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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

        private void StaffListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                StaffDetailsPanel.DataContext = e.AddedItems[0];
            }
        }

        private void NameSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var search = NameSearch.Text.ToString();
                StaffController.Search(search);
            }
        }

        private void StaffComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selected = e.AddedItems[0].ToString();
                StaffController.Filter(selected);

            }
        }

        private void TeachingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                TabControl.SelectedIndex = 1;

                // display unit code in unit detail header
                unitDetailHeader.DataContext = e.AddedItems[0];

                // get class detail of selected unit
                Unit selectedUnit = TeachingListBox.SelectedItem as Unit;
                string selectedUnitCode = selectedUnit.Code;
                UnitController.DisplayClass(selectedUnitCode);
                
                // update class datagrid
                classDataGrid.ItemsSource = UnitController.ViewableClass;

            }
        }

        private void unitListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count > 0)
            {
                // display unit code in unit detail header
                unitDetailHeader.DataContext = e.AddedItems[0];

                // get class detail of selected unit
                Unit selectedUnit = unitListBox.SelectedItem as Unit;
                string selectedUnitCode = selectedUnit.Code;
                UnitController.DisplayClass(selectedUnitCode);

                // update class datagrid
                classDataGrid.ItemsSource = UnitController.ViewableClass;
            }
        }

        private void Staff_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 0;

            TextBlock textblock = (TextBlock)sender;
            int id = Int32.Parse(textblock.Tag.ToString());

            StaffController.DisplayStaffDetail(id);

            StaffDetailsPanel.DataContext = StaffController.ViewableStaffDetail;

        }

    }
}
