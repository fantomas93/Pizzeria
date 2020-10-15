using Pizzeria.Base;
using Pizzeria.Models.Meals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class MenuPosition : ObservableObject
    {
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

        private PositionType _type;

        public PositionType Type
        {
            get => _type;
            set { SetProperty(ref _type, value); }
        }
        public MenuPosition(string name, double price, PositionType type)
        {
            Type = type;
            Name = name;
            Price = price;
        }



    }
}
