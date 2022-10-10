using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Tictactoe
{
    class SetLanguage
    {
        public string AppTitle;
        public string Player_1;
        public string Player_2;
        public string CPU;
        public string CPU_2;
        public string EASY;
        public string ADVANCED;
        public string UNBEATABLE;
        public string GAMEMODE;
        public string RESET;
        public string CONTINUE;
        public string MSGBOXTitle;
        public string MSGBOXQuestion;
        public string languagename;
        public string DRAW;
        public string WIN;
        public string GAMEOVER;
        public string PlaceYourBet;
        public string Bet;
        public string Clear;
            public string Apply;
            public string Balance;
            public string All_In;
        public SetLanguage(string language)
        {
            languagename = language;
           string Path = Environment.CurrentDirectory+"\\Language\\"+language+".txt";
            List<string> data = new List<string>();
           data= LoadFile(Path,data,22);
            SetValues(data);
        }
        public bool SetValues(List<string> data)
        {
            AppTitle = data[0];
            Player_1 = data[1];
            Player_2 = data[2];
            CPU = data[3];
            CPU_2 = data[4];
            EASY = data[5];
            ADVANCED = data[6];
            UNBEATABLE = data[7];
            GAMEMODE = data[8];
            RESET = data[9];
            CONTINUE = data[10];
            MSGBOXTitle = data[11];
            MSGBOXQuestion = data[12];
            DRAW = data[13];
            WIN = data[14];
            GAMEOVER = data[15];
            Bet = data[16];
            Clear = data[17];
            Apply = data[18];
            Balance = data[19];
            All_In = data[20];
            PlaceYourBet = data[21];
            
            return true;
        }
        public List<string> LoadFile(string path,List<string> data, int length)
        {
            try
            {
                
                StreamReader reader = new StreamReader(path);
                for(int i=0;i<length;i++)
                {
                    string a = reader.ReadLine();
                    var b = a.Split('=');
                    data.Add(b[1]);
                }
                reader.Close();
        
            }
            catch (FormatException e)
            {
                Exception bex = new Exception("The file is corrupted!", e);
                throw bex;
            }
           
            return data;
        }
    }
}
