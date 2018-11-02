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
using System.Windows.Shapes;

namespace Generator
{
    /// <summary>
    /// Interaction logic for step_by_stepxaml.xaml
    /// </summary

    public partial class step_by_stepxaml : Window
    {

        private const string Pattern = "[^0-9x\\+]";
        private const string Pattern1 = "[^0-1]";
        private geffe_step geffe;
        string res;
        public step_by_stepxaml()
        {
            InitializeComponent();
        }

        private void clear(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        private bool checkPoly(string text)
        {
            string test = Regex.Replace(text, Pattern, "");
            if (test == text)
                return true;
            return false;
        }

        private bool checkInit(string text)
        {
            string test = Regex.Replace(text, Pattern1, "");
            string tmp = Regex.Replace(text, "0", "");
            if (test == text && tmp != "")
                return true;
            return false;
        }

        private bool checkLsfr(string poly, string init)
        {
            if(checkInit(init) && checkPoly(poly))
            {
                string test = Regex.Replace(poly, "x", "");
                string[] arr = test.Split('+');
                int s = int.Parse(arr[0]);
                if (s == init.Length)
                    return true;
            }

            return false;
        }

        private bool checkNumer(string text)
        {
            string test = Regex.Replace(text, "[^0-9]", "");
            if (test == text && text[0] != '0')
                return true;
            return false;
        }
        private void init()
        {
            string _l1 = geffe.get1();
            string _l2 = geffe.get2();
            string _l3 = geffe.get3();

            FlowDocument mcFlowDoc = new FlowDocument();
            Paragraph para = new Paragraph();
            para.Inlines.Add(new Bold(new Run(_l1.Substring(0, 1))));
            para.Inlines.Add(new Run(_l1.Substring(1, _l1.Length - 2)));
            para.Inlines.Add(new Bold(new Run(_l1[_l1.Length - 1].ToString())));
            mcFlowDoc.Blocks.Add(para);
            l1.Document = mcFlowDoc;

            mcFlowDoc = new FlowDocument();
            para = new Paragraph();
            para.Inlines.Add(new Bold(new Run(_l2.Substring(0, 1))));
            para.Inlines.Add(new Run(_l2.Substring(1, _l2.Length - 2)));
            para.Inlines.Add(new Bold(new Run(_l2[_l2.Length - 1].ToString())));
            mcFlowDoc.Blocks.Add(para);
            l2.Document = mcFlowDoc;

            mcFlowDoc = new FlowDocument();
            para = new Paragraph();
            para.Inlines.Add(new Bold(new Run(_l3.Substring(0, 1))));
            para.Inlines.Add(new Run(_l3.Substring(1, _l3.Length - 2)));
            para.Inlines.Add(new Bold(new Run(_l3[_l3.Length - 1].ToString())));
            mcFlowDoc.Blocks.Add(para);
            l3.Document = mcFlowDoc;
        }
        private void start(object sender, RoutedEventArgs e)
        {
            if (checkLsfr(poly1.Text, init1.Text) == true &&
                checkLsfr(poly2.Text, init2.Text) == true &&
                checkLsfr(poly3.Text, init3.Text) == true )
            {
                lfsr _1 = new lfsr(poly1.Text, init1.Text);
                lfsr _2 = new lfsr(poly2.Text, init2.Text);
                lfsr _3 = new lfsr(poly3.Text, init3.Text);
                start1.Visibility = Visibility.Hidden;
                stop1.Visibility = Visibility.Visible;
                next1.Visibility = Visibility.Visible;
                res = "";
                geffe = new geffe_step(_1, _2, _3);
                init();
            }
            else
            {
                var resut = MessageBox.Show("Coś poszło nie tak, sprawdź wszystkie pola jeszcze raz.", "Błąd");
            }

        }

        private void next(object sender, RoutedEventArgs e)
        {
            
            Tuple<int, int> result_geffe = geffe.next();
            string next = result_geffe.Item1.ToString();
            string c = result_geffe.Item2.ToString();
            init();
            FlowDocument mcFlowDoc = new FlowDocument();
            Paragraph para = new Paragraph();
            para.Inlines.Add(new Bold(new Run(next)));
            para.Inlines.Add(new Run(res));
            res = res.Insert(0, next);
            mcFlowDoc.Blocks.Add(para);
            result.Document = mcFlowDoc;
            _2.Stroke = new SolidColorBrush(Colors.Black);
            _3.Stroke = new SolidColorBrush(Colors.Black);

            if(c == "1")
            {
                _2.Stroke = new SolidColorBrush(Colors.Black);
                _3.Stroke = new SolidColorBrush(Colors.Red);

            }
            else
            {
                _2.Stroke = new SolidColorBrush(Colors.Red);
                _3.Stroke = new SolidColorBrush(Colors.Black);
            }

        }

        private void stop(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
