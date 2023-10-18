﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Prestamos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Prestamos
{
    public partial class PrestamosAfiliadosAnularConfirmado : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ModificarDatos.PrestamosAfiliadosModificarDatosAceptar += new IU.Modulos.Prestamos.Controles.PrestamoAfiliadoModificarDatos.PrestamosAfiliadosDatosAceptarEventHandler(ModificarDatos_PrestamosAfiliadosModificarDatosAceptar);
            this.ModificarDatos.PrestamosAfiliadosModificarDatosCancelar += new IU.Modulos.Prestamos.Controles.PrestamoAfiliadoModificarDatos.PrestamosAfiliadosDatosCancelarEventHandler(ModificarDatos_PrestamosAfiliadosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdPrestamo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPrestamo"]);

                PrePrestamos prestamoAfiliado = new PrePrestamos();
                prestamoAfiliado.IdPrestamo = parametro;
                prestamoAfiliado.Afiliado = this.MiAfiliado;
                this.ModificarDatos.IniciarControl(prestamoAfiliado, Gestion.AnularConfirmar, IU.Modulos.Prestamos.Controles.PrestamoAfiliadoModificarDatos.TipoAutorizar.SinPrivilegio);
            }
        }

        protected void ModificarDatos_PrestamosAfiliadosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosListar.aspx"), true);
        }

        protected void ModificarDatos_PrestamosAfiliadosModificarDatosAceptar(object sender, PrePrestamos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosListar.aspx"), true);
        }
    }
}
