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

namespace Generator
{
    /// <summary>
    /// Interaction logic for step_by_stepxaml.xaml
    /// </summary>
    public partial class step_by_stepxaml : Window
    {
        public step_by_stepxaml()
        {
            InitializeComponent();
        }

        private void clear(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        private void start(object sender, RoutedEventArgs e)
        {
            start1.Visibility = Visibility.Hidden;
            stop1.Visibility = Visibility.Visible;
            next1.Visibility = Visibility.Visible;
        }

        private void next(object sender, RoutedEventArgs e)
        {
            start1.Visibility = Visibility.Hidden;
            stop1.Visibility = Visibility.Visible;
            next1.Visibility = Visibility.Visible;
        }

        private void stop(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
