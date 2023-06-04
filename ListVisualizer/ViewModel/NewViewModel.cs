using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListVisualizer.ViewModel
{
    public class NewViewModel : ListViewModel
    {
        protected override void ToggleCheckbox(object itemColumn)
        {
            //base.ToggleCheckbox(itemColumn);
            Console.WriteLine("Do nothing instead");
        }
    }
}
