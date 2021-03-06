﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
    /// Interaction logic for cipher.xaml
    /// </summary>
    public partial class cipher : Window
    {
        public cipher()
        {
            InitializeComponent();
        }

        private geffe geffe;
        private const string Pattern = "[^0-9x\\+]";
        private const string Pattern1 = "[^0-1]";
        private const string V = @".\Data\randomInit\";
        private Random r = new Random();


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

        private bool checkNumer(string text)
        {
            string test = Regex.Replace(text, "[^0-9]", "");
            if (test != "")
                if (test == text && text[0] != '0')
                    return true;
            return false;
        }

        private bool checkText(string text)
        {
            string test = Regex.Replace(text, "[żźćńółęąśŻŹĆĄŚĘŁÓŃ]", "");
            if (test != "")
                if (test == text && text[0] != '0')
                    return true;
            return false;
        }

        public string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in data.ToCharArray())
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            return sb.ToString();
        }

        public string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < data.Length; i += 8)
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        private void clear(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        private string gen(geffe g, string text)
        {
            short value_1 = 0;
            short value_2 = 0;
            string binaryData = StringToBinary(text);
            List<int> encypt = new List<int>();
            for (int i = 0; i < binaryData.Length; ++i)
            {
                value_1 = g.next() == '0' ? (short)0 : (short)1;
                if(binaryData[i] == '0')
                    value_2 = 0;
                else
                    value_2 = 1;
                
                encypt.Add(((value_1+value_2)%2));
            }

            return string.Join("", encypt.ToArray());
        }

        private string de(geffe g, string text)
        {
            short value_1 = 0;
            short value_2 = 0;

            List<int> encrypt = new List<int>();
            for (int i = 0; i < text.Length; ++i)
            {
                value_1 = g.next() == '0' ? (short)0 : (short)1;
                if (text[i] == '0')
                    value_2 = 0;
                else
                    value_2 = 1;
                encrypt.Add(((value_1 + value_2) % 2));
            }
            return BinaryToString( string.Join("", encrypt.ToArray()));
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
            if (checkLsfr(poly1.Text, init1.Text) == true &&
                checkLsfr(poly2.Text, init2.Text) == true &&
                checkLsfr(poly3.Text, init3.Text) == true &&
                result.Text.Length < 1000000 && checkText(result.Text))
            {
                lfsr _1 = new lfsr(poly1.Text, init1.Text);
                lfsr _2 = new lfsr(poly2.Text, init2.Text);
                lfsr _3 = new lfsr(poly3.Text, init3.Text);
                geffe = new geffe(_1, _2, _3);
                string s = result.Text;
                string test = await Task.Run(() => gen(geffe, s));
                result.Text = test;
                MessageBox.Show("Wygenerowano klucz o długości: " + test.Length.ToString(), "Długość klucza");
            }
            else
            {
                var resut = MessageBox.Show("Coś poszło nie tak, sprawdź wszystkie pola jeszcze raz.", "Błąd");
            }
        }

        private async void decode(object sender, RoutedEventArgs e)
        {
            if (checkLsfr(poly1.Text, init1.Text) == true &&
                checkLsfr(poly2.Text, init2.Text) == true &&
                checkLsfr(poly3.Text, init3.Text) == true &&
                checkInit(result.Text))
            {
                lfsr _1 = new lfsr(poly1.Text, init1.Text);
                lfsr _2 = new lfsr(poly2.Text, init2.Text);
                lfsr _3 = new lfsr(poly3.Text, init3.Text);
                geffe = new geffe(_1, _2, _3);
                string s = result.Text;
                string test = await Task.Run(() => de(geffe, s));
                result.Text = test;
            }
            else
            {
                var resut = MessageBox.Show("Coś poszło nie tak, sprawdź wszystkie pola jeszcze raz.", "Błąd");
            }
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

        private void load_about(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@".\about.html");
        }

        private void saveBinary(object sender, RoutedEventArgs e)
        {
            string dirName = DateTime.Now.ToString("yyyy_dd_M HH_mm_ss");
            System.IO.Directory.CreateDirectory(@".\Data\binary\cipher");
            System.IO.Directory.CreateDirectory(@".\Data\binary\cipher\" + dirName);
            using (FileStream fs = File.Create(@".\Data\binary\cipher\" + dirName + @"\bin.txt", 2048, FileOptions.None))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, Encoding.ASCII.GetBytes(result.Text));
            }
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
            MessageBox.Show("Dane początkowe zostały pobrane z folderu: " + path, "Info");
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
            MessageBox.Show("Dane początkowe zostały pobrane z folderu: " + path, "Info");
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
            if (checkNumer(number.Text) == true)
            {
                try
                {
                    int folderNum;
                    bool operation = Int32.TryParse(number.Text, out folderNum);
                    if (operation == true)
                        init(number.Text);
                    else
                        MessageBox.Show("Sprawdź poprawność wporwadzonej liczby.", "Błąd");
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
    }
}
