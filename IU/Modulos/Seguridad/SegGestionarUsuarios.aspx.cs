using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Seguridad.Entidades;
using Seguridad.FachadaNegocio;
using System.Collections;

namespace IU
{
    public partial class SegGestionarUsuarios : PaginaSegura
    {
        public List<Usuarios> MisUsuarios
        {
            get { return (List<Usuarios>)Session[this.MiSessionPagina + "UsuariosMisUsuarios"]; }
            set { Session[this.MiSessionPagina + "UsuariosMisUsuarios"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                //Limpio las varialbes de sesion
                this.MisUsuarios = new List<Usuarios>();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtBuscar, this.btnBuscar);
                Usuarios parametros = this.BusquedaParametrosObtenerValor<Usuarios>();
                this.CargarGrillaDatos(parametros);
            }
        }

        //void ctrUsuarioDatos_UsuariosDatosCancelar()
        //{
        //    this.btnAgregar.Visible = true;
        //    //this.mvGestion.SetActiveView(this.vDatos);
        //}

        //void ctrUsuarioDatos_UsuariosDatosAceptar(object sender, Usuarios e)
        //{
        //    this.CargarGrillaDatos();
        //    this.btnAgregar.Visible = true;
        //   // this.mvGestion.SetActiveView(this.vDatos);
        //}

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString() 
                || e.CommandName == Gestion.Modificar.ToString() 
                || e.CommandName == "CambiarContrasenia"))
                return;

            this.btnAgregar.Visible = this.ValidarPermiso("SegGestionarUsuariosAgregar.aspx");
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            Usuarios usuario = this.MisUsuarios[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdUsuario", usuario.IdUsuario);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                usuario.EstadoColeccion = EstadoColecciones.Borrado;
                usuario.IdUsuarioEvento = this.UsuarioActivo.IdUsuario;

                if (SeguridadF.UsuariosModificar(usuario))
                {
                    this.MostrarMensaje(usuario.CodigoMensaje, false);
                }
                else
                {
                    this.MostrarMensaje(usuario.CodigoMensaje, true);
                }
                this.CargarGrillaDatos(usuario);
            }
            else if (e.CommandName == "CambiarContrasenia")
            {
                usuario = SeguridadF.UsuariosObtenerDatosCompleto(usuario);
                this.UsuCambiarContrasenia.IniciarControl(usuario);
            }
            else
            {
                if (e.CommandName == Gestion.Consultar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegGestionarUsuariosConsultar.aspx"), true);
                }
                else if (e.CommandName == Gestion.Modificar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegGestionarUsuariosModificar.aspx"), true);
                }
                //this.mvGestion.SetActiveView(vCargarDatos);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Usuarios parametros = this.BusquedaParametrosObtenerValor<Usuarios>();
            this.CargarGrillaDatos(parametros);
        }

        private void CargarGrillaDatos(Usuarios pParametros)
        {
            pParametros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<Usuarios>(pParametros);
            this.MisUsuarios = SeguridadF.UsuariosObtenerFiltro(false, this.txtBuscar.Text);
            this.gvDatos.DataSource = this.MisUsuarios;
            this.gvDatos.DataBind();
        }
        
        //private Usuarios ObtenerUno(int pId)
        //{
        //    return this.MisUsuarios.Find(delegate(Usuarios usu) { return usu.IdUsuario == pId; });
        //}

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
           this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegGestionarUsuariosAgregar.aspx"), true);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnConsultar.Visible = this.ValidarPermiso("SegGestionarUsuariosConsultar.aspx");
                modificar.Visible = this.ValidarPermiso("SegGestionarUsuariosModificar.aspx");
                
                if (Convert.ToBoolean(e.Row.Cells[3].Text))
                    e.Row.Cells[3].Text = "Baja";
                else
                    e.Row.Cells[3].Text = "Activo";
            }
           
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Usuarios parametros = this.BusquedaParametrosObtenerValor<Usuarios>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<Usuarios>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisUsuarios;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisUsuarios = this.OrdenarGrillaDatos<Usuarios>(this.MisUsuarios, e);
            this.gvDatos.DataSource = this.MisUsuarios;
            this.gvDatos.DataBind();
        }
    }
}
