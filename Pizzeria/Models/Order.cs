using Pizzeria.Base;
using Pizzeria.Models.Meals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public class Order : ObservableObject
    {
        public Order()
        {
            Positions = new ObservableCollection<OrderPosition>();
            Positions.CollectionChanged += UpdateTotalPricee;
        }



        private string _recipient;
        public string Recipient
        {
            get => _recipient;
            set { SetProperty(ref _recipient, value); }
        }

        private string _remarks;
        public string Remarks
        {
            get => _remarks;
            set { SetProperty(ref _remarks, value); }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get => _totalPrice;
            private set { SetProperty(ref _totalPrice, value); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set { SetProperty(ref _date, value); }
        }

        private ObservableCollection<OrderPosition> _positions;
        public ObservableCollection<OrderPosition> Positions
        {
            get => _positions;
            set { SetProperty(ref _positions, value); }
        }

        private OrderPosition _selectedPosition;
        public OrderPosition SelectedPosition
        {
            get => _selectedPosition;
            set { SetProperty(ref _selectedPosition, value); }
        }

        private void UpdateTotalPricee(object sender, NotifyCollectionChangedEventArgs e)
        {
            TotalPrice = Positions.Sum(p => p.GetTotalPrice());
        }
    }
}
