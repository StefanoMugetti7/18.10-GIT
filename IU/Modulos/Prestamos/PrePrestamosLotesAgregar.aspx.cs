using Comunes.Entidades;
using Prestamos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Prestamos
{
    public partial class PrePrestamosLotesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.PrestamosLotesModificarDatosCancelar += new Controles.PrePrestamosLotesDatos.PrePrestamosLotesDatossCancelarEventHandler(ModificarDatos_PrestamosLotesModificarDatosCancelar);
            if (!IsPostBack)
            {
                ModificarDatos.IniciarControl(new PrePrestamosLotes(), Gestion.Agregar);
            }
        }

        void ModificarDatos_PrestamosLotesModificarDatosCancelar()
        {
            if (ViewState["UrlReferrer"] != null)
                Response.Redirect(ViewState["UrlReferrer"].ToString(), true);
            else
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrePrestamosLotesListar.aspx"), true);
        }
    }
}