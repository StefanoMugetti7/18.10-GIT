using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Afiliados.Entidades;
using Afiliados;
using Arba.WebServices;

namespace IU
{
    public partial class prueba : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            ConsultarPadronLN padronLN = new ConsultarPadronLN();
            ConsultarPadronEntidad entidad = new ConsultarPadronEntidad();
            entidad.NumeroCUIT = 30714930083;
            entidad.Fecha = DateTime.Now;
            padronLN.ConsultarPadron(entidad);

            Xml1.DocumentContent = entidad.Respuesta.InnerXml;

        }
    }
}