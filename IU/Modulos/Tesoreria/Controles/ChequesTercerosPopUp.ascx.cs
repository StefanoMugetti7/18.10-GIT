using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.Entidades;
using Comunes.Entidades;
using Bancos;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Text;

namespace IU.Modulos.Tesoreria.Controles
{
    public partial class ChequesTercerosPopUp : ControlesSeguros
    {
        //public enum ModuloCheque
        //{
        //    EgresosValores,
        //    CambiarCheque,
        //}

        //public ModuloCheque MiModuloCheque
        //{
        //    get { return (ModuloCheque)Session[this.MiSessionPagina + "ChequesBuscarMiModuloCheque"]; }
        //    set { Session[this.MiSessionPagina + "ChequesBuscarMiModuloCheque"] = value; }
        //}

        private List<TESCheques> MisCheques
        {
            get { return (List<TESCheques>)Session[this.MiSessionPagina + "ChequesBuscarPopUpMisCheques"]; }
            set { Session[this.MiSessionPagina + "ChequesBuscarPopUpMisCheques"] = value; }
        }

        private List<TESCheques> MisChequesFiltrar
        {
            get { return (List<TESCheques>)Session[this.MiSessionPagina + "ChequesBuscarPopUpMisChequesFiltrar"]; }
            set { Session[this.MiSessionPagina + "ChequesBuscarPopUpMisChequesFiltrar"] = value; }
        }
        //private List<CMPFamilias> MisFamilias
        //{
        //    get { return (List<CMPFamilias>)Session[this.MiSessionPagina + "ProductosBuscarPopUpMisFamilias"]; }
        //    set { Session[this.MiSessionPagina + "ProductosBuscarPopUpMisFamilias"] = value; }
        //}

        public delegate void ChequesBuscarEventHandler(TESCheques e);
        public event ChequesBuscarEventHandler ChequesBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoCheque, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroCheque, this.btnBuscar);
            }
        }

        public void IniciarControl()
        {
            this.CargarCombos();
            //this.pnlBuscar.Visible = true;
            //this.txtCodigoCheque.Text = string.Empty;
            this.txtNumeroCheque.Text = string.Empty;
            
            this.txtFechaDiferidoDesde.Text =  string.Empty;
            this.txtFechaDiferidoHasta.Text =string.Empty;
            this.txtTitularCheque.Text = string.Empty;
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalChequesTerceros", "ShowModalPopUpCheques();", true);
        }

        public void IniciarControl(TESCheques pCheque)
        {
            //this.pnlBuscar.Visible = false;
            TESCheques filtro = new TESCheques();
            filtro.IdCheque = pCheque.IdCheque;
            filtro.NumeroCheque = pCheque.NumeroCheque;
            this.MisCheques = BancosF.ChequesObtenerListaFiltro(filtro);
            AyudaProgramacion.CargarGrillaListas<TESCheques>(this.MisCheques, false, this.gvCheques, false);
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalChequesTerceros", "ShowModalPopUpCheques();", true);
        }

        //public void IniciarControl(List<TESCheques> listaFiltro)
        //{
        //    IniciarControl(listaFiltro, ModuloCheque.EgresosValores);
        //}

        public void IniciarControl(List<TESCheques> listaFiltro)
        {
            //MiModuloCheque = enumCheque;
            this.MisChequesFiltrar = listaFiltro;
            this.FiltrarCheques();
            this.CargarCombos();
            //this.pnlBuscar.Visible = true;
            
            this.txtNumeroCheque.Text = string.Empty;
            
            this.txtFechaDiferidoDesde.Text = string.Empty;
            this.txtFechaDiferidoHasta.Text = string.Empty;
            this.txtTitularCheque.Text = string.Empty;
           
            string script = " $(\"[id$='modalPopUpChequesTerceros']\").modal('show');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalChequesTerceros", script, true);
            this.CargarLista();
            //AyudaProgramacion.CargarGrillaListas<TESCheques>(this.MisCheques, false, this.gvCheques, false);
            //mpePopUp.Show();

        }

        private void CargarCombos()
        {
            
            this.ddlBancos.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Bancos);
            this.ddlBancos.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlBancos.DataTextField = "Descripcion";
            this.ddlBancos.DataBind();
            if (this.ddlBancos.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

           
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalPopUpChequesTerceros();", true);
        }

        protected void gvCheques_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCheques cheque = this.MisCheques[indiceColeccion];
            
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                    if (this.ChequesBuscarSeleccionar != null)
                    {
                        this.ChequesBuscarSeleccionar(cheque);
                    //mpePopUp.Hide();
                    
                    StringBuilder script = new StringBuilder();
                    script.Append("$('body').removeClass('modal-open');");
                    script.AppendLine("$('.modal-backdrop').remove();");
                    script.AppendLine("$(\"[id$='modalPopUpChequesTercerohuieks']\").modal('hide');");
                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalChequesTerceros", script.ToString(), true);
                }
            }
        }

        protected void gvCheques_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");

            //    //Permisos btnEliminar
            //    ibtnConsultar.Visible = this.ValidarPermiso("ProductosConsultar.aspx");
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCheques.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvCheques_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
            gvCheques.PageIndex = e.NewPageIndex;
            gvCheques.DataSource = this.MisCheques;
            gvCheques.DataBind();
            //mpePopUp.Show();
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalChequesTerceros", "ShowModalPopUpCheques();", true);
        }

        protected void gvCheques_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCheques = this.OrdenarGrillaDatos<TESCheques>(this.MisCheques, e);
            this.gvCheques.DataSource = this.MisCheques;
            this.gvCheques.DataBind();
            //mpePopUp.Show();
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalChequesTerceros", "ShowModalPopUpCheques();", true);
        }

        private void CargarLista()
        {
            
            TESCheques pCheque = new TESCheques();
            //pCheque.IdCheque = this.txtCodigoCheque.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoCheque.Text);
            pCheque.NumeroCheque = this.txtNumeroCheque.Text;
            pCheque.Banco.IdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
            //Fechas
            //pCheque.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            //pCheque.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pCheque.FechaDiferidoDesde = this.txtFechaDiferidoDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDiferidoDesde.Text);
            pCheque.FechaDiferidoHasta = this.txtFechaDiferidoHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDiferidoHasta.Text);
            pCheque.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            pCheque.ChequePropio = false;
            //SOLO ESO
            pCheque.Estado.IdEstado = (int)EstadosCheques.EnTesoreria;
            pCheque.TitularCheque = this.txtTitularCheque.Text;
            //if (MiModuloCheque == ModuloCheque.CambiarCheque)
            //{
            //    pCheque.CargarRechazado = true;
            //}
            this.MisCheques = BancosF.ChequesObtenerDisponibles(pCheque);
            this.FiltrarCheques();            
            this.gvCheques.DataSource = this.MisCheques;
            this.gvCheques.DataBind();
            AyudaProgramacion.FixGridView(gvCheques);
        }

        private void FiltrarCheques()
        {
            if (this.MisChequesFiltrar.Count() != 0)
            {
                foreach (TESCheques filtro in this.MisChequesFiltrar)
                {
                    this.MisCheques.Remove(this.MisCheques.Find(x => x.IdCheque == filtro.IdCheque));
                }
                AyudaProgramacion.AcomodarIndices<TESCheques>(this.MisCheques);
            }
        }
    }
}
