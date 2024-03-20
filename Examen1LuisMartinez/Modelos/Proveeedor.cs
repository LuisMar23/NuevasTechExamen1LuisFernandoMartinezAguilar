using Azure;
using Azure.Data.Tables;
using Examen1LuisMartinez.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1LuisMartinez.Modelos
{
    public class Proveedor : IProveedorModel, ITableEntity
    {
        public string Nombre { get ; set ; }
        public string Direccion { get ; set ; }
        public string estado { get; set; }
        public string PartitionKey { get ; set ; }
        public string RowKey { get ; set ; }
        public DateTimeOffset? Timestamp { get ; set ; }
        public ETag ETag { get ; set ; }

    }
}
