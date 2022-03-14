using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_AppBaterias.Data
{
    //Nota: los modos de todas las clases se diferencian por 1 y 0: donde 1 es insert y update y 0 todo lo demas en base de datos
    public class Db
    {
        //valores del servidor de prueba oracle
        public static string ultimoDigitoIp = "6";
        public static string ip = "192.20.6." + ultimoDigitoIp.ToString();//ip ficticio remplazar por uno verdadero
        public static string passprueba = "contrasenaFicticia"; //contrasena ficticia

        //conexion string
        public static string connectionString()
        {
            return "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + ip + ")(PORT = 1521))    ) (CONNECT_DATA = (SERVICE_NAME = VIAMAR)    )  ); Persist Security Info=True;User ID=smart;Password=" + passprueba + ";";
        }


        public static string validarDistribuidor(Model.Distribuidor codigo)
        {
            
            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT inv.num_cliente, c.nombre, c.num_cla, (select count(*) from inv_bateria_distribuidor where estatus = 'D') AS NUM_BATERIAS FROM INV_BATERIA_DISTRIBUIDOR inv, clientes c where inv.num_cliente = c.num_cliente  and inv.local = c.local  and inv.NUM_CLA = c.num_cla and inv.num_cliente = " + codigo.codigo_distribuidor + " and c.estado = 'S'  and c.TIPO_CLIENTE = 'N' and rownum < 2";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();

                da.Fill(dt);

                //proceso que convierte los datos que estan en la tabla en un objeto json
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                 con.Close();

                if(codigo.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }
               
                 
                 

            }
            catch(Exception err) 
            {
                return "mal" + err;
            }

        }


        public static string datosDistribuidor(Model.Distribuidor codigo)
        {
            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                //select que trae cedula o rnc, local y numCla
                cmd.CommandText = "select c.local, nvl(c.cedula, c.rnc) as cedula_rnc, c.num_cla from inv_bateria_distribuidor inv, clientes c where inv.num_cliente = c.num_cliente  and inv.num_cla = c.num_cla and inv.local = c.local  and inv.num_cliente = " + codigo.codigo_distribuidor + " and rownum< 2";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //proceso que convierte los datos que estan en la tabla en un objeto json
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                con.Close(); 

                if (codigo.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }


            }
            catch(Exception err)
            {
                return "mal" + err;
            }

        }


        public static string Fecha_ult_inv(Model.Distribuidor codigo)
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select distinct to_char(max(fecha_inicio),'dd/mm/yyyy') as fecha_ultimo_inventario ,max(id_inv) from hist_inv_bateria_distribuidor where num_cli_distribuidor = "+codigo.codigo_distribuidor+" and estado_inv = 'C'";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                con.Close();

                if (codigo.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }

            }
            catch(Exception err)
            {
                return "mal" + err;
            }


          
        }


        public static dynamic Traer_ult_id_inv()
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select nvl(max(id_inv),0) + 1 as id_inv  from hist_inv_bateria_distribuidor";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                con.Close();

                return dt.Rows[0].ItemArray[0];

            }
            catch(Exception err)
            {
                return "err" + err;
            }

        }


        public static string InsertarInventario(Model.Inventario inventario)
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into hist_inv_bateria_distribuidor (local,id_inv,num_empleado,fecha_inicio,num_cli_distribuidor,serial,estado_inv,geolocalización) values('"+inventario.locEmpleado+"', "+inventario.id_inv+", "+inventario.codEmpleado+", SYSDATE, "+inventario.codigoDistribuidor+", "+inventario.serial+", 'A', '"+inventario.latitude+","+inventario.longitude+"')";

                con.Open();

                if (inventario.mode == "1")
                {
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return "1";
                }
                else
                {
                    con.Close();
                    return "0";
                }
                

            }
            catch(Exception err)
            {
                return "mal" + err;
            }

           

        }


        public static string Seriales_no_encontrados(Model.Inventario inventario)
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select serial from hist_inv_bateria_distribuidor where id_inv = "+inventario.id_inv+"  and estado_inv = 'A' and serial not in (select serie from existencia_serie_baterias)";

                con.Open();

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);


                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                con.Close();

                if(inventario.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }

                

            }
            catch(Exception err)
            {
                return "mal" + err;
            }


            

        }


        public static string Reporte_seccion_A(Model.Inventario inventario)
        {
            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = " select h.serial, a.descripcion || ' ' || a.codigo as descripcion from hist_inv_bateria_distribuidor h, existencia_serie_baterias e, articulos a where h.serial = e.serie and e.local = a.local and e.articulo = a.articulo and id_inv = "+inventario.id_inv+" and e.estatus in ('F', 'FR') and h.estado_inv = 'A'  and h.serial not in(select serial from inv_bateria_distribuidor where num_cliente = "+inventario.codigoDistribuidor+")";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                con.Close();

                if (inventario.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }

            }
            catch(Exception err)
            {
                return "mal" + err;
            }


        }


        public static string Reporte_seccion_B(Model.Inventario inventario)
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT inv.num_cliente, inv.no_docu,inv.LOCAL,ex.SERIE,inv.articulo, a.DESCRIPCION,c.nombre,to_char(inv.FECHA, 'dd/mm/yyyy') as fecha,ex.local as localBate,ex.num_alm FROM INV_BATERIA_DISTRIBUIDOR inv, existencia_serie_baterias ex,clientes c, articulos a  where inv.articulo = ex.articulo and ex.articulo = a.articulo and inv.serial = ex.serie and ex.local = a.local and a.local = c.local  and inv.num_cliente = c.num_cliente and inv.NUM_CLA = c.num_cla and inv.num_cliente = '"+inventario.codigoDistribuidor+"' and inv.estatus = 'D'and inv.serial in (select serial from hist_inv_bateria_distribuidor where estado_inv = 'A' and id_inv = "+inventario.id_inv+") order by inv.num_cliente";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                con.Close();

                if (inventario.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }

            }
            catch(Exception err)
            {
                return "mal"+err;
            }

            

        }


        public static string Reporte_seccion_C(Model.Inventario inventario)
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select r.no_serial, a.descripcion, c.nombre, TO_CHAR(r.fecha_factura, 'dd/mm/yyyy') AS fecha_factura from registro_venta_baterias r, existencia_serie_baterias ex, articulos a, clientes c where ex.articulo = a.articulo and r.num_cliente = c.num_cliente and r.no_serial = ex.serie and ex.local = a.local and a.local = c.local and c.num_cla = r.num_cla and r.fecha_devolucion is null and r.estatus = 'E' and r.no_serial in (select serial from hist_inv_bateria_distribuidor where estado_inv = 'A' and id_inv = "+inventario.id_inv+")";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                con.Close();


                if(inventario.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }

            }
            catch(Exception err)
            {
                return "mal" + err;
            }

            
        }


        public static string Reporte_seccion_D(Model.Inventario inventario)
        {
            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select inv.serial, a.descripcion, inv.no_docu, TO_CHAR(inv.fecha, 'dd/mm/yyyy') as fecha, inv.articulo from inv_bateria_distribuidor inv, articulos a where inv.articulo = a.articulo and inv.local = a.local and inv.estatus = 'D' and inv.num_cliente = '"+inventario.codigoDistribuidor+"' and serial not in(select serial from hist_inv_bateria_distribuidor where estado_inv = 'A' and id_inv = "+inventario.id_inv+")";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                con.Close();

                if (inventario.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }

            }
            catch(Exception err)
            {
                return "mal" + err;
            }


        }


        public static string Seriales_sin_factura_viamar(Model.Inventario inventario)
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from hist_inv_bateria_distribuidor where id_inv = "+inventario.id_inv+"  and estado_inv = 'A' and serial in (select serie from existencia_serie_baterias where estatus <> 'F' and estatus<> 'FR') ";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                con.Close();

                if (inventario.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception err)
            {
                return "mal" + err;
            }

            

        }


        public static string Baterias_fuera_de_servicio(Model.Inventario inventario)
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT inv.no_docu, ex.SERIE,  inv.comentario  FROM INV_BATERIA_DISTRIBUIDOR inv, existencia_serie_baterias ex,clientes c, articulos a where inv.articulo = ex.articulo and ex.articulo = a.articulo and inv.serial = ex.serie  and ex.local = a.local  and a.local = c.local and inv.num_cliente = c.num_cliente and inv.NUM_CLA = c.num_cla and inv.num_cliente = '"+inventario.codigoDistribuidor+"' and inv.estatus = 'F'  and inv.serial in (select serial from hist_inv_bateria_distribuidor where estado_inv = 'A' and id_inv = "+inventario.id_inv+") order by inv.num_cliente ";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                con.Close();


                if (inventario.mode == "0")
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(rows);
                }
                else
                {
                    return "0";
                }

            }
            catch(Exception err)
            {
                return "mal" + err;
            }


        }

        //pendiente de revisar logica de insersion
        public static string Rotacion_individual(Model.Reporte reporte)
        {
            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select max(secuencial) +1 as maxnum  from enc_sol_bateria";


                /*
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string prueba = dr[0].ToString();
                }
                dr.Close();
                */


                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                string Max_num_sol_rotacion = dt.Rows[0].ItemArray[0].ToString();


                //------------------------
                
                //insertar encabesado solicitud rotacion baterias
                cmd.Connection = con;
                cmd.CommandText = "insert into enc_sol_bateria (id_sol,secuencial,local,num_cla,num_cliente,fecha,usuario,estatus,NUM_ALM) values('FRB" + Max_num_sol_rotacion + "'," + Max_num_sol_rotacion + ",'" + reporte.localBateria + "','30', " + reporte.codigoDistribuidor + ",SYSDATE,'WEB','S','" + reporte.num_alm + "')";
                
                con.Open();

               int result = cmd.ExecuteNonQuery();
                
                if (reporte.mode == "1" && result > 0)
                {

                    //insertar detalle rotacion baterias
                    cmd.CommandText = "insert into det_sol_bateria (id_sol, articulo, serie, cantidad, factura, local, estado) values('FRB"+Max_num_sol_rotacion+"', '"+reporte.articulo+"', "+reporte.serial+",1,'"+reporte.factura_bateria+"','"+reporte.localBateria+"', 'S' )";
                    
                    int result2 = cmd.ExecuteNonQuery();
                    if(result > 0)
                    {
                        con.Close();
                        return "1";
                    }

                    return "0"; 
                }
                else
                {
                    return "0";
                }


            }
            catch(Exception err)
            {
                return "mal"+err;
            }

       
        }


        public static string Rotacion_multiples(Model.Reporte reporte)
        {
            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select max(secuencial) +1 as maxnum  from enc_sol_bateria";

                OracleDataAdapter da = new OracleDataAdapter(cmd.CommandText, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                string Max_num_sol_rotacion = dt.Rows[0].ItemArray[0].ToString();


                //------------------------

                //insertar encabesado solicitud rotacion baterias
                cmd.Connection = con;
                cmd.CommandText = "insert into enc_sol_bateria (id_sol,secuencial,local,num_cla,num_cliente,fecha,usuario,estatus,NUM_ALM) values('FRB" + Max_num_sol_rotacion + "'," + Max_num_sol_rotacion + ",'" + reporte.localBateria + "','30', " + reporte.codigoDistribuidor + ",SYSDATE,'WEB','S','" + reporte.num_alm + "')";

                con.Open();

                int result = cmd.ExecuteNonQuery();

                if (reporte.mode == "1" && result > 0)
                {

                    //insertar detalle rotacion baterias
                    cmd.CommandText = "insert into det_sol_bateria (id_sol,articulo,serie,cantidad,factura,local,estado) SELECT 'FRB"+Max_num_sol_rotacion+"' AS ID_SOL,inv.articulo,ex.SERIE,'1' AS CANTIDAD,inv.no_docu,inv.LOCAL,'S' AS ESTADO FROM INV_BATERIA_DISTRIBUIDOR inv, existencia_serie_baterias ex, clientes c, articulos a where inv.articulo = ex.articulo and ex.articulo = a.articulo and inv.serial = ex.serie and ex.local = a.local and a.local = c.local and inv.num_cliente = c.num_cliente and inv.NUM_CLA = c.num_cla and inv.num_cliente = '"+reporte.codigoDistribuidor+"' and inv.estatus = 'D' and inv.serial in ("+reporte.serial+")";
                    cmd.ExecuteNonQuery();

                    con.Close();

                    return "1";
                }
                else
                {
                    return "0";
                }


            }
            catch (Exception err)
            {
                return "mal" + err;
            }

        }



        public static string Bateria_descompuesta(Model.Reporte reporte)
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update inv_bateria_distribuidor set estatus = 'F',comentario = '"+reporte.comentario_observacion+"' where serial = "+reporte.serial+" ";

                con.Open();

                int result = cmd.ExecuteNonQuery();

                if(reporte.mode == "1" && result > 0)
                {
                    con.Close();
                    return "1";
                }
                else
                {
                    con.Close();
                    return "0";
                }

            }
            catch(Exception err)
            {
                return "mal" + err;
            }


        }



        public static string Rehabilitar(Model.Reporte reporte)
        {
            OracleConnection con = new OracleConnection(connectionString());
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update registro_venta_baterias set fecha_devolucion = sysdate, estatus = 'C', OBSERVACION_WEB = '"+reporte.comentario_observacion+"' where no_serial = '"+reporte.serial+"' and estatus = 'E'";

            con.Open();

            int result = cmd.ExecuteNonQuery();

            if(reporte.mode == "1" && result > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }

  
        }



        public static string Registrar_venta_al_distribuidor(Model.Reporte reporte)
        {

            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into registro_venta_baterias (local, cedula_rnc, nombre,  numero_factura, fecha_factura, no_serial, num_cliente, fecha_registro, articulo, estatus, tipo_uso_comentario,  num_cla, tipo_uso ) values('"+reporte.localBateria+"', '"+reporte.cedula+"', '"+reporte.nombre+"', 'FT99999', sysdate, '"+reporte.serial+"', '"+reporte.codigoDistribuidor+"', sysdate, '" + reporte.articulo + "', 'E', 'Registrada por inventario web', " + reporte.numCla+", 'OTROS')";

                con.Open();

                int result = cmd.ExecuteNonQuery();

                if (reporte.mode == "1" && result > 0)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }

            }
            catch(Exception err)
            {
                return "mal" + err;
            }

            
            
        }



        public static string Finalizar_reporte_inventario(Model.Inventario inventario)
        {
            try
            {

                OracleConnection con = new OracleConnection(connectionString());
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update hist_inv_bateria_distribuidor set estado_inv = 'C', fecha_finaliza = sysdate where id_inv = "+inventario.id_inv+" ";

                con.Open();

                int result = cmd.ExecuteNonQuery();

                if(inventario.mode == "1" && result > 0)
                {
                    con.Close();
                    return "1";
                }
                else
                {
                    con.Close();
                    return "0";
                }
                

            }
            catch(Exception err)
            {
                return "mal" + err;
            }

            
        }

    }
}
