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
using Cargos.Entidades;
using System.Collections.Generic;
using Cobros.Entidades;
using Cargos;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.Entidades;
using Contabilidad;

namespace IU.Modulos.Cobros
{
    public partial class OrdenesCobrosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrlDatos.OrdenesCobrosModificarDatosAceptar += new IU.Modulos.Cobros.Controles.OrdenesCobrosDatos.OrdenesCobrosDatosAceptarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosAceptar);
            this.ctrlDatos.OrdenesCobrosModificarDatosCancelar += new IU.Modulos.Cobros.Controles.OrdenesCobrosDatos.OrdenesCobrosDatosCancelarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                //CobOrdenesCobros ordenCobro;
                //if (this.MisParametrosUrl.Contains("OrdenCobro"))
                //{
                //    ordenCobro = (CobOrdenesCobros)this.MisParametrosUrl["OrdenCobro"];
                //}
                //else
                //    ordenCobro = new CobOrdenesCobros();

                this.ctrlDatos.IniciarControl(new CobOrdenesCobros(), Gestion.Agregar);
            }
        }

        void ctrlDatos_OrdenesCobrosModificarDatosCancelar()
        {
            //this.MisParametrosUrl = new Hashtable();
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosListar.aspx"), true);
        }

        //void ctrlDatos_OrdenesCobrosModificarDatosAceptar(CobOrdenesCobros e)
        //{
        //    //this.MisParametrosUrl = new Hashtable();
        //    //this.MisParametrosUrl.Add("OrdenCobro", e);
        //    //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAgregarFormasCobros.aspx"), true);
        //}
    }
}
