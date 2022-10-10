using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Runtime;
namespace Tictactoe
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int pointer { get; set; }
        AI ai { get; set; }
        Line objLine { get; set; }
        SetLanguage language { get; set; }
        private byte[] stats { get; set; }
        private string player { get; set; }
        private int maxmodes { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            StartGame("X", 2, 3,"en");
        }
        public void TestLanguage(string l)
        {
            language = new SetLanguage(l);
            SetWindows();
        }
        Board board { get; set; }
        Balance balance { get; set; }
        public bool StartGame(string playerchar,  int ailevel,int boardvalueslength, string lang)
        {

            board = new Board(boardvalueslength);
            balance = new Balance(200,2);
            stats = new byte[2];
            objLine = new Line();
            ai = new AI(ailevel);
            if (lang == "")
                SetLanguage("");
            else
                SetLanguage(lang);
            SetSettings(playerchar);
            SetWindows();
            UpdateScore();
            player = "X";
            PlaceYourBet();
            return true;
        }
        public bool SetLanguage(string lang)
        {
            if(lang=="")
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InstalledUICulture;
                language = new SetLanguage(ci.TwoLetterISOLanguageName);
            }
            else
            {
                TestLanguage(lang);
            }
            
            return true;
        }
        public bool SetSettings(string playerchar)
        {
            pointer = 0;
            maxmodes = 3;
            player = playerchar;
            return true;
        }
        public bool SetWindows()
        {
            
            Oponent.Text = language.CPU;
            Title = language.AppTitle;
            Player_1.Text = language.Player_1;
            Continue.IsEnabled = false;
            Continue.Content =language.CONTINUE;
            Continue.Visibility = Visibility.Hidden;
            WinText.Visibility = Visibility.Hidden;
            Easy.Content = language.EASY;
            var a = language.GAMEMODE.Split(' ');
            if(a.Length > 1)
            {
                a[1].Replace('_', ' ');
                GAMEMODE.Content = a[0] + Environment.NewLine + a[1];
            }
            else
                GAMEMODE.Content = a[0];
            a = language.RESET.Split(' ');
            if (a.Length > 1)
            {
                a[1].Replace('_', ' ');
                RESET.Content = a[0] + Environment.NewLine + a[1];
            }
            else
                RESET.Content = a[0];
            
            Advanced.Content = language.ADVANCED;
            Unbeatable.Content = language.UNBEATABLE; 
            if (ai.level == 3)
                Unbeatable.IsChecked = true;
                
            else if (ai.level == 2)
                Advanced.IsChecked = true;
            else
                Easy.IsChecked = true;
            return true;
        }
        private void WhoWin(bool enabled, string winner)
        {
            if (enabled)
            {
                WinText.Visibility = Visibility.Visible;
                if (Player_1.Text == language.CPU && winner == "X")
                    WinText.Text = language.CPU+" "+language.WIN;
                else if (Player_1.Text == language.CPU && winner == "O")
                    WinText.Text = language.CPU_2 + " " + language.WIN;
                else if (Player_1.Text != language.CPU && winner == "O" && Oponent.Text != language.Player_2)
                    WinText.Text = language.CPU + " " + language.WIN;
                else if (Player_1.Text != language.CPU && winner == "X")
                    WinText.Text = language.Player_1+ " " + language.WIN;
                else if (Oponent.Text == language.Player_2 && winner == "X")
                    WinText.Text = language.Player_1 + " " + language.WIN;
                else if (Oponent.Text == language.Player_2 && winner == "O")
                    WinText.Text = language.Player_2 + " " + language.WIN;
                else
                    WinText.Text = language.DRAW;

            }
            else
            {
                WinText.Visibility = Visibility.Hidden;
            }

        }
        private void UpdateScore()
        {
            Score.Text = stats[0] + " : " + stats[1];
            if (stats[0] > 99 || stats[1] > 99)
                Reset();

        }
        public void DrawPath(string direction)
        {

            if (Oponent.Text != language.Player_2 && Player_1.Text == language.Player_1)
                objLine.Stroke = System.Windows.Media.Brushes.Green;
            else if(Player_1.Text==language.Player_1)
                objLine.Stroke = System.Windows.Media.Brushes.Blue;
            else
                objLine.Stroke = System.Windows.Media.Brushes.Red;
            objLine.Fill = objLine.Stroke;
            objLine.StrokeThickness = 10;

            switch (direction)
            {
                case "h":
                    var result = CheckForLine('h');
                    objLine.Y1 = result;
                    objLine.X1 = 0;
                    objLine.Y2 = result;
                    objLine.X2 = Area.Width;
                    break;
                case "v":
                    var result2 = CheckForLine('v');
                    objLine.Y1 = 0;
                    objLine.X1 = result2;
                    objLine.Y2 = Area.Height;
                    objLine.X2 = result2;
                    break;
                case "c1":
                    objLine.Y1 = 0;
                    objLine.X1 = 0;
                    objLine.Y2 = Area.Height;
                    objLine.X2 = Area.Width;
                    break;
                case "c2":
                    objLine.Y1 = Area.Height;
                    objLine.X1 = 0;
                    objLine.Y2 = 0;
                    objLine.X2 = Area.Width;
                    break;

            }
            Area.Children.Remove(objLine);
            Area.Children.Add(objLine);

        }
        private double CheckForLine(char direction)
        {
            double results = 0.0;
            switch (direction)

            {
                case 'h':
                    int i = 0;
                    while (true)
                    {
                        if (board.boardvalues[i, 0] + board.boardvalues[i, 1] + board.boardvalues[i, 2] == "XXX" || board.boardvalues[i, 0] + board.boardvalues[i, 1] + board.boardvalues[i, 2] == "OOO")
                            break;
                        i++;
                    }
                    if (i == 0)
                        results = Area.Height / 6;
                    else if (i == 1)
                        results = Area.Height / 2;
                    else if (i == 2)
                        results = 5 * Area.Height / 6;
                    break;
                case 'v':
                    int j = 0;
                    while (true)
                    {
                        if (board.boardvalues[0, j] + board.boardvalues[1, j] + board.boardvalues[2, j] == "XXX" || board.boardvalues[0, j] + board.boardvalues[1, j] + board.boardvalues[2, j] == "OOO")
                            break;
                        j++;
                    }
                    if (j == 0)
                        results = Area.Width / 6;
                    else if (j == 1)
                        results = Area.Width / 2;
                    else if (j == 2)
                        results = 5 * Area.Width / 6;
                    break;

            }


            return results;
        }
        private void MakeMoveBot(int j, int i)
        {
           var x= board.Search(j, i);
            board.boardvalues[j, i] = player;
            switch (x)
            {
                case 0:
                    Setvalue(_1);
                    break;
                case 1:
                    Setvalue(_2);
                    break;
                case 2:
                    Setvalue(_3);
                    break;
                case 3:
                    Setvalue(_4);
                    break;
                case 4:
                    Setvalue(_5);
                    break;
                case 5:
                    Setvalue(_6);
                    break;
                case 6:
                    Setvalue(_7);
                    break;
                case 7:
                    Setvalue(_8);
                    break;
                case 8:
                    Setvalue(_9);
                    break;
            }
        }
        private void SwitchPlayer()
        {
            if (player == "X")
                player = "O";
            else
                player = "X";
        }
        private void GameOver(string winner)
        {
            
            WhoWin(true, winner);
            if(Player_1.Text!=language.CPU)
            TurnOFFContinue(false);
            if (winner == "X")
                stats[0] += 1;  
            else if (winner == "O")
                stats[1] += 1;
            UpdateScore();
            if (Player_1.Text != language.CPU && Oponent.Text == language.CPU && winner == "X")
                balance.CashOut();
            if (Player_1.Text != language.CPU && Oponent.Text == language.CPU && winner != "X"&&balance.money==0)
            {
                MessageBox.Show(language.GAMEOVER, language.GAMEOVER);
                Close();
            }
            if (Player_1.Text != language.CPU && Oponent.Text == language.CPU && winner != "X" && winner != "O")
                balance.money += balance.bet;
        }
        private void AI()
        {
            player = "X";
            bool a = Checker();
            if (a == false)
            {
                player = "O";
                if (Easy.IsChecked == true)
                    AlgorithmForEasyBOT();
                else if (Advanced.IsChecked == true)
                    AlgorithmForAdvancedBOT();
                else
                    AlgorithmForUnbeatableBot();
                Checker();
            }
            player = "X";

        }
        
        private void ClearBackground()
        {
            _1.Content = "";
            _2.Content = "";
            _3.Content = "";
            _4.Content = "";
            _5.Content = "";
            _6.Content = "";
            _7.Content = "";
            _8.Content = "";
            _9.Content = "";
            Array.Clear(board.boardvalues, 0, board.boardvalues.Length);
        }
        private bool Checker()
        {
            int column = 0;
            int row = 0;
            int cross1 = 0;
            int cross2 = 0;
            int draw = 0;
            for (int i = 0; i <=board.Size - 1; i++)
            {
                if (board.boardvalues[i, i] == player)
                    cross1++;
                for (int j = 0; j <=board.Size - 1; j++)
                {

                    if (board.boardvalues[i, j] == player)
                        column++;
                    if (board.boardvalues[j, i] == player)
                        row++;
                    if (board.boardvalues[Convert.ToInt32(Math.Sqrt(board.boardvalues.Length)) - 1 - j, j] == player)
                        cross2++;
                    if (board.boardvalues[i, j] == "X" || board.boardvalues[i, j] == "O")
                        draw++;
                }

                if (column ==board.Size)
                {
                    DrawPath("h");
                    GameOver(player);
                    return true;
                }
                else if (row ==board.Size)
                {
                    DrawPath("v");
                    GameOver(player);
                    return true;
                }
                else if (cross1 ==board.Size)
                {
                    DrawPath("c1");
                    GameOver(player);
                    return true;
                }
                else if (cross2 ==board.Size)
                {
                    DrawPath("c2");
                    GameOver(player);
                    return true;
                }

                column = 0;
                row = 0;
                cross2 = 0;

            }
            if (draw == board.boardvalues.Length)
            {

                GameOver("");
                return true;
            }

            return false;
        }
        private void Setvalue(Button a)
        {
            if (player == "X"&&Player_1.Text==language.Player_1)
                a.Foreground = Brushes.Red;
            else if (Oponent.Text == language.CPU||(Player_1.Text == language.CPU && player == "X"))
                a.Foreground = Brushes.Blue;
            else
                a.Foreground = Brushes.Green;
            a.Content = player;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_1.Content == "")
            {
                Setvalue(_1);
                board.boardvalues[0, 0] = player;
                NextMove();
            }

        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (_2.Content == "")
            {
                Setvalue(_2);
                board.boardvalues[1, 0] = player;
                NextMove();

            }

        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (_3.Content == "")
            {
                Setvalue(_3);
                board.boardvalues[2, 0] = player;
                NextMove();
            }

        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (_4.Content == "")
            {
                Setvalue(_4);
                board.boardvalues[0, 1] = player;
                NextMove();
            }

        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (_5.Content == "")
            {
                Setvalue(_5);
                board.boardvalues[1, 1] = player;
                NextMove();
            }

        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (_6.Content == "")
            {
                Setvalue(_6);
                board.boardvalues[2, 1] = player;
                NextMove();
            }

        }
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (_7.Content == "")
            {
                Setvalue(_7);
                board.boardvalues[0, 2] = player;
                NextMove();
            }


        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (_8.Content == "")
            {
                Setvalue(_8);
                board.boardvalues[1, 2] = player;
                NextMove();
            }

        }
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (_9.Content == "")
            {
                Setvalue(_9);
                board.boardvalues[2, 2] = player;
                NextMove();
            }

        }
        private void TextChanged_Oponent(object sender, TextChangedEventArgs e)   {   }
       private void GameModePointer()
        {
            pointer++;
            if (pointer > maxmodes-1)
                pointer = 0;
        }
        private void Button_Click_GAMEMODE(object sender, RoutedEventArgs e)
        {
            GameModePointer();
            WhoWin(false,"A");
            Array.Clear(stats, 0, stats.Length);
            ClearBackground();
            UpdateScore();
            if (pointer==1)
            {
                Oponent.Text = language.Player_2;
                Oponent.Foreground = Brushes.Green;
                Advanced.IsEnabled = false;
                Unbeatable.IsEnabled = false;
                Easy.IsEnabled = false;
                Easy.Visibility=Visibility.Hidden;
                Advanced.Visibility = Visibility.Hidden;
                Unbeatable.Visibility = Visibility.Hidden;
                Area.Children.Remove(objLine);
                var tr = TurnOFFContinue(true);
                
            }
            else if(pointer==0)
            {
                Player_1.Text = language.Player_1;
                Player_1.Foreground = Brushes.Red;
                Oponent.Text = language.CPU;
                Oponent.Foreground = Brushes.Blue;
                Advanced.IsEnabled = true;
                Unbeatable.IsEnabled = true;
                Easy.IsEnabled = true;
                Easy.Visibility = Visibility.Visible;
                Advanced.Visibility = Visibility.Visible;
                Unbeatable.Visibility = Visibility.Visible;
                Area.Children.Remove(objLine);
                var tr = TurnOFFContinue(true);
                player ="X";
            }
            else
            {
                Player_1.Text = language.CPU;
                Player_1.Foreground = Brushes.Blue;
                Advanced.IsEnabled = true;
                Unbeatable.IsEnabled = true;
                Easy.IsEnabled = true;
                Easy.Visibility = Visibility.Visible;
                Advanced.Visibility = Visibility.Visible;
                Unbeatable.Visibility = Visibility.Visible;
                Oponent.Text = language.CPU_2;
                Oponent.Foreground = Brushes.Green;
                Area.Children.Remove(objLine);
                var tr = TurnOFFContinue(true);
                player = "X";
                CPUFight();
            }
            Array.Clear(stats, 0, stats.Length);
            ClearBackground();
            UpdateScore();

        }
        private bool TurnOffButtons(bool turnOff)
        {
            if(turnOff)
            {
                _9.IsEnabled = false;
                _8.IsEnabled = false;
                _7.IsEnabled = false;
                _6.IsEnabled = false;
                _5.IsEnabled = false;
                _4.IsEnabled = false;
                _3.IsEnabled = false;
                _2.IsEnabled = false;
                _1.IsEnabled = false;
            }
            else
            {
                _9.IsEnabled = true;
                _8.IsEnabled = true;
                _7.IsEnabled = true;
                _6.IsEnabled = true;
                _5.IsEnabled = true;
                _4.IsEnabled = true;
                _3.IsEnabled = true;
                _2.IsEnabled = true;
                _1.IsEnabled = true;
            }
            return true;
        }
        private bool CPUFight()
        {
            while (true)
            {
                ClearBackground();
                TurnOffButtons(true);
                GAMEMODE.IsEnabled = false;
                while (true)
                {
                    SwitchPlayer();

                    if (Easy.IsChecked == true)
                        AlgorithmForEasyBOT();
                    else if (Advanced.IsChecked == true)
                        AlgorithmForAdvancedBOT();
                    else
                        AlgorithmForUnbeatableBot();
                    bool b = Checker();
                    if (b)
                    {
                        GAMEMODE.IsEnabled = true;
                        break;
                    }
                    
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                    
                    
                    Thread.Sleep(1500);
                    }
              MessageBoxResult a= MessageBox.Show(language.MSGBOXQuestion,language.MSGBOXTitle,MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(a==MessageBoxResult.No)
                {
                    Area.Children.Remove(objLine);
                    TurnOffButtons(false);
                    GAMEMODE.IsEnabled = true;
                    WhoWin(false, null);
                    return true;

                }

                Area.Children.Remove(objLine);
                ClearBackground();
                WhoWin(false, null);
            }
            
        }
        private void NextMove()
        {
            if (Oponent.Text == language.CPU)
                AI();
            else
            {
                Checker();
                SwitchPlayer();
            }
        }
        private bool TurnOFFContinue(bool isOff)
        {
                
            switch (isOff)
            {
                case true:
                    if (Player_1.Text != language.CPU)
                        TurnOffButtons(false);
                    Continue.IsEnabled = false;
                    Continue.Visibility = Visibility.Hidden;
                    break;
                case false:
                    Continue.IsEnabled = true;
                    Continue.Visibility = Visibility.Visible;
                    TurnOffButtons(true);
                    break;
            }
            return true;
        }
        private void Button_Click_RESET(object sender, RoutedEventArgs e)
        {
            Area.Children.Remove(objLine);
            var tr=TurnOFFContinue(true);
           
            Reset();
             if(Unbeatable.IsChecked==true)
            {
                SwitchPlayer();
                PlaceInCorner();
                SwitchPlayer();
            }
        }
        private void Reset()
        {
            Array.Clear(stats, 0, stats.Length);
            ClearBackground();
            UpdateScore();
            WhoWin(false,"A");
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
        private void TextChanged_Score(object sender, TextChangedEventArgs e)
        {

        }
        private bool PlaceYourBet()
        {
            
            Window1 objWindow = new Window1(balance.money,language.languagename);
           
            if (objWindow.ShowDialog() == true)
            {
                balance.bet = objWindow.Money;
            }
            this.Visibility = Visibility.Hidden;
            this.Visibility = Visibility.Visible;
            QuickLoad();
            Account.Text = language.Balance+": " + balance.money;
            Bet.Text = language.Bet + ": " + balance.bet;
            return true;
        }
        private void QuickLoad()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(Environment.CurrentDirectory + "\\a.txt");
            balance.bet = Convert.ToUInt64(sr.ReadLine());
            balance.money -= balance.bet;
        }
      
        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            PlaceYourBet();
            Area.Children.Remove(objLine);
            ClearBackground();
            var tr = TurnOFFContinue(true);
            WhoWin(false,null);
            if(Unbeatable.IsChecked==true)
            {
                SwitchPlayer();
                PlaceInCorner();
                SwitchPlayer();
            }

        }
        private bool PlaceInCorner()
        {
         
            while (true)
            {
                Random rnd = new Random();
                int a = rnd.Next(3);
                SwitchPlayer();
                if (a == 0&&board.boardvalues[0,0]!=player)
                {
                    SwitchPlayer();
                    MakeMoveBot(0,0);
                    break;
                }
                else if (a == 1 && board.boardvalues[2,2] != player)
                {
                    SwitchPlayer();
                    MakeMoveBot(2,2);
                    break;
                }
                else if (a == 2 && board.boardvalues[0, 2] != player)
                {
                    SwitchPlayer();
                    MakeMoveBot(0,2);
                    break;
                }
                else if(a==3 && board.boardvalues[2, 0] != player)
                {
                    SwitchPlayer();
                    MakeMoveBot(2,0);
                    break;
                }
            }
            return true;
        }
        private void Unbeatable_Checked(object sender, RoutedEventArgs e)
        {
            Area.Children.Remove(objLine);
            ClearBackground();

            //SwitchPlayer();
            PlaceInCorner();
            SwitchPlayer();
        }
        
        
       
        public int Count()
        {
            int counter = 0;
            for (int i = 0; i <board.Size; i++)
            {
                for (int j = 0; j <board.Size; j++)
                {
                    if (board.boardvalues[j,i] == "")
                        counter++;
                }
            }
          
           return counter;
        }
      
        private bool AlgorithmForUnbeatableBot()
        {
            if (SearchForPossibilities(true))
                return true;
            if (SearchForPossibilities(false))
                return true;
            if (Count()== board.boardvalues.Length)
            {
                PlaceInCorner();
                return true;
            }
            if (Count() > board.boardvalues.Length-3)
            {
                SwitchPlayer();
                if (board.boardvalues[1, 1] != player)
                {
                    SwitchPlayer();
                    MakeMoveBot(1,1);
                    return true;
                }
                SwitchPlayer();
                if (board.boardvalues[0, 0] == player)
                {
                    MakeMoveBot(2,2);
                }
                else if (board.boardvalues[2,2] == player)
                {
                    MakeMoveBot(0,0);
                }
                else if (board.boardvalues[0, 2] == player)
                {
                    MakeMoveBot(2,0);
                }
                else if (board.boardvalues[2, 0] == player)
                {
                    MakeMoveBot(0,2);
            }
                return true;
            }
            if(AlgorithmForEasyBOT())
                return true;
            throw new Exception();

        }
        private bool CheckIfWin(string pl)
        {
            int column = 0;
            int row = 0;
            int cross1 = 0;
            int cross2 = 0;

            for (int i = 0; i <=board.Size - 1; i++)
            {
                if (board.boardvalues[i, i] == pl)
                    cross1++;

                for (int j = 0; j <=board.Size - 1; j++)
                {

                    if (board.boardvalues[i, j] == pl)
                        column++;
                    if (board.boardvalues[j, i] == pl)
                        row++;
                    var a = Convert.ToInt32(Math.Sqrt(board.boardvalues.Length)) - 1 - j;
                    if (board.boardvalues[Convert.ToInt32(Math.Sqrt(board.boardvalues.Length)) - 1 - j, j] == pl)
                        cross2++;
                }

                if (column ==board.Size || row ==board.Size || cross2 ==board.Size || cross1 ==board.Size)
                    return true;
                column = 0;
                row = 0;
                cross2 = 0;
            }



            return false;
        }
        public bool SearchForPossibilities(bool ForCPU)
        {
            
            for (int i = 0; i <board.Size; i++)
            {
                for (int j = 0; j <board.Size; j++)
                {

                    if (board.boardvalues[j, i] != "O" && board.boardvalues[j, i] != "X")
                    {
                        if (ForCPU)
                            board.boardvalues[j, i] = player;
                        else
                        {
                            SwitchPlayer();
                            board.boardvalues[j, i] = "X";
                            SwitchPlayer();
                        }

                        if (CheckIfWin(board.boardvalues[j, i]))
                        {
                            MakeMoveBot(j,i);
                            return true;
                        }

                        else
                            board.boardvalues[j, i] = "";

                    }
                }
            }
            return false;
        }
        private bool JustStarted()
        {
            foreach (string i in board.boardvalues)
                if (i == player)
                  return false;
                     
            return true;
        }
        public bool AlgorithmForAdvancedBOT()
        {
            int[] val = new int[2];
            if (JustStarted())
            {
                AlgorithmForEasyBOT();
                return true;
            }
            if (SearchForPossibilities(true))
                return true;
            if (SearchForPossibilities(false))
                return true;
            AlgorithmForEasyBOT();



            return true;
        }
        public bool AlgorithmForEasyBOT()
        {
            while (true)
            {
                int counter = 0;
                Random rnd = new Random();
                int a = rnd.Next(0, board.boardvalues.Length);
                for (int i = 0; i <board.Size; i++)
                {
                    for (int j = 0; j <board.Size; j++)
                    {
                        if (counter == a && board.boardvalues[j, i] != "O" && board.boardvalues[j, i] != "X")
                        {
                            MakeMoveBot(j,i);
                            return true;
                        }
                        counter++;
                    }
                }
            }
        }
        

        
    }
}