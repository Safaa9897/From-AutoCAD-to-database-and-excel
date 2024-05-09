using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cad_API_Project.Command
{
    public abstract class ICadCommand
    {
        /// <summary>
        /// Execute a function in cad application
        /// </summary>
        public abstract void Execute();
    }
}
