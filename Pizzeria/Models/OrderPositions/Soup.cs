using Pizzeria.Models.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Models.Meals
{
    public class Soup : OrderPosition
    {
        public override PositionType PositionType { get => PositionType.Soup; }

        public Soup(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public override void AddExtra(IExtra extra)
        {
            throw new Exception("Can't add extra to soup");
        }
    }
}
