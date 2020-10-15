using Pizzeria.Base;
using Pizzeria.Models.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Models.Meals
{
    public abstract class OrderPosition : ObservableObject
    {
        public abstract PositionType PositionType { get; }

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

        private List<IExtra> _extras;
        public List<IExtra> Extras
        {
            get => _extras;
            set { SetProperty(ref _extras, value); }
        }

        public string ExtrasStringList { get => string.Join(", ", Extras != null ? Extras.Select(p => p.Name).ToList() : new List<string>()); }

        public double TotalExtrasPrice { get => Extras != null ? Extras.Sum(e => e.Price) : 0d; }


        private string _extrasString;
        public string ExtrasString
        {
            get => _extrasString;
            set { SetProperty(ref _extrasString, value); }
        }

        public abstract void AddExtra(IExtra extra);

        public double GetTotalPrice()
        {
            double extrasTotalPrice = 0d;
            if (Extras != null && Extras.Count() > 0)
                extrasTotalPrice += Extras.Sum(p => p.Price);

            return extrasTotalPrice + Price;
        }

    }
}
