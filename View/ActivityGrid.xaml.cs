﻿using HRIS_KIT506.Control;
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
using System.Windows.Shapes;

namespace HRIS_KIT506.View
{
    /// <summary>
    /// Interaction logic for ActivityGrid.xaml
    /// </summary>
    public partial class ActivityGrid : Window
    {
        public ActivityGrid()
        {
            InitializeComponent();
            StaffController controller = new StaffController();
        }
    }
}
