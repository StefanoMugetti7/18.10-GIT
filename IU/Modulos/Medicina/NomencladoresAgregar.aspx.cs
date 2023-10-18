using Comunes.Entidades;
using Medicina.Entidades;
using System;

namespace IU.Modulos.Medicina
{
    public partial class NomencladoresAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModifDatos.ControlModificarDatosCancelar += new IU.Modulos.Medicina.Controles.NomencladoresDatos.ControlDatosCancelarEventHandler(ModifDatos_AfiliadosModificarDatosCancelar);

            if (!this.IsPostBack)
            {

                ModifDatos.IniciarControl(new MedNomencladores(), Gestion.Agregar);
            }
        }

        void ModifDatos_AfiliadosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/NomencladoresListar.aspx"), true);
        }

    }
}