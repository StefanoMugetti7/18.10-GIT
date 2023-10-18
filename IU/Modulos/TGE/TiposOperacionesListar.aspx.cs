using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using System.Data;
using Generales.FachadaNegocio;
using System.Collections;

namespace IU.Modulos.TGE
{
    public partial class TiposOperacionesListar : PaginaSegura
    {
        private DataTable MisTiposOperaciones
        {
            get { return (DataTable)Session[this.MiSessionPagina + "TiposOperacionesListarMisTiposOperaciones"]; }
            set { Session[this.MiSessionPagina + "TiposOperacionesListarMisTiposOperaciones"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtTipoOperacion, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoOperacion, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtRazonSocial, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("TiposOperacionesAgregar.aspx");
                this.CargarCombos();

                TGETiposOperaciones parametros = this.BusquedaParametrosObtenerValor<TGETiposOperaciones>();
                int idTipoOpe = new ListaParametros(this.MiSessionPagina).ObtenerValor("IdTipoOperacion");
                if (idTipoOpe > 0)
                {
                    parametros.IdTipoOperacion = idTipoOpe;
                    parametros.BusquedaParametros = true;
                }
                if (parametros.BusquedaParametros)
                {
                    this.txtTipoOperacion.Text = parametros.TipoOperacion;
                    this.txtCodigoOperacion.Text = parametros.IdTipoOperacion.ToString() == "0" ? string.Empty : parametros.IdTipoOperacion.ToString();
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString() ;
                    this.ddlTipoMovimiento.SelectedValue = parametros.TipoMovimiento.IdTipoMovimiento.ToString();
                    this.CargarLista();
                }
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesAgregar.aspx"), true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarLista();
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("Todos"));

            this.ddlTipoMovimiento.DataSource = TGEGeneralesF.TiposMovimientosListar();
            this.ddlTipoMovimiento.DataValueField = "IdTipoMovimiento";
            this.ddlTipoMovimiento.DataTextField = "TipoMovimiento";
            this.ddlTipoMovimiento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoMovimiento, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarLista()
        {
            TGETiposOperaciones tipoOp = new TGETiposOperaciones();
            tipoOp.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlEstados.SelectedValue);
            tipoOp.TipoOperacion = this.txtTipoOperacion.Text;
            tipoOp.IdTipoOperacion = this.txtCodigoOperacion.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoOperacion.Text);
            tipoOp.TipoMovimiento.IdTipoMovimiento = this.ddlTipoMovimiento.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoMovimiento.SelectedValue);
            this.MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerListaFiltroDT(tipoOp);
            this.gvDatos.DataSource = this.MisTiposOperaciones;
            this.gvDatos.DataBind();
            this.UpdatePanel1.Update();
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Modificar"
                || e.CommandName == "Consultar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            // DataRow fila = this.MisCargosAfiliadosMensuales.Rows.Find(index);


            switch (e.CommandName)
            {
                case "Modificar":
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("IdTipoOperacion", IdTipoOperacion);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesModificar.aspx"), true);
                    break;
                //case "Auditoria":
                //    cargoAfiliado = CargosF.CuentasCorrientesObtenerDatosCompletos(cargoAfiliado);
                //    this.ctrAuditoria.IniciarControl(cargoAfiliado);
                //    break;
                case "Consultar":
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("IdTipoOperacion", IdTipoOperacion);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesConsultar.aspx"), true);
                    break;
                default:
                    break;
            }
        }


         protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = Convert.ToInt32(e.Row.RowIndex);
                //CarCuentasCorrientes item = (CarCuentasCorrientes)e.Row.DataItem;
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                //ImageButton ibtnAuditoria = (ImageButton)e.Row.FindControl("btnAuditoria");
                //ibtnAuditoria.Visible = this.ValidarPermiso("AuditoriaDatos");
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");

                ibtnConsultar.Visible = this.ValidarPermiso("TiposOperacionesConsultar.aspx");
                ibtnModificar.Visible = this.ValidarPermiso("TiposOperacionesModificar.aspx");
                //int idEstado = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfIdEstado")).Value);
                
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                object suma;
                Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                suma = this.MisTiposOperaciones.Rows.Count;
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), suma);

            }
        }
        #endregion
    }
}