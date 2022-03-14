using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_AppBaterias.Data;

namespace WebApi_AppBaterias.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidarDistribuidorController : Controller
    {
        // valido que el distribuidor exista en base de datos 
        [HttpPost]
        [Route("Validar")]
        public ActionResult Validar([FromBody] Model.Distribuidor codigoDis)
        {

            return Ok(Db.validarDistribuidor(codigoDis));
        }

        
        [HttpPost]
        [Route("DatosCliente")]
        public ActionResult DatosCliente([FromBody] Model.Distribuidor codigoDis)
        {
            return Ok(Db.datosDistribuidor(codigoDis));
        }

        //fecha de ultimo inventario y el numero de id inventario
        [HttpPost]
        [Route("Fecha_ultimo_inventario")]
        public ActionResult Fecha_ultimo_inventario([FromBody] Model.Distribuidor codigoDis)
        {
            return Ok(Db.Fecha_ult_inv(codigoDis));
        }


    }
}
