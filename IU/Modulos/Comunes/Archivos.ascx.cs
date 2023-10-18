using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Generales.Entidades;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Comunes.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.Comunes
{
    public partial class Archivos : ControlesSeguros
    {
        private List<TGEArchivos> MisArchivos
        {
            get
            {
                return (Session[this.MiSessionPagina + "ArchivosMisArchivos"] == null ?
                    (List<TGEArchivos>)(Session[this.MiSessionPagina + "ArchivosMisArchivos"] = new List<TGEArchivos>()) : (List<TGEArchivos>)Session[this.MiSessionPagina + "ArchivosMisArchivos"]);
            }
            set { Session[this.MiSessionPagina + "ArchivosMisArchivos"] = value; }
        }

        private TGEArchivos MitmpArchivo
        {
            get
            {
                return (Session[this.MiSessionPagina + "ArchivosMitmpArchivo"] == null ?
                    (TGEArchivos)(Session[this.MiSessionPagina + "ArchivosMitmpArchivo"] = new TGEArchivos()) : (TGEArchivos)Session[this.MiSessionPagina + "ArchivosMitmpArchivo"]);
            }
            set { Session[this.MiSessionPagina + "ArchivosMitmpArchivo"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtArchivoDescripcion, this.btnAgregarArchivo);
            }
            else
            {
                this.DescargarArchivosRegisterPostBack();
            }
        }

        /// <summary>
        /// Inicializa el control para la descarga de archivos
        /// Llena la grilla con la lista de archivos cargada en el objeto
        /// </summary>
        /// <param name="pObjeto"></param>
        /// <returns></returns>
        public void IniciarControl(Objeto pObjeto, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MisArchivos = new List<TGEArchivos>();
            PropertyInfo prop = pObjeto.GetType().GetProperty("Archivos");
            if (prop == null)
            {
                this.pnlAgregar.Visible = false;
                return;
            }
            else
            {
                if (pGestion == Gestion.Consultar
                || pGestion == Gestion.Anular
                || pGestion == Gestion.AnularCancelar
                || pGestion == Gestion.Cancelar
                //|| pGestion == Gestion.ConfirmarAgregar
                || pGestion == Gestion.Pagar
                || pGestion == Gestion.Rechazar
                )
                {
                    this.pnlAgregar.Visible = false;
                }
                else
                {
                    this.pnlAgregar.Visible = true;
                    this.CargarCombos();
                }

                this.MisArchivos = (List<TGEArchivos>)prop.GetValue(pObjeto, null);
                AyudaProgramacion.CargarGrillaListas(this.MisArchivos, true, this.gvArchivos, true);
                this.DescargarArchivosRegisterPostBack();
            }
        }

        /// <summary>
        /// Obtiene la lista de Archivos actualizada para ser guardada
        /// en la base de datos.
        /// </summary>
        /// <returns></returns>
        public List<TGEArchivos> ObtenerLista()
        {
            return this.MisArchivos;
        }

        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            this.MitmpArchivo.Archivo = this.StreamToByteArray(this.afuArchivo.FileContent);
            this.MitmpArchivo.TipoArchivo = this.afuArchivo.ContentType;
            this.MitmpArchivo.Tamanio = this.afuArchivo.FileContent.Length;
            this.MitmpArchivo.NombreArchivo = this.afuArchivo.FileName;
        }
        
        protected void btnAgregarArchivo_Click(object sender, EventArgs e)
        {
            //if (!this.afuArchivo.HasFile)
            //{
            //    this.MostrarMensaje("ValidarArchivo", true);
            //    return;
            //}
            if (this.MitmpArchivo.Archivo == null || this.MitmpArchivo.Archivo.Length == 0)
            {
                this.MostrarMensaje("ValidarArchivo", true);
                return;
            }
            TGEArchivos archivo = new TGEArchivos();
            archivo.Fecha = DateTime.Now;
            //archivo.Archivo = this.afuArchivo.FileBytes;
            archivo.Archivo = this.MitmpArchivo.Archivo;
            archivo.TipoArchivo = this.MitmpArchivo.TipoArchivo;
            archivo.Tamanio = this.MitmpArchivo.Tamanio;
            archivo.NombreArchivo = this.MitmpArchivo.NombreArchivo;
            archivo.Descripcion = this.txtArchivoDescripcion.Text;
            archivo.ListaValorDetalle.IdListaValorDetalle = this.ddlTiposArchivos.SelectedValue==string.Empty ? 0 : Convert.ToInt32(this.ddlTiposArchivos.SelectedValue);
            archivo.ListaValorDetalle.Descripcion = this.ddlTiposArchivos.SelectedValue==string.Empty ? string.Empty : this.ddlTiposArchivos.SelectedItem.Text;
            archivo.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            archivo.EstadoColeccion = EstadoColecciones.Agregado;
            archivo.Estado.IdEstado = (int)Estados.Activo;
            this.MisArchivos.Add(archivo);
            archivo.IndiceColeccion = this.MisArchivos.IndexOf(archivo);

            MitmpArchivo = new TGEArchivos();

            this.txtArchivoDescripcion.Text = string.Empty;
            this.afuArchivo.ClearAllFilesFromPersistedStore();
            //this.afuArchivo = new AjaxControlToolkit.AsyncFileUpload();

            AyudaProgramacion.CargarGrillaListas(this.MisArchivos, true, this.gvArchivos, true);
            this.DescargarArchivosRegisterPostBack();
        }

        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            // This is optional
            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);
            return output;
        }

        protected void gvArchivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Eliminar"
                    || e.CommandName == Gestion.Impresion.ToString()
                    || e.CommandName == "Descargar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Eliminar")
            {
                this.MisArchivos[indiceColeccion].Estado.IdEstado=(int)Estados.Baja;
                this.MisArchivos[indiceColeccion].EstadoColeccion = EstadoColecciones.Borrado;
                this.MisArchivos = AyudaProgramacion.AcomodarIndices<TGEArchivos>(this.MisArchivos);
                AyudaProgramacion.CargarGrillaListas(this.MisArchivos, true, this.gvArchivos, true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                TGEArchivos archivo = this.MisArchivos[indiceColeccion];
                if (!(archivo.EstadoColeccion == EstadoColecciones.Agregado))
                    archivo = TGEGeneralesF.ArchivosObtener(archivo);
                //this.ctrPopUpComprobantes.CargarArchivo(archivo);
                UsuarioLogueado usu = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                usu.NombreArchivo = archivo.NombreArchivo;
                ExportPDF.ExportarPDF(archivo.Archivo, this.upArchivos, usu);
            }
            else if (e.CommandName == "Descargar")
            {
                TGEArchivos archivo = this.MisArchivos[indiceColeccion];
                if (!(archivo.EstadoColeccion == EstadoColecciones.Agregado))
                    archivo = TGEGeneralesF.ArchivosObtener(archivo);
                MemoryStream ms = new MemoryStream(archivo.Archivo);
                Response.Clear();
                Response.Buffer = true;

                Response.ContentType = archivo.TipoArchivo;
                Response.AddHeader("content-disposition", String.Format("attachment;filename=\"{0}\"", archivo.NombreArchivo));
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
        }

        protected void gvArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (this.GestionControl==Gestion.Agregar
                    || this.GestionControl==Gestion.Modificar)
                {
                ImageButton ibtn = (ImageButton)e.Row.Cells[4].FindControl("btnEliminarArchivo");
                string mensaje = this.ObtenerMensajeSistema("POArchivoConfirmarBaja");
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                ibtn.Attributes.Add("OnClick", funcion);
                ibtn.Visible = true;
                }
            }
        }

        private void DescargarArchivosRegisterPostBack()
        {
            foreach (GridViewRow fila in this.gvArchivos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    ImageButton ibtnDescargar = (ImageButton)fila.FindControl("btnDescargar");
                    this.toolkitScriptManager.RegisterPostBackControl(ibtnDescargar);
                }
            }
        }

        private void CargarCombos()
        {
            TGETiposFuncionalidadesListasValoresDetalles func = new TGETiposFuncionalidadesListasValoresDetalles();
            func.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            func.ListaValor.CodigoValor = EnumTGEListasValoresCodigos.TiposArchivos.ToString();
            this.ddlTiposArchivos.Items.Clear();
            this.ddlTiposArchivos.SelectedValue = null;
            this.ddlTiposArchivos.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(func);
            this.ddlTiposArchivos.DataValueField = "IdListaValorDetalle";
            this.ddlTiposArchivos.DataTextField = "Descripcion";
            this.ddlTiposArchivos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposArchivos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        /// <summary>
        /// Procedimiento para habilitar el boton de descarga de los archivos,
        /// se debe utilizar despues de la funcion generica para Habilitar o 
        /// Deshabilitar controles.
        /// </summary>
        public void HabilitarBotonDescargarArchivo()
        {
            foreach (GridViewRow fila in this.gvArchivos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    ImageButton ibtnDescargar = (ImageButton)fila.FindControl("btnDescargar");
                    ibtnDescargar.Visible = true;
                }
            }
        }

        /// <summary>
        /// Ejecuta el método Update del Ajax UpdatePanel
        /// </summary>
        public void ActualizarControl()
        {
            this.upArchivos.Update();
        }
    }
}