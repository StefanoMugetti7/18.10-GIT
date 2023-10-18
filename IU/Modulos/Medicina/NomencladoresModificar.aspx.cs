using Comunes.Entidades;
using Medicina.Entidades;
using System;

namespace IU.Modulos.Medicina
{
    public partial class NomencladoresModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            this.ModifDatos.ControlModificarDatosCancelar += new IU.Modulos.Medicina.Controles.NomencladoresDatos.ControlDatosCancelarEventHandler(ModifDatos_AfiliadosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                MedNomencladores nomenclador = new MedNomencladores();
                if (!this.MisParametrosUrl.Contains("IdNomenclador"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/NomencladoresListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdNomenclador"]);
                nomenclador.IdNomenclador = parametro;
                this.ModifDatos.IniciarControl(nomenclador, Gestion.Modificar);
            }
        }
        void ModifDatos_AfiliadosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/NomencladoresListar.aspx"), true);
        }
    }
}