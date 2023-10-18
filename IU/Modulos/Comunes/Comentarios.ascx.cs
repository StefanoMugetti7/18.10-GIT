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
using System.Collections.Generic;
using Generales.Entidades;

using System.Reflection;
using Comunes.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.Comunes
{
    public partial class Comentarios : ControlesSeguros
    {
        private List<TGEComentarios> MisComentarios
        {
            get
            {
                return (Session[this.MiSessionPagina + "ComentariosMisComentarios"] == null ?
                    (List<TGEComentarios>)(Session[this.MiSessionPagina + "ComentariosMisComentarios"] = new List<TGEComentarios>()) : (List<TGEComentarios>)Session[this.MiSessionPagina + "ComentariosMisComentarios"]);
            }
            set { Session[this.MiSessionPagina + "ComentariosMisComentarios"] = value; }
        }

        private List<TGEEstados> MisEstados
        {
            get { return (List<TGEEstados>)Session[this.MiSessionPagina + "ComentariosMisEstados"]; }
            set { Session[this.MiSessionPagina + "ComentariosMisEstados"] = value; }
        }
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    if (!this.IsPostBack)
        //        this.MisComentarios = new List<TGEComentarios>();
        //}
        public delegate void ComentariosPersistirDatosGrillaEventHandler();
        public event ComentariosPersistirDatosGrillaEventHandler ComentariosPersistirDatosGrilla;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtComentario, this.btnAgregarComentario);
            }
        }
   
        /// <summary>
        /// Inicializa el control para agregar comentarios
        /// Llena la grilla con la lista de comentarios cargada en el objeto
        /// </summary>
        /// <param name="pObjeto"></param>
        /// <returns></returns>
        public void IniciarControl(Objeto pObjeto, Gestion pGestion)
        {
            MisEstados = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosComentarios));
            this.ddlEstado.Items.Clear();
            this.ddlEstado.SelectedIndex = -1;
            this.ddlEstado.SelectedValue = null;
            this.ddlEstado.ClearSelection();
            this.ddlEstado.DataSource = MisEstados;
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (pGestion == Gestion.Consultar
                || pGestion == Gestion.Anular
                || pGestion == Gestion.AnularCancelar
                || pGestion == Gestion.Cancelar
                //|| pGestion==Gestion.ConfirmarAgregar
                || pGestion==Gestion.Pagar
                || pGestion==Gestion.Rechazar
                )
            {
                this.btnAgregarComentario.Visible = false;
                this.txtComentario.Visible = false;
                this.lblAgregarComentario.Visible = false;
            }

            PropertyInfo prop = pObjeto.GetType().GetProperty("Comentarios");
            if (prop == null)
            {
                this.btnAgregarComentario.Visible = false;
                this.txtComentario.Visible = false;
                this.lblAgregarComentario.Visible = false;
                return;
            }
            else
            {
                this.MisComentarios = (List<TGEComentarios>)prop.GetValue(pObjeto, null);
                AyudaProgramacion.CargarGrillaListas(this.MisComentarios, true, this.gvComentarios, true);
            }
        }

        /// <summary>
        /// Obtiene la lista de Comentarios actualizada para ser guardada
        /// en la base de datos.
        /// </summary>
        /// <returns></returns>
        public List<TGEComentarios> ObtenerLista()
        {
            PersistirDatosGrilla();
            return this.MisComentarios;
        }

        protected void btnAgregarComentario_Click(object sender, EventArgs e)
        {
            TGEComentarios comentario = new TGEComentarios();
            comentario.Fecha = DateTime.Now;
            comentario.Comentario = this.txtComentario.Text;
            comentario.FechaVencimiento = txtFechaVencimiento.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(txtFechaVencimiento.Text);
            comentario.Estado.IdEstado = Convert.ToInt32(ddlEstado.SelectedValue);
            comentario.Estado.Descripcion = ddlEstado.SelectedItem.Text;
            comentario.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            comentario.EstadoColeccion = EstadoColecciones.Agregado;
            this.MisComentarios.Add(comentario);
            this.txtComentario.Text = string.Empty;

            AyudaProgramacion.CargarGrillaListas(this.MisComentarios, true, this.gvComentarios, true);
        }

        protected void gvComentarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Eliminar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisComentarios.RemoveAt(indiceColeccion);
            this.MisComentarios = AyudaProgramacion.AcomodarIndices<TGEComentarios>(this.MisComentarios);
            this.gvComentarios.DataSource = this.MisComentarios;
            this.gvComentarios.DataBind();
        }

        protected void gvComentarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGEComentarios Comentarios = (TGEComentarios)e.Row.DataItem;
                DropDownList ddlEstado = (DropDownList)e.Row.FindControl("ddlEstados");
                
                ddlEstado.DataSource = MisEstados;
                ddlEstado.DataValueField = "IdEstado";
                ddlEstado.DataTextField = "Descripcion";
                ddlEstado.DataBind();
                if (Comentarios.Estado.IdEstado >= 0)
                {
                    ddlEstado.SelectedValue = Comentarios.Estado.IdEstado.ToString();
                }
   
                if (((TGEComentarios)e.Row.DataItem).EstadoColeccion == EstadoColecciones.Agregado)
                {
                    ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                    string mensaje = this.ObtenerMensajeSistema("POComentarioConfirmarBaja");
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    ibtn.Attributes.Add("OnClick", funcion);
                    ibtn.Visible = true;
                }
            }
        }

        public void PersistirDatosGrilla()
        {
            if (this.MisComentarios.Count > 0)
            {
                foreach (GridViewRow fila in this.gvComentarios.Rows)
                {
                   
                    DropDownList ddlEstados = (DropDownList)fila.FindControl("ddlEstados");
                    int IdComentario = Convert.ToInt32(((HiddenField)fila.FindControl("hdfIdcomentario")).Value);
                    if (this.MisComentarios[fila.DataItemIndex].Estado.IdEstado != Convert.ToInt32(ddlEstados.SelectedValue))
                    {
                        this.MisComentarios[fila.DataItemIndex].Estado.IdEstado = ddlEstados.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlEstados.SelectedValue);
                        this.MisComentarios[fila.DataItemIndex].Estado.Descripcion = ddlEstados.SelectedValue == string.Empty ? string.Empty : ddlEstados.SelectedItem.Text;
                        if(IdComentario > 0)
                        this.MisComentarios[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.Modificado;
                      //  this.MisComentarios[fila.DataItemIndex].UsuarioLogueado = UsuarioActivo.UsuarioLogueado;
                    }

                }
            }
        }
    }
}