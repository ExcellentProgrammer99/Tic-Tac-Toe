using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tictactoe
{
    class Balance
    {
        public ulong money { get; set; }
        public ulong bet { get; set; }
        public ulong multiplier { get; set; }
      
        public Balance(ulong money,  ulong multiplier)
        {

            
            this.money = money;
            this.multiplier = multiplier;

        }

        public bool CashOut()
        {
            try
            {
                money += bet*multiplier;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception();
            }

        }
        public bool Removemoney(ulong number)
        {
            try
            {
                money -= number;
                return true;
            }
            catch(ArgumentOutOfRangeException)
            {
                money = 0;
                return false;
            }

        }

    }
}
