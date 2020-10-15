using Newtonsoft.Json;
using Pizzeria.Models;
using Pizzeria.Models.Extras;
using Pizzeria.Models.Meals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Base
{
    public class BaseModel : ObservableObject
    {
        private Order _newOrder;
        public Order NewOrder
        {
            get => _newOrder;
            set { SetProperty(ref _newOrder, value); }
        }

        private Configuration _configuration;
        public Configuration Configuration
        {
            get => _configuration;
            set { SetProperty(ref _configuration, value); }
        }

        private bool _isSettingsVisible;
        public bool IsSettingsVisible
        {
            get => _isSettingsVisible;
            set { SetProperty(ref _isSettingsVisible, value); }
        }

        private ObservableCollection<MenuPosition> _menuPositions;
        public ObservableCollection<MenuPosition> MenuPositions
        {
            get => _menuPositions;
            set { SetProperty(ref _menuPositions, value); }
        }

        private ObservableCollection<Order> _historyOrders;
        public ObservableCollection<Order> HistoryOrders
        {
            get => _historyOrders;
            set { SetProperty(ref _historyOrders, value); }
        }

        private Order _selectedHistoryOrder;
        public Order SelectedHistoryOrder
        {
            get => _selectedHistoryOrder;
            set { SetProperty(ref _selectedHistoryOrder, value); }
        }

        

        internal void Initialize()
        {
            InitializeMenu();
            InitializeExtras();
            LoadHistoryOrders();
            NewOrder = new Order();
        }

        internal void LoadHistoryOrders()
        {
            if (File.Exists("historyOrders.json"))
            {
                var deserializedHistoryList = JsonConvert.DeserializeObject<List<Order>>(File.ReadAllText("historyOrders.json"), 
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                HistoryOrders = new ObservableCollection<Order>(deserializedHistoryList);
            }
        }

        internal void SaveHistoryOrders()
        {
            if (!File.Exists("historyOrders.json"))
            {
                File.Create("historyOrders.json");
            }

            string json = JsonConvert.SerializeObject(HistoryOrders.ToList(), 
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

            File.WriteAllText("historyOrders.json", json);

        }

        internal string GetEmailBody()
        {
            string body = "Hello \nYour order: \n";

            foreach (var orderPosition in NewOrder.Positions)
            {
                body += " - " + orderPosition.Name;
                if (orderPosition.Extras != null)
                {
                    body += " with Extras:\n";
                    body += string.Join(", ", orderPosition.Extras.Select(p => p.Name)) + "\n";
                }


                body += "\n";
            }
            body += string.Format("Total price: {0}zł", NewOrder.TotalPrice);
            return body;
        }

        private void InitializeExtras()
        {
            AllExtras = GetBaseExtras();
        }

        private ObservableCollection<IExtra> GetBaseExtras()
        {
            return new ObservableCollection<IExtra>()
            {
                new MainDishExtra("Salads", 5d),
                new MainDishExtra("Sauce", 6d),

                new PizzaExtra("Double cheese", 2d),
                new PizzaExtra("Salami", 2d),
                new PizzaExtra("Ham", 2d),
                new PizzaExtra("Mushrooms", 2d)
            };
        }

        private ObservableCollection<IExtra> _allExtras;
        public ObservableCollection<IExtra> AllExtras
        {
            get => _allExtras;
            set { SetProperty(ref _allExtras, value); }
        }

        private ObservableCollection<IExtra> _actualExtras;
        public ObservableCollection<IExtra> ActualExtras
        {
            get => _actualExtras;
            set { SetProperty(ref _actualExtras, value); }
        }

        private ObservableCollection<IExtra> _selectedExtras;
        public ObservableCollection<IExtra> SelectedExtras
        {
            get => _selectedExtras;
            set { SetProperty(ref _selectedExtras, value); }
        }

        private IExtra _selectedExtra;
        public IExtra SelectedExtra
        {
            get => _selectedExtra;
            set { SetProperty(ref _selectedExtra, value); }
        }

        private ObservableCollection<OrderPosition> _orderPositions;
        public ObservableCollection<OrderPosition> OrderPositions
        {
            get => _orderPositions;
            set { SetProperty(ref _orderPositions, value); }
        }

        private ObservableCollection<MenuPosition> _actualMenuPositions;
        public ObservableCollection<MenuPosition> ActualMenuPositions
        {
            get => _actualMenuPositions;
            set { SetProperty(ref _actualMenuPositions, value); }
        }

        private MenuPosition _selectedMenuPosition;
        public MenuPosition SelectedMenuPosition
        {
            get => _selectedMenuPosition;
            set { SetProperty(ref _selectedMenuPosition, value); UpdateExtras(); }
        }

        private void UpdateExtras()
        {
            ActualExtras = null;
            SelectedExtras = null;
            if (SelectedMenuPosition != null)
            {
                switch (SelectedMenuPosition.Type)
                {
                    case PositionType.Pizza:
                        ActualExtras = new ObservableCollection<IExtra>(AllExtras.Where(p => p.Type == ExtraType.Pizza).ToList());
                        break;
                    case PositionType.MainMeal:
                        ActualExtras = new ObservableCollection<IExtra>(AllExtras.Where(p => p.Type == ExtraType.MainMeal).ToList());
                        break;
                    case PositionType.Soup:
                        break;
                    case PositionType.Drink:
                        break;
                    default:
                        break;
                }
            }
        }

        private string _searchPhrase;
        public string SearchPhrase
        {
            get => _searchPhrase;
            set { SetProperty(ref _searchPhrase, value); RefreshAcutalMenuPositions(_searchPhrase); }
        }

        private void RefreshAcutalMenuPositions(string phrase)
        {
            ActualMenuPositions.Clear();

            if (string.IsNullOrEmpty(phrase))
            {
                ActualMenuPositions = new ObservableCollection<MenuPosition>(MenuPositions.ToList());
                return;
            }


            foreach (var item in MenuPositions.Where(p => p.Name.Contains(phrase, StringComparison.InvariantCultureIgnoreCase)))
            {
                ActualMenuPositions.Add(item);
            }
        }

        private void InitializeMenu()
        {
            MenuPositions = GetBaseMenu();
            ActualMenuPositions = GetBaseMenu();
        }

        private ObservableCollection<MenuPosition> GetBaseMenu()
        {
            return new ObservableCollection<MenuPosition>()
            {
                new MenuPosition("Margheritta", 20d, PositionType.Pizza),
                new MenuPosition("Vegetariana", 22d, PositionType.Pizza),
                new MenuPosition("Tosca", 25d, PositionType.Pizza),
                new MenuPosition("Venecia", 25d, PositionType.Pizza),

                new MenuPosition("Pork chop with fries/rice/potatoes", 30d, PositionType.MainMeal),
                new MenuPosition("Fish and chips", 28d, PositionType.MainMeal),
                new MenuPosition("Hungarian Cake", 27d, PositionType.MainMeal),

                new MenuPosition("Tomato soup", 12d, PositionType.Soup),
                new MenuPosition("Chicken soup", 10d, PositionType.Soup),

                new MenuPosition("Coffee", 5d, PositionType.Drink),
                new MenuPosition("Tea", 5d, PositionType.Drink),
                new MenuPosition("Cola", 5d, PositionType.Drink)

            };
        }

        public BaseCommand OpenSettings { get; set; }
        public BaseCommand SaveSettings { get; set; }
        public BaseCommand AddExtra { get; set; }
        public BaseCommand RemoveExtra { get; set; }
        public BaseCommand AddPosition { get; set; }
        public BaseCommand RemovePosition { get; set; }

        public MainWindow Window { get; internal set; }

        public BaseModel()
        {
            InitializeCommands();
        }

        public void LoadConfiguration()
        {
            try
            {
                if (File.Exists("configuration.json"))
                {
                    Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("configuration.json"));
                }
                else
                {
                    Configuration = GetDefaultConfiguration();
                    string json = JsonConvert.SerializeObject(Configuration);
                    File.WriteAllText("configuration.json", json);
                }
            }
            catch (Exception ex)
            {
                Window.ShowMessage("Error during load configuration", ex.Message);
            }
        }

        private void InitializeCommands()
        {
            OpenSettings = new BaseCommand(ExecOpenSettings);
            SaveSettings = new BaseCommand(ExecSaveSettings);
            AddExtra = new BaseCommand(ExecAddExtra);
            RemoveExtra = new BaseCommand(ExecRemoveExtra);
            AddPosition = new BaseCommand(ExecAddPosition);
            RemovePosition = new BaseCommand(ExecRemovePosition);
        }

        private void ExecRemovePosition(object obj)
        {
            var selectedPosition = obj as OrderPosition;
            if (selectedPosition != null)
            {
                if (NewOrder != null && NewOrder.Positions != null)
                    NewOrder.Positions.Remove(selectedPosition);
            }
        }

        private void ExecAddPosition(object obj)
        {
            if (SelectedMenuPosition == null)
                return;

            if (NewOrder.Positions == null)
                NewOrder.Positions = new ObservableCollection<OrderPosition>();

            NewOrder.Positions.Add(GetOrderPosition(SelectedMenuPosition));

            SelectedMenuPosition = null;
        }

        private OrderPosition GetOrderPosition(MenuPosition selectedMenuPosition)
        {
            if (selectedMenuPosition == null)
                return null;

            OrderPosition orderPosition = null;
            switch (selectedMenuPosition.Type)
            {
                case PositionType.Pizza:
                    orderPosition = new Pizza(selectedMenuPosition.Name, selectedMenuPosition.Price);
                    break;
                case PositionType.MainMeal:
                    orderPosition = new MainMeal(selectedMenuPosition.Name, selectedMenuPosition.Price);
                    break;
                case PositionType.Soup:
                    orderPosition = new Soup(selectedMenuPosition.Name, selectedMenuPosition.Price);
                    break;
                case PositionType.Drink:
                    orderPosition = new Drink(selectedMenuPosition.Name, selectedMenuPosition.Price);
                    break;
                default:
                    break;
            }
            if (SelectedExtras != null)
                orderPosition.Extras = new List<IExtra>(SelectedExtras);

            return orderPosition;
        }

        private void ExecRemoveExtra(object obj)
        {
            var selectedExtra = (obj as IExtra);
            if (selectedExtra != null)
            {
                SelectedExtras.Remove(selectedExtra);
            }
        }

        private void ExecAddExtra(object obj)
        {
            if (SelectedExtra != null)
            {
                if (SelectedExtras == null)
                    SelectedExtras = new ObservableCollection<IExtra>();

                SelectedExtras.Add(SelectedExtra);
            }
        }

        private void ExecSaveSettings(object obj)
        {
            try
            {
                string json = JsonConvert.SerializeObject(Configuration);
                File.WriteAllText("configuration.json", json);
                Window.ShowMessage("Success", "Settings has been saved");
            }
            catch (Exception ex)
            {
                Window.ShowMessage("Error during saving settings", ex.Message);
            }

        }

        private void ExecOpenSettings(object obj)
        {
            IsSettingsVisible = !IsSettingsVisible;
        }

        private Configuration GetDefaultConfiguration()
        {
            Configuration configuration = new Configuration();

            return configuration;
        }
    }
}
