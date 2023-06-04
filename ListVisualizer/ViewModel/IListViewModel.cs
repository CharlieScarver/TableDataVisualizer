using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ListVisualizer.ViewModel
{
    public interface IListViewModel
    {
        ICommand CmdFillDataGrid { get; }
        ICommand CmdToggleCheckbox { get; }
        ICommand CmdFetchTables { get; }
    }
}
