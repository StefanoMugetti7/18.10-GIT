using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Contabilidad;
using Comunes.Entidades;
using System.ComponentModel;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class CuentasContablesBuscarPopUp : ControlesSeguros
    {
        private List<CtbCuentasContables> MisCuentasRamas
        {
            get { return (List<CtbCuentasContables>)Session[this.MiSessionPagina + "MisCuentasRamas"]; }
            set { Session[this.MiSessionPagina + "MisCuentasRamas"] = value; }
        }

        private List<CtbCuentasContables> MisCuentasRamasYContables
        {
            get { return (List<CtbCuentasContables>)Session[this.MiSessionPagina + "MisCuentasRamasYContables"]; }
            set { Session[this.MiSessionPagina + "MisCuentasRamasYContables"] = value; }
        }
        const int niveles = 2;

        public delegate void CuentasContablesBuscarPopUpEventHandler(CtbCuentasContables e);
        public event CuentasContablesBuscarPopUpEventHandler CuentasContablesBuscarSeleccionarPopUp;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if(!this.IsPostBack)
                {
            }
        }

        public void IniciarControl(bool pLimpiarDatos)
        {
            this.IniciarControl(pLimpiarDatos, new List<CtbCuentasContables>());
        }

        public void IniciarControl(bool pLimpiarDatos, List<CtbCuentasContables> pCuentasContables)
        {
            this.IniciarControl(pLimpiarDatos, new CtbCuentasContables(), pCuentasContables);
        }

        public void IniciarControl(bool pLimpiarDatos, CtbCuentasContables pCuentaFiltro, List<CtbCuentasContables> pCuentasContables)
        {
            //ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "scriptBuscarCuentas", ResolveClientUrl("~/Modulos/Contabilidad/Controles/CuentasContablesBuscarPopUp.js"));
            CtbEjerciciosContables filtro = new CtbEjerciciosContables();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            List<CtbEjerciciosContables> lista = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtro);
            this.ddlEjercicioContable.DataSource = lista;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();
            if (this.ddlEjercicioContable.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEjercicioContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (pCuentaFiltro.IdEjercicioContable > 0)
            {
                this.ddlEjercicioContable.SelectedValue = pCuentaFiltro.IdEjercicioContable.ToString();
                this.ddlEjercicioContable.Enabled = false;
            }
            //this.ddlEjerciciosContables_SelectedIndexChanged(null, EventArgs.Empty);
            //string func = string.Format("$(\"#{0}\").modal('show');", modalBuscarCuentaContable.ClientID);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "scriptInitControl", "InitControl();", true);
        }

        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            CtbCuentasContables cta = new CtbCuentasContables();
            cta.IdCuentaContable = this.hdfPopUpCtaCbleBuscarIdCuentacontable.Value == string.Empty ? 0 : Convert.ToInt32( this.hdfPopUpCtaCbleBuscarIdCuentacontable.Value);
            //string keys = this.Request.Form.AllKeys.First(x => !string.IsNullOrEmpty(x) && x.Contains("hdfPopUpCtaCbleBuscarIdCuentacontable"));
            cta = ContabilidadF.CuentasContablesObtenerDatosCompletos(cta);
            if (CuentasContablesBuscarSeleccionarPopUp != null)
                CuentasContablesBuscarSeleccionarPopUp(cta);
        }
    }
}