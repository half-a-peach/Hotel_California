using System;
using System.Collections.Generic;
using Hotel_California.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hotel_California.View
{
    /// <summary>
    /// Логика взаимодействия для ReservationWindow.xaml
    /// </summary>
    public partial class ReservationWindow : UserControl
    {
        public ReservationWindow(ApplicationViewModel appViewModel)
        {
            InitializeComponent();
            DataContext = appViewModel;
        }
    }
}
