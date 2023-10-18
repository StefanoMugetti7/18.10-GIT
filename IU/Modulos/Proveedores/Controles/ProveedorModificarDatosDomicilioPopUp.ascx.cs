using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Proveedores.Entidades;
using Comunes.Entidades;
using Proveedores;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Collections.Generic;


namespace IU.Modulos.Proveedores.Controles
{
    public partial class ProveedorModificarDatosDomicilioPopUp : ControlesSeguros
    {
        private CapProveedoresDomicilios MiDomicilio
        {
            get { return (CapProveedoresDomicilios)Session[this.MiSessionPagina + "ProveedorModificarDatosDomicilioMiDomicilio"]; }
            set { Session[this.MiSessionPagina + "ProveedorModificarDatosDomicilioMiDomicilio"] = value; }
        }

        //public delegate void ProveedorModificarDatosDomicilioEventHandler(object sender, CapProveedoresDomicilios e, Gestion pGestion);
        //public event ProveedorModificarDatosDomicilioEventHandler ProveedoresModificarDatosAceptar;
        public delegate void ProveedorModificarDatosDomicilioEventHandler(object sender, CapProveedoresDomicilios e, Gestion pGestion);
        public event ProveedorModificarDatosDomicilioEventHandler ProveedoresModificarDatosAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCalle, btnAceptar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoPostal, btnAceptar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDepartamento, btnAceptar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumero, btnAceptar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPiso, btnAceptar);
            }
        }

        public void IniciarControl(CapProveedoresDomicilios pDomicilio, Gestion pGestion)
        {
            this.CargarCombos();
            this.MiDomicilio = pDomicilio;
            this.GestionControl = pGestion;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.txtFe.Text = DateTime.Now.ToShortDateString();
                    //pDomicilio.Estado.IdEstado = (int)Estados.Activo;
                    //pEmpleado.Estado = TGEGeneralesF.TGEEstadosObtener(pEmpleado.Estado);

                    break;
                case Gestion.Modificar:
                    this.CargarCodigosPostalesPorProvincia(this.MiDomicilio);
                    this.MapearObjetoAControles(this.MiDomicilio);
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.CargarCodigosPostalesPorProvincia(this.MiDomicilio);
                    this.MapearObjetoAControles(this.MiDomicilio);
                    this.btnAceptar.Visible = false;
                    this.txtCalle.Enabled = false;
                    this.txtCodigoPostal.Enabled = false;
                    this.txtDepartamento.Enabled = false;
                    this.txtNumero.Enabled = false;
                    this.txtPiso.Enabled = false;
                    this.ddlLocalidad.Enabled = false;
                    this.ddlProvincia.Enabled = false;
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarTurnos();", true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("ProveedoresDatosDomicilios");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiDomicilio);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiDomicilio.Estado.IdEstado = (int)Estados.Activo;
                    this.MiDomicilio.EstadoColeccion = EstadoColecciones.Agregado;
                    break;
                case Gestion.Modificar:
                    this.MiDomicilio.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiDomicilio, this.GestionControl);
                    break;
                default:
                    break;
            }

            if (this.ProveedoresModificarDatosAceptar != null)
                this.ProveedoresModificarDatosAceptar(sender, this.MiDomicilio, this.GestionControl);

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarTurnos();", true);
        }

        private void MapearControlesAObjeto(CapProveedoresDomicilios pDomicilio)
        {

            //USANDO DOMICILIO TIPO DE CLIENTES
            pDomicilio.TipoDomicilio.IdTipoDomicilio= Convert.ToInt32(this.ddlTipoDomicilio.SelectedValue);
            pDomicilio.Calle = this.txtCalle.Text;
            pDomicilio.DeptoOficina = this.txtDepartamento.Text;
            pDomicilio.Numero = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
            pDomicilio.Piso = this.txtPiso.Text;
            pDomicilio.Predeterminado = this.chkPredeterminado.Checked;
            //pDomicilio.Localidad = this.MisCodigosPostales[this.ddlLocalidad.SelectedIndex];
            pDomicilio.Localidad.IdCodigoPostal = this.ddlLocalidad.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlLocalidad.SelectedValue);

            //VERRRR
            pDomicilio.Localidad.Descripcion = this.ddlLocalidad.SelectedValue == string.Empty ? string.Empty : this.ddlLocalidad.SelectedItem.Text;
            pDomicilio.Localidad.Provincia.IdProvincia = Convert.ToInt32(this.ddlProvincia.SelectedValue);
            pDomicilio.Localidad.Provincia.Descripcion = this.ddlProvincia.SelectedItem.Text;
            //pDomicilio.CodigoPostal.CodigoPostal = Convert.ToInt32(this.txtCodigoPostal.Text);
            pDomicilio.Localidad.CodigoPostal = this.txtCodigoPostal.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoPostal.Text);
            pDomicilio.TipoDomicilio.Descripcion = this.ddlTipoDomicilio.SelectedItem.Text;
            
            
        }

        private void MapearObjetoAControles(CapProveedoresDomicilios pDomicilio)
        {
            this.txtCalle.Text = pDomicilio.Calle;
            this.txtCodigoPostal.Text = pDomicilio.Localidad.CodigoPostal.ToString();
            this.txtDepartamento.Text = pDomicilio.DeptoOficina;
            this.txtNumero.Text = pDomicilio.Numero.ToString();
            this.txtPiso.Text = pDomicilio.Piso.ToString();
            this.ddlProvincia.SelectedValue = pDomicilio.Localidad.Provincia.IdProvincia.ToString();
            this.ddlProvincia_SelectedIndexChanged(null, EventArgs.Empty);
            this.ddlLocalidad.SelectedValue = pDomicilio.Localidad.IdCodigoPostal.ToString();
            this.ddlTipoDomicilio.SelectedValue = pDomicilio.TipoDomicilio.IdTipoDomicilio.ToString();
            //this.txtCodigoPostal.Text = pDomicilio.CodigoPostal.CodigoPostal.ToString();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarTurnos();", true);
        }

        //protected void txtCodigoPostal_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtCodigoPostal.Text.Trim()))
        //    {
        //        MostrarMensaje("IngresarCodigoPostal", true);
        //    }
        //    else if (this.ddlProvincia.SelectedValue == "0")
        //    {
        //        this.txtCodigoPostal.Text = string.Empty;
        //        MostrarMensaje("SeleccionarProvincia", true);
        //    }
        //    else
        //    {
        //        TGECodigosPostales codigoPostal = new TGECodigosPostales();
        //        codigoPostal.CodigoPostal = Convert.ToInt32(this.txtCodigoPostal.GetCurrencyAmount());
        //        codigoPostal.Provincia.IdProvincia = Convert.ToInt32(this.ddlProvincia.SelectedValue);
        //        this.CargarCodigosPostalesPorProvincia(codigoPostal);
        //    }
        //    this.mpePopUp.Show();
        //}

        private void CargarCombos()
        {
            this.ddlTipoDomicilio.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposDomicilios);
            this.ddlTipoDomicilio.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoDomicilio.DataTextField = "Descripcion";
            this.ddlTipoDomicilio.DataBind();

            this.ddlProvincia.DataSource = TGEGeneralesF.ProvinciasObtenerLista();
            this.ddlProvincia.DataTextField = "Descripcion";
            this.ddlProvincia.DataValueField = "IdProvincia";
            this.ddlProvincia.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlProvincia, this.ObtenerMensajeSistema("SeleccioneOpcion"));


            this.ddlLocalidad.Items.Clear();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlLocalidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //ListItem item = new ListItem();
            //item.Text = this.ObtenerMensajeSistema("SeleccioneOpcion");
            //item.Value = string.Empty;
            //List<ListItem> lista = new List<ListItem>();
            //lista.Add(item);
            //this.ddlLocalidad.DataSource = lista;
            //this.ddlLocalidad.DataBind();
            //this.ddlLocalidad.SelectedValue = string.Empty;
        }

        private void CargarCodigosPostalesPorProvincia(CapProveedoresDomicilios pDomicilio)
        {
            //this.MisCodigosPostales = TGEGeneralesF.CodigosPostalesObtenerPorProvinciaCodigo(pCodigoPostal);
            this.ddlLocalidad.DataSource = null;
            this.ddlLocalidad.DataBind();
            List<TGECodigosPostales> codigos = TGEGeneralesF.CodigosPostalesObenerLocalidades(pDomicilio.Localidad.Provincia);
            if (codigos.Count > 0)
            {
                this.ddlLocalidad.DataSource = codigos;
                this.ddlLocalidad.DataValueField = "IdCodigoPostal";
                this.ddlLocalidad.DataTextField = "Descripcion";
                this.ddlLocalidad.DataBind();
                this.ddlLocalidad.SelectedValue = pDomicilio.Localidad.IdCodigoPostal.ToString();
            }
            else
            {
                //this.ddlLocalidad.Items.Clear();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlLocalidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                //Cuando no hay Localidades es porque Reside en el Exterior
                //Deshabilito el validador de Codigos Postales
                this.rfvCodigoPostal.Enabled = false;
            }
            //this.UpdatePanel1.Update();
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlProvincia.SelectedValue))
            {
                TGEProvincias prov = new TGEProvincias();
                prov.IdProvincia = Convert.ToInt32(this.ddlProvincia.SelectedValue);
                List<TGECodigosPostales> codigos = TGEGeneralesF.CodigosPostalesObenerLocalidades(prov);
                if (codigos.Count > 0)
                {
                    this.ddlLocalidad.Items.Clear();
                    this.ddlLocalidad.SelectedIndex = -1;
                    this.ddlLocalidad.SelectedValue = null;
                    this.ddlLocalidad.ClearSelection();
                    this.ddlLocalidad.DataSource = codigos;
                    this.ddlLocalidad.DataTextField = "Descripcion";
                    this.ddlLocalidad.DataValueField = "IdCodigoPostal";
                    this.ddlLocalidad.DataBind();
                    rfvCodigoPostal.Enabled = true;
                }
                else
                {
                    this.ddlLocalidad.Items.Clear();
                    this.ddlLocalidad.SelectedIndex = -1;
                    this.ddlLocalidad.SelectedValue = null;
                    this.ddlLocalidad.ClearSelection();
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlLocalidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    //Cuando no hay Localidades es porque Reside en el Exterior
                    //Deshabilito el validador de Codigos Postales
                    this.rfvCodigoPostal.Enabled = false;
                }
            }
            else
            {
                this.ddlLocalidad.Items.Clear();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlLocalidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
    }
}