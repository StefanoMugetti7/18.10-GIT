using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Seguridad.Entidades;
using Seguridad.FachadaNegocio;

namespace IU
{
    public partial class SegGestionarPerfiles : PaginaSegura
    {

        public List<Perfiles> MisPerfiles
        {
            get { return this.PropiedadObtenerValor<List<Perfiles>>("PerfilesMisPerfiles"); }
            set { this.PropiedadGuardarValor("PerfilesMisPerfiles", value); }
        }

        public Perfiles MiPerfil
        {
            get { return this.PropiedadObtenerValor<Perfiles>("PerfilesMiPerfil"); }
            set { this.PropiedadGuardarValor("PerfilesMiPerfil" ,value); }
        }

        public int IndiceMenuSeleccionado
        {
            get { return this.PropiedadObtenerValor<int>("PerfilesIndiceMenuSeleccionado"); }
            set { this.PropiedadGuardarValor("PerfilesIndiceMenuSeleccionado", value); }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.MenuesControlesPagina.SegGestionarPerfilesMenuesControlesPaginasAceptar += new IU.Modulos.Seguridad.SegGestionarPerfilesMenuesControlesPaginas.SegGestionarPerfilesMenuesControlesPaginasAceptarEventHandler(MenuesControlesPagina_SegGestionarPerfilesMenuesControlesPaginasAceptar);
            if (!this.IsPostBack)
            {
                //Limpio las varialbes de sesion
                this.MiPerfil = new Perfiles();
                this.MisPerfiles = new List<Perfiles>();

                this.tvMenu.Attributes.Add("onclick", "OnTreeClick(event)");
                //this.tvMenu.Attributes.Add("onclick", "postBackByObject(event)");
                this.CargarGrillaDatos();
                this.mvGestion.SetActiveView(vDatos);
                this.ctrCamposValores.IniciarControl(MiPerfil, new Objeto(), this.MiGestion);
            }
        }

        void MenuesControlesPagina_SegGestionarPerfilesMenuesControlesPaginasAceptar(object sender, Menues e, bool resultado)
        {
            this.MiPerfil.Menues[this.IndiceMenuSeleccionado] = e;
            this.CargarControlesPagina((Menues)e);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString() 
                || e.CommandName == Gestion.Modificar.ToString() 
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == "Copiar"))
                return;

            this.btnAgregar.Visible = false;
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiPerfil = this.MisPerfiles[indiceColeccion];

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.MiPerfil.EstadoColeccion = EstadoColecciones.Borrado;
                this.ModificarPerfil(this.MiPerfil);
                this.CargarGrillaDatos();
            }
            else
            {
                //Cargo el Arbol de Menues
                tvMenu.Nodes.Clear();
                this.ctrCamposValores.BorrarControlesParametros();
                TreeNode menu = null;
                Menues menuPadre = new Menues();
                menuPadre.IdMenuPadre = (int)EnumMenues.AdministrarTodos;
                menuPadre.IdPerfil = MiPerfil.IdPerfil;

                this.CargarMenu(menuPadre, menu);

                this.MiPerfil.Menues = SeguridadF.MenuesObtenerPorPerfil(this.MiPerfil);

                if (e.CommandName == "Copiar")
                {
                    this.CopiarPerfil(this.MiPerfil);
                }
                else
                {
                    AyudaProgramacion.MapearEntidadControles(this.MiPerfil, vCargarDatos);
                    this.MiPerfil.Usuarios = SeguridadF.UsuariosObtenerPorPerfil(this.MiPerfil);
                    this.CargarMenuPerfil(this.MiPerfil, tvMenu.Nodes);
                    this.ctrPerfilReportes.IniciarControl(this.MiPerfil);
                    this.ctrPerfilProcesos.IniciarControl(this.MiPerfil);
                    this.ctrCamposValores.IniciarControl(this.MiPerfil, new Objeto(), this.MiGestion);
                    this.tcPerfiles.ActiveTab = this.tpMenues;
                    this.mvGestion.SetActiveView(vCargarDatos);

                    if (e.CommandName == Gestion.Consultar.ToString())
                    {
                        this.MiGestion = Gestion.Consultar;
                        AyudaProgramacion.HabilitarControles(vCargarDatos, false, this);
                    }
                    else if (e.CommandName == Gestion.Modificar.ToString())
                    {
                        this.MiGestion = Gestion.Modificar;
                        AyudaProgramacion.HabilitarControles(vCargarDatos, true, this);
                    }
                }
            }
        }

        private bool CopiarPerfil(Perfiles pPerfil)
        {
            Perfiles perfilCopia = new Perfiles();
            perfilCopia.Perfil = string.Concat(pPerfil.Perfil, this.ObtenerMensajeSistema("PerfilCopiar"));
            this.txtPerfil.Text = perfilCopia.Perfil;
            perfilCopia.EstadoColeccion = EstadoColecciones.Agregado;
            Menues menCopia;
            //SegControlesPaginas controlCopia;
            foreach (Menues men in pPerfil.Menues)
            {
                menCopia = new Menues();
                menCopia.IdMenu = men.IdMenu;
                menCopia.Menu = men.Menu;
                menCopia.EstadoColeccion = EstadoColecciones.Agregado;

                //men.ControlesPaginas = SeguridadF.SegControlesPaginaObtenerLista(men);
                //foreach (SegControlesPaginas control in men.ControlesPaginas)
                //{
                //    if (control.TienePermiso)
                //    {
                //        controlCopia = new SegControlesPaginas();
                //        controlCopia.IdMenuesControlesPaginas = control.IdMenuesControlesPaginas;
                //        controlCopia.IdControlesPaginas = control.IdControlesPaginas;
                //        controlCopia.TienePermiso = control.TienePermiso;
                //        controlCopia.Descripcion = control.Descripcion;
                //        controlCopia.EstadoColeccion = EstadoColecciones.Agregado;
                //        menCopia.ControlesPaginas.Add(controlCopia);
                //        controlCopia.IndiceColeccion = menCopia.ControlesPaginas.IndexOf(controlCopia);
                //    }
                //}
                perfilCopia.Menues.Add(menCopia);
                menCopia.IndiceColeccion = perfilCopia.Menues.IndexOf(menCopia);
            }
            if (SeguridadF.PerfilesAgregar(perfilCopia))
            {
                this.CargarGrillaDatos();
                this.MostrarMensaje(perfilCopia.CodigoMensaje, false);
                return true;
            }
            else
            {
                this.MostrarMensaje(this.MiPerfil.CodigoMensaje, true);
                return false;
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtn = (ImageButton)e.Row.FindControl("btnCopiar");
                string mensaje = this.ObtenerMensajeSistema("PerfilConfirmarCopia");
                mensaje = string.Format(mensaje, e.Row.Cells[0].Text);
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                ibtn.Attributes.Add("OnClick", funcion);
            }
        }

        private void CargarGrillaDatos()
        {
            Objeto usuario= new Objeto();
            usuario.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MisPerfiles = SeguridadF.PerfilesObtenerTodos(false, usuario);
            this.gvDatos.DataSource = this.MisPerfiles;
            this.gvDatos.DataBind();
        }

        private void CargarMenu(Menues pMenuPadre, TreeNode mi)
        {
            List<Menues> Menues = SeguridadF.MenuesObtenerPorPadre(pMenuPadre);
            Menues menuPadre;
            foreach (Menues men in Menues)
            {
                TreeNode miNuevo = new TreeNode();
                bool bHijos = (bool)men.Hijos;
                menuPadre = new Menues();
                menuPadre.IdMenuPadre = men.IdMenu;
                menuPadre.IdPerfil = pMenuPadre.IdPerfil;
                miNuevo.Text = men.Menu;
                miNuevo.Value = men.IdMenu.ToString();

                if (mi == null)
                    tvMenu.Nodes.Add(miNuevo);
                else
                    mi.ChildNodes.Add(miNuevo);
                if (bHijos)
                    CargarMenu(menuPadre, miNuevo);
            }

        }

        private void CargarMenuPerfil(Perfiles pPerfil, TreeNodeCollection pNodes)
        {
            foreach (TreeNode n in pNodes)
            {
                foreach (Menues m in pPerfil.Menues)
                {
                    if (Convert.ToInt32(n.Value) == m.IdMenu)
                        n.Checked = true;
                }
                if (n.ChildNodes.Count > 0)
                    CargarMenuPerfil(pPerfil, n.ChildNodes);
            }
        }

        private void ObtenerPerfilMenu(Perfiles pPerfil, TreeNodeCollection pNodes)
        {
            Menues menu;
            foreach (TreeNode n in pNodes)
            {
                menu = pPerfil.Menues.Find(delegate(Menues men)
                { return men.IdMenu == Convert.ToInt32(n.Value); });

                if (menu != null)
                {
                    if (!n.Checked)
                    {
                        menu.EstadoColeccion = EstadoColecciones.Borrado;
                    }
                }
                else
                {
                    if (n.Checked)
                    {
                        menu = new Menues();
                        menu.IdMenu = Convert.ToInt32(n.Value);
                        menu.EstadoColeccion = EstadoColecciones.Agregado;
                        pPerfil.Menues.Add(menu);
                    }
                }
                if (n.ChildNodes.Count > 0)
                    ObtenerPerfilMenu(pPerfil, n.ChildNodes);
            }
        }

        private void LimpiarControles()
        {
            this.txtPerfil.Text = string.Empty;
            this.LimpiarTreeView(this.tvMenu.Nodes);
            this.mvGestion.SetActiveView(vDatos);
            this.btnAgregar.Visible = true;
            this.chkControles.DataSource = null;
            this.chkControles.DataBind();
            this.chkControles.Visible = false;
            this.btnAgregarControles.Visible = false;
            this.ctrCamposValores.IniciarControl(this.MiPerfil, new Objeto(), this.MiGestion);
            
        }

        private void LimpiarTreeView(TreeNodeCollection nodes)
        {
            foreach (TreeNode n in nodes)
            {
                n.Checked = false;
                n.Selected = false;
                if (n.ChildNodes.Count > 0)
                    LimpiarTreeView(n.ChildNodes);
            }
        }

        private void AgregarPerfil(Perfiles pPerfil)
        {
            pPerfil.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario( this.UsuarioActivo);

            if (SeguridadF.PerfilesAgregar(pPerfil))
            {
                this.LimpiarControles();
                this.MostrarMensaje(pPerfil.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(pPerfil.CodigoMensaje, true);
            }
        }

        private void ModificarPerfil(Perfiles pPerfil)
        {
            pPerfil.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            if (SeguridadF.PerfilesModificar(pPerfil))
            {
                this.LimpiarControles();
                this.MostrarMensaje(pPerfil.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(pPerfil.CodigoMensaje, true);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.MiGestion = Gestion.Agregar;
            this.MiPerfil = new Perfiles();

            //Cargo el Arbol de Menues
            tvMenu.Nodes.Clear();
            TreeNode menu = null;
            Menues menuPadre = new Menues();
            menuPadre.IdMenuPadre = (int)EnumMenues.AdministrarTodos;

            this.CargarMenu(menuPadre, menu);

            this.ctrCamposValores.BorrarControlesParametros();
            this.ctrCamposValores.IniciarControl(this.MiPerfil, new Objeto(), this.MiGestion);
            this.ctrPerfilReportes.IniciarControl(this.MiPerfil);
            this.ctrPerfilProcesos.IniciarControl(this.MiPerfil);
            this.mvGestion.SetActiveView(vCargarDatos);
            AyudaProgramacion.HabilitarControles(this.vCargarDatos, true, this);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Validate();
            if (!this.IsValid)
                return;

            switch (this.MiGestion)
            {
                case Gestion.Consultar:
                    this.LimpiarControles();
                    break;
                case Gestion.Agregar:
                    this.MiPerfil.EstadoColeccion = EstadoColecciones.Agregado;
                    AyudaProgramacion.MapearControlesEntidad(vCargarDatos, this.MiPerfil);
                    this.MiPerfil.Campos = this.ctrCamposValores.ObtenerLista();
                    this.MiPerfil.Menues = new List<Menues>();
                    this.ObtenerPerfilMenu(this.MiPerfil, tvMenu.Nodes);
                    this.MiPerfil.Reportes = this.ctrPerfilReportes.ObtenerPerfilReportes();
                    this.MiPerfil.Procesos = this.ctrPerfilProcesos.ObtenerPerfilProcesos();
                    this.AgregarPerfil(this.MiPerfil);
                    this.CargarGrillaDatos();
                    break;
                case Gestion.Modificar:
                    this.MiPerfil.EstadoColeccion = EstadoColecciones.Modificado;
                    AyudaProgramacion.MapearControlesEntidad(vCargarDatos, this.MiPerfil);
                    this.MiPerfil.Campos = this.ctrCamposValores.ObtenerLista();
                    //this.MiPerfil.Menues = new List<Menues>();
                    this.ObtenerPerfilMenu(this.MiPerfil, tvMenu.Nodes);
                    this.MiPerfil.Reportes = this.ctrPerfilReportes.ObtenerPerfilReportes();
                    this.MiPerfil.Procesos = this.ctrPerfilProcesos.ObtenerPerfilProcesos();
                    this.ModificarPerfil(this.MiPerfil);
                    this.CargarGrillaDatos();
                    break;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.LimpiarControles();
        }

        protected void tvMenu_SelectedNodeChanged(object sender, EventArgs e)
        {
            //Si el nodo no esta con el checkbok seleccionado, no hago nada
            // y limpio el checkboxList de ControlesPaginas  y oculto el boton Agregar Controles
            if (!((TreeView)sender).SelectedNode.Checked)
            {
                this.chkControles.DataSource = null;
                this.chkControles.DataBind();
                this.btnAgregarControles.Visible = false;
                return;
            }
            int idMenu = Convert.ToInt32(((TreeView)sender).SelectedValue);
            
            //Busco el menu en el perfil del usuario
            Menues menu = this.MiPerfil.Menues.Find(delegate(Menues men)
            { return men.IdMenu == idMenu;});

            if (menu == null)
            {
                menu = new Menues();
                menu.IdMenu = idMenu;
                menu.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(menu, Gestion.Agregar);
                this.MiPerfil.Menues.Add(menu);
            }

            if (menu != null && menu.EstadoColeccion != EstadoColecciones.Borrado)
            {
                if(UsuarioActivo.EsAdministradorGeneral)
                    this.btnAgregarControles.Visible = true;

                IndiceMenuSeleccionado = menu.IndiceColeccion;
                this.CargarControlesPagina(menu);                
            }
        }

        private void CargarControlesPagina(Menues menu)
        {
            List<SegControlesPaginas> controles = SeguridadF.SegControlesPaginaObtenerLista(menu);
            controles.AddRange(menu.ControlesPaginas.FindAll(delegate(SegControlesPaginas ctrl)
            { return ctrl.EstadoColeccion == EstadoColecciones.Agregado; }));
            this.chkControles.DataSource = controles;
            this.chkControles.DataValueField = "IdControlesPaginas";
            this.chkControles.DataTextField = "Descripcion";
            this.chkControles.DataBind();
            this.chkControles.Visible = true;

            foreach (SegControlesPaginas ctrlPag in menu.ControlesPaginas)
            {
                if (ctrlPag.TienePermiso)
                {
                    ListItem permiso = this.chkControles.Items.FindByValue(ctrlPag.IdControlesPaginas.ToString());
                    permiso.Selected = true;
                }
            }
        }

        protected void chkControles_SelectedIndexChanged(object sender, EventArgs e)
        {
            SegControlesPaginas ctrlPagina;

            CheckBoxList controlesPagina = (CheckBoxList)sender;

            foreach (ListItem item in controlesPagina.Items)
            {
                ctrlPagina = this.MiPerfil.Menues[IndiceMenuSeleccionado].ControlesPaginas.Find(delegate(SegControlesPaginas ctrlPag)
                { return ctrlPag.IdControlesPaginas == Convert.ToInt32(item.Value); });

               if (!ctrlPagina.TienePermiso && item.Selected)
                {
                    ctrlPagina.TienePermiso = item.Selected;
                    ctrlPagina.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(ctrlPagina, Gestion.Agregar);
                }
                else if (ctrlPagina.TienePermiso && !item.Selected)
                {
                    ctrlPagina.TienePermiso = false;
                    ctrlPagina.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(ctrlPagina, Gestion.Anular);
                }
            }
        }

        protected void tvMenu_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            int idMenu = Convert.ToInt32(e.Node.Value);
            Menues menu = this.MiPerfil.Menues.Find(delegate(Menues men)
            { return men.IdMenu == idMenu; });

            if (menu == null)
            {
                Menues men = new Menues();
                men.IdMenu = idMenu;
                men.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(men, Gestion.Agregar);
            }
            else
            {
                menu.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(menu, Gestion.Anular);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisPerfiles;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPerfiles = this.OrdenarGrillaDatos<Perfiles>(this.MisPerfiles, e);
            this.gvDatos.DataSource = this.MisPerfiles;
            this.gvDatos.DataBind();
        }

        protected void btnAgregarControles_Click(object sender, EventArgs e)
        {
            Menues menu = this.MiPerfil.Menues[this.IndiceMenuSeleccionado];

            if (menu != null)
            {
                this.MenuesControlesPagina.IniciarControl(menu);
            }
        }

        protected void btnUsuarios_Click(object sender, EventArgs e)
        {
            this.SegGestionarPerfilUsuario.IniciarControl(this.MiPerfil);
        }
        
    }
}
