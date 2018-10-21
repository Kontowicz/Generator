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
    /// </summary>
    public partial class step_by_stepxaml : Window
    {
        private const string Pattern = "[^0-9x\\+]";
        private const string Pattern1 = "[^0-1]";
        private geffe geffe;
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
        private void init()
        {

            string _l1 = geffe.get1();
            string _l2 = geffe.get2();
            string _l3 = geffe.get3();

            FlowDocument mcFlowDoc = new FlowDocument();
            Paragraph para = new Paragraph();
            para.Inlines.Add(new Run(_l1.Substring(0, _l1.Length - 2)));
            para.Inlines.Add(new Bold(new Run(_l1[_l1.Length - 1].ToString())));
            mcFlowDoc.Blocks.Add(para);
            l1.Document = mcFlowDoc;

            mcFlowDoc = new FlowDocument();
            para = new Paragraph();
            para.Inlines.Add(new Run(_l2.Substring(0, _l2.Length - 2)));
            para.Inlines.Add(new Bold(new Run(_l2[_l2.Length - 1].ToString())));
            mcFlowDoc.Blocks.Add(para);
            l2.Document = mcFlowDoc;

            mcFlowDoc = new FlowDocument();
            para = new Paragraph();
            para.Inlines.Add(new Run(_l3.Substring(0, _l3.Length - 2)));
            para.Inlines.Add(new Bold(new Run(_l3[_l3.Length - 1].ToString())));
            mcFlowDoc.Blocks.Add(para);
            l3.Document = mcFlowDoc;
        }
        private void start(object sender, RoutedEventArgs e)
        {
            if (checkPoly(poly1.Text) == true &&
                checkPoly(poly2.Text) == true &&
                checkPoly(poly3.Text) == true &&
                checkInit(init1.Text) == true &&
                checkInit(init2.Text) == true &&
                checkInit(init3.Text) == true )
            {
                lfsr _1 = new lfsr(poly1.Text, init1.Text);
                lfsr _2 = new lfsr(poly2.Text, init2.Text);
                lfsr _3 = new lfsr(poly3.Text, init3.Text);
                start1.Visibility = Visibility.Hidden;
                stop1.Visibility = Visibility.Visible;
                next1.Visibility = Visibility.Visible;
                res = "";
                geffe = new geffe(_1, _2, _3);
                init();
            }
            else
            {
                var resut = MessageBox.Show("Coś poszło nie tak, sprawdź wszystkie pola jeszcze raz.");
            }

        }

        private void next(object sender, RoutedEventArgs e)
        {
            string next = geffe.next().ToString();
            FlowDocument mcFlowDoc = new FlowDocument();
            Paragraph para = new Paragraph();
            para.Inlines.Add(new Bold(new Run(next)));
            para.Inlines.Add(new Run(res));
            res = res.Insert(0, next);
            mcFlowDoc.Blocks.Add(para);
            result.Document = mcFlowDoc;
        }

        private void stop(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
