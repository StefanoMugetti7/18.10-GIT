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
    public partial class SimulacionesConsultar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            //this.ConsultarDatos.PrestamosAfiliadosModificarDatosAceptar += new IU.Modulos.Prestamos.Controles.PrestamoSimulacionModificarDatos.PrestamosAfiliadosDatosAceptarEventHandler(ConsultarDatos_PrestamosAfiliadosModificarDatosAceptar);
            this.ConsultarDatos.PrestamosAfiliadosModificarDatosCancelar += new IU.Modulos.Prestamos.Controles.PrestamoSimulacionModificarDatos.PrestamosAfiliadosDatosCancelarEventHandler(ConsultarDatos_PrestamosAfiliadosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSimulacion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/SimulacionesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdSimulacion"]);

                PrePrestamos prestamoAfiliado = new PrePrestamos();
                prestamoAfiliado.IdSimulacion = parametro;
                prestamoAfiliado.Afiliado = this.MiAfiliado;
                this.ConsultarDatos.IniciarControl(prestamoAfiliado, Gestion.Consultar);
            }
        }

        void ConsultarDatos_PrestamosAfiliadosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/SimulacionesListar.aspx"), true);
        }

        //void ConsultarDatos_PrestamosAfiliadosModificarDatosAceptar(object sender, PrePrestamos e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/SimulacionesListar.aspx"), true);
        //}

    }
}
