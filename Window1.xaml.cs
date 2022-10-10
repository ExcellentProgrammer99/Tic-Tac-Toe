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

namespace Tictactoe
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
       Balance balance { get; set; }
        SetLanguage language { get; set; }
        public Window1(ulong money,string a)
        {
            language = new SetLanguage(a);
            balance = new Balance(money,2);
            InitializeComponent();
            balance.bet = 0;
            Balance.Text = language.Balance + ": " + balance.money;
            Display.Text = "$0";
            Title = language.PlaceYourBet;
            Apply.Content = language.Apply;
            BetAll.Content = language.All_In;
            Clear.Content = language.Clear;
            if (language.languagename == "ar")
                OK.Content = "حسنا";
            if (language.languagename == "jp")
                OK.Content = "よし";
        }
        public ulong Money {get { return balance.bet; }  }
        public string CurrentDirectoryPath   {  get {  return Environment.CurrentDirectory; }   }

      public bool DisplayCurrentBet(ulong howmuch)
        {
          
                var a = balance.money - (howmuch + balance.bet);
                for (ulong i = 0; i <= howmuch + balance.bet; i++)
            {
                if (a == ulong.MaxValue)
                {
                    balance.bet = balance.money;
                    Display.Text = '$' + balance.bet.ToString();
                    return false;
                }
                    
                a++;
            }
                 balance.bet += howmuch;
                Display.Text = '$' + balance.bet.ToString();
                return true;
          
        }
        private void QuickLoad()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(Environment.CurrentDirectory + "\\a.txt");
            balance.bet = Convert.ToUInt64(sr.ReadLine());
            balance.money -= balance.bet;
        }
        public bool QuickSave()
        {
            System.IO.StreamWriter sw = System.IO.File.CreateText(CurrentDirectoryPath+"\\a.txt");
            sw.WriteLine(balance.bet);
            sw.Close();
            return true;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            balance.Removemoney(balance.bet);
            QuickSave();
            Close();
        }

        private void BetALL_Click(object sender, RoutedEventArgs e)
        {
            DisplayCurrentBet(balance.money);
        }

       
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                DisplayCurrentBet(Convert.ToUInt64(GivenValue.Text));
            }
            catch(FormatException)
            {
                GivenValue.Text = "";
            }
            catch(System.OverflowException)
            {
                GivenValue.Text = "";
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "$0";
            balance.bet = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DisplayCurrentBet(10);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DisplayCurrentBet(1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DisplayCurrentBet(100);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DisplayCurrentBet(1000);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DisplayCurrentBet(10000);
        }

        private void GivenValue_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
