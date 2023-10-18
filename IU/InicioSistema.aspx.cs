using System;
using System.Collections.Generic;
using Reportes.Entidades;

namespace IU
{
    public partial class InicioSistema : PaginaSegura
    {
        private List<REPGraficos> MisGraficos
        {
            get { return (List<REPGraficos>)Session["InicioSistemaMisGraficos"]; }
            set { Session["InicioSistemaMisGraficos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                if (ValidarPermiso("Dashboard.aspx"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("Dashboard.aspx"), true);
            }
        }
    }
}


