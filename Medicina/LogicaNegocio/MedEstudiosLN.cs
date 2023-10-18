using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medicina.Entidades;
using Comunes;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using System.Data;
using Generales.Entidades;
using Reportes.FachadaNegocio;
using System.Text.RegularExpressions;
using System.Web;

namespace Medicina.LogicaNegocio
{
    class MedEstudiosLN : BaseLN<MedEstudios>
    {
        public override bool Agregar(MedEstudios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.FechaEvento = DateTime.Now;
            pParametro.FechaAlta = DateTime.Now;
            //if (!Validar(pParametro))
              //  return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdEstudio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "MedEstudiosInsertar");
                    if (pParametro.IdEstudio == 0)
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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

        public override bool Modificar(MedEstudios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            pParametro.FechaEvento = DateTime.Now;

            //if (!Validar(pParametro))
            //    return false;

            MedEstudios valorViejo = new MedEstudios();
            valorViejo.IdEstudio = pParametro.IdEstudio;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "MedEstudiosActualizar"))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        public bool Anular(MedEstudios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            MedEstudios valorViejo = new MedEstudios();
            valorViejo.IdEstudio = pParametro.IdEstudio;
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "MedEstudiosActualizarEstado");

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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

        private bool Validar(MedEstudios pParametro)
        {
            if (pParametro.InformeEstudio.Length > 0)
            {
                List<StringStartEnd> camposNoEncontrados = AyudaProgramacionLN.FindAllString(pParametro.InformeEstudio, "{", "}");
                bool containsHTML;
                string plantilla = pParametro.InformeEstudio;
                string oldChar;
                foreach (StringStartEnd item in camposNoEncontrados)
                {
                    oldChar = pParametro.InformeEstudio.Substring(item.start, item.end - item.start);
                    containsHTML = (oldChar != HttpUtility.HtmlEncode(oldChar));
                    if (containsHTML)
                    {
                        plantilla = plantilla.Replace(oldChar, Regex.Replace(oldChar, "<.*?>", string.Empty));
                    }
                }
                pParametro.InformeEstudio = plantilla;
                pParametro.InformeEstudio = AyudaProgramacionLN.LimpiarCodigosHtml(pParametro.InformeEstudio);
            }
            return true;
        }

        public override MedEstudios ObtenerDatosCompletos(MedEstudios pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<MedEstudios>("MedEstudiosSeleccionar", pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<MedEstudios> ObtenerListaFiltro(MedEstudios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedEstudios>("MedEstudiosSeleccionarFiltro", pParametro);
        }
        public List<MedEstudios> ObtenerListaFiltroPacientes(MedEstudios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedEstudios>("MedEstudiosSeleccionarFiltroPacientes", pParametro);
        }

        public DataSet ObtenerDataSetPdf(MedEstudios parametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerDataSet("MedEstudiosObtenerDataSetPdf", parametro);
        }

        public string ObtenerPlantilla(MedEstudios pParametro)
        {
            TGEPlantillas filtro = new TGEPlantillas();
            filtro.Codigo = "MedEstudiosInformeMedico";
            filtro = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(filtro);

            string htmlPlantilla = filtro.HtmlPlantilla;
            List<StringStartEnd> posiciones = AyudaProgramacionLN.FindAllString(htmlPlantilla, "{", "}");
            List<string> campos = new List<string>();
            string campo;
            foreach (StringStartEnd pos in posiciones)
            {
                campo = htmlPlantilla.Substring(pos.start, pos.end - pos.start).Replace("{", "").Replace("}", "");
                if (campo.Length > 0)
                    campos.Add(campo);
            }
            AyudaProgramacionLN.MapearEntidad(ref htmlPlantilla, campos, pParametro.Afiliado);
            return htmlPlantilla;
        }

        public byte[] ObtenerComprobante(MedEstudios parametro)
        {
            DataSet ds = MedicinaF.EstudiosObtenerDataSetPdf(parametro);
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.HtmlPlantilla = ds.Tables[0].Rows[0]["InformeEstudio"].ToString();

            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdEstudio", parametro.UsuarioLogueado);

            List<TGEArchivos> archivos = TGEGeneralesF.ArchivosObtenerLista(parametro);
            //List<byte[]> listaBytes = new List<byte[]>();
            List<TGEArchivos> listaBytes = new List<TGEArchivos>();
            if (archivos.Count > 0)
            {
                listaBytes.Add(new TGEArchivos() { Archivo = pdf , NombreArchivo = "Estudio"});

                foreach (TGEArchivos archivo in archivos)
                    listaBytes.Add(TGEGeneralesF.ArchivosObtener(archivo));
                
                pdf = ReportesF.ConcatenarPdfs(listaBytes);
            }

            return pdf;
        }
    }
}
