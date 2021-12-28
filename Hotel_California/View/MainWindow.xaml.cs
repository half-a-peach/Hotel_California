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

namespace Hotel_California
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //связываем представление и модель через ViewModel
            DataContext = new ApplicationViewModel(this);
        }
    }
}
