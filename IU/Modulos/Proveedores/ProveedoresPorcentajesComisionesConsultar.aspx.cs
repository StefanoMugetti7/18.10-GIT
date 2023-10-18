﻿using Comunes.Entidades;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Proveedores
{
    public partial class ProveedoresPorcentajesComisionesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ModificarDatosAceptar += new Controles.ProveedoresPorcentajesComisionesDatos.ModificarDatosAceptarEventHandler(ModificarDatos_ProveedorModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.ProveedoresPorcentajesComisionesDatos.ModificarDatosCancelarEventHandler(ModificarDatos_ProveedorModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CapProveedoresPorcentajesComisiones proveedor = new CapProveedoresPorcentajesComisiones();
                //Control y Validacion de Parametros
                //if (!this.MisParametrosUrl.Contains("IdProveedor"))
                ListaParametros listaparametros = new ListaParametros(this.MiSessionPagina);
                if (!listaparametros.Existe("IdProveedorPorcentajeComision"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedorerPorcentajesComisionesListar.aspx"), true);

                //int parametro = Convert.ToInt32(this.MisParametrosUrl["IdProveedor"]);
                int parametro = listaparametros.ObtenerValor("IdProveedorPorcentajeComision");
                proveedor.IdProveedorPorcentajeComision = parametro;

                this.ModificarDatos.IniciarControl(proveedor, Gestion.Consultar);
            }
        }

        void ModificarDatos_ProveedorModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresPorcentajesComisionesListar.aspx"), true);
        }

        void ModificarDatos_ProveedorModificarDatosAceptar(object sender, CapProveedoresPorcentajesComisiones e)
        {
            //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresListar.aspx"), true);
            ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
            parametros.Agregar("IdProveedorPorcentajeComision", e.IdProveedorPorcentajeComision);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresPorcentajesComisionesListar.aspx"), true);
        }
    }
}