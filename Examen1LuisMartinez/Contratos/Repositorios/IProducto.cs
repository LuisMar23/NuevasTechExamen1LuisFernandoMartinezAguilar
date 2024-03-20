using Examen1LuisMartinez.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1LuisMartinez.Contratos.Repositorios
{
    public interface IProducto
    {
        public Task<List<Producto>> Listar();
        public Task<Producto> Obtener(string id);
        public Task<bool> Insertar(Producto producto);
        public Task<bool> Actualizar(Producto producto);
        public Task<bool> Eliminar(string partitionkey, string rowkey);
    }
}
