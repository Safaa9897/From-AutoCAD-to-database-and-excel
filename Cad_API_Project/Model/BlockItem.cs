using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cad_API_Project.Model
{
    public class BlockItem
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BlockCount { get; set; }
    }
}
