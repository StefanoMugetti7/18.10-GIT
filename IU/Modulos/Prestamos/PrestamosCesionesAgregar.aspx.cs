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
    public partial class PrestamosCesionesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.ModificarDatosAceptar += new Controles.PrestamosCesionesDatos.PrestamosCesionesDatosAceptarEventHandler(AgregarDatos_ModificarDatosAceptar);
            this.AgregarDatos.ModificarDatosCancelar += new Controles.PrestamosCesionesDatos.PrestamosCesionesDatosCancelarEventHandler(AgregarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new PrePrestamosCesiones(), Gestion.Agregar);
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