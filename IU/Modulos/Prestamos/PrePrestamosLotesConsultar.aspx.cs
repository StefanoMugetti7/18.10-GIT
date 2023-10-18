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
    public partial class PrePrestamosLotesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.PrestamosLotesModificarDatosCancelar += new Controles.PrePrestamosLotesDatos.PrePrestamosLotesDatossCancelarEventHandler(ModificarDatos_PrestamosLotesModificarDatosCancelar);
            if (!IsPostBack)
            {
                PrePrestamosLotes prestamoLote = new PrePrestamosLotes();
                //Control y Validacion de Parametros
                if (!MisParametrosUrl.Contains("IdPrestamoLote"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrePrestamosLotesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPrestamoLote"]);
                prestamoLote.IdPrestamoLote = parametro;

                ModificarDatos.IniciarControl(prestamoLote, Gestion.Consultar);
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