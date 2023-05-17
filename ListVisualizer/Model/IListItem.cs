using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListVisualizer.Model
{
    public interface IListItem
    {
        public string Id { get; }

        public string Name { get; }
    }
}
