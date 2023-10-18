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
using Seguridad.Entidades;
using Seguridad.FachadaNegocio;
using Comunes.Entidades;
using System.Collections.Generic;
using AjaxControlToolkit;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Servicio.Encriptacion;
using System.Net.Mail;
using System.IO;
using Auditoria;

namespace IU.Modulos.Seguridad
{
    public partial class SegGestionarUsuariosDatos : ControlesSeguros
    {
        public Usuarios MiUsuario
        {
            get { return (Usuarios)Session[this.MiSessionPagina + "SegGestionarUsuariosDatosMiUsuario"]; }
            set { this.Session[this.MiSessionPagina + "SegGestionarUsuariosDatosMiUsuario"] = value; }
        }

        //public List<CentroCostos> MisCentroCostosFiliales
        //{
        //    get { return (List<CentroCostos>)Session[this.MiSessionPagina + "SegGestionarUsuariosDatosMisCentroCostosFiliales"]; }
        //    set { this.Session[this.MiSessionPagina + "SegGestionarUsuariosDatosMisCentroCostosFiliales"] = value; }
        //}

        //public delegate void SegGestionarUsuariosDatosAceptarEventHandler(object sender, Usuarios e);
        //public event SegGestionarUsuariosDatosAceptarEventHandler UsuariosDatosAceptar;
        public delegate void SegGestionarUsuariosDatosAceptarEventHandlerCancelarEventHandler();
        public event SegGestionarUsuariosDatosAceptarEventHandlerCancelarEventHandler UsuariosDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrUsuariosSectores.UsuariosSectoresAgregar += new IU.Modulos.Seguridad.Controles.SegUsuariosSectoresDatos.SegUsuariosSectoresAgregarEventHandler(ctrUsuariosSectores_UsuariosSectoresAgregar);
            this.lblValidarFilial.Visible = false;
            this.lblValidarSectores.Visible = false;
        }

        void ctrUsuariosSectores_UsuariosSectoresAgregar(object sender, TGESectores e)
        {
            if (Convert.ToInt32(this.ddlIdFilial.SelectedValue) == e.Filial.IdFilial)
            {
                TGEFiliales filial = this.MiUsuario.Filiales.Find(x => x.IdFilial == e.Filial.IdFilial);
                this.CargarSectores(filial);
                this.upSectores.Update();
            }
        }

        public void IniciarControl(Usuarios pUsuario, Gestion pGestion)
        {
            this.ddlIdSector.Items.Clear();
            this.CargarFiliales();
            this.CargarPerfiles();
            //this.CargarFuncionalidades();
            this.MiUsuario = pUsuario;
            this.GestionControl = pGestion;
            this.tcUsuarios.ActiveTab = this.tpPerfiles;
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    this.pnlContrasenia.Visible = true;
                    //TGEFiliales fil = new TGEFiliales();
                    //fil.IdFilial = Convert.ToInt32(this.ddlIdFilial.SelectedValue);
                    //this.CargarSectores(fil);
                    this.ctrCamposValores.IniciarControl(this.MiUsuario, new Objeto(), this.GestionControl);
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlIdSector, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    AyudaProgramacion.HabilitarControles(this, true, this.paginaSegura);

                    this.txtUsuario.Enabled = true;
                    break;
                case Gestion.Modificar:
                    this.MiUsuario = SeguridadF.UsuariosObtenerDatosCompleto(this.MiUsuario);
                    this.ctrCamposValores.IniciarControl(this.MiUsuario, new Objeto(), this.GestionControl);
                    this.MapearEntidadAControles();

                    AyudaProgramacion.HabilitarControles(this, true, this.paginaSegura);
                    this.txtUsuario.Enabled = false;
                    this.pnlContrasenia.Visible = false;
                    this.btnEnviarMail.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MiUsuario = SeguridadF.UsuariosObtenerDatosCompleto(this.MiUsuario);
                    this.MapearEntidadAControles();
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.pnlContrasenia.Visible = false;
                    this.btnEnviarMail.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void MapearEntidadAControles()
        {
            //AyudaProgramacion.MapearEntidadControles(this.MiUsuario, this);
            this.txtUsuario.Text = this.MiUsuario.Usuario;
            this.txtNombre.Text = this.MiUsuario.Nombre;
            this.txtApellido.Text = this.MiUsuario.Apellido;
            this.txtCorreoElectronico.Text = this.MiUsuario.CorreoElectronico;
            this.chkCambiarContrasenia.Checked = this.MiUsuario.CambiarContrasenia;
            this.chkBajaLogica.Checked = this.MiUsuario.BajaLogica;

            this.CargarPerfilesUsuario(this.MiUsuario);
            this.CargarComboFiliales();

            AyudaProgramacion.AgregarItemSeleccione(this.ddlIdFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.CargarFilialesUsuario(this.MiUsuario);
            if (this.MiUsuario.Filiales.Exists(x => x.IdFilial == this.MiUsuario.FilialPredeterminada.IdFilial))
            {
                ListItem item = this.ddlIdFilial.Items.FindByValue(this.MiUsuario.FilialPredeterminada.IdFilial.ToString());
                if (item !=null && item.Value ==this.MiUsuario.FilialPredeterminada.IdFilial.ToString())
                    this.ddlIdFilial.SelectedValue = this.MiUsuario.FilialPredeterminada.IdFilial.ToString();
            }
            TGEFiliales filial = this.MiUsuario.Filiales.Find(x => x.IdFilial == this.MiUsuario.FilialPredeterminada.IdFilial);
            if (filial != null)
                this.CargarSectores(filial);
            //ListItem itemSector = this.ddlIdSector.Items.FindByValue(this.MiUsuario.SectorPredeterminado.IdSector.ToString());
            //if (itemSector != null && itemSector.Value == this.MiUsuario.SectorPredeterminado.IdSector.ToString())
            //    this.ddlIdSector.SelectedValue = this.MiUsuario.SectorPredeterminado.IdSector.ToString();
            this.ctrCamposValores.IniciarControl(this.MiUsuario, new Usuarios(), this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiUsuario, this.MiUsuario, this.GestionControl);
            //this.CargarFuncionalidadesUsuario(this.MiUsuario);
            //this.CargarFilialsUsuarioCentroCostos(this.MiUsuario);
            //this.ctrUsuariosSectores.IniciarControl(this.MiUsuario, this.GestionControl);
            this.ctrlAuditoria.IniciarControl(this.MiUsuario);
        }

        private bool UsuariosAgregar(Usuarios pUsuario)
        {
            pUsuario.IdUsuarioEvento = this.UsuarioActivo.IdUsuario;
            return SeguridadF.UsuariosAgregar(pUsuario);
        }

        private bool UsuariosModificar(Usuarios pUsuario)
        {
            pUsuario.IdUsuarioEvento = this.UsuarioActivo.IdUsuario;
            return SeguridadF.UsuariosModificar(pUsuario);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.lblValidarFilial.Visible = false;
            this.lblValidarSectores.Visible = false;
            this.Page.Validate("Aceptar");
            if (!this.Page.IsValid)
            {
                this.upSectores.Update();
                return;
            }
            if(this.ddlIdFilial.SelectedValue==string.Empty)
            {
                this.lblValidarFilial.Visible=true;
                this.upSectores.Update();
                return;
            }
            //if (this.ddlIdSector.SelectedValue == string.Empty)
            //{
            //    this.lblValidarSectores.Visible = true;
            //    this.upSectores.Update();
            //    return;
            //}

            bool resultado = false;
            this.MiUsuario.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Consultar:
                    AyudaProgramacion.LimpiarControles(this, true);
                    resultado = true;
                    break;
                case Gestion.Agregar:
                    this.MiUsuario.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MapearControlesObjeto(this.MiUsuario);
                    resultado = this.UsuariosAgregar(this.MiUsuario);
                    //this.CargarGrillaDatos();
                    break;
                case Gestion.Modificar:
                    this.MiUsuario.EstadoColeccion = EstadoColecciones.Modificado;
                    string contrasenia = this.MiUsuario.Contrasenia;
                    this.MapearControlesObjeto(this.MiUsuario);
                    this.MiUsuario.Contrasenia = contrasenia;
                    resultado = this.UsuariosModificar(this.MiUsuario);
                    //this.CargarGrillaDatos();
                    break;
            }
            //if (resultado && this.UsuariosDatosAceptar != null)
            //    this.UsuariosDatosAceptar(this, this.MiUsuario);
            if (resultado)
            {
                this.btnAceptar.Visible = false;
                this.btnEnviarMail.Visible = true;
                this.MostrarMensaje(this.MiUsuario.CodigoMensaje, false);
            }
            else
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiUsuario.CodigoMensaje, false, this.MiUsuario.CodigoMensajeArgs);
            }

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            AyudaProgramacion.LimpiarControles(this, true);
            if (this.UsuariosDatosCancelar != null)
                this.UsuariosDatosCancelar();
        }

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            Usuarios usuario = new Usuarios();
            usuario.CorreoElectronico = this.txtCorreoElectronico.Text;
            if (SeguridadF.UsuariosValidarCorreoElectronico(usuario))
            {
                usuario = SeguridadF.UsuariosObtenerPorCorreoElectronico(usuario);
                string url = string.Concat(this.Page.Request.Url.Scheme, "://", this.Page.Request.Url.Host, this.ObtenerAppPath(), "RecuperarContraseniaPaso2.aspx?parametros=");
                string parametros = string.Concat(this.txtCorreoElectronico.Text, "|", DateTime.Now.Year.ToString().PadLeft(2, '0'), DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0'));
                string link = string.Concat(url, Encriptar.EncriptarTexto(parametros));

                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(usuario.CorreoElectronico));
                mail.Subject = "Evol SRL - Bienvenido";

                mail.IsBodyHtml = true;
                string template = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Templates\\MailEnviarAltaUsuario.htm");
                mail.Body = new StreamReader(template).ReadToEnd();
                mail.Body = mail.Body.Replace("%ApellidoNombre%", usuario.ApellidoNombre);
                mail.Body = mail.Body.Replace("%Usuario%", usuario.Usuario);
                mail.Body = mail.Body.Replace("%Link%", link);
                mail.Body = mail.Body.Replace("%Empresa%", TGEGeneralesF.EmpresasSeleccionar().Empresa);

                bool resultado = AuditoriaF.MailsEnviosAgregar(mail, usuario, 13);

                if (resultado)
                {
                    this.MostrarMensaje("Se ha enviado un correo electronico a la direccion ingresada.", false);
                }
                else
                {
                    this.MostrarMensaje(usuario.CodigoMensaje, true, usuario.CodigoMensajeArgs);
                }
            }
        }

        private void CargarPerfiles()
        {
            Objeto usuario = new Objeto();
            usuario.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.chkPerfiles.DataSource = SeguridadF.PerfilesObtenerTodos(true, usuario);
            this.chkPerfiles.DataValueField = "IdPerfil";
            this.chkPerfiles.DataTextField = "Perfil";
            this.chkPerfiles.DataBind();
        }

        private void CargarFiliales()
        {
            List<TGEFiliales> bodegas = TGEGeneralesF.FilialesObenerLista();
            this.chkFiliales.DataSource = bodegas;
            this.chkFiliales.DataValueField = "IdFilial";
            this.chkFiliales.DataTextField = "Filial";
            this.chkFiliales.DataBind();

            AyudaProgramacion.AgregarItemSeleccione(this.ddlIdFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarSectores(TGEFiliales pFilial)
        {
            //TGESectores filtro = new TGESectores();
            //filtro.Estado.IdEstado = (int)Estados.Activo;
            //filtro.Filial.IdFilial = pFilial.IdFilial;
            //List<TGESectores> sectores = TGEGeneralesF.SectoresObtenerListaFiltro(filtro);

            this.ddlIdSector.Items.Clear();
            this.ddlIdSector.SelectedValue = null;
            this.ddlIdSector.DataSource = pFilial.Sectores; // sectores;
            this.ddlIdSector.DataValueField = "IdSector";
            this.ddlIdSector.DataTextField = "Sector";
            this.ddlIdSector.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlIdSector, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (pFilial.Sectores.Exists(x => x.IdSector == this.MiUsuario.SectorPredeterminado.IdSector))
                this.ddlIdSector.SelectedValue = this.MiUsuario.SectorPredeterminado.IdSector.ToString();
        }

        private void CargarPerfilesUsuario(Usuarios pUsuario)
        {
            foreach (Perfiles per in pUsuario.Perfiles)
            {
                foreach (ListItem item in chkPerfiles.Items)
                {
                    if (Convert.ToInt32(item.Value) == per.IdPerfil)
                        item.Selected = true;
                }
            }
        }

        private void CargarFilialesUsuario(Usuarios pUsuario)
        {
            foreach (TGEFiliales filial in pUsuario.Filiales)
            {
                foreach (ListItem item in chkFiliales.Items)
                {
                    if (Convert.ToInt32(item.Value) == filial.IdFilial)
                        item.Selected = true;
                }
            }
        }

        private void MapearControlesObjeto(Usuarios pUsuario)
        {
            //AyudaProgramacion.MapearControlesEntidad(this, pUsuario);
            pUsuario.Usuario = this.txtUsuario.Text;
            pUsuario.Nombre = this.txtNombre.Text;
            pUsuario.Apellido = this.txtApellido.Text;
            pUsuario.Contrasenia = this.txtContrasenia.Text;
            pUsuario.CorreoElectronico = this.txtCorreoElectronico.Text;
            pUsuario.CambiarContrasenia = this.chkCambiarContrasenia.Checked;
            pUsuario.BajaLogica = this.chkBajaLogica.Checked;
            pUsuario.FilialPredeterminada.IdFilial = Convert.ToInt32(this.ddlIdFilial.SelectedValue);
            pUsuario.FilialPredeterminada.Filial = this.ddlIdFilial.SelectedItem.Text;
            pUsuario.SectorPredeterminado.IdSector = this.ddlIdSector.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlIdSector.SelectedValue);
            pUsuario.SectorPredeterminado.Sector = this.ddlIdSector.SelectedItem.Text;
            this.ObtenerPerfiles(pUsuario);
            //this.ObtenerFuncionalidades(pUsuario);
            //this.ObtenerFiliales(pUsuario);
            pUsuario.Campos = this.ctrCamposValores.ObtenerLista();
            pUsuario.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();

        }

        private void ObtenerPerfiles(Usuarios pUsuario)
        {
            Perfiles perfil;
            //pUsuario.Perfiles = new List<Perfiles>();

            foreach (ListItem lst in this.chkPerfiles.Items)
            {
                perfil = pUsuario.Perfiles.Find(delegate(Perfiles per)
                { return per.IdPerfil == Convert.ToInt32(lst.Value); });

                if (perfil == null && lst.Selected)
                {
                    perfil = new Perfiles();
                    perfil.IdPerfil = Convert.ToInt32(lst.Value);
                    pUsuario.Perfiles.Add(perfil);
                    perfil.EstadoColeccion = EstadoColecciones.Agregado;
                }
                else if (perfil != null && !lst.Selected)
                    perfil.EstadoColeccion = EstadoColecciones.Borrado;

            }
        }

        //private void ObtenerFuncionalidades(Usuarios pUsuario)
        //{
        //    SegFuncionalidades funcion;

        //    foreach (ListItem lst in this.chkFuncionalidades.Items)
        //    {
        //        funcion = pUsuario.Funcionalidades.Find(delegate(SegFuncionalidades per)
        //        { return per.IdFuncionalidad == Convert.ToInt32(lst.Value); });

        //        if (funcion == null && lst.Selected)
        //        {
        //            funcion = new SegFuncionalidades();
        //            funcion.IdFuncionalidad = Convert.ToInt32(lst.Value);
        //            pUsuario.Funcionalidades.Add(funcion);
        //            funcion.EstadoColeccion = EstadoColecciones.Agregado;
        //        }
        //        else if (funcion != null && !lst.Selected)
        //            funcion.EstadoColeccion = EstadoColecciones.Borrado;

        //    }
        //}

        //private void ObtenerFiliales(Usuarios pUsuario)
        //{
        //    TGEFiliales filial;

        //    foreach (ListItem lst in this.chkFiliales.Items)
        //    {
        //        filial = pUsuario.Filiales.Find(delegate(TGEFiliales fil)
        //        { return fil.IdFilial == Convert.ToInt32(lst.Value); });

        //        if (filial == null && lst.Selected)
        //        {
        //            filial = new TGEFiliales();
        //            filial.IdFilial = Convert.ToInt32(lst.Value);
        //            filial.Filial = lst.Text;
        //            pUsuario.Filiales.Add(filial);
        //            filial.EstadoColeccion = EstadoColecciones.Agregado;
        //            filial.IndiceColeccion = pUsuario.Filiales.IndexOf(filial);
        //        }
        //        else if (filial != null && !lst.Selected)
        //            filial.EstadoColeccion = EstadoColecciones.Borrado;

        //    }
        //}

        protected void tcUsuarios_ActiveTabChanged(object sender, EventArgs e)
        {
            if (((TabContainer)sender).ActiveTab == this.tpSectores)
            {
                //this.ObtenerFiliales(this.MiUsuario);
                this.ctrUsuariosSectores.IniciarControl(this.GestionControl);
            }
        }

        protected void ddlIdFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlIdFilial.SelectedValue))
                return;

            TGEFiliales filial = this.MiUsuario.Filiales.Find(x => x.IdFilial == Convert.ToInt32(this.ddlIdFilial.SelectedValue));
            if (filial != null)
                this.CargarSectores(filial);
        }

        protected void chkFiliales_SelectedIndexChanged(object sender, EventArgs e)
        {
            TGEFiliales cc;
            foreach (ListItem lst in this.chkFiliales.Items)
            {
                if (!this.MiUsuario.Filiales.Exists(x => x.IdFilial == Convert.ToInt32(lst.Value))
                    && lst.Selected)
                {
                    cc = new TGEFiliales();
                    cc.IdFilial = Convert.ToInt32(lst.Value);
                    cc.Filial = lst.Text;
                    cc.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(cc, Gestion.Agregar);
                    this.MiUsuario.Filiales.Add(cc);
                    cc.IndiceColeccion = this.MiUsuario.Filiales.IndexOf(cc);
                }
                else if (this.MiUsuario.Filiales.Exists(x => x.IdFilial== Convert.ToInt32(lst.Value)))
                {
                    cc = this.MiUsuario.Filiales.Single(x => x.IdFilial == Convert.ToInt32(lst.Value));
                    if (!lst.Selected && cc.EstadoColeccion == EstadoColecciones.SinCambio)
                        cc.EstadoColeccion = EstadoColecciones.Borrado;
                    else if (!lst.Selected && cc.EstadoColeccion == EstadoColecciones.Agregado)
                    {
                        this.MiUsuario.Filiales.Remove(cc);
                        this.MiUsuario.Filiales = AyudaProgramacion.AcomodarIndices<TGEFiliales>(this.MiUsuario.Filiales);
                    }
                    else if (lst.Selected && cc.EstadoColeccion == EstadoColecciones.Borrado)
                        cc.EstadoColeccion = EstadoColecciones.SinCambio;
                }
            }
            this.CargarComboFiliales();
        }

        private void CargarComboFiliales()
        {
            ListItem itemAnterior = this.ddlIdFilial.SelectedItem;
            this.ddlIdFilial.Items.Clear();
            this.ddlIdFilial.SelectedValue = null;
            this.ddlIdFilial.DataSource = this.MiUsuario.Filiales.Where(x=>x.EstadoColeccion==EstadoColecciones.Agregado
                || x.EstadoColeccion==EstadoColecciones.Modificado
                || x.EstadoColeccion==EstadoColecciones.SinCambio).ToList();
            this.ddlIdFilial.DataValueField = "IdFilial";
            this.ddlIdFilial.DataTextField = "Filial";
            this.ddlIdFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlIdFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            ListItem itemExiste = this.ddlIdFilial.Items.FindByValue(itemAnterior.Value);
            if (itemExiste != null)
                this.ddlIdFilial.SelectedValue = itemAnterior.Value;
        }
    }
}