using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private geffe geffe;
        long ammount;
        private const string Pattern = "[^0-9x\\+]";
        private const string Pattern1 = "[^0-1]";
        private const string V = @".\randomInit\";
        private Random r = new Random();

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
            if(test != "")
                if (test == text && text[0] != '0')
                    return true;

            return false;
        }

        private void clear(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        private string gen(geffe g, long amm)
        {
            string to_return = "";
            for (long i = 0; i < amm; ++i)
                to_return += geffe.next().ToString();

            return to_return;
        }

        private async void generate_Click(object sender, RoutedEventArgs e)
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
                geffe = new geffe(_1, _2, _3);
                ammount = long.Parse(len.Text);


                string test = await Task.Run(() => gen(geffe, ammount));
                result.Text = test;

                using (System.IO.StreamWriter fileResult = new System.IO.StreamWriter(@".\"+ number.Text + "_" + len.Text + @".txt"))
                {
                    fileResult.Write(result.Text);
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

        private void init()
        {
            int n = r.Next(1, 2900);
            string path = V + n.ToString();
            number.Text = n.ToString();
            MessageBox.Show("Dane początkowe zostały pobrane z folderu: " + path);
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

        private void init(string num)
        {
            string path = V + num;
            MessageBox.Show("Dane początkowe zostały pobrane z folderu: " + path);
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

        private void load_init(object sender, RoutedEventArgs e)
        {
            string t = number.Text;
            if(checkNumer(number.Text) == true)
            {
                try
                {
                    int folderNum;
                    bool res = Int32.TryParse(number.Text, out folderNum);
                    if(res == true)
                    {
                        init(number.Text);
                    }else
                    {
                        MessageBox.Show("Sprawdź poprawność wporwadzonej liczby.");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Sprawdź poprawność wporwadzonej liczby.");
                }
            }
            else
            {
                init();
            }
        }

        private void cipher(object sender, RoutedEventArgs e)
        {
            cipher c = new cipher();
            c.Show();
        }

        private void serial(object sender, RoutedEventArgs e)
        {
            if(result.Text.Length >= 20000)
            {
                string data = result.Text.Substring(0, 20000);
                string[] data_arr = Regex.Split(data, @"(0+)");
                var number = new Dictionary<int, int>();
                foreach (var a in data_arr)
                {
                    if (!number.ContainsKey(a.Length))
                        number[a.Length] = 1;
                    else
                        ++number[a.Length];
                }
                int[,] arr = new int[5,2];
                arr[0,0] = 2315;
                arr[0,1] = 2685;

                arr[1, 0] = 1114;
                arr[1, 1] = 1386;

                arr[2, 0] = 527;
                arr[2, 1] = 723;

                arr[3, 0] = 240;
                arr[3, 1] = 384;

                arr[4, 0] = 103;
                arr[4, 1] = 209;

                bool resultrr = true;

                foreach (KeyValuePair<int, int> kvp in number)
                {
                    if(kvp.Key < 0 && kvp.Key > 5)
                    {
                        if(!(arr[kvp.Key-1,0] <= kvp.Value) && !(kvp.Value <= arr[kvp.Key - 1, 1]))
                        {
                            resultrr = false;
                        }
                    }
                    else
                    {
                        if (!(arr[4, 0] <= kvp.Value) && !(kvp.Value <= arr[4, 1]))
                        {
                            resultrr = false;
                        }
                    }                   
                }

                if(resultrr == true)
                {
                    MessageBox.Show("Test zdany.");
                }
                else
                {
                    MessageBox.Show("Test nie zdany.");
                }
            }else
            {
                MessageBox.Show("Badany ciąg jest za krótki.");
            }
            
        }

        private void bits(object sender, RoutedEventArgs e)
        {
            if (result.Text.Length >= 20000)
            {
                string data = result.Text.Substring(0, 20000);
                int amm = 0;
                foreach (var s in data)
                    if (s == '1')
                        ++amm;

                if (amm>=9725 && amm<=10275)
                    MessageBox.Show("Test zdany.\nWystąpiło :" + amm.ToString() + " jedynek.");
                else
                    MessageBox.Show("Test nie zdany.");
            }
            else
            {
                MessageBox.Show("Badany ciąg jest za krótki.");
            }

        }

        private void poker(object sender, RoutedEventArgs e)
        {


            if (result.Text.Length >= 20000)
            {
                var number = new Dictionary<string, int>();
                for(int i = 0;  i < 20000; i+=4)
                {
                    if (!number.ContainsKey(result.Text.Substring(i,4)))
                        number[result.Text.Substring(i, 4)] = 1;
                    else
                        ++number[result.Text.Substring(i, 4)];
                }

                double val = 0;
                foreach(var eee in number)
                {
                    val += (eee.Value* eee.Value);
                }

                val *= (16.00 / 5000.00);
                val -= 5000;
                if (2.16<val && val < 46.17)
                    MessageBox.Show("Test zdany.\nWartość X = " + val.ToString());
                else
                    MessageBox.Show("Test nie zdany.");
            }
            else
            {
                MessageBox.Show("Badany ciąg jest za krótki.");
            }
        }

        private void longSerial(object sender, RoutedEventArgs e)
        {
            if (result.Text.Length >= 20000)
            {
                string data = result.Text.Substring(0, 20000);
                string[] data_arr = Regex.Split(data, @"(0+)");
                bool res = true;
                foreach (var a in data_arr)
                {
                    if(a.Length>25)
                    {
                        res = false;
                        break;
                    }
                }

                if (res)
                {
                    MessageBox.Show("Test zdany.");
                }
                else
                {
                    MessageBox.Show("Test nie zdany.");
                }
            }
            else
            {
                MessageBox.Show("Badany ciąg jest za krótki.");
            }
        }
    }
}
