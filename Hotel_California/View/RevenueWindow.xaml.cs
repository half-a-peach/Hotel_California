using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hotel_California.ViewModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Hotel_California.View
{
    /// <summary>
    /// Логика взаимодействия для RevenueWindow.xaml
    /// </summary>
    public partial class RevenueWindow : UserControl
    {
        public RevenueWindow(ApplicationViewModel appViewModel)
        {
            InitializeComponent();
            DataContext = appViewModel;
        }
    }
}
