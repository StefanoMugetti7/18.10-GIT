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
using WooCommerce.NET.WordPress.v2;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosCentroMedico : PaginaAfiliados
    {
        private DataTable MisDatos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisDatosTablas"]; }
            set { Session[this.MiSessionPagina + "MisDatosTablas"] = value; }
        }
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                AfiAfiliados afiliado = new AfiAfiliados();
                afiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
                afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(afiliado);
                this.IniciarControl(afiliado);
            }
        }
        public void IniciarControl(AfiAfiliados pParametro)
        {
            this.MisDatos = this.ObtenerDatosReporte(pParametro);

            this.gvCentroMedico.DataSource = this.MisDatos;
            this.gvCentroMedico.DataBind();
        }
        private DataTable ObtenerDatosReporte(AfiAfiliados pParametro)
        {
            return AfiliadosF.AfiliadosObtenerDatosCentroMedicoOMINT(MiAfiliado);
        }
        protected void gvCentroMedico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void gvCentroMedico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
    }
}