using Comunes.Entidades;
using CuentasPagar.FachadaNegocio;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class AnticiposTurismoPopUp : ControlesSeguros
    {
        private DataTable MisReservas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "AnticiposTurismoPopUpMisReservas"]; }
            set { Session[this.MiSessionPagina + "AnticiposTurismoPopUpMisReservas"] = value; }
        }

        private CapProveedores MiProveedor
        {
            get { return (CapProveedores)Session[this.MiSessionPagina + "AnticiposTurismoPopUpMiProveedor"]; }
            set { Session[this.MiSessionPagina + "AnticiposTurismoPopUpMiProveedor"] = value; }
        }

        public delegate void AnticiposTurismoBuscarEventHandler();
        public event AnticiposTurismoBuscarEventHandler AnticiposTurismoBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                MiProveedor = new CapProveedores();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtBuscar, this.btnBuscar);
            }
        }
        
        public void IniciarControl(CapProveedores pProveedor)
        {
            MiProveedor = pProveedor;
            CargarLista();
            UpdatePanel1.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalAnticipoTurismo();", true);
        }
               
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalAnticipoTurismo();", true);
        }

        #region Grilla
        
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                
            }
        }
        
        #endregion

        private void CargarLista()
        {
            MiProveedor.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            MiProveedor.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            this.MisReservas = CuentasPagarF.SolicitudPagoObtenerAnticiposReservasTurismoPendientes(MiProveedor);
            this.gvDatos.DataSource = this.MisReservas;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
                    if (chkIncluir.Checked)
                    {
                        string query = string.Format("IdTipoCargoAfiliadoFormaCobro={0}", gvDatos.DataKeys[fila.DataItemIndex].Value);
                        this.MisReservas.Select(query).FirstOrDefault().SetField<bool>("Incluir", true);
                    }
                }
            }
            Objeto resultado = new Objeto();
            resultado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            if (CuentasPagarF.SolicitudPagoAgregarAnticiposTurismo(resultado, MisReservas))
                this.AnticiposTurismoBuscarSeleccionar?.Invoke();
            else
            {
                MostrarMensaje(resultado.CodigoMensaje, true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalAnticipoTurismo();", true);
            }

        }
    }
}