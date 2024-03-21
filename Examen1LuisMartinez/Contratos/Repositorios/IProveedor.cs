using Examen1LuisMartinez.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1LuisMartinez.Contratos.Repositorios
{
    public interface IProveedor
    {
        public Task<List<Proveedor>> Listar();
        public Task<Proveedor> Obtener(string id);
        public Task<bool> Insertar(Proveedor proveedor);
        public Task<bool> Actualizar(Proveedor proveedor);
        public Task<bool> Eliminar(string partitionkey, string rowkey);
    }
}
