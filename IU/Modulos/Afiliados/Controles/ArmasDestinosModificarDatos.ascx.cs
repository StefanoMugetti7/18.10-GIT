using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Afiliados;


namespace IU.Modulos.Afiliados.Controles
{
    public partial class ArmasDestinosModificarDatos : ControlesSeguros
    {
      
         private AfiArmasDestinos  MiArmaDestino
        {
            get { return (AfiArmasDestinos)Session[this.MiSessionPagina + "ArmasModificarDatosMiArmaDestino"]; }
            set { Session[this.MiSessionPagina + "ArmasModificarDatosMiArmaDestino"] = value; }
        }
        
              
         public delegate void ArmasDestinosDatosAceptarEventHandler(object sender, AfiArmasDestinos e);
         public event ArmasDestinosDatosAceptarEventHandler ArmasDestinosModificarDatosAceptar;
         public delegate void ArmasDestinosDatosCancelarEventHandler();
         public event ArmasDestinosDatosCancelarEventHandler ArmasDestinosModificarDatosCancelar;


        protected override void PageLoadEvent(object sender, EventArgs e)
        {
             base.PageLoadEvent(sender, e);
             this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
             
             if (!this.IsPostBack)
             {
                 if (this.MiArmaDestino == null && this.GestionControl != Gestion.Agregar)
                 {
                     this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);

                 }

             }
         
         }

         public void IniciarControl(AfiArmasDestinos pArmas, Gestion pGestion)
         {
             this.GestionControl = pGestion;
             this.CargarCombos();
             this.MiArmaDestino = pArmas;
             this.MiArmaDestino = AfiliadosF.ArmasDestinosObtenerDatosCompletos(pArmas);

             switch (this.GestionControl)
             {
                 case Gestion.Agregar:
                     break;
                 case Gestion.Modificar:
                     MapearObjetoAControles(this.MiArmaDestino);
                     break;
                 case Gestion.Consultar:
                     MapearObjetoAControles(this.MiArmaDestino);
                     this.ddlArma.Enabled = false;
                     this.ddlLocalidad.Enabled = false;
                     this.ddlProvincia.Enabled = false;
                     this.txtCalle.Enabled = false;
                     this.txtCodigoPostal.Enabled = false;
                     this.txtDepartamento.Enabled = false;
                     this.txtDestino.Enabled = false;
                     this.txtNumero.Enabled = false;
                     this.txtPiso.Enabled = false;
                     this.btnAceptar.Visible = false;
                     break;
             }

         }
        
         private void CargarCombos()
         {
             this.ddlProvincia.DataSource = TGEGeneralesF.ProvinciasObtenerLista();
             this.ddlProvincia.DataTextField = "Descripcion";
             this.ddlProvincia.DataValueField = "IdProvincia";
             this.ddlProvincia.DataBind();
             AyudaProgramacion.AgregarItemSeleccione(this.ddlProvincia, this.ObtenerMensajeSistema("SeleccioneOpcion"));


             this.ddlArma.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Armas);
             this.ddlArma.DataValueField = "IdListaValorSistemaDetalle";
             this.ddlArma.DataTextField = "Descripcion";
             this.ddlArma.DataBind();
             AyudaProgramacion.AgregarItemSeleccione(this.ddlArma, this.ObtenerMensajeSistema("SeleccioneOpcion"));


             this.ddlLocalidad.Items.Clear();
             AyudaProgramacion.AgregarItemSeleccione(this.ddlLocalidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            
         }
        
         protected void popUpMensajes_popUpMensajesPostBackAceptar()
         {
             if (this.ArmasDestinosModificarDatosAceptar != null)
                 this.ArmasDestinosModificarDatosAceptar(null, this.MiArmaDestino);
         }

         #region ArmasDatos

         private void MapearControlesAObjeto(AfiArmasDestinos pArmasDestinos)
         {
             pArmasDestinos.Destino = this.txtDestino.Text;
             pArmasDestinos.Arma.IdArma = Convert.ToInt32(this.ddlArma.SelectedValue);
             pArmasDestinos.Arma.Descripcion = this.ddlArma.SelectedItem.Text;
             pArmasDestinos.Calle = this.txtCalle.Text;
             pArmasDestinos.Localidad.CodigoPostal = this.txtCodigoPostal.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoPostal.Text);
             pArmasDestinos.Departamento = this.txtDepartamento.Text;
             pArmasDestinos.Numero = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
             pArmasDestinos.Piso = this.txtPiso.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPiso.Text);
             pArmasDestinos.Localidad.IdCodigoPostal = this.ddlLocalidad.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlLocalidad.SelectedValue);
             pArmasDestinos.Localidad.Descripcion = this.ddlLocalidad.SelectedValue == string.Empty ? string.Empty : this.ddlLocalidad.SelectedItem.Text;
             pArmasDestinos.Localidad.Provincia.IdProvincia = Convert.ToInt32(this.ddlProvincia.SelectedValue);
             pArmasDestinos.Localidad.Provincia.Descripcion = this.ddlProvincia.SelectedItem.Text;
             pArmasDestinos.Predeterminado = this.chkPredeterminado.Checked;
             pArmasDestinos.CodigoPostal = this.txtCodigoPostal.Text;
         }

         private void MapearObjetoAControles(AfiArmasDestinos pArmasDestinos)
         {
             this.txtDestino.Text = pArmasDestinos.Destino;
             this.txtCalle.Text = pArmasDestinos.Calle;
            
             this.txtDepartamento.Text = pArmasDestinos.Departamento;
             this.txtNumero.Text = pArmasDestinos.Numero.ToString();
             this.txtPiso.Text = pArmasDestinos.Piso.ToString();
             this.ddlProvincia.SelectedValue = pArmasDestinos.Localidad.Provincia.IdProvincia.ToString();
             this.ddlProvincia_SelectedIndexChanged(null, EventArgs.Empty);
             this.ddlLocalidad.SelectedValue = pArmasDestinos.Localidad.IdCodigoPostal.HasValue ? pArmasDestinos.Localidad.IdCodigoPostal.ToString() : string.Empty;
             this.ddlArma.SelectedValue = pArmasDestinos.Arma.IdArma.ToString();
             this.chkPredeterminado.Checked = pArmasDestinos.Predeterminado;
             this.txtCodigoPostal.Text = pArmasDestinos.CodigoPostal;
         }

         #endregion

         #region Localidades
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

        #endregion

         protected void btnAceptar_Click(object sender, EventArgs e)
         {
             bool guardo = true;
             this.Page.Validate("ArmasDestinosModificarDatosAceptar");
             if (!this.Page.IsValid)
                 return;
             this.MapearControlesAObjeto(this.MiArmaDestino);
             this.MiArmaDestino.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
             switch (this.GestionControl)
             {
                 case Gestion.Agregar:
                     this.MiArmaDestino.Estado.IdEstado = (int)Estados.Activo;

                     guardo = AfiliadosF.ArmasDestinosAgregar(this.MiArmaDestino);
                     break;
                 case Gestion.Modificar:
                     this.MiArmaDestino.Estado.IdEstado = (int)Estados.Activo;

                     guardo = AfiliadosF.ArmasDestinosModificar(this.MiArmaDestino);
                     break;

                 //case Gestion.Anular:
                 //    this.MiArmaDestino.Estado.IdEstado = (int)Estados.Baja;
                 //    guardo = CuentasPagarF.SolicitudPagoAnular(this.MiSolicitud);
                 //    break;
                 default:
                     break;
             }
             if (guardo)
             {

                 this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiArmaDestino.CodigoMensaje));
             }
             else
             {
                 this.MostrarMensaje(this.MiArmaDestino.CodigoMensaje, true, this.MiArmaDestino.CodigoMensajeArgs);
             }
         }

         protected void btnCancelar_Click(object sender, EventArgs e)
         {

             if (this.ArmasDestinosModificarDatosCancelar != null)
                 this.ArmasDestinosModificarDatosCancelar();
         }

    }
}