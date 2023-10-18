using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace IU.Modulos.Bancos
{
    public partial class ChequesTransferirBancos : PaginaTesoreria
    {
        protected override void PageLoadEventTesoreria(object sender, EventArgs e)
        {
            base.PageLoadEventTesoreria(sender, e);
            this.ModificarDatos.ChequesModificarListarCancelar += new IU.Modulos.Bancos.Controles.ChequesModificarListar.ChequesModificarListarCancelarEventHandler(ModificarDatos_ChequesModificarListarCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(this.MiTesoreria);
            }
        }

        void ModificarDatos_ChequesModificarListarCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientos.aspx"), true);
        }
        //protected override void PageLoadEvent(object sender, EventArgs e)
        //{
        //    base.PageLoadEvent(sender, e);
        //    if (!this.IsPostBack)
        //    {
        //        this.ModificarDatos.IniciarControl();
        //    }
        //}
    }
}