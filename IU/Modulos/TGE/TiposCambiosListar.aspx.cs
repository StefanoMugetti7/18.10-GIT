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
using System.Collections.Generic;
using Generales.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.TGE
{
    public partial class TiposCambiosListar : PaginaSegura
    {
        private List<TGETiposCambios> MisTiposCambios
        {
            get { return (List<TGETiposCambios>)Session[this.MiSessionPagina + "TiposCambiosListarMisTiposCambios"]; }
            set { Session[this.MiSessionPagina + "TiposCambiosListarMisTiposCambios"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpModificarDatos.TiposCambiosDatosAceptar += new IU.Modulos.TGE.Control.TiposCambiosDatosPopUp.TiposCambiosDatosAceptarEventHandler(popUpModificarDatos_TiposCambiosDatosAceptar);
            //this.popUpModificarDatos.TiposCambiosDatosCancelar += new IU.Modulos.TGE.Control.TiposCambiosDatosPopUp.TiposCambiosCancelarEventHandler(popUpModificarDatos_TiposCambiosDatosCancelar);
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("TiposCambiosAgregar.aspx");
                this.CargarDatos();
            }
        }

        //void popUpModificarDatos_TiposCambiosDatosCancelar()
        //{
            
        //}

        void popUpModificarDatos_TiposCambiosDatosAceptar(object sender, TGETiposCambios e)
        {
            this.CargarDatos();   
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.popUpModificarDatos.IniciarControl();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisTiposCambios;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        private void CargarDatos()
        {
            this.gvDatos.DataSource = TGEGeneralesF.TGETiposCambiosObtenerLista();
            this.gvDatos.DataBind();
        }
    }
}
