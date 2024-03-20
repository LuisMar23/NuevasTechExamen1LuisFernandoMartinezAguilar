using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1LuisMartinez.Interfaces
{
    public interface IProveedorModel
    {
        //public string Id { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }
        public string estado { get; set; };
    }
}
