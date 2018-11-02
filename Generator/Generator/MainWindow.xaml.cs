using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private geffe geffe;
        private const string Pattern = "[^0-9x\\+]";
        private const string Pattern1 = "[^0-1]";
        private const string V = @".\Data\randomInit\";
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
            string tmp = Regex.Replace(text, "0", "");
            if (test == text && tmp != "")
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

        private string gen(geffe g, long len)
        {
            string to_return = "";
            for (long i = 0; i < len; ++i)
                to_return += geffe.next().ToString();
            return to_return;
        }
        private bool checkLsfr(string poly, string init)
        {
            if (checkInit(init) && checkPoly(poly))
            {
                string test = Regex.Replace(poly, "x", "");
                string[] arr = test.Split('+');
                int s = int.Parse(arr[0]);
                if (s == init.Length)
                    return true;
            }

            return false;
        }

        private async void generate_Click(object sender, RoutedEventArgs e)
        {
            long textLen = 0 ;

            if (checkLsfr(poly1.Text, init1.Text) == true &&
                checkLsfr(poly2.Text, init2.Text) == true &&
                checkLsfr(poly3.Text, init3.Text) == true &&
                checkNumer(len.Text) && long.TryParse(len.Text, out textLen))
            {
                lfsr _1 = new lfsr(poly1.Text, init1.Text);
                lfsr _2 = new lfsr(poly2.Text, init2.Text);
                lfsr _3 = new lfsr(poly3.Text, init3.Text);
                geffe = new geffe(_1, _2, _3);
                
                string test = await Task.Run(() => gen(geffe, textLen));
                result.Text = test;
                int tryParse = -1;
                bool res = int.TryParse(number.Text, out tryParse);
                if (!res || ( tryParse < 1 && tryParse > 2900))
                {
                    string dirName = DateTime.Now.ToString("yyyy_dd_M HH_mm_ss");
                    System.IO.Directory.CreateDirectory(@".\Data\user\" + dirName);
                    using (System.IO.StreamWriter fileResult = new System.IO.StreamWriter(@".\Data\user\" + dirName + @"\lsfr1.txt"))
                    {
                        fileResult.WriteLine(poly1.Text);
                        fileResult.WriteLine(init1.Text);
                    }
                    using (System.IO.StreamWriter fileResult = new System.IO.StreamWriter(@".\Data\user\" + dirName + @"\lsfr2.txt"))
                    {
                        fileResult.WriteLine(poly2.Text);
                        fileResult.WriteLine(init3.Text);
                    }
                    using (System.IO.StreamWriter fileResult = new System.IO.StreamWriter(@".\Data\user\" + dirName + @"\lsfr3.txt"))
                    {
                        fileResult.WriteLine(poly3.Text);
                        fileResult.WriteLine(init3.Text);
                    }
                    using (System.IO.StreamWriter fileResult = new System.IO.StreamWriter(@".\Data\user\" + dirName + @"\result.txt"))
                    {
                        fileResult.Write(result.Text);
                    }
                }
                else
                    using (System.IO.StreamWriter fileResult = new System.IO.StreamWriter(@".\Data\results\"+ number.Text + "_" + len.Text + @".txt"))
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
            result.Text = "";
            number.Text = "Numer folderu z danymi.";
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
            step_by_stepxaml stepBystep = new step_by_stepxaml();
            stepBystep.Show();
        }

        private void load_about(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@".\about.html");
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

        private void initUser(string path)
        {
            try
            {
                using (System.IO.StreamReader param1 = new System.IO.StreamReader(path + @"\lsfr1.txt"))
                {
                    poly1.Text = param1.ReadLine();
                    init1.Text = param1.ReadLine();
                }
                using (System.IO.StreamReader param1 = new System.IO.StreamReader(path + @"\lsfr2.txt"))
                {
                    poly2.Text = param1.ReadLine();
                    init2.Text = param1.ReadLine();
                }
                using (System.IO.StreamReader param1 = new System.IO.StreamReader(path + @"\lsfr3.txt"))
                {
                    poly3.Text = param1.ReadLine();
                    init3.Text = param1.ReadLine();
                }
                using (System.IO.StreamReader param1 = new System.IO.StreamReader(path + @"\result.txt"))
                {
                    result.Text = param1.ReadToEnd();
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
            if(number.Text.Contains("user"))
            {//2018_29_10 12_34_00
                //string [] s = number.Text.Split('\\');
                string pattern = @"\d{4}_\d{2}_\d{2} \d{2}_\d{2}_\d{2}";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(number.Text);
                initUser(@".\Data\user\" + match.Value);
                number.Text = "Numer folderu z danymi.";
                return;
            }
            int folderNum;
            bool res = Int32.TryParse(number.Text, out folderNum);
            if (res)
            {
                try
                {
                    if(folderNum > 0 && folderNum <2901)
                        init(number.Text);
                    else
                        MessageBox.Show("Wybierz liczbę w zakresie 1 - 2900.", "Błąd");
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
            cipher encypt = new cipher();
            encypt.Show();
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

            int[,] compare_arr = new int[5, 2];
            compare_arr[0, 0] = 2315;
            compare_arr[0, 1] = 2685;

            compare_arr[1, 0] = 1114;
            compare_arr[1, 1] = 1386;

            compare_arr[2, 0] = 527;
            compare_arr[2, 1] = 723;

            compare_arr[3, 0] = 240;
            compare_arr[3, 1] = 384;

            compare_arr[4, 0] = 103;
            compare_arr[4, 1] = 209;

            bool toReturn = true;

            foreach (KeyValuePair<int, int> kvp in number)
            {
                if (kvp.Key < 0 && kvp.Key > 5)
                {
                    if (!(compare_arr[kvp.Key - 1, 0] <= kvp.Value) && !(kvp.Value <= compare_arr[kvp.Key - 1, 1]))
                        toReturn = false;
                }
                else
                {
                    if (!(compare_arr[4, 0] <= kvp.Value) && !(kvp.Value <= compare_arr[4, 1]))
                        toReturn = false;
                }
            }

            return toReturn;
        }

        private void serial(object sender, RoutedEventArgs e)
        {
            if(result.Text.Length >= 20000)
            {
                if(serialTest(result.Text.Substring(0,20000)) == true)
                    MessageBox.Show("Test zdany.", "Wynik");
                else
                    MessageBox.Show("Test nie zdany.", "Wynik");
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
            var combinations = new Dictionary<string, int>();
            for (int i = 0; i < 20000; i += 4)
            {
                if (!combinations.ContainsKey(text.Substring(i, 4)))
                    combinations[text.Substring(i, 4)] = 1;
                else
                    ++combinations[text.Substring(i, 4)];
            }

            double X = 0;
            foreach (var eee in combinations)
            {
                X += (eee.Value * eee.Value);
            }

            X *= (16.00 / 5000.00);
            X -= 5000;

            if (2.16 < X && X < 46.17)
                return new Tuple<bool, double>(true, X);

            return new Tuple<bool, double>(false, X);
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

        private void runAllWholeDirctory(object sender, RoutedEventArgs e)
        {
            System.IO.Directory.CreateDirectory(@".\Data\testy");
            DirectoryInfo d = new DirectoryInfo(@".\Data\testy");

            FileInfo[] Files = d.GetFiles("*.txt");
            if(Files.Length == 0)
            {
                MessageBox.Show("Brak plików txt w folderze .\\Data\\testy.", "Błąd");
            }
            else
            {
                string allTests = "";
                foreach (FileInfo file in Files)
                {
                    using (System.IO.StreamReader param1 = new System.IO.StreamReader(@".\Data\" + file))
                    {
                        string t = param1.ReadToEnd();
                        if (t.Length >= 20000)
                        {
                            allTests += file.ToString();
                            allTests += "\n";
                            allTests += runAllResult(t);
                            allTests += "\n";
                        }
                    }
                }
                using (System.IO.StreamWriter fileResult = new System.IO.StreamWriter(@".\Data\results.txt"))
                {
                    string[] dataToFile = allTests.Split('\n');
                    foreach (var testRes in dataToFile)
                        fileResult.WriteLine(testRes);
                }
            }

        }

        private void save_binary(object sender, RoutedEventArgs e)
        {
            if(result.Text == "" || !checkInit(result.Text))
            {
                MessageBox.Show("Błędne dane do zapisu.", "Błąd");
            }
            else
            {

                string dirName = DateTime.Now.ToString("yyyy_dd_M HH_mm_ss");
                System.IO.Directory.CreateDirectory(@".\Data\binary\generated");
                System.IO.Directory.CreateDirectory(@".\Data\binary\generated\" + dirName);
                using (FileStream fs = File.Create(@".\Data\binary\generated\" + dirName + @"\bin.txt", 2048, FileOptions.None))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, Encoding.ASCII.GetBytes(result.Text));
                }
            }

        }
    }
}
