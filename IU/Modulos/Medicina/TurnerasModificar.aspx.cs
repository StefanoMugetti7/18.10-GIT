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
    public partial class TurnerasModificar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrModificarDatos.ModificarDatosAceptar += new Controles.TurnerasModificarDatos.ModificarDatosEventHandler(ctrModificarDatos_ModificarDatosAceptar);
            //this.ctrModificarDatos.ModificarDatosCancelar += new Controles.ModificarDatos.ModificarDatosCancelarEventHandler(ctrModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                //MedPrestadores parametro = new MedPrestadores();
                ////Control y Validacion de Parametros
                //if (!this.MisParametrosUrl.Contains("IdPrestador"))
                //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestadoresListar.aspx"), true);

                //int valor = Convert.ToInt32(this.MisParametrosUrl["IdPrestador"]);
                //parametro.IdPrestador = valor;
                this.ctrModificarDatos.IniciarControl(new MedTurneras(), Gestion.Modificar);
            }
        }

        //void ctrModificarDatos_ModificarDatosAceptar(MedTurneras e, global::Comunes.Entidades.Gestion pGestion)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestadoresListar.aspx"), true);
        //}
    }
}