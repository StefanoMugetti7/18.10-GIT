using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Facturas.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Facturas
{
    public partial class FacturacionesHabitualesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ModificarDatosAceptar += new Controles.FacturacionesHabitualesDatos.FacturacionesHabitualesDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.FacturacionesHabitualesDatos.FacturacionesHabitualesDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                VTAFacturacionesHabituales FacturaHabitual = new VTAFacturacionesHabituales();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdFacturacionHabitual"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesConsultar.aspx"), true);
                if (!this.MisParametrosUrl.Contains("IdAfiliado"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesConsultar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdFacturacionHabitual"]);
                FacturaHabitual.IdFacturacionHabitual = parametro;
                FacturaHabitual.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"]);
                this.ModificarDatos.IniciarControl(FacturaHabitual, Gestion.Consultar);
            }
        }

        void ModificarDatos_ModificarDatosCancelar(VTAFacturacionesHabituales e, Gestion f)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", f);
            this.MisParametrosUrl.Add("IdAfiliado", e.IdAfiliado);

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesConsultar.aspx"), true);

        }

        void ModificarDatos_ModificarDatosAceptar(VTAFacturacionesHabituales e, Gestion f)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", f);
            this.MisParametrosUrl.Add("IdAfiliado", e.IdAfiliado);

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesConsultar.aspx"), true);
        }
    }
}