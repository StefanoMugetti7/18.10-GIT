using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.Entidades;
using Comunes.Entidades;
using Tesorerias.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class ChequesCambiarValoresAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.ChequesCambiarDatosAceptar += new Controles.ChequesCambiarValoresDatos.ChequesModificarDatosAceptarEventHandler(ModificarDatos_ChequesCambiarDatosAceptar);
            //this.ModificarDatos.ChequesCambiarDatosCancelar += new Controles.ChequesCambiarValoresDatos.ChequesModificarDatosCancelarEventHandler(ModificarDatos_ChequesCambiarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new TESCajasMovimientos(), Gestion.Agregar);
            }
        }

        //void ModificarDatos_ChequesCambiarDatosCancelar()
        //{
        //    this.Response.Redirect("~/Modulos/Bancos/ChequesListar.aspx", true);
        //}

        //void ModificarDatos_ChequesCambiarDatosAceptar(global::Bancos.Entidades.TESCheques e)
        //{
        //    this.Response.Redirect("~/Modulos/Bancos/ChequesListar.aspx", true);
        //}
    }
}