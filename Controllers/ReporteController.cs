using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_AppBaterias.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : Controller
    {
        
        [HttpPost]
        [Route("RotacionIndividual")]
        public ActionResult RotacionIndividual([FromBody] Model.Reporte reporte)
        {
            return Ok(Data.Db.Rotacion_individual(reporte));
        }


        [HttpPost]
        [Route("RotacionMultiples")]
        public ActionResult RotacionMultiples([FromBody] Model.Reporte reporte)
        {
            return Ok(Data.Db.Rotacion_multiples(reporte));
        }


        [HttpPost]
        [Route("InhabilitarBateria")]
        public ActionResult InhabilitarBateria([FromBody] Model.Reporte reporte)
        {
            return Ok(Data.Db.Bateria_descompuesta(reporte));
        }


        [HttpPost]
        [Route("RehabilitarSerial")]
        public ActionResult RehabilitarSerial([FromBody] Model.Reporte reporte)
        {
            return Ok(Data.Db.Rehabilitar(reporte));
        }


        [HttpPost]
        [Route("RegistrarAutoVenta")]
        public ActionResult RegistrarAutoVenta([FromBody] Model.Reporte reporte)
        {
            return Ok(Data.Db.Registrar_venta_al_distribuidor(reporte));
        }


    }
}
