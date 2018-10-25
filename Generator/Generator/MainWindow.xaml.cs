using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        private const string V = @".\randomInit\";

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

        private void clear(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "";
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
                    to_return += ge.next().ToString();

                result.Text = to_return;

                using (System.IO.StreamWriter param1 = new System.IO.StreamWriter(@".\param1.txt"))
                {
                    param1.WriteLine(poly1.Text);
                    param1.WriteLine(init1.Text);
                }
                using (System.IO.StreamWriter param2 = new System.IO.StreamWriter(@".\param2.txt"))
                {
                    param2.WriteLine(poly2.Text);
                    param2.WriteLine(init2.Text);
                }
                using (System.IO.StreamWriter param3 = new System.IO.StreamWriter(@".\param3.txt"))
                {
                    param3.WriteLine(poly3.Text);
                    param3.WriteLine(init3.Text);
                }
                using (System.IO.StreamWriter param3 = new System.IO.StreamWriter(@".\len.txt"))
                {
                    param3.WriteLine(len.Text);
                }
                using (System.IO.StreamWriter fileResult = new System.IO.StreamWriter(@".\result.txt"))
                {
                    fileResult.Write(to_return);
                }
            }
            else
            {
                var resut = MessageBox.Show("Coś poszło nie tak, sprawdź wszystkie pola jeszcze raz.");
            }
        }

        private void reset(object sender, RoutedEventArgs e)
        {
            init1.Text = "Wartości";
            init2.Text = "Wartości";
            init3.Text = "Wartości";
            poly1.Text = "Wielomian";
            poly2.Text = "Wielomian";
            poly3.Text = "Wielomian";
            len.Text = "Długość generowanego ciągu.";
        }

        private void load_file(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                if (op.ShowDialog() == true)
                    result.Text = File.ReadAllText(op.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coś poszło nie tak.\n Orginal message:" + ex.Message);
            }
        }

        private void step_by_step(object sender, RoutedEventArgs e)
        {
            step_by_stepxaml aspdk = new step_by_stepxaml();
            aspdk.Show();
        }

        private void load_about(object sender, RoutedEventArgs e)
        {
        }
        
        private void save_file(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
                File.WriteAllText(dialog.FileName, result.Text);
        }

        private void load_init(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            int n = r.Next(1, 2900);
            string path = V + n.ToString();
            System.Console.Write(path);
            try
            {

                using (System.IO.StreamReader param1 = new System.IO.StreamReader(path + @"\poly1.txt"))
                {
                    poly1.Text = param1.ReadLine();
                    init1.Text = param1.ReadLine();
                }
                using (System.IO.StreamReader param1 = new System.IO.StreamReader(path + @"\poly2.txt"))
                {
                    poly2.Text = param1.ReadLine();
                    init2.Text = param1.ReadLine();
                }
                using (System.IO.StreamReader param1 = new System.IO.StreamReader(path + @"\poly3.txt"))
                {
                    poly3.Text = param1.ReadLine();
                    init3.Text = param1.ReadLine();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Coś poszło nie tak, sprawdź istnieją wszystkie pliki.");
            }
        }


    }
}
