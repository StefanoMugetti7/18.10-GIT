using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class CertificadoSupervivenciaConsultar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ModificarDatos.CertificadosSupervivenciaModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.CertificadoSupervivenciaDatos.AfiCertificadosSupervivenciaAceptarEventHandler(ModificarDatos_CertificadosSupervivenciaModificarDatosAceptar);
            this.ModificarDatos.CertificadosSupervivenciaModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.CertificadoSupervivenciaDatos.AfiCertificadosSupervivenciaCancelarEventHandler(ModificarDatos_CertificadosSupervivenciaModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AfiCertificadosSupervivencia certificado = new AfiCertificadosSupervivencia();

                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCertificadoSupervivencia"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CertificadoSupervivenciaListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCertificadoSupervivencia"]);
                certificado.IdCertificadoSupervivencia = parametro;
                this.ModificarDatos.IniciarControl(certificado, Gestion.Consultar);
            }
        }

        void ModificarDatos_CertificadosSupervivenciaModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CertificadoSupervivenciaListar.aspx"), true);
        }

        void ModificarDatos_CertificadosSupervivenciaModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiCertificadosSupervivencia e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CertificadoSupervivenciaListar.aspx"), true);
        }
    }
}
