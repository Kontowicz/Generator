using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Pattern = "[^0-9x\\+]";
        private const string Pattern1 = "[^0-1]";

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool checkPoly(string text)
        {
            string test = Regex.Replace(text, Pattern, "");
            if(test == text)
                return true;

            return false;
        }

        private bool checkInit(string text)
        {
            string test = Regex.Replace(text, Pattern1, "");
            if (test == text)
                return true;

            return false;
        }

        private bool checkNumer(string text)
        {
            string test = Regex.Replace(text, "[^0-9]", "");
            if (test == text && text[0] != '0')
                return true;

            return false;
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            if (checkPoly(poly1.Text) == true &&
                checkPoly(poly2.Text) == true &&
                checkPoly(poly3.Text) == true &&
                checkInit(init1.Text) == true &&
                checkInit(init2.Text) == true &&
                checkInit(init3.Text) == true &&
                checkNumer(len.Text))
            {
                lfsr _1 = new lfsr(poly1.Text, init1.Text);
                lfsr _2 = new lfsr(poly2.Text, init2.Text);
                lfsr _3 = new lfsr(poly3.Text, init3.Text);
                geffe ge = new geffe(_1, _2, _3);
                long tmp = long.Parse(len.Text);
                string to_return = "";
                for(long i = 0; i<tmp; ++i)
                {
                    to_return += ge.next().ToString();
                }
                result.Text = to_return;
            }
            else
            {
                var resut = MessageBox.Show("Coś poszło nie tak, sprawdź wszystkie pola jeszcze raz.");
            }
        }
    }
}
