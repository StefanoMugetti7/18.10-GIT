using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Comunes.Entidades;
using System.Collections;

namespace IU.Modulos.Facturas
{
    public partial class FacturacionesHabitualesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ModificarDatosAceptar += new Controles.FacturacionesHabitualesDatos.FacturacionesHabitualesDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.FacturacionesHabitualesDatos.FacturacionesHabitualesDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                if (!this.MisParametrosUrl.Contains("IdAfiliado"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesModificar.aspx"), true);

                VTAFacturacionesHabituales FacturaHabitual = new VTAFacturacionesHabituales();
                FacturaHabitual.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"]);
                this.ModificarDatos.IniciarControl(FacturaHabitual, Gestion.Agregar);
            }
        }

        void ModificarDatos_ModificarDatosCancelar(VTAFacturacionesHabituales e, Gestion f)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", Gestion.Modificar);
            this.MisParametrosUrl.Add("IdAfiliado", e.IdAfiliado);

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesModificar.aspx"), true);

        }

        void ModificarDatos_ModificarDatosAceptar(VTAFacturacionesHabituales e, Gestion f)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", Gestion.Modificar);
            this.MisParametrosUrl.Add("IdAfiliado", e.IdAfiliado);

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesModificar.aspx"), true);
        }
    }
}