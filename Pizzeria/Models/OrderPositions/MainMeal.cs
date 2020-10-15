using Pizzeria.Models.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Models.Meals
{
    public class MainMeal : OrderPosition
    {
        public override PositionType PositionType { get => PositionType.MainMeal; }

        public MainMeal(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public override void AddExtra(IExtra extra)
        {
            if(extra.Type == ExtraType.MainMeal)
            {
                if (Extras == null)
                    Extras = new List<IExtra>();

                Extras.Add(extra);
            }
            else
            {
                throw new Exception("Wrong extra typ");
            }

        }
    }
}
