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
using Afiliados.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class AfiliadoModificarDatosDomicilioPopUp : ControlesSeguros
    {
        private AfiDomicilios  MiDomicilio
        {
            get { return (AfiDomicilios)Session[this.MiSessionPagina + "AfiliadoModificarDatosDomicilioMiDomicilio"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosDomicilioMiDomicilio"] = value; }
        }

        public delegate void AfiliadoModificarDatosDomicilioEventHandler(object sender, AfiDomicilios e, Gestion pGestion);
        public event AfiliadoModificarDatosDomicilioEventHandler AfiliadosModificarDatosAceptar;

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

        public void IniciarControl(AfiDomicilios pDomicilio, Gestion pGestion)
        {
            AyudaProgramacion.LimpiarControles(this, true);
            this.CargarCombos();
            this.MiDomicilio = pDomicilio;
            this.GestionControl = pGestion;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.txtFe.Text = DateTime.Now.ToShortDateString();
                    pDomicilio.Estado.IdEstado = (int)Estados.Activo;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    //pAfiliado.Estado = TGEGeneralesF.TGEEstadosObtener(pAfiliado.Estado);

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
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalDomicilios();", true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("AfiliadosDatosDomicilios");
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

            if (this.AfiliadosModificarDatosAceptar != null)
                this.AfiliadosModificarDatosAceptar(sender, this.MiDomicilio, this.GestionControl);
            AyudaProgramacion.LimpiarControles(this, true);
            //this.mpePopUp.Hide();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalDomicilios", "HideModalDomicilios();", true);
        }

        private void MapearControlesAObjeto(AfiDomicilios pDomicilio)
        {
            pDomicilio.DomicilioTipo.IdDomicilioTipo = Convert.ToInt32(this.ddlTipoDomicilio.SelectedValue);
            pDomicilio.DomicilioTipo.Descripcion = this.ddlTipoDomicilio.SelectedItem.Text;
            pDomicilio.Calle = this.txtCalle.Text;
            //pDomicilio.Localidad.CodigoPostal = this.txtCodigoPostal.Text; // == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoPostal.Text);
            pDomicilio.Departamento = this.txtDepartamento.Text;
            pDomicilio.Numero = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
            pDomicilio.Piso = this.txtPiso.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPiso.Text);
            //pDomicilio.Localidad = this.MisCodigosPostales[this.ddlLocalidad.SelectedIndex];
            pDomicilio.Localidad.IdCodigoPostal = this.ddlLocalidad.SelectedValue==string.Empty? default(int?)  : Convert.ToInt32( this.ddlLocalidad.SelectedValue);
            pDomicilio.Localidad.Descripcion = this.ddlLocalidad.SelectedValue==string.Empty? string.Empty : this.ddlLocalidad.SelectedItem.Text;
            pDomicilio.Localidad.Provincia.IdProvincia = Convert.ToInt32(this.ddlProvincia.SelectedValue);
            pDomicilio.Localidad.Provincia.Descripcion = this.ddlProvincia.SelectedItem.Text;
            pDomicilio.Predeterminado = this.chkPredeterminado.Checked;
            pDomicilio.CodigoPostal = this.txtCodigoPostal.Text;
            pDomicilio.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pDomicilio.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
        }

        private void MapearObjetoAControles(AfiDomicilios pDomicilio)
        {
            this.txtCalle.Text = pDomicilio.Calle;
            //this.txtCodigoPostal.Text = pDomicilio.CodigoPostal; //pDomicilio.Localidad.CodigoPostal.ToString();
            this.txtDepartamento.Text = pDomicilio.Departamento;
            this.txtNumero.Text = pDomicilio.Numero.ToString();
            this.txtPiso.Text = pDomicilio.Piso.ToString();
            this.ddlProvincia.SelectedValue = pDomicilio.Localidad.Provincia.IdProvincia.ToString();
            this.ddlProvincia_SelectedIndexChanged(null, EventArgs.Empty);
            this.ddlLocalidad.SelectedValue = pDomicilio.Localidad.IdCodigoPostal.HasValue ? pDomicilio.Localidad.IdCodigoPostal.ToString() : string.Empty;
            this.ddlTipoDomicilio.SelectedValue = pDomicilio.DomicilioTipo.IdDomicilioTipo.ToString();
            this.chkPredeterminado.Checked = pDomicilio.Predeterminado;
            this.txtCodigoPostal.Text = pDomicilio.CodigoPostal;
            this.ddlEstado.SelectedValue = pDomicilio.Estado.IdEstado.ToString();
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
            this.ddlTipoDomicilio.DataSource = AfiliadosF.DomiciliosTiposObtenerLista();
            this.ddlTipoDomicilio.DataValueField = "IdDomicilioTipo";
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

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }

        private void CargarCodigosPostalesPorProvincia(AfiDomicilios pDomicilio)
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