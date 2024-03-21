using Azure.Data.Tables;
using Examen1LuisMartinez.Contratos.Repositorios;
using Examen1LuisMartinez.Modelos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1LuisMartinez.Implementacion
{
    public class ProductoRepositorio:IProducto
    {

        private readonly string cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;
        public ProductoRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Producto";
        }

        public async Task<bool> Actualizar(Producto Producto)
        {
            try
            {
                var TablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await TablaCliente.UpdateEntityAsync(Producto, Producto.ETag);
                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }

        public async Task<bool> Eliminar(string partitionkey, string rowkey)
        {
            try
            {
                var TablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await TablaCliente.DeleteEntityAsync(partitionkey, rowkey);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Insertar(Producto Producto)
        {
            try
            {
                var TablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await TablaCliente.UpsertEntityAsync(Producto);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<Producto>> Listar()
        {
            List<Producto> lista = new List<Producto>();
            var TablaCliente = new TableClient(cadenaConexion, tablaNombre);
            await foreach (Producto Producto in TablaCliente.QueryAsync<Producto>())
            {
                lista.Add(Producto);
            }
            return lista;

        }

        public async Task<Producto> Obtener(string id)
        {
            var TablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq  RowKey eq '{id}'";
            await foreach (Producto Producto in TablaCliente.QueryAsync<Producto>(filter: filtro))
            {
                return Producto;
            }
            return null;
        }
    }
}
