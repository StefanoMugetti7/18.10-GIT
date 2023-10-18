using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Mailing.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using Servicio.AccesoDatos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using ProcesosDatos.Entidades;

namespace Mailing.LogicaNegocio
{
    class TGEMailingLN : BaseLN<TGEMailing>
    {
        public override TGEMailing ObtenerDatosCompletos(TGEMailing pParametro)
        {


            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TGEMailing>("TGEMailingSeleccionar", pParametro);

            pParametro.MailingAdjuntos = BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEMailingAdjuntos>("TGEMailingAdjuntosSeleccionar", pParametro);
            pParametro.DetalleEnvioProcesamiento = BaseDatos.ObtenerBaseDatos().ObtenerLista("TGEMailingProcesamientosSeleccionar", pParametro);
            pParametro.MailingProcesos.Parametros = BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEMailingParametros>("TGEMailingParametrosSeleccionarDescripcionesPorTGEMailingProcesos", pParametro);
            pParametro.DetalleEnvio = BaseDatos.ObtenerBaseDatos().ObtenerLista("TGEMailingProcesamientosSeleccionarDetalles", pParametro);

            return pParametro;
        }

        public TGEMailingProcesos ParametrosObtenerDatosCompletos(TGEMailing pParametro)
        {
            TGEMailingProcesos proceso = new TGEMailingProcesos();

            proceso.Parametros = BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEMailingParametros>("TGEMailingParametrosSeleccionarDescripcionesPorTGEMailingProcesos", pParametro);
            pParametro.MailingProcesos.Parametros = proceso.Parametros;
            return pParametro.MailingProcesos;
        }

        public TGEMailingProcesamientosAdjuntos MailingProcesamientosAdjuntosObtenerDatosCompletos(TGEMailingProcesamientosAdjuntos pParametro)
        {


            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TGEMailingProcesamientosAdjuntos>("TGEMailingProcesamientosAdjuntosSeleccionar", pParametro);
          
            return pParametro;
        }

        public TGEMailing ObtenerDatosCompletosFiltro(TGEMailing pParametro)
        {
        
            pParametro.DetalleEnvio = BaseDatos.ObtenerBaseDatos().ObtenerLista("TGEMailingProcesamientosSeleccionarDetalles", pParametro);

            return pParametro;
        }

        public DataTable ObtenerDatosCompletosFiltroDT(TGEMailing pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TGEMailingProcesamientosSeleccionarDetalles", pParametro);

            //if (resultado && BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "TGEMailingEnvioManual"))
            //    resultado = false;

        }

        public TGEMailing ObtenerPlantillaMailingProceso(TGEMailing pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TGEMailing>("TGEMailingProcesosObtenerPlantilla", pParametro);

            return pParametro;
        }

        public TGEMailingProcesamientosPlantillas PlatillaObtenerDatosCompletos(TGEMailingProcesamientosPlantillas pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TGEMailingProcesamientosPlantillas>("TGEMailingPlantillasSeleccionar", pParametro);
            DataTable dt = BaseDatos.ObtenerBaseDatos().ObtenerLista("TGEMailingPlantillasCamposSeleccionar", pParametro);
            TGEPlantillasCampos plantillasCampos;
            foreach (DataColumn col in dt.Columns)
            {
                plantillasCampos = new TGEPlantillasCampos();
                plantillasCampos.Etiqueta = col.ColumnName;
                plantillasCampos.Nombre = col.ColumnName;
                pParametro.PlantillasCampos.Add(plantillasCampos);
            }
            pParametro.Campos = pParametro.Campos.OrderBy(x => x.Nombre).ToList();

            return pParametro;
        }

        public override bool Agregar(TGEMailing pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.LoteMailingParametros = ArmarXmlParametros(pParametro);
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {


                    pParametro.IdMailing = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TGEMailingInsertar");
                    if (pParametro.IdMailing == 0)
                        resultado = false;

                    if (resultado)
                    {
                        int res = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, "TGEMailingParametrosInsertar");
                        if (res == 0)
                            resultado = false;
                    }

                    if (resultado && !this.ItemsActualizar(pParametro, new TGEMailing(), bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public DataSet ObtenerDatosParametro(TGEMailingProcesos pProceso)
        {
            DataSet dataSet = new DataSet();
            try
            {
                int tiempoEspera = Convert.ToInt32(TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DBTiempoEjecucionProcesosDatos).ParametroValor);

                DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database db = factory.CreateDefault();
                DbCommand dbCommand = db.GetStoredProcCommand(pProceso.StoredProcedure);
                if (tiempoEspera > dbCommand.CommandTimeout)
                    dbCommand.CommandTimeout = tiempoEspera;
                db.DiscoverParameters(dbCommand);
                foreach (TGEMailingParametros parametro in pProceso.Parametros)
                {
                    switch (parametro.TipoParametro.IdTipoParametro)
                    {
                        case (int)EnumSisTipoParametros.Int:
                        case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                            break;
                        case (int)EnumSisTipoParametros.CheckBoxList:
                        case (int)EnumSisTipoParametros.GridViewCheckFecha:
                        case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro;
                            break;
                        case (int)EnumSisTipoParametros.DateTime:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                            break;
                        case (int)EnumSisTipoParametros.TextBox:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro;
                            break;
                        case (int)EnumSisTipoParametros.IntNumericInput:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                            break;
                        case (int)EnumSisTipoParametros.DateTimeRange:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                            break;
                        case (int)EnumSisTipoParametros.YearMonthCombo:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                            break;
                        default:
                            break;
                    }
                }
                dataSet = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                pProceso.ErrorAccesoDatos = true;
                pProceso.CodigoMensaje = "RepAccesoDatos";
                pProceso.ErrorException = ex.Message;
                throw ex;
            }
            return dataSet;
        }

        public TGEMailingProcesamientosPlantillas ObtenerDatosCompletosPorCodigo(TGEMailingProcesamientosPlantillas pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TGEMailingProcesamientosPlantillas>("TGEMailingPlantillasSeleccionarPorCodigo", pParametro);
            //pParametro.HtmlPlantilla = AyudaProgramacionLN.            
            return pParametro;
        }

        public bool MailingProcesamientosInsertar(TGEMailing pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {


                    pParametro.MailingProcesamiento.IdMailingProcesamiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiAfiliadosInsertarMailingProcesamiento");
                    if (pParametro.MailingProcesamiento.IdMailingProcesamiento == 0)
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(TGEMailing pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;

            pParametro.LoteMailingParametros = ArmarXmlParametros(pParametro);
            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            TGEMailing valorViejo = new TGEMailing();
            valorViejo.IdMailing = pParametro.IdMailing;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TGEMailingActualizar"))
                        resultado = false;

                    if (resultado)
                    {
                        int res = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, "TGEMailingParametrosActualizar");
                        if (res == 0)
                            resultado = false;
                    }

                    if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        private bool ItemsActualizar(TGEMailing pParametro, TGEMailing pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (TGEMailingAdjuntos item in pParametro.MailingAdjuntos)
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.IdMailing = pParametro.IdMailing;
                        //if (item.Descripcion.Length > 0)
                        //    item.Descripcion = string.Concat(item.DescripcionProducto, " - ", item.Descripcion);
                        //else
                        //    item.Descripcion = item.DescripcionProducto;

                        item.IdMailingAdjunto = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "TGEMailingAdjuntosInsertar");
                        if (item.IdMailingAdjunto == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    #region Modificado
                    case EstadoColecciones.Modificado:
                    case EstadoColecciones.Borrado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "TGEMailingAdjuntosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.MailingAdjuntos.Find(x => x.IdMailingAdjunto == item.IdMailingAdjunto), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }

            foreach (TGEMailingProcesamientosAdjuntos item in pParametro.MailingProcesamientosAdjuntos)
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.IdMailingProcesamiento = pParametro.MailingProcesamiento.IdMailingProcesamiento;
                        //if (item.Descripcion.Length > 0)
                        //    item.Descripcion = string.Concat(item.DescripcionProducto, " - ", item.Descripcion);
                        //else
                        //    item.Descripcion = item.DescripcionProducto;

                        item.IdMailingProcesamiento = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "TGEMailingProcesamientosAdjuntosInsertar");
                        if (item.IdMailingProcesamiento == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    #region Modificado
                    case EstadoColecciones.Modificado:
                    case EstadoColecciones.Borrado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "TGEMailingProcesamientosAdjuntosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.MailingProcesamientosAdjuntos.Find(x => x.IdMailingProcesamientoAdjunto == item.IdMailingProcesamientoAdjunto), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            return true;
        }

        public override List<TGEMailing> ObtenerListaFiltro(TGEMailing pParametro)
        {
            throw new NotImplementedException();
        }

        public List<TGEMailingProcesos> ObtenerListaTGEMailingProceso()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEMailingProcesos>("TGEMailingObtenerMailingProceso");
        }

        
        public List<TGEMailingProcesamientosPlantillas> PlantillasObtenerLista(TGEPlantillas plantillas)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEMailingProcesamientosPlantillas>("TGEMailingProcesamientosPlantillasSeleccionar", plantillas);
        }

        public List<TGEMailing> SeleccionarGrilla()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEMailing>("TGEMailingSeleccionarGrilla");
        }
        
        public List<TGEMailing> ObtenerGenerarDatos()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEMailing>("TGEMailingSeleccionarGenerarDatos");
        }
        public XmlDocument GenerarDatosEnvios(string idMailing, string key, string value)
        {
            XmlDocument doc = new XmlDocument();
            // Create an XML declaration.
            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "ISO-8859-1";
            xmldecl.Standalone = "yes";
            // Add the new node to the document.
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmldecl, root);

            XmlElement correo = doc.CreateElement(string.Empty, "Mail", string.Empty);
            doc.AppendChild(correo);
            XmlElement asunto = doc.CreateElement(string.Empty, "Asunto", string.Empty);
            correo.AppendChild(asunto);
            XmlElement copia = doc.CreateElement(string.Empty, "Copia", string.Empty);
            correo.AppendChild(copia);
            XmlElement copiaOculta = doc.CreateElement(string.Empty, "CopiaOculta", string.Empty);
            correo.AppendChild(copiaOculta);

            int id = 0;
            if (!int.TryParse(idMailing, out id))
            {
                throw new Exception("El parametro idMailing es obligatorio y del tipo entero.");
            }
            TGEMailing mailing = new TGEMailing();
            mailing.IdMailing = id;
            mailing = new TGEMailingLN().ObtenerDatosCompletos(mailing);

            TGEPlantillas plantillaMail = new TGEPlantillas();
            plantillaMail.Codigo = mailing.Plantillas.Codigo;
            plantillaMail = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantillaMail);


            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = key;
            param.ValorParametro = value;
            reporte.Parametros.Add(param);
            reporte.StoredProcedure = plantillaMail.NombreSP;
            //reporte.UsuarioLogueado = usuario;
            DataSet dataSet = ReportesF.ReportesObtenerDatos(reporte);

            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                string mail = ReportesF.ReemplazarCamposPlantillaHtml(plantillaMail, dataSet.Tables[0], key, value, new UsuarioLogueado());
                mail = mail.Replace("&nbsp;", " "); //HttpUtility.HtmlDecode(mail);
                string html = AyudaProgramacionLN.StripHtml(AyudaProgramacionLN.getBetween(mail, "[Evol:Asunto]", "[/Evol:Asunto]")
                    .Replace("[Evol:Asunto]", "").Replace("[/Evol:Asunto]", "")).Trim();
                html = AyudaProgramacionLN.StripHtmlTags(html);
                XmlText textAsunto = doc.CreateTextNode(html);
                asunto.AppendChild(textAsunto);

                html = AyudaProgramacionLN.getBetween(mail, "[Evol:Copia]", "[/Evol:Copia]")
                    .Replace("[Evol:Copia]", "").Replace("[/Evol:Copia]", "").Trim();
                html = AyudaProgramacionLN.StripHtmlTags(html);
                XmlText textCopia = doc.CreateTextNode(html);
                copia.AppendChild(textCopia);

                html = AyudaProgramacionLN.getBetween(mail, "[Evol:CopiaOculta]", "[/Evol:CopiaOculta]")
                    .Replace("[Evol:CopiaOculta]", "").Replace("[/Evol:CopiaOculta]", "").Trim();
                html = AyudaProgramacionLN.StripHtmlTags(html);
                XmlText textCopiaOculta = doc.CreateTextNode(html);
                copiaOculta.AppendChild(textCopiaOculta);

                html = AyudaProgramacionLN.getBetween(mail, "[Evol:Cuerpo]", "[/Evol:Cuerpo]")
                    .Replace("[Evol:Cuerpo]", "").Replace("[/Evol:Cuerpo]", "").Trim();
                html = AyudaProgramacionLN.EscapearCaracteresEspeciasl(html, AyudaProgramacionLN.CodificarCaracteres.Codificar);
                html = AyudaProgramacionLN.LimpiarCodigosHtml(html);
                html = AyudaProgramacionLN.EscapearCaracteresEspeciasl(html, AyudaProgramacionLN.CodificarCaracteres.Decodificar);
                html = string.Concat("<Cuerpo>", html, "</Cuerpo>");

                string atachments = string.Empty;
                string separador = string.Empty;
                bool hayimagen = true;
                string img, newImg, url, file;
                string[] arrUrl;
                while (hayimagen)
                {
                    img = string.Empty;
                    url = string.Empty;
                    file = string.Empty;
                    img = AyudaProgramacionLN.getBetween(html, "<img ", "/>");
                    if (!string.IsNullOrEmpty(img))
                    {
                        url = AyudaProgramacionLN.getBetween(img, "src=\"", "\"");
                        arrUrl = url.Split('/');
                        file = arrUrl[arrUrl.Length - 1];
                        /*Adjuntos embebidos en el cuerpo del mail*/
                        atachments = string.Concat(atachments, separador, AppDomain.CurrentDomain.BaseDirectory, "ImagenesCliente\\", file);
                        separador = ";";
                        newImg = img.Replace(url, string.Concat("cid:", file));
                        html = html.Replace(img, newImg);
                        html = html.Replace(string.Concat("<img ", newImg), string.Concat("<imgEvol ", newImg));
                    }
                    else
                        hayimagen = false;
                }
                html = html.Replace("<imgEvol ", "<img ");

                XmlDocument xmlCuerpo = new XmlDocument();
                xmlCuerpo.LoadXml(html);
                XmlNode node = doc.ImportNode(xmlCuerpo.FirstChild, true);
                doc.DocumentElement.AppendChild(node);

                TGEPlantillas plantillaAdjunto;
                string fileSystemPath, nombrePDF;
                separador = atachments.Length > 0 ? ";" : string.Empty;
                foreach (TGEMailingAdjuntos ma in mailing.MailingAdjuntos)
                {
                    /*Por cada adjunto de Plantilla genero el PDF y despues lo adjunto*/
                    plantillaAdjunto = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(ma.Plantilla);
                    reporte = new RepReportes();
                    param = new RepParametros();
                    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                    param.Parametro = key;
                    param.ValorParametro = value;
                    reporte.Parametros.Add(param);
                    reporte.StoredProcedure = plantillaAdjunto.NombreSP;
                    //reporte.UsuarioLogueado = usuario;
                    dataSet = ReportesF.ReportesObtenerDatos(reporte);
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0
                        && dataSet.Tables[0].Columns.Contains("NombreArchivo"))
                    {
                        nombrePDF = string.Concat(dataSet.Tables[0].Rows[0]["NombreArchivo"].ToString(), ".pdf");
                    }
                    else
                    {
                        nombrePDF = string.Concat(ma.Plantilla.Codigo, "_", DateTime.Now.ToString("yyyyMMddhhmmss"), ".pdf");
                    }
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantillaAdjunto, dataSet, key, new UsuarioLogueado());
                    fileSystemPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "tempPDF\\", nombrePDF);
                    File.WriteAllBytes(fileSystemPath, pdf);
                    atachments = string.Concat(atachments, separador, fileSystemPath);
                    separador = ";";
                }

                /* Agrego los Adjuntos */
                XmlElement adjunto = doc.CreateElement(string.Empty, "Adjunto", string.Empty);
                correo.AppendChild(adjunto);
                XmlText textAdjunto = doc.CreateTextNode(atachments);
                adjunto.AppendChild(textAdjunto);

                //string directorio = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "tempMailing");
                //string nombreArchivo = string.Format(string.Concat(directorio, "ResumenDeCuenta_{0}.pdf"), dr["IdAfiliado"].ToString().PadLeft(5, '0'));
                //if (!Directory.Exists(directorio))
                //    Directory.CreateDirectory(directorio);
                //if (File.Exists(nombreArchivo))
                //    File.Delete(nombreArchivo);
                //File.WriteAllBytes(nombreArchivo, pdf);
                //row["Adjuntos"] = nombreArchivo;
            }
            return doc;
        }

        public XmlDocument GenerarDatosEnviosV2(string idMailing, string idMailingProcesamiento, string txtAsunto, string key, string value )
        {
            XmlDocument doc = new XmlDocument();
            // Create an XML declaration.
            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "ISO-8859-1";
            xmldecl.Standalone = "yes";
            // Add the new node to the document.
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmldecl, root);

            XmlElement correo = doc.CreateElement(string.Empty, "Mail", string.Empty);
            doc.AppendChild(correo);
            XmlElement asunto = doc.CreateElement(string.Empty, "Asunto", string.Empty);
            correo.AppendChild(asunto);
            XmlElement copia = doc.CreateElement(string.Empty, "Copia", string.Empty);
            correo.AppendChild(copia);
            XmlElement copiaOculta = doc.CreateElement(string.Empty, "CopiaOculta", string.Empty);
            correo.AppendChild(copiaOculta);

            int id = 0;
            if (!int.TryParse(idMailingProcesamiento, out id))
            {
                throw new Exception("El parametro idMailingProcesamiento es obligatorio y del tipo entero.");
            }

            TGEMailing mailing = new TGEMailing();
            mailing.MailingProcesamiento.IdMailingProcesamiento = Convert.ToInt32(idMailingProcesamiento);
            mailing = BaseDatos.ObtenerBaseDatos().Obtener<TGEMailing>("TGEMailingSeleccionarPorIdMailingProcesamiento", mailing);

            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = key;
            param.ValorParametro = value;
            reporte.Parametros.Add(param);
            reporte.StoredProcedure = mailing.Plantillas.NombreSP;
            //reporte.UsuarioLogueado = usuario;
            DataSet dataSet = ReportesF.ReportesObtenerDatos(reporte);

            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                string mail = ReportesF.ReemplazarCamposPlantillaHtml(mailing.Plantillas, dataSet.Tables[0], key, value, new UsuarioLogueado());
                mail = mail.Replace("&nbsp;", " "); //HttpUtility.HtmlDecode(mail);
                string html = txtAsunto;
                html = AyudaProgramacionLN.StripHtmlTags(html);
                XmlText textAsunto = doc.CreateTextNode(html);
                asunto.AppendChild(textAsunto);

                html = AyudaProgramacionLN.getBetween(mail, "[Evol:Copia]", "[/Evol:Copia]")
                    .Replace("[Evol:Copia]", "").Replace("[/Evol:Copia]", "").Trim();
                html = AyudaProgramacionLN.StripHtmlTags(html);
                XmlText textCopia = doc.CreateTextNode(html);
                copia.AppendChild(textCopia);

                html = AyudaProgramacionLN.getBetween(mail, "[Evol:CopiaOculta]", "[/Evol:CopiaOculta]")
                    .Replace("[Evol:CopiaOculta]", "").Replace("[/Evol:CopiaOculta]", "").Trim();
                html = AyudaProgramacionLN.StripHtmlTags(html);
                XmlText textCopiaOculta = doc.CreateTextNode(html);
                copiaOculta.AppendChild(textCopiaOculta);
                if (mail.Contains("[Evol:Cuerpo]"))
                {
                    html = AyudaProgramacionLN.getBetween(mail, "[Evol:Cuerpo]", "[/Evol:Cuerpo]")
                        .Replace("[Evol:Cuerpo]", "").Replace("[/Evol:Cuerpo]", "").Trim();
                }
                else
                {
                    html = mail;
                }

                html = AyudaProgramacionLN.EscapearCaracteresEspeciasl(html, AyudaProgramacionLN.CodificarCaracteres.Codificar);
                html = AyudaProgramacionLN.LimpiarCodigosHtml(html);
                html = AyudaProgramacionLN.EscapearCaracteresEspeciasl(html, AyudaProgramacionLN.CodificarCaracteres.Decodificar);
                html = string.Concat("<Cuerpo>", mail, "</Cuerpo>");

                string atachments = string.Empty;
                string separador = string.Empty;
                bool hayimagen = true;
                string img, newImg, url, file;
                string[] arrUrl;
                while (hayimagen)
                {
                    img = string.Empty;
                    url = string.Empty;
                    file = string.Empty;
                    img = AyudaProgramacionLN.getBetween(html, "<img ", "/>");
                    if (!string.IsNullOrEmpty(img))
                    {
                        url = AyudaProgramacionLN.getBetween(img, "src=\"", "\"");
                        arrUrl = url.Split('/');
                        file = arrUrl[arrUrl.Length - 1];
                        /*Adjuntos embebidos en el cuerpo del mail*/
                        atachments = string.Concat(atachments, separador, AppDomain.CurrentDomain.BaseDirectory, "ImagenesCliente\\" , file);
                        separador = ";";
                        newImg = img.Replace(url, string.Concat("cid:", file));
                        html = html.Replace(img, newImg);
                        html = html.Replace(string.Concat("<img ", newImg), string.Concat("<imgEvol ", newImg));
                    }
                    else
                        hayimagen = false;
                }
                html = html.Replace("<imgEvol ", "<img ");
               
                XmlDocument xmlCuerpo = new XmlDocument();
                html = System.Net.WebUtility.HtmlDecode(html);
                xmlCuerpo.LoadXml(html);
                XmlNode node = doc.ImportNode(xmlCuerpo.FirstChild, true);
                doc.DocumentElement.AppendChild(node);



                mailing.MailingProcesamiento.IdMailingProcesamiento = Convert.ToInt32(idMailingProcesamiento);
                mailing.MailingProcesamientosAdjuntos =  BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEMailingProcesamientosAdjuntos>("TGEMailingProcesamientosAdjuntosSeleccionar", mailing);


                TGEPlantillas plantillaAdjunto;
                string fileSystemPath, nombrePDF;
                separador = atachments.Length > 0 ? ";" : string.Empty;

                foreach (TGEMailingProcesamientosAdjuntos ma in mailing.MailingProcesamientosAdjuntos)
                {
                    /*Por cada adjunto de Plantilla genero el PDF y despues lo adjunto*/
                    plantillaAdjunto = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(ma.Plantilla);
                    reporte = new RepReportes();
                    param = new RepParametros();
                    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                    param.Parametro = key;
                    param.ValorParametro = value;
                    reporte.Parametros.Add(param);
                    reporte.StoredProcedure = plantillaAdjunto.NombreSP;
                    //reporte.UsuarioLogueado = usuario;
                    dataSet = ReportesF.ReportesObtenerDatos(reporte);
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0
                        && dataSet.Tables[0].Columns.Contains("NombreArchivo"))
                    {
                        nombrePDF = string.Concat(dataSet.Tables[0].Rows[0]["NombreArchivo"].ToString(), ".pdf");
                    }
                    else
                    {
                        nombrePDF = string.Concat(ma.Plantilla.Codigo, "_", key, "_", value.PadLeft(10,'0'), ".pdf");
                    }
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantillaAdjunto, dataSet, key, new UsuarioLogueado());
                    fileSystemPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "tempPDF\\", nombrePDF);
                    File.WriteAllBytes(fileSystemPath, pdf);
                    atachments = string.Concat(atachments, separador, fileSystemPath);
                    separador = ";";
                }

                /* Agrego los Adjuntos */
                XmlElement adjunto = doc.CreateElement(string.Empty, "Adjunto", string.Empty);
                correo.AppendChild(adjunto);
                XmlText textAdjunto = doc.CreateTextNode(atachments);
                adjunto.AppendChild(textAdjunto);

                

                //string directorio = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "tempMailing");
                //string nombreArchivo = string.Format(string.Concat(directorio, "ResumenDeCuenta_{0}.pdf"), dr["IdAfiliado"].ToString().PadLeft(5, '0'));
                //if (!Directory.Exists(directorio))
                //    Directory.CreateDirectory(directorio);
                //if (File.Exists(nombreArchivo))
                //    File.Delete(nombreArchivo);
                //File.WriteAllBytes(nombreArchivo, pdf);
                //row["Adjuntos"] = nombreArchivo;
            }
            return doc;
        }
          
        [Obsolete]
        public bool GenerarDatosEnvios(TGEMailing item)
        {
            bool resultado = true;
            DataTable dt;
            TGEPlantillas plantillaMail;
            TGEMailingProcesamiento procesamiento;
            TGEParametrosMails paramMail = new TGEParametrosMails();
            paramMail = TGEGeneralesF.ParametrosMailsObtenerDatosCompletos(paramMail);
            DataTable dtMails = BaseDatos.ObtenerBaseDatos().ObtenerLista("AudMailsEnviosSeleccionar", new MailsEnvios());
            if (item.MailingProcesos.IdMailingProceso == 1)
            {
                dt = BaseDatos.ObtenerBaseDatos().ObtenerLista(item.MailingProcesos.StoredProcedure, new Objeto());
                plantillaMail = new TGEPlantillas();
                plantillaMail.Codigo = item.Plantillas.Codigo;
                plantillaMail = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantillaMail);

                DataRow row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = dtMails.NewRow();
                    #region Armo el Mail                   
                    
                    RepReportes reporte = new RepReportes();
                    RepParametros param = new RepParametros();
                    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                    param.Parametro = "IdAfiliado";
                    param.ValorParametro = dr["IdAfiliado"];
                    reporte.Parametros.Add(param);
                    reporte.StoredProcedure = plantillaMail.NombreSP;
                    //reporte.UsuarioLogueado = usuario;
                    DataSet dataSet = ReportesF.ReportesObtenerDatos(reporte);
                    
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        DataRow drMail = dataSet.Tables[0].Rows[0];

                        row["Tabla"] = "AfiAfiliados";
                        row["IdRefTabla"] = Convert.ToInt32(drMail["IdAfiliado"]);
                        //row["FechaEnvio"] = DateTime.Now;
                        row["De"] = paramMail.DireccionCorreo;
                        row["DeMostrar"] = paramMail.Nombre;
                        row["A"] = drMail["CorreoElectronico"];
                        row["AMostrar"] = drMail["RazonSocial"];

                        byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantillaMail, dataSet, "IdAfiliado", new UsuarioLogueado());

                        string mail = System.Text.Encoding.Default.GetString(pdf);
                        //string directorio = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "tempMailing");
                        //string nombreArchivo = string.Format(string.Concat(directorio, "ResumenDeCuenta_{0}.pdf"), dr["IdAfiliado"].ToString().PadLeft(5, '0'));
                        //if (!Directory.Exists(directorio))
                        //    Directory.CreateDirectory(directorio);
                        //if (File.Exists(nombreArchivo))
                        //    File.Delete(nombreArchivo);
                        //File.WriteAllBytes(nombreArchivo, pdf);
                        //row["Adjuntos"] = nombreArchivo;

                        row["Asunto"] = AyudaProgramacionLN.StripHtml(AyudaProgramacionLN.getBetween(mail, "[Evol:Asunto]", "[/Evol:Asunto]")
                            .Replace("[Evol:Asunto]", "").Replace("[/Evol:Asunto]", "")).Trim();
                        row["Cuerpo"] = AyudaProgramacionLN.getBetween(mail, "[Evol:Cuerpo]", "[/Evol:Cuerpo]")
                            .Replace("[Evol:Cuerpo]", "").Replace("[/Evol:Cuerpo]", "").Trim();
                        
                        dtMails.Rows.Add(row);
                    }
                    #endregion
                }
            }

            procesamiento = new TGEMailingProcesamiento();
            procesamiento.IdMailing = item.IdMailing;
            procesamiento.Fecha = DateTime.Now;
            procesamiento.Estado.IdEstado = (int)Estados.Activo;
            procesamiento.UsuarioLogueado.IdUsuarioEvento = 120;
            dtMails.TableName = "MailsEnvios";
            procesamiento.MailsEnvios = dtMails.ToXmlDocument();

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    procesamiento.IdMailingProcesamiento = BaseDatos.ObtenerBaseDatos().Agregar(procesamiento, bd, tran, "TGEMailingProcesamientoInsertar");
                    if (procesamiento.IdMailingProcesamiento == 0)
                    {
                        AyudaProgramacionLN.MapearError(procesamiento, item);
                        resultado = false;
                    }
                    if (resultado)
                    {
                        tran.Commit();
                        item.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    resultado = false;
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    item.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    item.CodigoMensajeArgs.Add(ex.Message);
                }
            }
            return resultado;
        }

        public XmlDocument ArmarXmlParametros(TGEMailing mailing)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement lista = doc.CreateElement(string.Empty, "Parametros", string.Empty);
            doc.AppendChild(lista);
            foreach (TGEMailingParametros cps in mailing.MailingProcesamiento.Proceso.Parametros)
            {
                //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                //XmlElement root = doc.DocumentElement;
                //doc.InsertBefore(xmlDeclaration, root);
                XmlElement Parametroxml = doc.CreateElement(string.Empty, "Parametro", string.Empty);
                lista.AppendChild(Parametroxml);
                XmlElement IdParametroxml = doc.CreateElement(string.Empty, "IdParametro", string.Empty);
                XmlText txtIdParametro = doc.CreateTextNode(cps.IdParametro.ToString());
                Parametroxml.AppendChild(IdParametroxml);
                IdParametroxml.AppendChild(txtIdParametro);
                XmlElement IdTipoParametroxml = doc.CreateElement(string.Empty, "IdTipoParametro", string.Empty);
                XmlText txtTipoParametro = doc.CreateTextNode(cps.TipoParametro.IdTipoParametro.ToString());
                Parametroxml.AppendChild(IdTipoParametroxml);
                IdTipoParametroxml.AppendChild(txtTipoParametro);
                XmlElement pParametroxml = doc.CreateElement(string.Empty, "pParametro", string.Empty);
                XmlText txtParametro = doc.CreateTextNode(cps.Parametro.ToString());
                Parametroxml.AppendChild(pParametroxml);
                pParametroxml.AppendChild(txtParametro);
                XmlElement NombreParametroxml = doc.CreateElement(string.Empty, "NombreParametro", string.Empty);
                XmlText txtNombreParametro = doc.CreateTextNode(cps.NombreParametro.ToString());
                Parametroxml.AppendChild(NombreParametroxml);
                NombreParametroxml.AppendChild(txtNombreParametro);
                XmlElement StoredProcedurexml = doc.CreateElement(string.Empty, "StoredProcedure", string.Empty);
                XmlText txtStoredProcedure = doc.CreateTextNode(cps.StoredProcedure.ToString());
                Parametroxml.AppendChild(StoredProcedurexml);
                StoredProcedurexml.AppendChild(txtStoredProcedure);
                XmlElement ValorParametroxml = doc.CreateElement(string.Empty, "ValorParametro", string.Empty);
                XmlText txtValorParametro = doc.CreateTextNode(cps.ValorParametro.ToString());
                Parametroxml.AppendChild(ValorParametroxml);
                ValorParametroxml.AppendChild(txtValorParametro);




            }
            return doc;

        }

        public bool EnviarMails(TGEMailing pParametro)
        {
            bool resultado = true;

            //DbTransaction tran;
            //DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            //using (DbConnection con = bd.CreateConnection())
            //{
            //    con.Open();
            //    tran = con.BeginTransaction();
            try
            {
                pParametro.MailingProcesamiento.IdMailingProcesamiento = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, pParametro.MailingProcesos.StoredProcedure);

                //pParametro.MailingProcesamiento.IdMailingProcesamiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, pParametro.MailingProcesos.StoredProcedure);
                if (pParametro.MailingProcesamiento.IdMailingProcesamiento == 0)
                    resultado = false;

                if (resultado)
                {

                    pParametro.CodigoMensaje = "ResultadoTransaccion"; //"EnviarMailOk";
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                ExceptionHandler.HandleException(ex, "LogicaNegocio");
                //tran.Rollback();
                pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                pParametro.CodigoMensajeArgs.Add(ex.Message);
            }
            //}
            return resultado;
        }
        public bool EnviarMailsV2(TGEMailing pParametro)
        {
            bool resultado = true;

            //DbTransaction tran;
            //DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            //using (DbConnection con = bd.CreateConnection())
            //{
            //    con.Open();
            //    tran = con.BeginTransaction();

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            Hashtable listaParametros = new Hashtable();
            string sp = string.Empty;
            //DataSet dataSet = new DataSet();
            int tiempoEspera = Convert.ToInt32(TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DBTiempoEjecucionProcesosDatos).ParametroValor);

            listaParametros.Add("PruebaEnvio", pParametro.MailingProcesos.PruebaEnvio);
            listaParametros.Add("IdUsuarioEvento", pParametro.UsuarioLogueado.IdUsuarioEvento.ToString());
            listaParametros.Add("IdMailing", pParametro.IdMailing);
            listaParametros.Add("IdMailingProcesamiento", pParametro.MailingProcesamiento.IdMailingProcesamiento);
            listaParametros.Add("Asunto", pParametro.Asunto);
            listaParametros.Add("EnvioManual", 1);


            pParametro.LoteMailingParametros = ArmarXmlParametros(pParametro);

            foreach (TGEMailingParametros parametro in pParametro.MailingProcesos.Parametros)
            {
                //listaParametros.Add(sisParam.Parametro, sisParam.ValorParametro);
                switch (parametro.TipoParametro.IdTipoParametro)
                {
                    case (int)EnumSisTipoParametros.Int:
                    case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                        break;
                    case (int)EnumSisTipoParametros.CheckBoxList:
                    case (int)EnumSisTipoParametros.GridViewCheckFecha:
                    case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro);
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : parametro.ValorParametro;
                        break;
                    case (int)EnumSisTipoParametros.DateTime:
                        //listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : (object)(Convert.ToDateTime(parametro.ValorParametro).ToString("s")));
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                        break;
                    case (int)EnumSisTipoParametros.TextBox:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro);
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : parametro.ValorParametro;
                        break;
                    case (int)EnumSisTipoParametros.IntNumericInput:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(Int64)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                        break;
                    case (int)EnumSisTipoParametros.DateTimeRange:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                        break;
                    case (int)EnumSisTipoParametros.YearMonthCombo:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                        break;
                    default:
                        break;
                }
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database db = factory.CreateDefault();
            DbCommand dbCommand = db.GetStoredProcCommand(pParametro.MailingProcesos.StoredProcedure);
            db.DiscoverParameters(dbCommand);
            Servicio.AccesoDatos.Mapeador.MapearEntidadParametros(listaParametros, dbCommand);

            using (DbConnection con = db.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                   
                    if (tiempoEspera > dbCommand.CommandTimeout)
                        dbCommand.CommandTimeout = tiempoEspera;
                    //  int spRes =      BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, pParametro.MailingProcesos.StoredProcedure);

                    int spRes = 1;
                    if (!this.ItemsActualizar(pParametro, new TGEMailing(), db, tran))
                        spRes = 0;

                   

                    //pParametro.MailingProcesamiento.IdMailingProcesamiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, pParametro.MailingProcesos.StoredProcedure);
                    if (spRes == 0)
                        resultado = false;
                    tran.Commit();

                    if (resultado)
                    {//estos metodos estan fuera de la transaccion porque tienen que llamar a la api, y si estan dentro de la transaccion queda colgado
                        spRes = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, listaParametros, pParametro.MailingProcesos.StoredProcedure, tiempoEspera);

                        if (spRes == 1)
                            spRes = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, "TGEMailingProcesamientosParametrosInsertar");
                        pParametro.CodigoMensaje = "ResultadoTransaccion"; //"EnviarMailOk";
                    }
                    else
                    {
                        tran.Rollback();
                        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    }
                }
                catch (Exception ex)
                {
                    resultado = false;
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                }
            }
            return resultado;
        }

        public bool PruebaEnvio(TGEMailing pParametro)
        {
            bool resultado = true;
            try
            {
                resultado = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, "TGEMailingEjecutarPruebaEnvio", BaseDatos.conexionPredeterminada);

                //pParametro.MailingProcesamiento.IdMailingProcesamiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, pParametro.MailingProcesos.StoredProcedure);

                if (resultado)
                {

                    pParametro.CodigoMensaje = "Se ha enviado la prueba de envío con exito."; //"EnviarMailOk";
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                ExceptionHandler.HandleException(ex, "LogicaNegocio");
                //tran.Rollback();
                pParametro.CodigoMensaje = "No se ha podido realizar la prueba de envío.";
                pParametro.CodigoMensajeArgs.Add(ex.Message);
            }
            //}
            return resultado;
        }

        public bool EnviarMailsActualizar(TGEMailing pParametro)
        {
            bool resultado = true;

            //DbTransaction tran;
            //DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            //using (DbConnection con = bd.CreateConnection())
            //{
            //    con.Open();
            //    tran = con.BeginTransaction();
            try
            {
                pParametro.MailingProcesamiento.IdMailingProcesamiento = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, pParametro.MailingProcesos.StoredProcedure);

                //pParametro.MailingProcesamiento.IdMailingProcesamiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, pParametro.MailingProcesos.StoredProcedure);
                if (pParametro.MailingProcesamiento.IdMailingProcesamiento == 0)
                    resultado = false;

                if (resultado)
                {

                    pParametro.CodigoMensaje = "ResultadoTransaccion"; //"EnviarMailOk";
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                ExceptionHandler.HandleException(ex, "LogicaNegocio");
                //tran.Rollback();
                pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                pParametro.CodigoMensajeArgs.Add(ex.Message);
            }
            //}
            return resultado;
        }
        public DataTable ObtenerMailsAEnviar(TGEMailing pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TGEMailingEnvioManual", pParametro);

            //if (resultado && BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "TGEMailingEnvioManual"))
            //    resultado = false;

        }

        public bool EnviarMailsSeleccionados(DataTable e, TGEMailing pParametro)
        {
            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            e.TableName = "MailingEnvioManual";
            pParametro.LoteMailingEnvioManual = e.ToXmlDocument();

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "TGEMailingEnvioManualXML");

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "EnviarMailOk";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            if(resultado)
                TGEGeneralesF.EnviarProcesoMails();

            return resultado;
        }

        public AudMailsEnvios ObtenerMensaje(AudMailsEnvios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AudMailsEnvios>("AudMailsEnviosSeleccionar", pParametro);
        }

        public bool PlantillaModificar(TGEMailingProcesamientosPlantillas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!Validar(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            //TGEMailingProcesamientosPlantillas valorViejo = new TGEMailingProcesamientosPlantillas();
            //valorViejo.IdPlantilla = pParametro.IdPlantilla;
            //valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            //valorViejo = this.ObtenerDatosCompletosPlantilla(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TGEMailingPlantillasActualizar"))
                        resultado = false;

                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado)
                        pParametro.SelloTiempo = BaseDatos.ObtenerBaseDatos().ObtenerSelloTiempo("TGEMailingPlantillasSeleccionar", pParametro, bd, tran);

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }
        public bool PlantillaAgregar(TGEMailingProcesamientosPlantillas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!Validar(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            //TGEMailingProcesamientosPlantillas valorViejo = new TGEMailingProcesamientosPlantillas();
            //valorViejo.IdPlantilla = pParametro.IdPlantilla;
            //valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            //valorViejo = this.ObtenerDatosCompletosPlantilla(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdMailing = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TGEMailingPlantillasInsertar");
                    if (pParametro.IdMailing == 0)
                        resultado = false;
                    //if (resultado)
                    //    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AudEnviosMailsActualizarPlantilla"))
                    //        resultado = false;


                    if (resultado)
                        pParametro.SelloTiempo = BaseDatos.ObtenerBaseDatos().ObtenerSelloTiempo("TGEMailingPlantillasSeleccionar", pParametro, bd, tran);

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        private bool Validar(TGEMailingProcesamientosPlantillas pParametro)
        {
            if (pParametro.HtmlPlantilla.Length > 0)
            {
                List<StringStartEnd> camposNoEncontrados = FindAllString(pParametro.HtmlPlantilla, "{", "}");
                bool containsHTML;
                string plantilla = pParametro.HtmlPlantilla;
                string oldChar;
                foreach (StringStartEnd item in camposNoEncontrados)
                {
                    oldChar = pParametro.HtmlPlantilla.Substring(item.start, item.end - item.start);
                    containsHTML = (oldChar != HttpUtility.HtmlEncode(oldChar));
                    if (containsHTML)
                    {
                        plantilla = plantilla.Replace(oldChar, Regex.Replace(oldChar, "<.*?>", string.Empty));
                    }
                }
                pParametro.HtmlPlantilla = plantilla;
                pParametro.HtmlPlantilla = AyudaProgramacionLN.LimpiarCodigosHtml(pParametro.HtmlPlantilla);
                //var htmlDoc = new HtmlDocument();
                //htmlDoc.LoadHtml(pParametro.HtmlPlantilla);
                //foreach (var error in htmlDoc.ParseErrors)
                //{
                //    // Prints: TagNotOpened
                //    pParametro.CodigoMensaje = string.Concat(pParametro.Codigo, error.Code, " ", error.Reason, " - Linea: ", error.Line, " Columna: ", error.LinePosition, "<br />");
                //    //Console.WriteLine(error.Code);
                //    // Prints: Start tag <u> was not found
                //    //Console.WriteLine(error.Reason);
                //    return false;
                //}
            }
            return true;
        }
        private static List<StringStartEnd> FindAllString(string str, string start, string end)
        {
            List<StringStartEnd> indexes = new List<StringStartEnd>();
            StringStartEnd item;
            for (int index = 0; ; index += end.Length)
            {
                index = str.IndexOf(start, index);
                if (index == -1)
                    break;
                else
                {
                    item = new StringStartEnd();
                    item.start = index;
                    index = str.IndexOf(end, index + start.Length);
                    item.end = index + end.Length;
                }
                indexes.Add(item);
            }
            return indexes;
        }

        //public override TGEMailingProcesamientosPlantillas ObtenerDatosCompletosPlantilla(TGEMailingProcesamientosPlantillas pParametro)
        //{
        //    pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TGEMailingProcesamientosPlantillas>("TGEPlantillasSeleccionar", pParametro);
        //    DataTable dt = BaseDatos.ObtenerBaseDatos().ObtenerLista("TGEPlantillasCamposSeleccionar", pParametro);
        //    TGEPlantillasCampos plantillasCampos;
        //    foreach (DataColumn col in dt.Columns)
        //    {
        //        plantillasCampos = new TGEPlantillasCampos();
        //        plantillasCampos.Etiqueta = col.ColumnName;
        //        plantillasCampos.Nombre = col.ColumnName;
        //        pParametro.Campos.Add(plantillasCampos);
        //    }
        //    pParametro.Campos = pParametro.Campos.OrderBy(x => x.Nombre).ToList();

        //    return pParametro;
        //}

        public  List<TGEMailingProcesamientosPlantillas> ObtenerListaFiltroPlantillas(TGEMailingProcesamientosPlantillas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEMailingProcesamientosPlantillas>("TGEPlantillasObtenerListaFiltro", pParametro);
        }

    }
}
