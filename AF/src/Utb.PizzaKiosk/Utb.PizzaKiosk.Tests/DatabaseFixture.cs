using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.Tests
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            using PizzaKioskContext context = new PizzaKioskContext();
        }


    }
}
