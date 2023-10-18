using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using Comunes.Entidades;
using System.Text;

namespace IU.Modulos.Reportes
{
    public partial class GestionarReportesDatosParametros : ControlesSeguros
    {
        private RepParametros MiParametro
        {
            get { return (RepParametros)Session[this.MiSessionPagina + "GestionarReportesDatosParametrosMiParametro"]; }
            set { Session[this.MiSessionPagina + "GestionarReportesDatosParametrosMiParametro"] = value; }
        }

        public delegate void GestionarReportesDatosParametrosAceptarEventHandler(object sender, Objeto e);
        public event GestionarReportesDatosParametrosAceptarEventHandler GestionarReportesDatosParametrosAceptar;
        public delegate void GestionarReportesDatosParametrosCancelarEventHandler();
        public event GestionarReportesDatosParametrosCancelarEventHandler GestionarReportesDatosParametrosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.MiParametro = new RepParametros();
                //this.CargarTiposParametros();
            }
        }

        public void IniciarControl(RepParametros pParametro, Gestion pGestion)
        {
            this.CargarTiposParametros();
            this.MiParametro = pParametro;
            this.GestionControl = pGestion;
            switch (this.GestionControl)
            {
                case Gestion.Modificar:
                    this.txtNombreParametro.Text = this.MiParametro.NombreParametro;
                    this.txtOrden.Text = this.MiParametro.Orden.ToString();
                    this.txtParamDependiente.Text = this.MiParametro.ParamDependiente;
                    this.txtParametro.Text = this.MiParametro.Parametro;
                    this.txtStoredProcedure.Text = this.MiParametro.StoredProcedure;
                    this.ddlTipoParametroIdTipoParametro.SelectedValue = this.MiParametro.TipoParametro.IdTipoParametro.ToString();
                    break;
                default:
                    break;
            }
            string script =  " $(\"[id$='modalPopUpGestionarReportesDatosParametros']\").modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalParametros", script, true);
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("GestionarReportesDatosParametros");
            if (!this.Page.IsValid)
            {

                string script3 = " $(\"[id$='modalPopUpGestionarReportesDatosParametros']\").modal('show');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalParametros", script3, true);
                return;
            }

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiParametro.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiParametro.Estado.IdEstado = (int)Estados.Activo;
                    break;
                case Gestion.Modificar:
                    this.MiParametro.EstadoColeccion = EstadoColecciones.Modificado;
                    break;
                default:
                    break;
            }
            this.MiParametro.NombreParametro = this.txtNombreParametro.Text;
            this.MiParametro.Orden = Convert.ToInt32(this.txtOrden.Text);
            this.MiParametro.ParamDependiente = this.txtParamDependiente.Text;
            this.MiParametro.Parametro = this.txtParametro.Text;
            this.MiParametro.StoredProcedure = this.txtStoredProcedure.Text;
            this.MiParametro.TipoParametro.IdTipoParametro = Convert.ToInt32(this.ddlTipoParametroIdTipoParametro.SelectedValue);
            this.MiParametro.TipoParametro.Descripcion = this.ddlTipoParametroIdTipoParametro.SelectedItem.Text;
            this.MiParametro.UsuarioLogueado= AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            if (GestionarReportesDatosParametrosAceptar != null)
                GestionarReportesDatosParametrosAceptar(sender, this.MiParametro);





            this.ScriptCancelar();

        }





        protected void btnCancelar_Click(object sender, EventArgs e)
        {
         
            if (this.GestionarReportesDatosParametrosCancelar != null)
                this.GestionarReportesDatosParametrosCancelar();

            this.ScriptCancelar();
          
        }


        private void ScriptCancelar()
        {
            //       < script type = "text/javascript" lang = "javascript" >
            //               function ShowModalPopUpParametros() 
            //            {
            //                  alert("hello world");
            //                  $("[id$='modalPopUpGestionarReportesDatosParametros']").modal('show');
            //            }

            //               function HideModalPopUpParametros()
            //            {
            //                 $('body').removeClass('modal-open');
            //                 $('.modal-backdrop').remove();
            //                 $("[id$='modalPopUpGestionarReportesDatosParametros']").modal('hide');
            //            }
            //       </ script >

            StringBuilder script = new StringBuilder() ;
            script.Append("$('body').removeClass('modal-open');");
            script.AppendLine("$('.modal-backdrop').remove();");
            script.AppendLine("$(\"[id$='modalPopUpGestionarReportesDatosParametros']\").modal('hide');");
            
   
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalParametros", script.ToString(), true);
        }

        private void CargarTiposParametros()
        {
            this.ddlTipoParametroIdTipoParametro.DataSource = ReportesF.TipoParametrosObtenerTodos();
            this.ddlTipoParametroIdTipoParametro.DataValueField = "IdTipoParametro";
            this.ddlTipoParametroIdTipoParametro.DataTextField = "Descripcion";
            this.ddlTipoParametroIdTipoParametro.DataBind();
        }
    }
}