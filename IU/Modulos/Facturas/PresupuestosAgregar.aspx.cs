using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Facturas
{
    public partial class PresupuestosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.PresupuestoModificarDatosAceptar += new Controles.PresupuestosDatos.PresupuestosDatosAceptarEventHandler(ModificarDatos_PresupuestoModificarDatosAceptar);
            this.ModificarDatos.PresupuestoModificarDatosCancelar += new Controles.PresupuestosDatos.PresupuestosDatosCancelarEventHandler(ModificarDatos_PresupuestoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (this.MisParametrosUrl.Contains("Gestion")
                    && (Gestion)this.MisParametrosUrl["Gestion"] == Gestion.Copiar)
                {
                    VTAPresupuestos presu = new VTAPresupuestos();
                    presu.IdPresupuesto = Convert.ToInt32(this.MisParametrosUrl["IdPresupuesto"]);
                    this.ModificarDatos.IniciarControl(presu, Gestion.Copiar);
                }
                else
                    this.ModificarDatos.IniciarControl(new VTAPresupuestos(), Gestion.Agregar);
            }
        }

        void ModificarDatos_PresupuestoModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosListar.aspx"), true);
        }
              
    }
}