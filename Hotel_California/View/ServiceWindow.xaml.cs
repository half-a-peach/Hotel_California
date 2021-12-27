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
using Hotel_California.ViewModel;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hotel_California.View
{
    public partial class ServiceWindow : UserControl
    {
        
        public ServiceWindow(ApplicationViewModel appViewModel)
        {
            InitializeComponent();
            DataContext = appViewModel;
           
        }

    }
}
