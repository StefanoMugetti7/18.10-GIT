using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestamos.Entidades;
using Comunes.Entidades;
using System.Text;
using Servicio.AccesoDatos;
using Prestamos;
using System.Data;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PrestamosCuotasPopUp : ControlesSeguros
    {
        public PrePrestamos MiCancelacion
        {
            get { return (PrePrestamos)Session[this.MiSessionPagina + "PrestamosCuotasPopUpMiPrePrestamos"]; }
            set { Session[this.MiSessionPagina + "PrestamosCuotasPopUpMiPrePrestamos"] = value; }
        }

        //public int MiFlag
        //{
        //    get { return (int)Session[this.MiSessionPagina + "PrestamosCuotasPopUpMiFlag"]; }
        //    set { Session[this.MiSessionPagina + "PrestamosCuotasPopUpMiFlag"] = value; }
        //}

        DataTable HabilitarControles;
        bool Habilitar;
        bool Incluir;

        public delegate void PrestamosCuotasEventHandler(PrePrestamos e);
        public event PrestamosCuotasEventHandler PrestamosCuotasSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(PrePrestamos cancelacion, Gestion pGestion, PrePrestamos prestamoNuevo)
        {
            this.MiCancelacion = cancelacion;
            SetearOpciones(prestamoNuevo);
            AyudaProgramacion.CargarGrillaListas<PrePrestamosCuotas>(this.MiCancelacion.ObtenerCuotasPendientes(false), false, this.gvDatos, true);



            string script = " $(\"[id$='modalPopUpPrestamosCuotas']\").modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalPrestamosCuotas", script, true);
        }
        private void SetearOpciones(PrePrestamos pParametro)
        {
            this.HabilitarControles=PrePrestamosF.PrestamosHabilitarControlesCancelaciones(pParametro);
            if(this.HabilitarControles.Rows.Count > 0)
            {
                this.Habilitar = (bool)this.HabilitarControles.Rows[0].ItemArray[0];
                this.Incluir = (bool)this.HabilitarControles.Rows[0].ItemArray[1];
                //this.MiFlag = 1;
            }

        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool incluir;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                incluir = ((CheckBox)fila.FindControl("chkIncluir")).Checked;
                int idPrestamoCuota = Convert.ToInt32(this.gvDatos.DataKeys[fila.DataItemIndex]["IdPrestamoCuota"].ToString());
                this.MiCancelacion.PrestamosCuotas.Find(x => x.IdPrestamoCuota == idPrestamoCuota).Incluir = incluir;
            }

            if (this.PrestamosCuotasSeleccionar != null)
                this.PrestamosCuotasSeleccionar(this.MiCancelacion);
            this.MiCancelacion = null;
            this.ScriptCancelar();
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            if (this.PrestamosCuotasSeleccionar != null)
                this.PrestamosCuotasSeleccionar(this.MiCancelacion);
            this.MiCancelacion = null;
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

            StringBuilder script = new StringBuilder();
            script.Append("$('body').removeClass('modal-open');");
            script.AppendLine("$('.modal-backdrop').remove();");
            script.AppendLine("$(\"[id$='modalPopUpPrestamosCuotas']\").modal('hide');");


            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalPrestamosCuotas", script.ToString(), true);
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //if(this.MiFlag == 0)


            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox ichkAll = (CheckBox)e.Row.FindControl("checkAll");
                ichkAll.Enabled = this.Habilitar;
                ichkAll.Checked = this.Incluir;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                ibtnConsultar.Enabled = this.Habilitar;
                ibtnConsultar.Checked = this.Incluir;
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiCancelacion.ObtenerCuotasPendientes(false);
            this.gvDatos.DataBind();
        }
    }
}