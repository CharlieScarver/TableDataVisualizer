using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListVisualizer.Model
{
    public class TableDataRow
    {
        public TableDataRow(List<string> cells)
        {
            Cells = cells;
        }

        public List<string> Cells { get; }
    }
}
