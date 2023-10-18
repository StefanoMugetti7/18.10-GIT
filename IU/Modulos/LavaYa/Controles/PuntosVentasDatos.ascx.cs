using Comunes.Entidades;
using Generales.FachadaNegocio;
using LavaYa.Entidades;
using LavaYa;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.LavaYa.Controles
{
    public partial class PuntosVentasDatos : ControlesSeguros
    {
        public LavPuntosVentas MiPuntoVenta
        {
            get { return this.PropiedadObtenerValor<LavPuntosVentas>("PuntoVentaDatosMiPuntoVenta"); }
            set { this.PropiedadGuardarValor("PuntoVentaDatosMiPuntoVenta", value); }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"] = value; }
        }
        public List<TGEListasValoresDetalles> MisDias
        {
            get { return this.PropiedadObtenerValor<List<TGEListasValoresDetalles>>("PuntosVentasDatosMisDias"); }
            set { this.PropiedadGuardarValor("PuntosVentasDatosMisDias", value); }
        }


        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.ctrDiasHoras.DiasHorasModificarDatosAceptar += new DiasHorasModificarDatosPopUp.DiasHorasModificarDatosEventHandler(ctrDiasHoras_DiasHorasModificarDatosAceptar);
            base.PageLoadEvent(sender, e);
        }


        public void IniciarControl(LavPuntosVentas pParametro, Gestion pGestion)
        {
            this.MiPuntoVenta = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrCamposValores.IniciarControl(this.MiPuntoVenta, new Objeto(), this.GestionControl);
                    this.ddlEstado.Enabled = false;
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Modificar:
                    this.MiPuntoVenta = PuntosVentasF.PuntosVentasObtenerDatosCompletos(this.MiPuntoVenta);
                    this.MapearObjetoAControles(this.MiPuntoVenta);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MarcarMapa", "MarcarMapa();", true);
                    break;
                case Gestion.Consultar:
                    this.MiPuntoVenta = PuntosVentasF.PuntosVentasObtenerDatosCompletos(this.MiPuntoVenta);
                    this.MapearObjetoAControles(this.MiPuntoVenta);
                    this.ddlLocalizacion.Enabled = false;
                    this.txtContacto.Enabled = false;
                    this.txtDescripcion.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAgregarDiasHoras.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MarcarMapa", "MarcarMapa();", true);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

        }

        private void MapearObjetoAControles(LavPuntosVentas pPuntoVenta)
        {
            this.ddlEstado.SelectedValue = pPuntoVenta.Estado.IdEstado.ToString();
            this.txtCodigoPostal.Text = pPuntoVenta.CodigoPostal;
            this.txtContacto.Text = pPuntoVenta.Contacto;
            this.txtDescripcion.Text = pPuntoVenta.Descripcion;
            this.txtLocalidad.Text = pPuntoVenta.Localidad;
            this.txtNumero.Text = pPuntoVenta.NumeroDireccion.ToString();
            this.txtProvincia.Text = pPuntoVenta.Provincia;
            this.txtPartido.Text = pPuntoVenta.Partido;
            this.txtDireccion.Text = pPuntoVenta.Direccion;
            //this.txtLatitud.Text = pPuntoVenta.Latitud.ToString();
            this.hdfLatitud.Value = pPuntoVenta.Latitud.ToString();
            //this.txtLongitud.Text = pPuntoVenta.Longitud.ToString();
            this.hdfLongitud.Value = pPuntoVenta.Longitud.ToString();
            this.hdfLocalizacionCompleta.Value = pPuntoVenta.Localizacion;

            this.ctrCamposValores.IniciarControl(pPuntoVenta, new Objeto(), this.GestionControl);


            if(pPuntoVenta.PuntosVentasDetalles.Count > 0)
            AyudaProgramacion.CargarGrillaListas<LavPuntosVentasDetalle>(pPuntoVenta.PuntosVentasDetalles, false, this.gvDiasHoras, true);

            if (!string.IsNullOrEmpty(pPuntoVenta.Latitud.ToString()))
            {
                var item = this.ddlLocalizacion.Items.FindByValue(pPuntoVenta.Localizacion.ToString());
                if (item == null)
                    this.ddlLocalizacion.Items.Add(new ListItem(pPuntoVenta.Localizacion, pPuntoVenta.Localizacion));
                this.ddlLocalizacion.SelectedValue = pPuntoVenta.Localizacion;
            }
        }

        private void MapearControlesAObjeto(LavPuntosVentas pPuntoVenta)
        {

            pPuntoVenta.CodigoPostal = this.txtCodigoPostal.Text;
            pPuntoVenta.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pPuntoVenta.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
            pPuntoVenta.Contacto = this.txtContacto.Text;
            pPuntoVenta.Descripcion = this.txtDescripcion.Text;
            pPuntoVenta.Localidad = this.txtLocalidad.Text;
            pPuntoVenta.NumeroDireccion = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
            pPuntoVenta.Provincia = this.txtProvincia.Text;
            pPuntoVenta.Partido = this.txtPartido.Text;
            pPuntoVenta.Direccion = this.txtDireccion.Text;
            pPuntoVenta.Latitud = Convert.ToDecimal(this.hdfLatitud.Value);
            pPuntoVenta.Longitud = Convert.ToDecimal(this.hdfLongitud.Value);
            pPuntoVenta.Localizacion = this.hdfLocalizacionCompleta.Value.ToString();
            
         //   pPuntoVenta.Campos = this.ctrCamposValores.ObtenerLista();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiPuntoVenta);
            this.MiPuntoVenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    MiPuntoVenta.IdPuntoVenta = 0;
                    guardo = PuntosVentasF.PuntosVentasAgregar(this.MiPuntoVenta);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiPuntoVenta.CodigoMensaje, false);
                        this.btnAgregar.Visible = true;
                    }
                    break;
                case Gestion.Modificar:
                    guardo = PuntosVentasF.PuntosVentasModificar(this.MiPuntoVenta);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiPuntoVenta.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiPuntoVenta.CodigoMensaje, true, this.MiPuntoVenta.CodigoMensajeArgs);
                if (this.MiPuntoVenta.dsResultado != null)
                {
                   // this.ctrPopUpGrilla.IniciarControl(this.MiReserva);
                    this.MiPuntoVenta.dsResultado = null;
                }
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/PuntosVentasAgregar.aspx"), true);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }






        protected void gvDiasHoras_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrDiasHoras.IniciarControl(this.MiPuntoVenta.PuntosVentasDetalles[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrDiasHoras.IniciarControl(this.MiPuntoVenta.PuntosVentasDetalles[indiceColeccion], Gestion.Consultar);
            }
        }

        protected void gvDiasHoras_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LavPuntosVentasDetalle item = (LavPuntosVentasDetalle)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            //ImageButton ibtn = (ImageButton)e.Row.Cells[4].FindControl("btnEliminar");
                            ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("DiasHorasConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:

                        break;
                }
            }
        }


        void ctrDiasHoras_DiasHorasModificarDatosAceptar(LavPuntosVentasDetalle e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiPuntoVenta.PuntosVentasDetalles.Add(e);
                    e.IndiceColeccion = this.MiPuntoVenta.PuntosVentasDetalles.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiPuntoVenta.PuntosVentasDetalles[this.MiIndiceDetalleModificar] = e;
                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiPuntoVenta.PuntosVentasDetalles, true, this.gvDiasHoras, true);
            this.upDiasHoras.Update();
        }

        protected void btnAgregarDiasHoras_Click(object sender, EventArgs e)
        {
            LavPuntosVentasDetalle diasHoras = new LavPuntosVentasDetalle();
            diasHoras.IdPuntoVenta = this.MiPuntoVenta.IdPuntoVenta;
            this.ctrDiasHoras.IniciarControl(diasHoras, Gestion.Agregar);
        }

        //GoogleSigned TestingApiKey;
        //GeocodingService CreateService()
        //{
        //    string key = "AIzaSyBq6iLEN_gnzfCnDqJ5bCb2Q8op3Is_9iM";
        //    GoogleSigned aux = new GoogleSigned(key);


        //    var svc = new GeocodingService(aux);
        //    return svc;
        //}
        //public void Localizacion()
        //{
        //    var request = new GeocodingRequest();
        //    request.Address = "Belgrano 124";
        //    var response = CreateService().GetResponse(request);
        //    this.txtLatitud.Text = response.Results[0] == null ? "" : response.Results[0].Geometry.Location.Latitude.ToString();
        //    this.txtLongitud.Text = response.Results[0] == null ? "" : response.Results[0].Geometry.Location.Longitude.ToString();
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "MarcarMapa", "MarcarMapa();", true);
        //}


    }
}