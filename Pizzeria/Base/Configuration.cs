using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Base
{
    public class Configuration : ObservableObject
    {
        private string _email;
        public string Email
        {
            get => _email;
            set { SetProperty(ref _email, value); }
        }

        private string _SMTP;
        public string SMTP
        {
            get => _SMTP;
            set { SetProperty(ref _SMTP, value); }
        }

        private int _port;
        public int Port
        {
            get => _port;
            set { SetProperty(ref _port, value); }
        }

    }
}
