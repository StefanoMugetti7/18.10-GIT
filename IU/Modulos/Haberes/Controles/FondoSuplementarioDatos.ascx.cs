using Afiliados.Entidades;
using Cargos.Entidades;
using Comunes.Entidades;
using Evol.Controls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Haberes;
using Haberes.Entidades;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Haberes.Controles
{
    public partial class FondoSuplementarioDatos : ControlesSeguros
    {
        private HabFondoSuplementario MiFondoSuplementario
        {
            get { return (HabFondoSuplementario)Session[MiSessionPagina + "MiFondoSuplementarioDatosMiFondoSuplementario"]; }
            set { Session[MiSessionPagina + "MiFondoSuplementarioDatosMiFondoSuplementario"] = value; }
        }

        private AfiAfiliados MiAfiliado
        {
            get { return (AfiAfiliados)Session[MiSessionPagina + "MiFondoSuplementarioDatosMiAfiliado"]; }
            set { Session[MiSessionPagina + "MiFondoSuplementarioDatosMiAfiliado"] = value; }
        }

        private DataTable Datos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ImportarArchivoDatos"]; }
            set { Session[this.MiSessionPagina + "ImportarArchivoDatos"] = value; }
        }

        private bool mostrarValidarArchivo
        {
            get { return (bool)Session[this.MiSessionPagina + "FondoSumeplementarioDatosMostrarValidarArchivo"]; }
            set { Session[this.MiSessionPagina + "FondoSumeplementarioDatosMostrarValidarArchivo"] = value; }
        }

        private string mensajeValidarArchivo
        {
            get { return (string)Session[this.MiSessionPagina + "FondoSumeplementarioDatosmensajeValidarArchivo"]; }
            set { Session[this.MiSessionPagina + "FondoSumeplementarioDatosmensajeValidarArchivo"] = value; }
        }

        private string totalMeses
        {
            get { return (string)Session[this.MiSessionPagina + "FondoSumeplementarioDatostotalMeses"]; }
            set { Session[this.MiSessionPagina + "FondoSumeplementarioDatostotalMeses"] = value; }
        }

        private bool noactualizarGrilla
        {
            get { return (bool)Session[this.MiSessionPagina + "noactualizarGrilla"]; }
            set { Session[this.MiSessionPagina + "noactualizarGrilla"] = value; }
        }

        private List<CarCuentasCorrientes> MisCargosPendientes
        {
            get { return (List<CarCuentasCorrientes>)Session[this.MiSessionPagina + "FondoSumplentarioDatosMisCargosPendientes"]; }
            set { Session[this.MiSessionPagina + "FondoSumplentarioDatosMisCargosPendientes"] = value; }
        }

        public delegate void FondoSuplementarioDatosAceptarEventHandler(HabFondoSuplementario e);
        public event FondoSuplementarioDatosAceptarEventHandler FondoSuplementarioDatosAceptar;
        public delegate void FondoSuplementarioDatosCancelarEventHandler();
        public event FondoSuplementarioDatosCancelarEventHandler FondoSuplementarioDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!IsPostBack)
            {            
            }
        }

        public void IniciarControl(HabFondoSuplementario pParametro, Gestion pGestion)
        {
            MiFondoSuplementario = HaberesF.FondoSuplementarioObtenerDatosCompletos(pParametro);
            hdfValidarCalcular.Value = "1";
            //MiFondoSuplementario.Afiliado.IdAfiliado = pParametro.Afiliado.IdAfiliado;
            MiAfiliado = pParametro.Afiliado;
            MiFondoSuplementario.Afiliado = MiAfiliado; 
            GestionControl = pGestion;
            btnAceptar.Visible = true;
            if(MiFondoSuplementario.TieneRecibo)
            this.txtPeriodoInicioJubilatorio.Enabled = false;

            MapearObjetoAControles();

            switch (GestionControl)
            {
                case Gestion.Agregar:
                    this.tpLiquidacionTotal.Visible = false;
                    this.AgregarItem();
                    break;
                case Gestion.Modificar:
                    this.MapearObjetoAGrilla();
                    break;
                case Gestion.Consultar:
                    this.tpLiquidacionTotal.Visible = true;
                    this.txtPeriodoInicioJubilatorio.Enabled = false;
                    this.txtFechaJubilacion.Enabled = false;
                    this.btnCalcularAporte.Visible = false;
                    this.txtCantidadAgregar.Visible = false;
                    this.lblCantidadAgregar.Visible = false;
                    this.btnAgregarItem.Visible = false;
                    this.tpArchivo.Visible = false;
                    this.btnAceptar.Visible = false;
                    if (this.MiFondoSuplementario.SueldosHaberes.Count > 0)
                    {
                        this.MapearObjetoAGrilla();
                        this.btnCalcularAporte_Click(null, new EventArgs());
                        this.btnModificar.Visible = true;
                    }
                    else
                    {
                        this.btnAgregar.Visible = true;
                    }
                    break;
                case Gestion.Listar:
                    this.tpLiquidacionTotal.Visible = true;
                    this.txtPeriodoInicioJubilatorio.Enabled = false;
                    this.txtFechaJubilacion.Enabled = false;
                    this.btnCalcularAporte.Visible = false;
                    this.txtCantidadAgregar.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.tpArchivo.Visible = false;
                    this.lblCantidadAgregar.Visible = false;
                    this.txtCantidadAgregar.Visible = false;
                    this.btnAceptar.Visible = false;
                    if (this.MiFondoSuplementario.SueldosHaberes.Count > 0)
                    {
                        this.MapearObjetoAGrilla();
                        btnCalcularAporte_Click(null, new EventArgs());
                        this.btnModificar.Visible = true;
                    }
                    else
                    {
                        this.btnAgregar.Visible = true;
                    }
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles()
        {
            if (this.MiFondoSuplementario.FechaJubilacion.HasValue)
                this.txtFechaJubilacion.Text = MiFondoSuplementario.FechaJubilacion.Value.ToShortDateString();
            else
                this.txtFechaJubilacion.Text = string.Empty;

            if (this.MiFondoSuplementario.PeriodoInicio > 0)
                this.txtPeriodoInicioJubilatorio.Text = MiFondoSuplementario.PeriodoInicio.ToString();

        }

        private void MapearControlesAObjeto()
        {
            if (DateTime.TryParse(txtFechaJubilacion.Text, out DateTime fecha))
                MiFondoSuplementario.FechaJubilacion = DateTime.Parse(fecha.ToString("d"));
            if (!string.IsNullOrWhiteSpace(txtPeriodoInicioJubilatorio.Text))
                MiFondoSuplementario.PeriodoInicio = Convert.ToInt32(txtPeriodoInicioJubilatorio.Text);

        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/FondoSuplementarioAgregar.aspx"), true);
        }
        #region "Liquidación Inicial"
        protected void btnCalcularAporte_Click(object sender, EventArgs e)
        {
            this.CalcularMeses();
            this.CargarLiquidacionTotal();
        }

        protected void gvLiquidacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void gvLiquidacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void CargarLiquidacionTotal()
        {
            this.MapearControlesAObjeto();
            this.MiFondoSuplementario.Afiliado = MiAfiliado;
            this.MapearGrillaAObjeto();

            DataTable tabla = HaberesF.FondosSuplementariosCalcularImporteJubilacion(MiFondoSuplementario);

            if (tabla.Rows.Count > 0)
            {
                this.CargarAportes(tabla);

                this.hdfValidarCalcular.Value = "0";

                tcDatos.ActiveTab = tpLiquidacionTotal;
                tpLiquidacionTotal.Visible = true;
                upLiquidacionTotal.Update();
                upFondoSuplementario.Update();

                gvLiquidacion.DataSource = tabla;
                this.gvLiquidacion.DataBind();
                AyudaProgramacion.FixGridView(gvLiquidacion);
            }
            else
            {

                if (string.IsNullOrEmpty(MiFondoSuplementario.CodigoMensaje))
                    MiFondoSuplementario.CodigoMensaje = "Error al calcular.";

                if (hdfValidarCalcular.Value != "0" && MiFondoSuplementario.CodigoMensaje == "ResultadoTransaccion")
                    MiFondoSuplementario.CodigoMensaje = "Error al calcular.";

                MostrarMensaje(MiFondoSuplementario.CodigoMensaje, true);
            }
        }

        protected void CargarAportes(DataTable tabla)
        {
            try
            {
                decimal aporteInicialPorcentual = Convert.ToDecimal(tabla.Rows[0][0]);
                decimal aporteInicial = Convert.ToDecimal(tabla.Rows[0][1]);
                decimal aporteTotalPorcentual = Convert.ToDecimal(tabla.Rows[0][2]);
                decimal aporteTotal = Convert.ToDecimal(tabla.Rows[0][3]);
                decimal sueldoPromedio = Convert.ToDecimal(tabla.Rows[0][4]);
                decimal sueldoBruto = Convert.ToDecimal(tabla.Rows[0][5]);
                decimal sueldoSuplementario = Convert.ToDecimal(tabla.Rows[0][6]);
          
                if (aporteInicial > 0)
                    MiFondoSuplementario.AporteInicial = aporteInicial;
                else
                    MiFondoSuplementario.AporteInicial = 0;

                if (aporteInicialPorcentual > 0)
                    MiFondoSuplementario.AporteInicialPorcentual = aporteInicialPorcentual;
                else
                    MiFondoSuplementario.AporteInicialPorcentual = 0;

                if (aporteTotal > 0)
                    MiFondoSuplementario.AporteTotal = aporteTotal;
                else
                    MiFondoSuplementario.AporteTotal = 0;

                if (aporteTotalPorcentual > 0)
                    MiFondoSuplementario.AporteTotalPorcentual = aporteTotalPorcentual;
                else
                    MiFondoSuplementario.AporteTotalPorcentual = 0;

                if (sueldoPromedio > 0)
                    MiFondoSuplementario.SueldoPromedio = sueldoPromedio;
                else
                    MiFondoSuplementario.SueldoPromedio = 0;

                if (sueldoBruto > 0)
                    MiFondoSuplementario.SueldoBruto = sueldoBruto;
                else
                    MiFondoSuplementario.SueldoBruto = 0;

                if (sueldoSuplementario > 0)
                    MiFondoSuplementario.SueldoSuplementario = sueldoSuplementario;
                else
                    MiFondoSuplementario.SueldoSuplementario = 0;

            }
            catch (Exception ex)
            {
                MiFondoSuplementario.CodigoMensaje = ex.Message;
            }
        }
        #endregion

        #region "Importar Archivo"
        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            noactualizarGrilla = true;
            var excel = new ExcelPackage(new MemoryStream(this.StreamToByteArray(this.afuArchivo.FileContent)));
            this.Datos = ExcelPackageExtensions.ToDataTable(excel);//la secuencia no posee elementos
            mostrarValidarArchivo = false;

            if (this.Datos.Rows.Count == 0)
            {
                this.MostrarMensaje("ValidarArchivo", true);
                return;
            }

            if (!this.Datos.Columns.Contains("Nombre"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "Nombre" });
                return;
            }
            if (!this.Datos.Columns.Contains("Nº"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "Nº" });
                return;
            }
            if (!this.Datos.Columns.Contains("AÑO"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "AÑO" });
                return;
            }
            if (!this.Datos.Columns.Contains("MES"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "MES" });
                return;
            }
            if (!this.Datos.Columns.Contains("CAT"))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "CAT" });
                return;
            }
            if (!this.Datos.Columns.Contains("COEF."))
            {
                this.MostrarMensaje("ValidarArchivo", true, new List<string>() { "COEF." });
                return;
            }

            decimal coeficiente = 0;
            var datosGroupBy = Datos.AsEnumerable()
                .GroupBy(g => new { Col1 = g["CAT"] })
                .Select(g =>
                {

                    var row = Datos.NewRow();

                    row["CAT"] = g.First()["CAT"].ToString();
                    row["MES"] = g.Count();

                    coeficiente = 0;

                    foreach (DataRow r in g)
                    {
                        try
                        {
                            coeficiente += Convert.ToDecimal(r["COEF."].ToString());
                        }
                        catch
                        { }

                    }

                    row["COEF."] = coeficiente;

                    return row;
                }).CopyToDataTable();

            MiFondoSuplementario.SueldosHaberes.Clear();//////////////////////////////////////???????????????????????????
            HabSueldoHaberes detalle;
            List<TGEListasValoresDetalles> categorias = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CategoriasSaltoGrande);
            /* Validacion de categorias existentes en el sistema contra las del excel: */
            if (datosGroupBy.AsEnumerable().Where(x => !categorias.Any(x2 => x2.CodigoValor == x.Field<string>("CAT"))).Count() > 0)
            {
                DataTable noEncontradas = datosGroupBy.AsEnumerable().Where(x => !categorias.Any(x2 => x2.CodigoValor == x.Field<string>("CAT"))).CopyToDataTable();

                if (noEncontradas.Rows.Count > 0)
                {
                    string msg = "Las siguientes Categorias no estan definidas en el sistema:<BR/>";
                    foreach (DataRow item in noEncontradas.Rows)
                        msg = string.Concat(msg, item["CAT"].ToString(), "<BR/>");
                    mensajeValidarArchivo = msg;
                    mostrarValidarArchivo = true;
                    return;
                }
            }
            /* Fin validacion. */

            foreach (DataRow row in datosGroupBy.Rows)
            {
                detalle = new HabSueldoHaberes();
                detalle.CantidadMeses = Convert.ToInt32(row["MES"].ToString());
                var listaValoresDetalles = categorias.Find(x => x.CodigoValor == row["CAT"].ToString().Trim());
                detalle.CategoriaHaber.IdCategoriaHaber = listaValoresDetalles.IdListaValorDetalle;
                detalle.CategoriaHaber.Descripcion = listaValoresDetalles.Descripcion;
                detalle.CategoriaHaber.CodigoValor = listaValoresDetalles.CodigoValor;

                MiFondoSuplementario.SueldosHaberes.Add(detalle);
            }

            MiFondoSuplementario.SueldosHaberes = AyudaProgramacion.AcomodarIndices<HabSueldoHaberes>(MiFondoSuplementario.SueldosHaberes);
            this.afuArchivo.FailedValidation = false;
            this.afuArchivo.ClearAllFilesFromPersistedStore();
        }

        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);
            return output;
        }

        protected void button_Click(object sender, EventArgs e)
        {
            if (mostrarValidarArchivo)
            {
                MostrarMensaje(mensajeValidarArchivo, true);
                return;
            }
            AyudaProgramacion.CargarGrillaListas(MiFondoSuplementario.SueldosHaberes, false, gvDatos, true);
            afuArchivo.FailedValidation = false;
            afuArchivo.ClearAllFilesFromPersistedStore();
            tcDatos.ActiveTab = tpGrilla;
            upGrilla.Update();
            upFondoSuplementario.Update();
            noactualizarGrilla = false;
        }
        #endregion

     
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.CalcularMeses();
            bool guardo = false;
            if (this.hdfValidarCalcular.Value == "1")
            {
                MostrarMensaje("HaberesValidarCalcularFondoSuplementario", true);
                return;
            }
            MapearControlesAObjeto();
            MapearGrillaAObjeto();
            MiFondoSuplementario.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    MiFondoSuplementario.CargarLoteAportesJubilatoriosDetalles();
                    if (HaberesF.FondoSuplementarioValidacionesCalculos(MiFondoSuplementario))
                    {
                        guardo = HaberesF.FondoSuplementarioAgregar(MiFondoSuplementario);
                        if(guardo)
                        {
                            this.IniciarControl(this.MiFondoSuplementario, Gestion.Consultar);
                        }
                    }
                    else
                        MostrarMensaje(MiFondoSuplementario.CodigoMensaje, true, MiFondoSuplementario.CodigoMensajeArgs);
                    break;
                case Gestion.Modificar:
                    MiFondoSuplementario.CargarLoteAportesJubilatoriosDetalles();
                    if (HaberesF.FondoSuplementarioValidacionesCalculos(MiFondoSuplementario))
                        guardo = HaberesF.FondoSuplementarioModificar(MiFondoSuplementario);
                    else
                        MostrarMensaje(MiFondoSuplementario.CodigoMensaje, true, MiFondoSuplementario.CodigoMensajeArgs);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.btnCalcularAporte.Visible = false;
                MostrarMensaje("ResultadoTransaccion", false, MiFondoSuplementario.CodigoMensajeArgs);
                MiFondoSuplementario = HaberesF.FondoSuplementarioObtenerDatosCompletos(MiFondoSuplementario);
            }
            else
                MostrarMensaje(MiFondoSuplementario.CodigoMensaje, true, MiFondoSuplementario.CodigoMensajeArgs);
        }
        protected void btnModificar_Click (object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/FondoSuplementarioModificar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (FondoSuplementarioDatosCancelar != null)
                FondoSuplementarioDatosCancelar();
        }

        #region "Aportes Jubilatorios"
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            PersistirDatosGrilla();
            AgregarItem();
            txtCantidadAgregar.Text = string.Empty;
        }

        private void AgregarItem()
        {
            if (this.txtCantidadAgregar.Text == string.Empty || txtCantidadAgregar.Text == "0")
                txtCantidadAgregar.Text = "1";

            if (!int.TryParse(txtCantidadAgregar.Text, out int cantidad2))
                return;

            HabSueldoHaberes item;
            int cantidad = Convert.ToInt32(this.txtCantidadAgregar.Text);

            for (int i = 0; i < cantidad; i++)
            {
                item = new HabSueldoHaberes();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                item.IndiceColeccion = MiFondoSuplementario.SueldosHaberes.IndexOf(item);
                MiFondoSuplementario.SueldosHaberes.Add(item);
            }
            AyudaProgramacion.CargarGrillaListas<HabSueldoHaberes>(MiFondoSuplementario.SueldosHaberes, false, this.gvDatos, true);
        }

        private void PersistirDatosGrilla()
        {
            try
            {
                foreach (GridViewRow fila in gvDatos.Rows)
                {
                    decimal CantidadMeses = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidadMeses")).Decimal;
                    decimal Coeficiente = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCoeficiente")).Decimal;
                    DropDownList ddlCategorias = (DropDownList)fila.FindControl("ddlCategorias");

                    MiFondoSuplementario.SueldosHaberes[fila.RowIndex].CantidadMeses = Convert.ToInt32(CantidadMeses);
                    MiFondoSuplementario.SueldosHaberes[fila.RowIndex].Coeficiente = Convert.ToDecimal(Coeficiente);
                    MiFondoSuplementario.SueldosHaberes[fila.RowIndex].CategoriaHaber.IdCategoriaHaber = Convert.ToInt32(ddlCategorias.SelectedValue);
                    MiFondoSuplementario.SueldosHaberes[fila.RowIndex].CategoriaHaber.Descripcion = ddlCategorias.SelectedItem.ToString();
                }
            }
            catch (Exception ex)
            {
                string log = ex.Message;
            }
        }
        protected void btnDescargarPlantilla_Click(object sender, EventArgs e)
        {
            if (MiFondoSuplementario.SueldosHaberes.Count > 0)
            {
                HabFondoSuplementario fs = new HabFondoSuplementario();
                // fs.Afiliado.IdAfiliado = MiFondoSuplementario.Afiliado.IdAfiliado;
                fs.IdFondoSuplementario = MiFondoSuplementario.IdFondoSuplementario;

                DataTable dt = HaberesF.FondoSuplementarioObtenerPlantilla(fs);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ExportData exportar = new ExportData();
                exportar.ExportExcel(Page, ds, true, "PlantillaAportes", "PlantillaAportes");
            }
            else
                this.MostrarMensaje("Plantilla vacia", true);

        }

        private void CalcularMeses()
        {
            ScriptManager.RegisterStartupScript(this.upGrilla, this.upGrilla.GetType(), "CalcularMeses", "CalcularMeses();", true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                if (MiIndiceDetalleModificar > 0)
                {
                    HaberesF.HaberesSuplemetariosEliminarAporte(MiFondoSuplementario.SueldosHaberes[index]);
                    MiFondoSuplementario.SueldosHaberes = AyudaProgramacion.AcomodarIndices<HabSueldoHaberes>(MiFondoSuplementario.SueldosHaberes);
                }
                MiFondoSuplementario.SueldosHaberes.RemoveAt(index);
                AyudaProgramacion.CargarGrillaListas<HabSueldoHaberes>(MiFondoSuplementario.SueldosHaberes, false, gvDatos, true);

                if (MiFondoSuplementario.SueldosHaberes.Count == 0)
                    this.AgregarItem();

            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HabSueldoHaberes item = (HabSueldoHaberes)e.Row.DataItem;

                DropDownList ddlCategorias = (DropDownList)e.Row.FindControl("ddlCategorias");
                ddlCategorias.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CategoriasSaltoGrande);
                ddlCategorias.DataValueField = "IdListaValorDetalle";
                ddlCategorias.DataTextField = "Descripcion";
                ddlCategorias.DataBind();
                ddlCategorias.Attributes.Add("onchange", "ValidarCalcular();");

                if (ddlCategorias.Items.Count == 0)
                    AyudaProgramacion.AgregarItemSeleccione(ddlCategorias, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                TextBox _txtCantidadMeses = (TextBox)e.Row.FindControl("txtCantidadMeses");
                _txtCantidadMeses.Attributes.Add("onchange", "ValidarCalcular(); CalcularMeses();");



                if (item.CategoriaHaber.IdCategoriaHaber > 0)
                    ddlCategorias.SelectedValue = item.CategoriaHaber.IdCategoriaHaber.ToString();

                if (this.GestionControl == Gestion.Agregar)
                {
                    ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                    btnEliminar.Visible = this.ValidarPermiso("FondoSuplementarioAgregar.aspx");
                    CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidadMeses");
                }
                else if (GestionControl == Gestion.Consultar || GestionControl == Gestion.Listar)
                {
                    ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                    TextBox txtCantidadMeses = (TextBox)e.Row.FindControl("txtCantidadMeses");
                    TextBox txtCoeficiente = (TextBox)e.Row.FindControl("txtCoeficiente");

                    btnEliminar.Visible = false;
                    ddlCategorias.Enabled = false;
                    txtCantidadMeses.Enabled = false;
                    txtCoeficiente.Enabled = false;
                    btnAceptar.Visible = false;
                    lblCantidadAgregar.Visible = false;
                    txtCantidadAgregar.Visible = false;
                    this.gvDatos.Columns[3].Visible = false; //ELIMINO LA COLUMNA "ELIMINAR"
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                lblTotal.Text = string.Format(ObtenerMensajeSistema("GrillaTotalMeses"), MiFondoSuplementario.SueldosHaberes.Sum(x => x.CantidadMeses).ToString());//.Count);
                totalMeses = MiFondoSuplementario.SueldosHaberes.Sum(x => x.CantidadMeses).ToString();
            }
        }

        private void MapearObjetoAGrilla()
        {
            
            int cantidad = MiFondoSuplementario.SueldosHaberes.Count;

            gvDatos.DataSource = null;
            gvDatos.DataBind();

            if (cantidad == 0)
            {
                AgregarItem();
                return;
            }

            AyudaProgramacion.CargarGrillaListas<HabSueldoHaberes>(MiFondoSuplementario.SueldosHaberes, false, this.gvDatos, true);

            for (int i = 0; i < cantidad; i++)
            {
                GridViewRow fila = gvDatos.Rows[i];

                TextBox txtCantidadMeses = (TextBox)fila.FindControl("txtCantidadMeses");
                DropDownList ddlCategorias = (DropDownList)fila.FindControl("ddlCategorias");
                TextBox txtCoeficiente = (TextBox)fila.FindControl("txtCoeficiente");

                txtCantidadMeses.Text = MiFondoSuplementario.SueldosHaberes[i].CantidadMeses.ToString();
                txtCoeficiente.Text = MiFondoSuplementario.SueldosHaberes[i].Coeficiente.ToString();
                ddlCategorias.SelectedValue = MiFondoSuplementario.SueldosHaberes[i].CategoriaHaber.IdCategoriaHaber.ToString();
            }
        }

        private void MapearGrillaAObjeto()
        {
            HabSueldoHaberes sueldo;
            TextBox txtCantidadMeses;
            TextBox txtCoeficiente;
            DropDownList ddlCategorias;
            int i = 0;

            if (MiFondoSuplementario.SueldosHaberes.Count == 0)
                return;

            foreach (GridViewRow row in gvDatos.Rows)
            {
                txtCantidadMeses = (TextBox)row.FindControl("txtCantidadMeses");
                ddlCategorias = (DropDownList)row.FindControl("ddlCategorias");
                txtCoeficiente = (TextBox)row.FindControl("txtCoeficiente");

                int auxCantidadMeses = txtCantidadMeses.Text == string.Empty ? 0 : Convert.ToInt32(txtCantidadMeses.Text);
                int auxCategoria = ddlCategorias.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlCategorias.SelectedValue);
                decimal auxCoeficiente = txtCoeficiente.Text == string.Empty ? 0 : Convert.ToDecimal(txtCoeficiente.Text.Replace('.', ','));

                sueldo = MiFondoSuplementario.SueldosHaberes[i];
                sueldo.CantidadMeses = auxCantidadMeses;
                sueldo.CategoriaHaber.IdCategoriaHaber = auxCategoria;
                sueldo.Coeficiente = auxCoeficiente;
                sueldo.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(sueldo, GestionControl);
                i++;
            }
        }
        #endregion

        #region "Cargos a Incluir - Cancelaciones"
        protected void txtPeriodoInicioJubilatorio_TextChanged(object sender, EventArgs e)
        {
            CarCuentasCorrientes filtro = new CarCuentasCorrientes();
            filtro.IdAfiliado = MiFondoSuplementario.Afiliado.IdAfiliado;
            filtro.Periodo = Convert.ToInt32(txtPeriodoInicioJubilatorio.Text);
            MisCargosPendientes = new List<CarCuentasCorrientes>();
            MisCargosPendientes = HaberesF.HaberesSumplementariosObtenerCargosPendientes(filtro);
            foreach (CarCuentasCorrientes cargoCuenta in this.MisCargosPendientes)
                if (!cargoCuenta.Incluir)
                    cargoCuenta.Incluir = MiFondoSuplementario.CargosPendientesCancelar.Exists(x => x.IdCuentaCorriente == cargoCuenta.IdCuentaCorriente && x.Incluir == true);

            MiFondoSuplementario.CargosPendientesCancelar = new List<CarCuentasCorrientes>();
            MiFondoSuplementario.CargosPendientesCancelar = MisCargosPendientes.Where(x => x.Incluir).ToList();

            //AyudaProgramacion.CargarGrillaListas<CarCuentasCorrientes>(MisCargosPendientes, false, this.gvCargosCancelaciones, true);
            upFondoSuplementario.Update();
        }
        #endregion


    }
}
