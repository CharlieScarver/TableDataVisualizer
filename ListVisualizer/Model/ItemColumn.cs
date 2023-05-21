using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListVisualizer.Model
{
    public class ItemColumn
    {
        public ItemColumn() { }

        public ItemColumn(string name, bool isVisible)
        {
            Name = name;
            IsVisible = isVisible;
        }

        public string Name { get; set; }

        public bool IsVisible { get; set; }
    }
}
