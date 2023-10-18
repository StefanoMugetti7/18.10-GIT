using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestamos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Prestamos
{
    public partial class PrestamosCesionesAutorizar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ModificarDatosAceptar += new Controles.PrestamosCesionesDatos.PrestamosCesionesDatosAceptarEventHandler(AgregarDatos_ModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.PrestamosCesionesDatos.PrestamosCesionesDatosCancelarEventHandler(AgregarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdPrestamoCesion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosCesionesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPrestamoCesion"]);
                PrePrestamosCesiones cesion = new PrePrestamosCesiones();
                cesion.IdPrestamoCesion = parametro;
                this.ModificarDatos.IniciarControl(cesion, Gestion.Autorizar);
            }
        }
        void AgregarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosCesionesListar.aspx"), true);
        }

        void AgregarDatos_ModificarDatosAceptar(object sender, global::Prestamos.Entidades.PrePrestamosCesiones e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosCesionesListar.aspx"), true);
        }
    }
}