using Microsoft.AspNetCore.Mvc;

namespace WebApi_AppBaterias.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : Controller
    {

        [HttpGet]
        [Route("MaxIdInv")]
        public ActionResult MaxIdInv()
        {
            return Ok(Data.Db.Traer_ult_id_inv());
        }

        // POST: InventarioController/Create
        [HttpPost]
        [Route("GuardarInventario")]
        public ActionResult GuardarInventario([FromBody] Model.Inventario inv)
        {
            return Ok(Data.Db.InsertarInventario(inv));
        }


        [HttpPost]
        [Route("SerialesNoEncontrados")]
        public ActionResult SerialesNoEncontrados([FromBody] Model.Inventario inv)
        {
            return Ok(Data.Db.Seriales_no_encontrados(inv));
        }


        [HttpPost]
        [Route("ReporteSeccionA")]
        public ActionResult ReporteSeccionA([FromBody] Model.Inventario inv)
        {
            return Ok(Data.Db.Reporte_seccion_A(inv));
        }


        [HttpPost]
        [Route("ReporteSeccionB")]
        public ActionResult ReporteSeccionB([FromBody] Model.Inventario inv)
        {
            return Ok(Data.Db.Reporte_seccion_B(inv));
        }


        [HttpPost]
        [Route("ReporteSeccionC")]
        public ActionResult ReporteSeccionC([FromBody] Model.Inventario inv)
        {
            return Ok(Data.Db.Reporte_seccion_C(inv));
        }


        [HttpPost]
        [Route("ReporteSeccionD")]
        public ActionResult ReporteSeccionD([FromBody] Model.Inventario inv)
        {
            return Ok(Data.Db.Reporte_seccion_D(inv));
        }


        [HttpPost]
        [Route("Seriales_sin_Factura")]
        public ActionResult Seriales_sin_Factura([FromBody] Model.Inventario inv)
        {
            return Ok(Data.Db.Seriales_sin_factura_viamar(inv));
        }


        [HttpPost]
        [Route("BateriasDeBaja")]
        public ActionResult BateriasDeBaja([FromBody] Model.Inventario inv)
        {
            return Ok(Data.Db.Baterias_fuera_de_servicio(inv));
        }


        [HttpPost]
        [Route("FinalizarInventario")]
        public ActionResult FinalizarInventario([FromBody] Model.Inventario inv)
        {
            return Ok(Data.Db.Finalizar_reporte_inventario(inv));
        }

    }
}
