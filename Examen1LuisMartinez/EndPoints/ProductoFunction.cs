using Examen1LuisMartinez.Contratos.Repositorios;
using Examen1LuisMartinez.Modelos;
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

namespace Examen1LuisMartinez.EndPoints
{
    public class ProductoFunction
    {
        private readonly ILogger<ProductoFunction> _logger;
        private readonly IProducto repos;

        public ProductoFunction(ILogger<ProductoFunction> logger, IProducto repos)
        {
            _logger = logger;
            this.repos = repos;
        }
        [Function("InsertarProducto")]
        [OpenApiOperation("Listarspec", "InsertarProducto", Description = "Sirve para insertar Producto")]
        [OpenApiRequestBody("application/json", typeof(Producto),
           Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Producto),
            Description = "Insertara un Producto")]
        public async Task<HttpResponseData> InsertarProducto([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuetsa;
            try
            {
                var registro = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar una Producto con todos sus datos");
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
        [Function("ListarProducto")]
        [OpenApiOperation("Listarspec", "ListarProducto", Description = "Sirve para listar Producto")]
        [OpenApiRequestBody("application/json", typeof(Producto),
           Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Producto),
            Description = "Listara los Producto")]
        public async Task<HttpResponseData> ListarProducto([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
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
        [Function("obtenerProducto")]
        [OpenApiOperation("Listarspec", "obtenerProducto", Description = "Sirve para obtener un Producto")]
        [OpenApiRequestBody("application/json", typeof(Producto),
           Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Producto),
            Description = "Obtendra un Producto")]
        public async Task<HttpResponseData> ObtenerProducto([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenerProducto/{id}")] HttpRequestData req, string id)
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

        [Function("ModificarProducto")]
        [OpenApiOperation("Listarspec", "ModificarProducto", Description = "Sirve para modificar un Producto")]
        [OpenApiRequestBody("application/json", typeof(Producto),
           Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Producto),
            Description = "Modificara un Producto")]
        public async Task<HttpResponseData> ModificarProducto(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarProducto/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData respuesta;
            try
            {
                var Producto = await repos.Obtener(id);
                if (Producto == null)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.NotFound);
                    return respuesta;
                }

                var d = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar los datos de la institución a modificar");

                Producto.PartitionKey = d.PartitionKey;
                Producto.Nombre = d.Nombre;
                Producto.Precio = d.Precio;
                Producto.estado = d.estado;
                Producto.Timestamp = DateTime.UtcNow;
                bool resultado = await repos.Actualizar(Producto);
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

        [Function("EliminarProducto")]
        [OpenApiOperation("Listarspec", "EliminarProducto", Description = "Sirve para eliminar un Producto")]
        [OpenApiRequestBody("application/json", typeof(Producto),
           Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(Producto),
            Description = "Eliminara un Producto")]
        public async Task<HttpResponseData> EliminarProducto(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Productoes/{partitionKey}/{rowKey}")] HttpRequestData req,
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
