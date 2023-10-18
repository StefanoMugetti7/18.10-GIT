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
using Comunes.Entidades;
using Seguridad.FachadaNegocio;
using System.Collections.Generic;

namespace IU.Modulos.Seguridad.Controles
{
    public partial class SegMenuesModificarDatos : ControlesSeguros
    {
        private List<Menues> MisMenues
        {
            get { return (List<Menues>)Session[this.MiSessionPagina + "SegMenuesModificarDatosMisMenues"]; }
            set { Session[this.MiSessionPagina + "SegMenuesModificarDatosMisMenues"] = value; }
        }

        private Menues MiMenu
        {
            get { return (Menues)Session[this.MiSessionPagina + "SegMenuesModificarDatosMiMenu"]; }
            set { Session[this.MiSessionPagina + "SegMenuesModificarDatosMiMenu"] = value; }
        }

        public delegate void SegMenuesModificarDatosAceptarEventHandler(object sender, Menues e);
        public event SegMenuesModificarDatosAceptarEventHandler MenuesModificarDatosAceptar;
        public delegate void SegMenuesModificarDatosCancelarEventHandler();
        public event SegMenuesModificarDatosCancelarEventHandler MenuesModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.ctrPopUpMenu.MenuesModificarDatosAceptar += new SegMenuesModificarDatosPopUp.MenuesModificarDatosPopUpEventHandler(ctrPopUpMenu_MenuesModificarDatosAceptar);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtMenu, this.btnAceptar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtOrden, this.btnAceptar);
            }
        }

        void ctrPopUpMenu_MenuesModificarDatosAceptar(object sender, Menues e, Gestion pGestion)
        {
            if (pGestion == Gestion.Agregar)
            {
                e.EstadoColeccion = EstadoColecciones.SinCambio;
                this.MisMenues.Add(e);
            }
            else {
                Menues actua = this.MisMenues.FirstOrDefault(x => x.IdMenu == e.IdMenu);
                AyudaProgramacion.MatchObjectProperties(e, actua);
            }
            //Actualizo el Menu Padre
            Menues men = this.MisMenues.Single(x=>x.IdMenu==e.IdMenuPadre);
            if (!men.Hijos)
            {
                men.Hijos = true;
                men.EstadoColeccion = EstadoColecciones.Modificado;
                SeguridadF.MenuesActualizar(men);


            }

            this.tvMenues.Nodes.Clear();
            TreeNode menu = null;

            this.CargarMenu(this.MiMenu, menu);
            //this.tvMenues.ExpandAll();
            this.upMenues.Update();
        }

        public void IniciarControl(Menues pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MisMenues = SeguridadF.MenuesObtenerLista(AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo)).OrderBy(x=>x.Orden).ToList();
            this.MiMenu = this.MisMenues.Single(x => x.IdMenu == pParametro.IdMenu);
            this.btnAgregar.Visible = this.UsuarioActivo.EsAdministradorGeneral;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    break;
                case Gestion.Modificar:
                    this.MapearObjetoAControles(this.MiMenu);
                    break;
                case Gestion.Consultar:
                    break;
                default:
                    break;
            }
            this.btnAceptar.Visible = false;
        }

        private void MapearObjetoAControles(Menues pParametro)
        {
            this.txtMenu.Text = pParametro.Menu;
            this.txtOrden.Text = pParametro.Orden.ToString();
            TreeNode menu = null;
            this.CargarMenu(pParametro, menu);
        }

        private void CargarMenu(Menues pMenuPadre, TreeNode mi)
        {
            List<Menues> Menues = this.MisMenues.FindAll(delegate(Menues m) { return m.IdMenuPadre == pMenuPadre.IdMenu; });
            Menues menuPadre;
            foreach (Menues men in Menues)
            {
                TreeNode miNuevo = new TreeNode();
                bool bHijos = (bool)men.Hijos;
                menuPadre = new Menues();
                menuPadre.IdMenu = men.IdMenu;
                miNuevo.Text = men.Menu;
                miNuevo.Value = men.IdMenu.ToString();

                if (mi == null)
                    this.tvMenues.Nodes.Add(miNuevo);
                else
                    mi.ChildNodes.Add(miNuevo);
                if (bHijos)
                    CargarMenu(menuPadre, miNuevo);
            }

        }

        private void MapearControlesAObjeto(Menues pParametro)
        {
            pParametro.Menu = this.txtMenu.Text;
            pParametro.Orden = this.txtOrden.Text == string.Empty ? 0 : Convert.ToInt32(txtOrden.Text);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiMenu);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiMenu.EstadoColeccion = EstadoColecciones.Agregado;
                    guardo = SeguridadF.MenuesActualizar(this.MiMenu);
                    break;
                case Gestion.Modificar:
                    this.MiMenu.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiMenu, GestionControl);
                    guardo = SeguridadF.MenuesActualizar(this.MiMenu);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema("ResultadoTransaccion"));
            }
            else
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema("ResultadoTransaccionIncorrecto"));
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.MenuesModificarDatosCancelar != null)
                this.MenuesModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.MenuesModificarDatosAceptar != null)
                this.MenuesModificarDatosAceptar(null, this.MiMenu);
        }

        protected void tvMenues_SelectedNodeChanged(object sender, EventArgs e)
        {
            int idMenu = Convert.ToInt32(((TreeView)sender).SelectedValue);
            Menues men = this.MisMenues.Single(x => x.IdMenu == idMenu);
            this.ctrPopUpMenu.IniciarControl(men, this.GestionControl, false, this.MiMenu.IdMenu);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.ctrPopUpMenu.IniciarControl(new Menues(), Gestion.Agregar, false, this.MiMenu.IdMenu);
        }
        
    }
}