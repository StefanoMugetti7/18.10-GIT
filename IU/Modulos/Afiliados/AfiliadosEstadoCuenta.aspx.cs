using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosEstadoCuenta : PaginaAfiliados
    {
        private DataSet MisDatos
        {
            get { return (DataSet)Session[this.MiSessionPagina + "MisDatosTablas"]; }
            set { Session[this.MiSessionPagina + "MisDatosTablas"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                this.IniciarControl();
            }
        }
        public void IniciarControl()
        {
            this.MisDatos = this.ObtenerDatosReporte();

            this.gvAhorros.DataSource = this.MisDatos.Tables[1];
            this.gvAhorros.DataBind();

            this.gvAyudasEconomicas.DataSource = this.MisDatos.Tables[2];
            this.gvAyudasEconomicas.DataBind();

            this.gvCodeudor.DataSource = this.MisDatos.Tables[3];
            this.gvCodeudor.DataBind();

            this.gvCuotasCentroMedico.DataSource = this.MisDatos.Tables[4];
            this.gvCuotasCentroMedico.DataBind();

            this.gvCuotasSociales.DataSource = this.MisDatos.Tables[5];
            this.gvCuotasSociales.DataBind();

        }

        private DataSet ObtenerDatosReporte()
        {
            return AfiliadosF.AfiliadosObtenerEstadoCuenta(MiAfiliado);
        }


        protected void gvAyudasEconomicas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void gvAyudasEconomicas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void gvCodeudor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void gvCodeudor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void gvAhorros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void gvAhorros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void gvCuotasSociales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void gvCuotasSociales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void gvCuotasCentroMedico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void gvCuotasCentroMedico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
    }
}