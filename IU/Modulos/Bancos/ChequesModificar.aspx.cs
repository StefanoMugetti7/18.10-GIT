using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Bancos.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class ChequesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ChequesModificarDatosAceptar += new Controles.ChequesModificarDatos.ChequesModificarDatosAceptarEventHandler(ModificarDatos_ChequesModificarDatosAceptar);
            this.ModificarDatos.ChequesModificarDatosCancelar += new Controles.ChequesModificarDatos.ChequesModificarDatosCancelarEventHandler(ModificarDatos_ChequesModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TESCheques cheque = new TESCheques();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCheque"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCheque"]);
                cheque.IdCheque = parametro;

                this.ModificarDatos.IniciarControl(cheque, Gestion.Modificar);
            }
        }

        void ModificarDatos_ChequesModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesListar.aspx"), true);
        }

        void ModificarDatos_ChequesModificarDatosAceptar(TESCheques e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesListar.aspx"), true);
        }
    }
}