using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Contabilidad;
using Generales.FachadaNegocio;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class RubrosDatos : ControlesSeguros
    {
        private CtbRubros MiRubro
        {
            get { return (CtbRubros)Session[this.MiSessionPagina + "MiRubro"]; }
            set { Session[this.MiSessionPagina + "MiRubro"] = value; }
        }

        private List<CtbSubRubros> MisSubRubros
        {
            get { return (List<CtbSubRubros>)Session[this.MiSessionPagina + "SubRubrosListar"]; }
            set { Session[this.MiSessionPagina + "SubRubrosListar"] = value; }
        }

        public delegate void RubroDatosAceptarEventHandler(object sender, CtbRubros e);
        public event RubroDatosAceptarEventHandler RubroDatosAceptar;

        public delegate void RubroDatosCancelarEventHandler();
        public event RubroDatosCancelarEventHandler RubroDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiRubro == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
     
        /// <summary>
        /// Inicializa el control de Alta y Modificación de un Rubro
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbRubros pRubro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            CtbSubRubros parametros = this.BusquedaParametrosObtenerValor<CtbSubRubros>();
            this.CargarLista(parametros);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiRubro = pRubro;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();;
                    break;
                case Gestion.Modificar:
                    this.MiRubro = ContabilidadF.RubrosObtenerDatosCompletos(pRubro);
                    this.MapearObjetoAControles(this.MiRubro);
                    break;
                case Gestion.Consultar:
                    this.MiRubro = ContabilidadF.RubrosObtenerDatosCompletos(pRubro);
                    this.MapearObjetoAControles(this.MiRubro);
                    this.txtRubro.Enabled = false;
                    this.txtCodigoRubro.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiRubro);
            this.MiRubro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.RubrosAgregar(this.MiRubro);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.RubrosModificar(this.MiRubro);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiRubro.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiRubro.CodigoMensaje, true, this.MiRubro.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.RubroDatosCancelar != null)
                this.RubroDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.RubroDatosAceptar != null)
                this.RubroDatosAceptar(null, this.MiRubro);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
       {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton movimientos = (ImageButton)e.Row.FindControl("btnMovimientos");
                //Deshabilita los checkbox cuando se entra en modo consulta
                if (this.GestionControl == Gestion.Consultar)
                {
                    CheckBox chkBox = (CheckBox)e.Row.FindControl("chkRow");
                    if (chkBox != null)
                    {
                        chkBox.Enabled = false;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSubRubros.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.MapearGrillaAObjeto();
            CtbSubRubros parametros = this.BusquedaParametrosObtenerValor<CtbSubRubros>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbSubRubros>(parametros);
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisSubRubros;
            this.gvDatos.DataBind();
            this.SeleccionarSubRubrosGrilla();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisSubRubros = this.OrdenarGrillaDatos<CtbSubRubros>(this.MisSubRubros, e);
            this.gvDatos.DataSource = this.MisSubRubros;
            this.gvDatos.DataBind();
        }

        private void MapearObjetoAControles(CtbRubros pRubro)
        {
            this.txtRubro.Text = pRubro.Rubro;
            this.txtCodigoRubro.Text = pRubro.CodigoRubro;
            this.ddlEstado.SelectedValue = pRubro.Estado.IdEstado.ToString();
            this.SeleccionarSubRubrosGrilla();
        }

        private void SeleccionarSubRubrosGrilla()
        {
            foreach (GridViewRow row in gvDatos.Rows)
            {
                int indiceColeccion = Convert.ToInt32(gvDatos.DataKeys[row.RowIndex].Value);
                CtbSubRubros subRubro = this.MisSubRubros[indiceColeccion];
                if (this.MiRubro.SubRubros.Exists(x => x.IdSubRubro == subRubro.IdSubRubro))
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    chkRow.Checked = true;
                }
            }
        }

        private void MapearGrillaAObjeto()
        {
            foreach (GridViewRow row in gvDatos.Rows)
            {
                CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                int indiceColeccion = Convert.ToInt32(gvDatos.DataKeys[row.RowIndex].Value);
                CtbSubRubros subRubro = this.MisSubRubros[indiceColeccion];
                if (chkRow.Checked && !(this.MiRubro.SubRubros.Exists(x => x.IdSubRubro == subRubro.IdSubRubro)))
                    this.MiRubro.SubRubros.Add(ContabilidadF.SubRubrosObtenerDatosCompletos(subRubro));
            }
        }

        private void MapearControlesAObjeto(CtbRubros pRubro)
        {
            pRubro.Rubro = this.txtRubro.Text;
            pRubro.CodigoRubro = this.txtCodigoRubro.Text;
            pRubro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }

        private void CargarLista(CtbSubRubros pSubRubros)
        {
            pSubRubros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbSubRubros>(pSubRubros);
            this.MisSubRubros = ContabilidadF.SubRubrosObtenerListar(pSubRubros);
            this.gvDatos.DataSource = this.MisSubRubros;
            this.gvDatos.PageIndex = pSubRubros.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}