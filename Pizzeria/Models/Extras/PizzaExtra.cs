using Pizzeria.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Models.Extras
{
    public class PizzaExtra : ObservableObject, IExtra
    {
       public PizzaExtra(string name, double price)
        {
            Name = name;
            Price = price;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { SetProperty(ref _name, value); }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set { SetProperty(ref _price, value); }
        }

        public ExtraType Type
        {
            get => ExtraType.Pizza;
        }


    }
}
