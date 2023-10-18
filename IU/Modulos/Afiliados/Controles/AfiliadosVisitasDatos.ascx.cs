using Afiliados;
using Afiliados.Entidades;
using Afiliados.Entidades.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class AfiliadosVisitasDatos : ControlesSeguros
    {
        private AfiAfiliadosVisitas MiAfiliadoVisita
        {
            get { return (AfiAfiliadosVisitas)Session[this.MiSessionPagina + "AfiliadosVisitasDatosMiAfiliadoVisita"]; }
            set { Session[this.MiSessionPagina + "AfiliadosVisitasDatosMiAfiliadoVisita"] = value; }
        }

        public delegate void AfiliadoDatosAceptarEventHandler(object sender, AfiAfiliadosVisitas e);
        public delegate void AfiliadoDatosCancelarEventHandler();
        public event AfiliadoDatosCancelarEventHandler AfiliadosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                AfiAfiliadosVisitas parametros = this.BusquedaParametrosObtenerValor<AfiAfiliadosVisitas>();
            }
        }

        public void IniciarControl(AfiAfiliadosVisitas pAfiliado, Gestion pGestion)
        {
            //AfiAfiliadosVisitas parametros = this.BusquedaParametrosObtenerValor<AfiAfiliadosVisitas>();
            //if (pAfiliado.IdAfiliado != parametros.IdAfiliado)
            //{
            //    parametros = new AfiAfiliadosVisitas();
            //    this.BusquedaParametrosGuardarValor<AfiAfiliadosVisitas>(parametros);
            //}
            this.MiAfiliadoVisita = pAfiliado;
            this.GestionControl = pGestion;
            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrCamposValores.IniciarControl(this.MiAfiliadoVisita, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
           // this.MiAfiliadoVisita = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoDocumento, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        private void MapearObjetoAControles(AfiAfiliadosVisitas pAfiliado)
        {
            this.ddlTipoDocumento.SelectedValue = pAfiliado.TipoDocumento.IdTipoDocumento.ToString();
            this.txtApellido.Text = pAfiliado.Apellido;
            this.hdfIdAfiliado.Value = pAfiliado.IdAfiliado.ToString();
            this.hdfNumeroDocumento.Value = pAfiliado.NumeroDocumento.ToString();
            this.txtNombre.Text = pAfiliado.Nombre;
            this.ddlNumeroDocumento.SelectedValue = pAfiliado.NumeroDocumento.ToString();

            //    this.ddlTipoDocumento.SelectedValue = pAfiliado.TipoDocumento.IdTipoDocumento.ToString();
            //    this.txtApellido.Text = pAfiliado.Apellido;
            //    this.ddlNumeroDocumento.Items.Add(new ListItem(pAfiliado.NumeroDocumento, pAfiliado.IdAfiliado.HasValue ? pAfiliado.IdAfiliado.ToString() : pParametro.NumeroDocumento));
            //    this.hdfIdAfiliado.Value = pAfiliado.IdAfiliado.HasValue ? pAfiliado.IdAfiliado.ToString() : string.Empty;
            //    this.hdfNumeroDocumento.Value = pAfiliado.NumeroDocumento.ToString();
            //    this.txtNombre.Text = pAfiliado.Nombre;

            if (pAfiliado.IdAfiliado > 0)
            {
                AfiAfiliadosVisitas afi = new AfiAfiliadosVisitas();
                afi.IdAfiliado = Convert.ToInt32(pAfiliado.IdAfiliado);
            }

            this.ctrCamposValores.IniciarControl(this.MiAfiliadoVisita, new Objeto(), this.GestionControl);
        }

        private void MapearControlesAObjeto(AfiAfiliadosVisitas pAfiliado)
        {
            pAfiliado.NumeroDocumento = Convert.ToInt64(this.hdfNumeroDocumento.Value);
            pAfiliado.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            pAfiliado.Nombre = this.txtNombre.Text;
            pAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.Apellido = this.txtApellido.Text;

            pAfiliado.Campos = this.ctrCamposValores.ObtenerLista();
        }

        
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiAfiliadoVisita);

            this.MiAfiliadoVisita.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;

            this.MiAfiliadoVisita.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAfiliadoVisita.FechaIngreso = DateTime.Now;
                    this.MiAfiliadoVisita.Estado.IdEstado = (int)Estados.Activo;
                    guardo = AfiliadosF.AfiliadosVisitasAgregar(this.MiAfiliadoVisita);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiAfiliadoVisita.CodigoMensaje, false, this.MiAfiliadoVisita.CodigoMensajeArgs);
                        this.popUpMensajes.MostrarMensaje("Desea Cargar otro Ingreso", true);
                        //this.MiAfiliadoVisita = new AfiAfiliadosVisitas();
                        //this.ddlNumeroDocumento.ClearSelection();
                        //this.ctrCamposValores.IniciarControl(this.MiAfiliadoVisita, new Objeto(), this.GestionControl);
                        //this.IniciarControl(MiAfiliadoVisita, Gestion.Agregar);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiAfiliadoVisita.CodigoMensaje, true, this.MiAfiliadoVisita.CodigoMensajeArgs);
            }
           
        }

        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosVisitasAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //this.MisParametrosUrl.Remove("IdAfiliado");//??
            
            if (this.AfiliadosModificarDatosCancelar != null)
                this.AfiliadosModificarDatosCancelar();
        }

    }
}