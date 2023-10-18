using Comunes.Entidades;
using ProcesosDatos;
using ProcesosDatos.Entidades;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace IU.Modulos.ProcesosDatos.Controles
{
    public partial class ProcesosDatosModificarDatosDetalles : ControlesSeguros
    {
        protected SisProcesosProcesamiento MiProcesoProcesamiento
        {
            get { return (SisProcesosProcesamiento)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiProcesoProcesamientoLotes"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiProcesoProcesamientoLotes"] = value; }
        }
        protected DataTable MisDetalles
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetallesDT"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetallesDT"] = value; }
        }
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
            }
        }
        public void IniciarControl(SisProcesosProcesamiento pParametro, Gestion pGestion)
        {
            pParametro.Proceso = ProcesosDatosF.ProcesosObtenerDatosCompletosPorProcesamiento(pParametro);
            this.MiProcesoProcesamiento = pParametro;
            this.GestionControl = pGestion;
            this.MapearObjetoAControles(this.MiProcesoProcesamiento);
            switch (pGestion)
            {
                case Gestion.Consultar:
                    this.CargarLista(this.MiProcesoProcesamiento);
                    break;
                default:
                    break;
            }
        }
        private void MapearObjetoAControles(SisProcesosProcesamiento pParametro)
        {
            this.ddlFiltro.Items.Add(new ListItem(pParametro.Proceso.Descripcion, pParametro.Proceso.IdProceso.ToString()));
            this.txtCodigo.Text = pParametro.IdProcesoProcesamiento.ToString();
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }

        #region GVDatosLotes
        protected void gvDatosLote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDetalles.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        private void CargarLista(SisProcesosProcesamiento pParametro)
        {
            this.MisDetalles = ProcesosDatosF.ProcesosObtenerListaParametros(pParametro);
            this.gvDatosLote.Visible = true;
            this.gvDatosLote.DataSource = this.MisDetalles;
            this.gvDatosLote.DataBind();
            this.upDatos.Update();
        }
        #endregion
    }
}