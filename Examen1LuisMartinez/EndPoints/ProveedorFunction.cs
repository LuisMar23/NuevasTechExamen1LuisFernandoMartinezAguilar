using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Examen1LuisMartinez.Contratos.Repositorios;
using Examen1LuisMartinez.Modelos;

namespace Examen1LuisMartinez.EndPoints
{
    public class ProveedorFunction
    {
        private readonly ILogger<ProveedorFunction> _logger;
        private readonly IProveedor repos;

        public ProveedorFunction(ILogger<ProveedorFunction> logger, IProveedor repos)
        {
            _logger = logger;
            this.repos = repos;
        }
        [Function("InsertarProveedor")]
        [OpenApiOperation("Listarspec", "InsertarProveedor", Description = "Sirve para insertar Proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor),
           Description = "Proveedor modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Proveedor),
            Description = "Insertara un proveedor")]
        public async Task<HttpResponseData> InsertarProveedor([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuetsa;
            try
            {
                var registro = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar una Proveedor con todos sus datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;

                bool sw = await repos.Insertar(registro);
                if (sw)
                {
                    respuetsa = req.CreateResponse(HttpStatusCode.OK);
                    return respuetsa;
                }
                else
                {
                    respuetsa = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuetsa;
                }
            }
            catch (Exception)
            {

                respuetsa = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuetsa;
            }
        }
        [Function("ListarProveedor")]
        [OpenApiOperation("Listarspec", "ListarProveedor", Description = "Sirve para listar Proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor),
           Description = "Proveedor modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Proveedor),
            Description = "Listara los Proveedor")]
        public async Task<HttpResponseData> ListarProveedor([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData respuetsa;
            try
            {
                var lista = repos.Listar();
                respuetsa = req.CreateResponse(HttpStatusCode.OK);
                await respuetsa.WriteAsJsonAsync(lista.Result);
                return respuetsa;
            }
            catch (Exception)
            {

                respuetsa = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuetsa;
            }
        }
        [Function("obtenerProveedor")]
        [OpenApiOperation("Listarspec", "obtenerProveedor", Description = "Sirve para obtener un proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor),
           Description = "Proveedor modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Proveedor),
            Description = "Obtendra un proveedor")]
        public async Task<HttpResponseData> ObtenerProveedor([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerProveedor/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuetsa;
            try
            {
                var lista = repos.Obtener(id);
                respuetsa = req.CreateResponse(HttpStatusCode.OK);
                await respuetsa.WriteAsJsonAsync(lista.Result);
                return respuetsa;
            }
            catch (Exception)
            {

                respuetsa = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuetsa;
            }
        }

        [Function("ModificarProveedor")]
        [OpenApiOperation("Listarspec", "ModificarProveedor", Description = "Sirve para modificar un proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor),
           Description = "Proveedor modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Proveedor),
            Description = "Modificara un proveedor")]
        public async Task<HttpResponseData> ModificarProveedor(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarProveedor/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData respuesta;
            try
            {
                var Proveedor = await repos.Obtener(id);
                if (Proveedor == null)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.NotFound);
                    return respuesta;
                }

                var d = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar los datos de la institución a modificar");

                //Proveedor.PartitionKey = d.PartitionKey;
                Proveedor.Nombre=d.Nombre;
                Proveedor.Direccion=d.Direccion;
                Proveedor.estado = d.estado;
                Proveedor.Timestamp = DateTime.UtcNow;
                bool resultado = await repos.Actualizar(Proveedor);
                if (resultado)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("EliminarProveedor")]
        [OpenApiOperation("Listarspec", "EliminarProveedor", Description = "Sirve para eliminar un proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor),
           Description = "Proveedor modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Proveedor),
            Description = "Eliminara un proveedor")]
        public async Task<HttpResponseData> EliminarProveedor(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Proveedores/{partitionKey}/{rowKey}")] HttpRequestData req,
            string partitionKey,
            string rowKey)
        {
            HttpResponseData respuesta;
            try
            {
                bool resultado = await repos.Eliminar(partitionKey, rowKey);
                if (resultado)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return respuesta;
        }
    }
}
