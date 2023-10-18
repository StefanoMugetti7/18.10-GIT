using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Nichos;
using Nichos.Entidades;
using Nichos.LogicaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.Nichos.Controles
{
    public partial class NichosDatos : ControlesSeguros
    {
        public NCHNichos MiNicho
        {
            get { return PropiedadObtenerValor<NCHNichos>("NichoDatosMiNicho"); }
            set { PropiedadGuardarValor("NichoDatosMiNicho", value); }
        }

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!IsPostBack)
            {
                if (MiNicho == null && GestionControl != Gestion.Agregar)               
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);             
            }
        }

        public void IniciarControl(NCHNichos pParametro, Gestion pGestion)
        {
            MiNicho = pParametro;
            GestionControl = pGestion;
            CargarCombos();
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    ddlEstado.Enabled = false;
                    ddlEstado.SelectedValue = 1.ToString();
                    AyudaProgramacion.AgregarItemSeleccione(ddlPanteon, ObtenerMensajeSistema("SeleccioneOpcion"));
                    break;
                case Gestion.Anular:
                    ddlEstado.Enabled = false;
                    ddlTipoNicho.Enabled = false;
                    break;
                case Gestion.Modificar:
                    MiNicho = NichosF.NichosObtenerDatosCompletos(MiNicho);
                    ddlEstado.SelectedValue = 1.ToString();                  
                    MapearObjetoAControles(MiNicho);
                    break;
                case Gestion.Consultar:
                    MiNicho = NichosF.NichosObtenerDatosCompletos(MiNicho);
                    ddlEstado.Enabled = false;
                    ddlSubUbicacion.Enabled = false;
                    ddlUbicacion.Enabled = false;
                    txtCodigo.Enabled = false;
                    ddlCementerio.Enabled = false;
                    MapearObjetoAControles(MiNicho);
                    ddlTipoNicho.Enabled = false;
                    ddlNichoCapacidad.Enabled = false;
                    ddlPanteon.Enabled = false;              
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            //ESTADOS
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //CEMENTERIOS
            this.ddlCementerio.DataSource = CementeriosF.CementeriosObtenerListaActiva(new NCHCementerios());
            this.ddlCementerio.DataValueField = "IdCementerio";
            this.ddlCementerio.DataTextField = "Descripcion";
            this.ddlCementerio.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCementerio, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //TIPOS NICHOS
            this.ddlTipoNicho.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposNichos);
            this.ddlTipoNicho.DataValueField = "IdListaValorDetalle";
            this.ddlTipoNicho.DataTextField = "Descripcion";
            this.ddlTipoNicho.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoNicho, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //CAPACIDAD
            this.ddlNichoCapacidad.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposNichosCapacidad);
            this.ddlNichoCapacidad.DataValueField = "IdListaValorDetalle";
            this.ddlNichoCapacidad.DataTextField = "Descripcion";
            this.ddlNichoCapacidad.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlNichoCapacidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));         

            //UBICACION
            this.ddlUbicacion.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.NichosUbicacion);
            this.ddlUbicacion.DataValueField = "IdListaValorDetalle";
            this.ddlUbicacion.DataTextField = "Descripcion";
            this.ddlUbicacion.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlUbicacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //SUBUBICACION
            this.ddlSubUbicacion.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.NichosSubUbicacion);
            this.ddlSubUbicacion.DataValueField = "IdListaValorDetalle";
            this.ddlSubUbicacion.DataTextField = "Descripcion";
            this.ddlSubUbicacion.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlSubUbicacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoAControles(NCHNichos pNicho) 
        {
            txtCodigo.Text = pNicho.Codigo.ToString();

            //CEMENTERIO
            ListItem item0 = ddlCementerio.Items.FindByValue(pNicho.Panteon.Cementerio.IdCementerio.ToString());
            if (item0 == null)
                ddlCementerio.Items.Add(new ListItem(pNicho.Panteon.Cementerio.Descripcion, pNicho.Panteon.Cementerio.IdCementerio.ToString()));
        
                ddlCementerio.SelectedValue = pNicho.Panteon.Cementerio.IdCementerio.ToString();

            ddlCementerio_OnSelectedIndexChanged(null, EventArgs.Empty);

            //PANTEON
            ListItem item10 = ddlPanteon.Items.FindByValue(pNicho.Panteon.IdPanteon.ToString());
            if (item10 == null)
                ddlPanteon.Items.Add(new ListItem(pNicho.Panteon.Descripcion, pNicho.Panteon.IdPanteon.ToString()));

                ddlPanteon.SelectedValue = pNicho.Panteon.IdPanteon.ToString();

            //TIPO
            ListItem item = ddlTipoNicho.Items.FindByValue(pNicho.TipoNicho.IdTipoNicho.ToString());
            if(item == null)          
                ddlTipoNicho.Items.Add(new ListItem(pNicho.TipoNicho.TipoNicho, pNicho.TipoNicho.IdTipoNicho.ToString()));    
            else
                ddlTipoNicho.SelectedValue = pNicho.TipoNicho.IdTipoNicho.ToString(); 

            //CAPACIDAD
            ListItem item2 = ddlNichoCapacidad.Items.FindByValue(pNicho.NichoCapacidad.IdNichosCapacidad.ToString());
            if(item2 == null)           
                ddlNichoCapacidad.Items.Add(new ListItem(pNicho.NichoCapacidad.NichoCapacidad, pNicho.NichoCapacidad.IdNichosCapacidad.ToString()));         
            else
                ddlNichoCapacidad.SelectedValue = pNicho.NichoCapacidad.IdNichosCapacidad.ToString();

                this.CargarCapacidad();//CARGO TODAS LAS CAPACIDADES
                ddlNichoCapacidad.SelectedValue = pNicho.NichoCapacidad.IdNichosCapacidad.ToString();//ELIJO LA QUE TENIA 


            //ESTADO
            ListItem item1 = ddlTipoNicho.Items.FindByValue(pNicho.TipoNicho.IdTipoNicho.ToString());
            if(item1 == null)          
                ddlEstado.Items.Add(new ListItem(pNicho.Estado.Descripcion, pNicho.Estado.IdEstado.ToString()));    
            else
                ddlEstado.SelectedValue = pNicho.Estado.IdEstado.ToString();


            //UBICACION
            ListItem item3 = ddlUbicacion.Items.FindByValue(pNicho.NichoUbicacion.IdNichosUbicacion.ToString());
            if(item3 == null)
                ddlUbicacion.Items.Add(new ListItem(pNicho.NichoUbicacion.NichoUbicacion, pNicho.NichoUbicacion.IdNichosUbicacion.ToString()));
            else
                ddlUbicacion.SelectedValue = pNicho.NichoUbicacion.IdNichosUbicacion.ToString(); 

            //SUBUBICACION
            ListItem item4 = ddlSubUbicacion.Items.FindByValue(pNicho.NichoSubUbicacion.IdNichosSubUbicacion.ToString());
            if(item4 == null)
                ddlSubUbicacion.Items.Add(new ListItem(pNicho.NichoSubUbicacion.NichoSubUbicacion, pNicho.NichoSubUbicacion.IdNichosSubUbicacion.ToString()));      
            else
                ddlSubUbicacion.SelectedValue = pNicho.NichoSubUbicacion.IdNichosSubUbicacion.ToString();
        }
        private void MapearControlesAObjeto(NCHNichos pParametro)
        {
            pParametro.Estado.IdEstado = ddlEstado.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlEstado.SelectedValue);
            pParametro.NichoCapacidad.IdNichosCapacidad = ddlNichoCapacidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlNichoCapacidad.SelectedValue);
            pParametro.NichoSubUbicacion.IdNichosSubUbicacion = ddlSubUbicacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlSubUbicacion.SelectedValue);
            pParametro.NichoUbicacion.IdNichosUbicacion = ddlUbicacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlUbicacion.SelectedValue);
            pParametro.TipoNicho.IdTipoNicho = ddlTipoNicho.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoNicho.SelectedValue);
            pParametro.Panteon.IdPanteon = ddlPanteon.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlPanteon.SelectedValue);
            pParametro.Codigo = txtCodigo.Text;           
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            btnAceptar.Visible = false;
            MapearControlesAObjeto(MiNicho);

            MiNicho.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    guardo = NichosF.NichosAgregar(MiNicho);
                    if (guardo)
                        MostrarMensaje(MiNicho.CodigoMensaje, false, MiNicho.CodigoMensajeArgs);
                    break;
                case Gestion.Anular:
                    MiNicho.Estado.IdEstado = (int)Estados.Baja;
                    guardo = NichosF.NichosModificar(MiNicho);
                    if (guardo)
                        MostrarMensaje(MiNicho.CodigoMensaje, false);
                    break;
                case Gestion.Modificar:
                    guardo = NichosF.NichosModificar(MiNicho);
                    if (guardo)
                        MostrarMensaje(MiNicho.CodigoMensaje, false);
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                btnAceptar.Visible = true;
                MostrarMensaje(MiNicho.CodigoMensaje, true, MiNicho.CodigoMensajeArgs);
                if (MiNicho.dsResultado != null)
                    MiNicho.dsResultado = null;

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (ControlModificarDatosCancelar != null)
                ControlModificarDatosCancelar();
        }

        protected void ddlTipoNicho_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTipoNicho.SelectedValue))          
                this.CargarCapacidad();         
            else
            {
                this.ddlNichoCapacidad.Items.Clear();
                this.ddlNichoCapacidad.Enabled = false;
                AyudaProgramacion.AgregarItemSeleccione(ddlNichoCapacidad, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        protected void ddlCementerio_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlCementerio.SelectedValue))
            {
                this.ddlPanteon.Enabled = true;
                NCHPanteones panteones = new NCHPanteones();
                panteones.Cementerio.IdCementerio = Convert.ToInt32(ddlCementerio.SelectedValue);
                this.ddlPanteon.DataSource = PanteonesF.PanteonesObtenerListaActiva(panteones);
                this.ddlPanteon.DataValueField = "IdPanteon";
                this.ddlPanteon.DataTextField = "Descripcion";
                this.ddlPanteon.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlPanteon, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
            {
                ddlPanteon.Items.Clear();
                ddlPanteon.Enabled = false;
                AyudaProgramacion.AgregarItemSeleccione(ddlPanteon, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            this.MisParametrosUrl = new Hashtable();

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/NichosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/NichosConsultar.aspx"), true);
            }
        }

        #region gv

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                ibtnConsultar.Visible = this.ValidarPermiso("ReservasConsultar.aspx");

                DataRowView dr = (DataRowView)e.Row.DataItem;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //????
            }
        }
        #endregion
        private void CargarCapacidad()
        {
            TGEListasValoresDetalles lista = new TGEListasValoresDetalles();
            lista.IdRefListaValorDetalle = Convert.ToInt32(ddlTipoNicho.SelectedValue);
            this.ddlNichoCapacidad.Items.Clear();
            this.ddlNichoCapacidad.DataSource = TGEGeneralesF.ListasValoresDetallesDependientes(lista);
            this.ddlNichoCapacidad.DataValueField = "IdListaValorDetalle";
            this.ddlNichoCapacidad.DataTextField = "Descripcion";
            this.ddlNichoCapacidad.DataBind();
            this.ddlNichoCapacidad.Enabled = true;
            AyudaProgramacion.AgregarItemSeleccione(this.ddlNichoCapacidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
       
    }
}