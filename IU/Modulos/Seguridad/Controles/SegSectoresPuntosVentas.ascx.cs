
using Comunes.Entidades;
using Facturas;
using Facturas.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Web.UI.WebControls;

namespace IU.Modulos.Seguridad.Controles
{
    public partial class SegSectoresPuntosVentas : ControlesSeguros
    {

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;




        public VTASectoresPuntosVentas MiSectorPuntoVenta
        {
            get { return PropiedadObtenerValor<VTASectoresPuntosVentas>("MiSectorPuntoVentaDatosMiMiSectorPuntoVenta"); }
            set { PropiedadGuardarValor("MiSectorPuntoVentaDatosMiMiSectorPuntoVenta", value); }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!IsPostBack)
            {
                if (this.MiSectorPuntoVenta == null && GestionControl != Gestion.Agregar)
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        public void IniciarControl(VTASectoresPuntosVentas pParametro, Gestion pGestion)
        {
            this.MiSectorPuntoVenta = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlEstado.Enabled = false;
                    this.ddlEstado.SelectedValue = 1.ToString();
                    this.ddlSector.Enabled = false;
                    break;
                case Gestion.Modificar:
                    this.MiSectorPuntoVenta = FacturasF.VTASectoresPuntosVentasObtenerDatosCompletos(MiSectorPuntoVenta);
                    //this.CargarLista(MiSectorPuntoVenta);
                    this.MapearObjetoAControles(MiSectorPuntoVenta);
                    break;
                case Gestion.Consultar:
                    this.MiSectorPuntoVenta = FacturasF.VTASectoresPuntosVentasObtenerDatosCompletos(MiSectorPuntoVenta);
                    this.ddlSector.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.ddlFilial.Enabled = false;
                    this.txtPuntoVenta.Enabled = false;
                    this.MapearObjetoAControles(MiSectorPuntoVenta);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            ddlEstado.DataValueField = "IdEstado";
            ddlEstado.DataTextField = "Descripcion";
            ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlEstado, ObtenerMensajeSistema("SeleccioneOpcion"));

            ddlFilial.DataSource = UsuarioActivo.Filiales;
            ddlFilial.DataValueField = "IdFilial";
            ddlFilial.DataTextField = "Filial";
            ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlFilial, ObtenerMensajeSistema("SeleccioneOpcion"));

            if(this.MiSectorPuntoVenta.IdSectorPuntoVenta > 0)
            {
                ddlSector.DataSource = FacturasF.VTASectoresPuntosVentasObtenerSectoresPorFilial(this.MiSectorPuntoVenta,this.MiSectorPuntoVenta.Filial.IdFilial);
                ddlSector.DataValueField = "IdSector";
                ddlSector.DataTextField = "Sector";
                ddlSector.DataBind();
            }
                AyudaProgramacion.InsertarItemSeleccione(ddlSector, ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoAControles(VTASectoresPuntosVentas pParametro)
        {
            this.txtPuntoVenta.Text = pParametro.PuntoVenta.ToString();
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();

            ListItem filial = this.ddlFilial.Items.FindByValue(pParametro.Filial.IdFilial.ToString());
            if (filial == null && pParametro.Filial.IdFilial > 0)
                this.ddlFilial.Items.Add(new ListItem(pParametro.Filial.Filial, pParametro.Filial.IdFilial.ToString()));
            this.ddlFilial.SelectedValue = pParametro.Filial.IdFilial == 0 ? string.Empty : pParametro.Filial.IdFilial.ToString();

            ListItem sector = this.ddlSector.Items.FindByValue(pParametro.Sector.IdSector.ToString());
            if (sector == null && pParametro.Sector.IdSector > 0)
                this.ddlSector.Items.Add(new ListItem(pParametro.Sector.Sector, pParametro.Sector.IdSector.ToString()));
            this.ddlSector.SelectedValue = pParametro.Sector.IdSector == 0 ? string.Empty : pParametro.Sector.IdSector.ToString();

        }

        private void MapearControlesAObjeto(VTASectoresPuntosVentas pParametro)
        {
            pParametro.PuntoVenta = Convert.ToInt32(this.txtPuntoVenta.Text);
            pParametro.Filial.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
            pParametro.Sector.IdSector = Convert.ToInt32(this.ddlSector.SelectedValue);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
        }

        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlFilial.SelectedValue))
            {
                this.ddlSector.Enabled = false;
                this.ddlSector.Items.Clear();
                AyudaProgramacion.InsertarItemSeleccione(ddlSector, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
            {
                this.ddlSector.Enabled = true;
            }

            this.MiSectorPuntoVenta.Filial.IdFilial =this.ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);

            this.ddlSector.Items.Clear();
            this.ddlSector.DataSource = FacturasF.VTASectoresPuntosVentasObtenerSectoresPorFilial(this.MiSectorPuntoVenta, this.MiSectorPuntoVenta.Filial.IdFilial);
            this.ddlSector.DataValueField = "IdSector";
            this.ddlSector.DataTextField = "Sector";
            this.ddlSector.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(ddlSector, ObtenerMensajeSistema("SeleccioneOpcion"));

        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiSectorPuntoVenta);

            this.MiSectorPuntoVenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    guardo = FacturasF.VTASectoresPuntosVentasAgregar(MiSectorPuntoVenta);
                    if (guardo)
                        MostrarMensaje(MiSectorPuntoVenta.CodigoMensaje, false, MiSectorPuntoVenta.CodigoMensajeArgs);
                    break;
                case Gestion.Anular:
                    MiSectorPuntoVenta.Estado.IdEstado = (int)Estados.Baja;
                    guardo = FacturasF.VTASectoresPuntosVentasModificar(MiSectorPuntoVenta);
                    if (guardo)
                        MostrarMensaje(MiSectorPuntoVenta.CodigoMensaje, false);
                    break;
                case Gestion.Modificar:
                    guardo = FacturasF.VTASectoresPuntosVentasModificar(MiSectorPuntoVenta);
                    if (guardo)
                        MostrarMensaje(MiSectorPuntoVenta.CodigoMensaje, false);
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                MostrarMensaje(MiSectorPuntoVenta.CodigoMensaje, true, MiSectorPuntoVenta.CodigoMensajeArgs);
                if (MiSectorPuntoVenta.dsResultado != null)
                    MiSectorPuntoVenta.dsResultado = null;

            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (ControlModificarDatosCancelar != null)
                ControlModificarDatosCancelar();
        }
    }
}