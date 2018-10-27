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
                var resut = MessageBox.Show("Coś poszło nie tak, sprawdź wszystkie pola jeszcze raz.", "Błąd");
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
                MessageBox.Show("Coś poszło nie tak.\n Orginal message:" + ex.Message, "Błąd");
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
                MessageBox.Show("Coś poszło nie tak, sprawdź istnieją wszystkie pliki.", "Błąd");
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
                MessageBox.Show("Coś poszło nie tak, sprawdź istnieją wszystkie pliki.", "Błąd");
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
                        MessageBox.Show("Sprawdź poprawność wporwadzonej liczby.", "Błąd");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Sprawdź poprawność wporwadzonej liczby.", "Błąd");
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

        private bool serialTest( string text)
        {
            string[] data_arr = Regex.Split(text, @"(0+)");
            var number = new Dictionary<int, int>();
            foreach (var a in data_arr)
            {
                if (!number.ContainsKey(a.Length))
                    number[a.Length] = 1;
                else
                    ++number[a.Length];
            }

            int[,] section = new int[5, 2];
            section[0, 0] = 2315;
            section[0, 1] = 2685;

            section[1, 0] = 1114;
            section[1, 1] = 1386;

            section[2, 0] = 527;
            section[2, 1] = 723;

            section[3, 0] = 240;
            section[3, 1] = 384;

            section[4, 0] = 103;
            section[4, 1] = 209;

            bool toReturn = true;

            foreach (KeyValuePair<int, int> kvp in number)
            {
                if (kvp.Key < 0 && kvp.Key > 5)
                {
                    if (!(section[kvp.Key - 1, 0] <= kvp.Value) && !(kvp.Value <= section[kvp.Key - 1, 1]))
                    {
                        toReturn = false;
                    }
                }
                else
                {
                    if (!(section[4, 0] <= kvp.Value) && !(kvp.Value <= section[4, 1]))
                    {
                        toReturn = false;
                    }
                }
            }

            return toReturn;
        }

        private void serial(object sender, RoutedEventArgs e)
        {
            if(result.Text.Length >= 20000)
            {
                if(serialTest(result.Text.Substring(0,20000)) == true)
                {
                    MessageBox.Show("Test zdany.", "Wynik");
                }
                else
                {
                    MessageBox.Show("Test nie zdany.", "Wynik");
                }
            }else
            {
                MessageBox.Show("Badany ciąg jest za krótki.", "Błąd");
            }
            
        }

        private Tuple<bool, int> bitsTest(string text)
        {
            int ammount = 0;
            foreach (var s in text)
                if (s == '1')
                    ++ammount;

            if (ammount >= 9725 && ammount <= 10275)
                return new Tuple<bool, int>(true, ammount);

            return new Tuple<bool, int>(false, ammount);
        }

        private void bits(object sender, RoutedEventArgs e)
        {
            if (result.Text.Length >= 20000)
            {
                var testResult = bitsTest(result.Text.Substring(0, 20000));
                if (testResult.Item1)
                    MessageBox.Show("Test zdany.\nWystąpiło : " + testResult.Item2.ToString() + " jedynek.", "Wynik");
                else
                    MessageBox.Show("Test nie zdany.", "Wynik");
            }
            else
            {
                MessageBox.Show("Badany ciąg jest za krótki.", "Błąd");
            }

        }

        private Tuple<bool, double> pokerTest(string text)
        {
            var number = new Dictionary<string, int>();
            for (int i = 0; i < 20000; i += 4)
            {
                if (!number.ContainsKey(text.Substring(i, 4)))
                    number[text.Substring(i, 4)] = 1;
                else
                    ++number[text.Substring(i, 4)];
            }

            double val = 0;
            foreach (var eee in number)
            {
                val += (eee.Value * eee.Value);
            }

            val *= (16.00 / 5000.00);
            val -= 5000;

            if (2.16 < val && val < 46.17)
                return new Tuple<bool, double>(true, val);

            return new Tuple<bool, double>(false, val);
        }

        private void poker(object sender, RoutedEventArgs e)
        {
            if (result.Text.Length >= 20000)
            {
                var testResult = pokerTest(result.Text.Substring(0, 20000));
                if (testResult.Item1)
                    MessageBox.Show("Test zdany.\nWartość X = " + testResult.Item2, "Wynik");
                else
                    MessageBox.Show("Test nie zdany.", "Wynik");
            }
            else
            {
                MessageBox.Show("Badany ciąg jest za krótki.", "Błąd");
            }
        }

        private bool longSerialTest(string text)
        {
            string[] data_arr = Regex.Split(text, @"(0+)");
            bool res = true;
            foreach (var a in data_arr)
            {
                if (a.Length > 25)
                    return false;
            }
            return true;
        }

        private void longSerial(object sender, RoutedEventArgs e)
        {
            if (result.Text.Length >= 20000)
            {
                string data = result.Text.Substring(0, 20000);

                if (longSerialTest(result.Text.Substring(0, 20000)))
                    MessageBox.Show("Test zdany.", "Wynik");
                else
                    MessageBox.Show("Test nie zdany. ", "Wynik");
            }
            else
            {
                MessageBox.Show("Badany ciąg jest za krótki.", "Błąd");
            }
        }
        private string runAllResult(string text)
        {
            var serialTestResult = serialTest(text);
            var bitsTestResult = bitsTest(text);
            var pokerTestResult = pokerTest(text);
            var lonegSeriesResult = longSerialTest(text);

            string messageBoxText = "";
            if (serialTestResult)
                messageBoxText += "Test serii: pozytywny\n";
            else
                messageBoxText += "Test serii: negatywny\n";

            if (bitsTestResult.Item1)
                messageBoxText += "Test pojedynczych bitów: pozytywny. Wynik testu: " + bitsTestResult.Item2.ToString() + "\n";
            else
                messageBoxText += "Test pojedynczych bitów: negatywny. Wynik testu: " + bitsTestResult.Item2.ToString() + "\n";

            if (pokerTestResult.Item1)
                messageBoxText += "Test pokerowy: pozytywny. Wynik testu: " + pokerTestResult.Item2.ToString() + "\n";
            else
                messageBoxText += "Test pokerowy: negatywny. Wynik testu: " + pokerTestResult.Item2.ToString() + "\n";

            if (lonegSeriesResult)
                messageBoxText += "Test długich serii: pozytywny\n";
            else
                messageBoxText += "Test długich serii: negatyny\n";

            return messageBoxText;
        }

        private void runAll(object sender, RoutedEventArgs e)
        {
            if (result.Text.Length >= 20000)
            {
                string data = result.Text.Substring(0, 20000);
                MessageBox.Show(runAllResult(data), "Wyniki testów");
            }
            else
            {
                MessageBox.Show("Badany ciąg jest za krótki.", "Błąd");
            }
        }

        private void runAllWholeDictionary(object sender, RoutedEventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(@"..\Data");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                using (System.IO.StreamReader param1 = new System.IO.StreamReader(@"..\Data\" + file))
                {
                    string t = param1.ReadToEnd();
                    if (t.Length >= 20000)
                    {
                        str += file.ToString();
                        str += "\n";
                        str += runAllResult(t);
                        str += "\n";
                    }
                }
                System.Console.WriteLine(str);
            }
            using (System.IO.StreamWriter fileResult = new System.IO.StreamWriter(@"..\Data\results.txt"))
            {
                string[] toFile = str.Split('\n');
                foreach(var eeeee in toFile)
                    fileResult.WriteLine(eeeee);
            }

            

        }
    }
}
