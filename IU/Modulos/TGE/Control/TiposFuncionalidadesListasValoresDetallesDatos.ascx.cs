using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace IU.Modulos.TGE.Control
{
    public partial class TiposFuncionalidadesListasValoresDetallesDatos : ControlesSeguros
    {
        private TGETiposFuncionalidadesListasValoresDetalles MiTipoFuncionalidadListaValor
        {
            get
            {
                return (Session[this.MiSessionPagina + "TiposFuncionalidadesListasValoresDetallesDatosMiListaValor"] == null ?
                    (TGETiposFuncionalidadesListasValoresDetalles)(Session[this.MiSessionPagina + "TiposFuncionalidadesListasValoresDetallesDatosMiListaValor"] = new TGETiposFuncionalidadesListasValoresDetalles()) : (TGETiposFuncionalidadesListasValoresDetalles)Session[this.MiSessionPagina + "TiposFuncionalidadesListasValoresDetallesDatosMiListaValor"]);
            }
            set { Session[this.MiSessionPagina + "TiposFuncionalidadesListasValoresDetallesDatosMiListaValor"] = value; }
        }

        public delegate void ModificarDatosAceptarEventHandler();
        public event ModificarDatosAceptarEventHandler ModificarDatosAceptar;

        public delegate void ModificarDatosCancelarEventHandler();
        public event ModificarDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar +=new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
            }
        }

        public void IniciarControl(Gestion pGestion)
        {
            //this.MiEstado = new TGEEstados();
            this.GestionControl = pGestion;

            if (pGestion == Gestion.Modificar)
            {
                this.btnAceptar.Visible = true;
                this.chkListasValoresDetalles.Enabled = true;
            }
            this.CargarCombos();
        }

        private void CargarCombos()
        {
            this.ddlTiposFuncionalidades.DataSource = TGEGeneralesF.TGETiposFuncionalidadesObtenerLista();
            this.ddlTiposFuncionalidades.DataValueField = "IdTipoFuncionalidad";
            this.ddlTiposFuncionalidades.DataTextField = "TipoFuncionalidad";
            this.ddlTiposFuncionalidades.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposFuncionalidades, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TGEListasValores lista = new TGEListasValores();
            lista.Estado.IdEstado = (int)Estados.Activo;
            this.ddlListasValores.DataSource = TGEGeneralesF.ListasValoresObtenerListaFiltro(lista);
            this.ddlListasValores.DataValueField = "IdListaValor";
            this.ddlListasValores.DataTextField = "ListaValor";
            this.ddlListasValores.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlListasValores, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTiposFuncionalidades.SelectedValue)
                && !string.IsNullOrEmpty(this.ddlListasValores.SelectedValue))
            {
                this.MiTipoFuncionalidadListaValor.IdTipoFuncionalidad = Convert.ToInt32(this.ddlTiposFuncionalidades.SelectedValue);
                this.MiTipoFuncionalidadListaValor.ListaValor.IdListaValor = Convert.ToInt32(this.ddlListasValores.SelectedValue);

                this.chkListasValoresDetalles.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(this.MiTipoFuncionalidadListaValor.ListaValor);
                this.chkListasValoresDetalles.DataValueField = "IdListaValorDetalle";
                this.chkListasValoresDetalles.DataTextField = "Descripcion";
                this.chkListasValoresDetalles.DataBind();
                
                this.MiTipoFuncionalidadListaValor.ListaValor.ListasValoresDetalles = TGEGeneralesF.ListasValoresObtenerListaDetalle(this.MiTipoFuncionalidadListaValor);
                this.CargarTiposFuncionalidadesListaValoresDetalles(this.MiTipoFuncionalidadListaValor.ListaValor);
                this.btnAceptar.Visible = true;
            }
            else
            {
                this.chkListasValoresDetalles.Items.Clear();
                this.btnAceptar.Visible = false;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = false;
            this.MiTipoFuncionalidadListaValor.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ObtenerTiposFuncionalidadesListasValoresDetalles(this.MiTipoFuncionalidadListaValor.ListaValor);
            switch (this.GestionControl)
            {
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.TiposFuncionalidadesListasValoresDetalles(this.MiTipoFuncionalidadListaValor);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.MostrarMensaje(this.MiTipoFuncionalidadListaValor.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(this.MiTipoFuncionalidadListaValor.CodigoMensaje, true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ModificarDatosCancelar != null)
                this.ModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ModificarDatosAceptar != null)
                this.ModificarDatosAceptar();
        }

        private void CargarTiposFuncionalidadesListaValoresDetalles(TGEListasValores pParametro)
        {
            foreach (TGEListasValoresDetalles detalle in pParametro.ListasValoresDetalles)
            {
                foreach (ListItem item in this.chkListasValoresDetalles.Items)
                {
                    if (Convert.ToInt32(item.Value) == detalle.IdListaValorDetalle)
                        item.Selected = true;
                }
            }
        }

        private void ObtenerTiposFuncionalidadesListasValoresDetalles(TGEListasValores pParametro)
        {
            TGEListasValoresDetalles detalle;

            foreach (ListItem lst in this.chkListasValoresDetalles.Items)
            {
                detalle = pParametro.ListasValoresDetalles.Find(delegate(TGEListasValoresDetalles per)
                { return per.IdListaValorDetalle == Convert.ToInt32(lst.Value); });

                if (detalle == null && lst.Selected)
                {
                    detalle = new TGEListasValoresDetalles();
                    detalle.IdListaValorDetalle = Convert.ToInt32(lst.Value);
                    detalle.Descripcion = lst.Text;
                    pParametro.ListasValoresDetalles.Add(detalle);
                    detalle.EstadoColeccion = EstadoColecciones.Agregado;
                }
                else if (detalle != null && !lst.Selected)
                    detalle.EstadoColeccion = EstadoColecciones.Borrado;

            }
        }
    }
}