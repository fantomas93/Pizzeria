using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Models.Extras
{
    public interface IExtra
    {
        string Name { get; set; }
        double Price { get; set; }
        ExtraType Type { get; }
    }
}
