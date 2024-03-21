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
    public class ProveedorRepositorio:IProveedor
    {

        private readonly string cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;
        public ProveedorRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Proveedor";
        }

        public async Task<bool> Actualizar(Proveedor Proveedor)
        {
            try
            {
                var TablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await TablaCliente.UpdateEntityAsync(Proveedor, Proveedor.ETag);
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

        public async Task<bool> Insertar(Proveedor Proveedor)
        {
            try
            {
                var TablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await TablaCliente.UpsertEntityAsync(Proveedor);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<Proveedor>> Listar()
        {
            List<Proveedor> lista = new List<Proveedor>();
            var TablaCliente = new TableClient(cadenaConexion, tablaNombre);
            await foreach (Proveedor Proveedor in TablaCliente.QueryAsync<Proveedor>())
            {
                lista.Add(Proveedor);
            }
            return lista;

        }

        public async Task<Proveedor> Obtener(string id)
        {
            var TablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq  RowKey eq '{id}'";
            await foreach (Proveedor Proveedor in TablaCliente.QueryAsync<Proveedor>(filter: filtro))
            {
                return Proveedor;
            }
            return null;
        }
    }
}
