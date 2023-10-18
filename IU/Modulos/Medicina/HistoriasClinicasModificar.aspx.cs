using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Medicina
{
    public partial class HistoriasClinicasModificar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ctrModificarDatos.ModificarDatosAceptar += new Controles.HistoriasClinicasModificarDatos.ModificarDatosAceptarEventHandler(ctrModificarDatos_ModificarDatosAceptar);
            this.ctrModificarDatos.ModificarDatosCancelar += new Controles.HistoriasClinicasModificarDatos.ModificarDatosCancelarEventHandler(ctrModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                if (this.MiAfiliado.IdAfiliado == 0)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/TurnosMedicosListar.aspx"), true);
                }
                MedHistoriasClinicas parametro = new MedHistoriasClinicas();
                int valor=0;
                //Control y Validacion de Parametros
                if (this.MisParametrosUrl.Contains("IdPrestacion"))
                 valor = Convert.ToInt32(this.MisParametrosUrl["IdPrestacion"]);

                parametro.IdAfiliado = this.MiAfiliado.IdAfiliado;
                this.ctrModificarDatos.IniciarControl(parametro, valor, Gestion.Modificar);
            }
        }

        void ctrModificarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/TurnosMedicosListar.aspx"), true);
        }

        void ctrModificarDatos_ModificarDatosAceptar(MedHistoriasClinicas e, Gestion pGestion)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/TurnosMedicosListar.aspx"), true);
        }
    }
}
