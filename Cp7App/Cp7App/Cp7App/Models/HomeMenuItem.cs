using System;
using System.Collections.Generic;
using System.Text;

namespace Cp7App.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        SmartConfig
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
