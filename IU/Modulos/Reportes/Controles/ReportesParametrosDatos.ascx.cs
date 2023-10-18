using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reportes.Entidades;
using System.Data;
using Reportes.FachadaNegocio;
using AjaxControlToolkit;

namespace IU.Modulos.Reportes.Controles
{
    public partial class ReportesParametrosDatos : ControlesSeguros
    {
        protected RepReportes MiReporte
        {
            get { return (RepReportes)Session[this.MiSessionPagina + "ReportesParametrosDatosMiReporte"]; }
            set { Session[this.MiSessionPagina + "ReportesParametrosDatosMiReporte"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(RepReportes pParametro)
        {
            this.MiReporte = pParametro;
            if (pParametro.Parametros.Count > 0)
            {
                this.ArmarTablaParametros(pParametro);
                this.pnlParametros.Visible = true;
            }
            else
                this.pnlParametros.Visible = false;

            this.UpdatePanel1.Update();
        }

        /// <summary>
        /// Carga los valores de los parametros en el objeto Reporte
        /// </summary>
        public void CargarParametros(RepReportes pParametro)
        {
            this.MiReporte = pParametro;
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            Control ctrl;
            DateTime fechaHasta;
            foreach (RepParametros parametro in pParametro.Parametros)
            {
                foreach (Control tr in this.pnlParametros.Controls)
                {
                    ctrl = tr.FindControl(parametro.Parametro);
                    if (ctrl != null)
                    {
                        if (ctrl is TextBox)
                        {
                            if (parametro.TipoParametro.IdTipoParametro == (int)EnumRepTipoParametros.DateTime
                                && parametro.Parametro.ToLower().EndsWith("hasta"))
                            {
                                fechaHasta = Convert.ToDateTime(((TextBox)ctrl).Text);
                                //fechaHasta = fechaHasta.AddDays(1);
                                parametro.ValorParametro = fechaHasta;
                            }
                            else
                            {
                                parametro.ValorParametro = ((TextBox)ctrl).Text;
                            }
                            ctrl = null;
                            break;
                        }
                    }
                }

                switch (parametro.TipoParametro.IdTipoParametro)
                {
                    case (int)EnumRepTipoParametros.Int:
                        Control control = this.BuscarControlRecursivo(this.pnlParametros, "lst" + parametro.Parametro);
                        if (control != null)
                        {
                            DropDownList ddl = (DropDownList)control;
                            parametro.ValorParametro = ddl.SelectedValue;
                        }
                        break;
                    case (int)EnumRepTipoParametros.DateTime:
                        break;
                    case (int)EnumRepTipoParametros.TextBox:
                        break;
                    case (int)EnumRepTipoParametros.IntNumericInput:
                        break;
                    case (int)EnumRepTipoParametros.DateTimeRange:
                        break;
                    case (int)EnumRepTipoParametros.YearMonthCombo:
                        break;
                    default:
                        break;
                }
            }
        }

        #region "ArmarControlesParametros"

        private void ArmarTablaParametros(RepReportes pReporte)
        {
            //this.tablaParametros = new Table();
            this.pnlParametros.Controls.Clear();
            foreach (RepParametros parametro in pReporte.Parametros)
            {
                DataSet dsParametros = new DataSet();
                RepReportes rep;
                RepParametros param;

                if (!string.IsNullOrEmpty(parametro.StoredProcedure))
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    rep = new RepReportes();
                    rep.StoredProcedure = parametro.StoredProcedure;
                    param = new RepParametros();
                    param = pReporte.Parametros.Find(x => x.Parametro == parametro.ParamDependiente);
                    if (param != null)
                    {
                        param.ValorParametro = "0";
                        rep.Parametros.Add(param);
                    }
                    rep.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    dsParametros = ReportesF.ReportesObtenerDatos(rep);
                }

                switch (parametro.TipoParametro.IdTipoParametro)
                {
                    case (int)EnumRepTipoParametros.Int:
                        this.pnlParametros.Controls.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumRepTipoParametros.DateTime:
                        this.pnlParametros.Controls.Add(AddDateBoxRow(parametro.NombreParametro, parametro.Parametro));
                        //AddDateControl(parametro.NombreParametro, counter, parametro.Parametro);
                        break;
                    case (int)EnumRepTipoParametros.TextBox:
                        break;
                    case (int)EnumRepTipoParametros.IntNumericInput:
                        break;
                    case (int)EnumRepTipoParametros.DateTimeRange:
                        this.pnlParametros.Controls.Add(AddDateRangeBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumRepTipoParametros.YearMonthCombo:
                        break;
                    default:
                        break;
                }
                parametro.ParametroDibujado = true;
            }
        }

        private void CargarValoresParametros(RepParametros parametro, string pNombreControl)
        {
            DataSet dsParametros = new DataSet();
            RepReportes rep;

            if (!string.IsNullOrEmpty(parametro.StoredProcedure))
            {
                //Obtengo los valores para llenar las opciones del parametro
                rep = new RepReportes();
                rep.StoredProcedure = parametro.StoredProcedure;
                rep.Parametros.Add(parametro);
                rep.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                dsParametros = ReportesF.ReportesObtenerDatos(rep);
            }
            switch (parametro.TipoParametro.IdTipoParametro)
            {
                case (int)EnumRepTipoParametros.Int:
                    Control control = this.BuscarControlRecursivo(this.pnlParametros, "lst" + pNombreControl);
                    if (control != null)
                    {
                        DropDownList ddl = (DropDownList)control;
                        ddl.Items.Clear();
                        ddl.DataSource = dsParametros;
                        ddl.DataValueField = pNombreControl;
                        ddl.DataTextField = "Descripcion";
                        ddl.DataBind();
                        ListItem item = new ListItem("Seleccione una opción", "0");
                        item.Selected = true;
                        ddl.Items.Add(item);
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));

                    break;
                case (int)EnumRepTipoParametros.DateTime:
                    //tablaParametros.Rows.Add(AddDateBoxRow(parametro.NombreParametro, parametro.Parametro));

                    break;
                case (int)EnumRepTipoParametros.TextBox:
                    break;
                case (int)EnumRepTipoParametros.IntNumericInput:
                    break;
                case (int)EnumRepTipoParametros.DateTimeRange:
                    //tablaParametros.Rows.Add(AddDateRangeBoxRow(parametro.NombreParametro, parametro.Parametro));
                    break;
                case (int)EnumRepTipoParametros.YearMonthCombo:
                    break;
                default:
                    break;
            }
        }

        private Panel AddListBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            Panel miPanel = new Panel();
            Label miLabel = new Label();
            //TableRow tr = new TableRow();

            //TableCell tc1 = new TableCell();
            //TableCell tc2 = new TableCell();
            //TableCell tc3 = new TableCell();
            //TableCell tc4 = new TableCell();

            //tc1.Text = NombreParametro;
            //tc1.Width = Unit.Percentage(20);
            //tc1.CssClass = "TableCellBold";
            //tr.Cells.Add(tc1);
            miLabel.Text = NombreParametro;

            DropDownList drpFiltro = new DropDownList();
            //tc2.Width = Unit.Percentage(25);

            drpFiltro.DataValueField = Parametro;
            drpFiltro.DataTextField = "Descripcion";
            drpFiltro.DataSource = dsDatos;
            drpFiltro.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(drpFiltro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //ListItem item = new ListItem("Seleccione una opción", "0");
            //item.Selected = true;
            //drpFiltro.Items.Add(item);

            //drpFiltro.CssClass = "ListBox";
            drpFiltro.ID = "lst" + Parametro;
            //drpFiltro.Width = Unit.Percentage(80);
            drpFiltro.SelectedIndexChanged += new EventHandler(drpFiltro_SelectedIndexChanged);
            drpFiltro.AutoPostBack = this.MiReporte.Parametros.Exists(x => x.ParamDependiente == Parametro);
            //UIUtils.FillCombo(drpFiltro, dsDatos, dsDatos.Tables[0].Columns[0].ColumnName, dsDatos.Tables[0].Columns[1].ColumnName, true, false);

            //tc2.Controls.Add(drpFiltro);
            //tc2.CssClass = "TableCell";

            TextBox Text = new TextBox();
            //Text.CssClass = "TextBox";
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;
            //tc2.Controls.Add(Text);
            //tr.Cells.Add(tc2);

            miPanel.Controls.Add(miLabel);
            miPanel.Controls.Add(drpFiltro);
            miPanel.Controls.Add(Text);

            return miPanel;
        }

        void drpFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepParametros paramValor = this.MiReporte.Parametros.Find(x => x.Parametro == ((DropDownList)sender).ID.Remove(0, 3));
            RepParametros paramLlenar = this.MiReporte.Parametros.Find(x => x.ParamDependiente == paramValor.Parametro);
            paramValor.ValorParametro = ((DropDownList)sender).SelectedValue;
            if (paramLlenar != null)
            {
                RepParametros nuevo = new RepParametros();
                nuevo.Parametro = paramValor.Parametro;
                nuevo.TipoParametro = paramValor.TipoParametro;
                nuevo.StoredProcedure = paramLlenar.StoredProcedure;
                nuevo.ValorParametro = paramValor.ValorParametro;
                this.CargarValoresParametros(nuevo, paramLlenar.Parametro);
            }
        }

        private Panel AddMultipleListBoxRow(string NombreParametro, DataSet dsDatos, int Counter, string Parametro)
        {
            Panel miPanel = new Panel();
            Label miLabel = new Label();

            //TableRow tr = new TableRow();

            //TableCell tc1 = new TableCell();
            //TableCell tc2 = new TableCell();
            //TableCell tc3 = new TableCell();
            //TableCell tc4 = new TableCell();

            miLabel.Text = NombreParametro;
            //tc1.Text = NombreParametro;
            //tc1.Width = Unit.Percentage(20);
            //tc1.CssClass = "TableCellBold";
            //tr.Cells.Add(tc1);

            ListBox lstMultipleList = new ListBox();
            //tc2.Width = Unit.Percentage(25);

            //lstMultipleList.CssClass = "TextBox";
            lstMultipleList.ID = "MultipleListBox" + Counter.ToString();
            //lstMultipleList.Width = Unit.Percentage(80);
            lstMultipleList.SelectionMode = ListSelectionMode.Multiple;
            lstMultipleList.Rows = 8;

            //UIUtils.FillList(lstMultipleList, dsDatos, dsDatos.Tables[0].Columns[0].ColumnName, dsDatos.Tables[0].Columns[1].ColumnName);

            //tc2.Controls.Add(lstMultipleList);
            //tc2.CssClass = "TableCell";

            TextBox Text = new TextBox();
            //Text.CssClass = "TextBox";
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;
            //tc2.Controls.Add(Text);
            //tr.Cells.Add(tc2);

            miPanel.Controls.Add(miLabel);
            miPanel.Controls.Add(lstMultipleList);
            miPanel.Controls.Add(Text);
            return miPanel;
        }

        private Panel AddTextBoxRow(string NombreParametro, string Parametro)
        {
            Panel miPanel = new Panel();
            Label miLabel = new Label();
            //TableRow tr = new TableRow();

            //TableCell tc1 = new TableCell();
            //TableCell tc2 = new TableCell();
            miLabel.Text = NombreParametro;

            //tc1.Text = NombreParametro;
            //tc1.Width = Unit.Percentage(20);
            //tc1.CssClass = "TableCellBold";
            //tr.Cells.Add(tc1);

            //tc2.Width = Unit.Percentage(80);
            //tc2.ColumnSpan = 3;
            //tc2.CssClass = "TableCell";

            TextBox Text = new TextBox();
            //Text.CssClass = "TextBox";
            Text.ID = "txt1" + Parametro;
            TextBox Text2 = new TextBox();
            //Text2.CssClass = "TextBox";
            Text2.ID = "txt2" + Parametro;
            Text2.Visible = false;
            Text2.Text = Parametro;
            //tc2.Controls.Add(Text);

            //tc2.Controls.Add(Text2);
            //tr.Cells.Add(tc2);
            miPanel.Controls.Add(miLabel);
            miPanel.Controls.Add(Text);
            miPanel.Controls.Add(Text2);
            return miPanel;
        }

        private Panel AddNumericInputBoxRow(string NombreParametro, string Parametro)
        {
            Panel miPanel = new Panel();
            Label miLabel = new Label();
            miLabel.Text = NombreParametro;
            //TableRow tr = new TableRow();

            //TableCell tc1 = new TableCell();
            //TableCell tc2 = new TableCell();

            //tc1.Text = NombreParametro;
            //tc1.Width = Unit.Percentage(20);
            //tc1.CssClass = "TableCellBold";
            //tr.Cells.Add(tc1);

            //tc2.Width = Unit.Percentage(80);
            //tc2.ColumnSpan = 3;
            //tc2.CssClass = "TableCell";

            TextBox txtNumerico = new TextBox();
            txtNumerico.ID = NombreParametro;
            FilteredTextBoxExtender fte = new FilteredTextBoxExtender();
            fte.TargetControlID = NombreParametro;
            fte.FilterType = FilterTypes.Numbers;
            //txtNumerico.CssClass = "TextBox";

            //tc2.Controls.Add(txtNumerico);
            //tr.Cells.Add(tc2);
            miPanel.Controls.Add(miLabel);
            miPanel.Controls.Add(txtNumerico);
            miPanel.Controls.Add(fte);
            return miPanel;
        }

        private Panel AddDateRangeBoxRow(string NombreParametro, string Parametro)
        {
            Panel miPanel = new Panel();
            Label miLabel = new Label();
            miLabel.Text = NombreParametro;
            miPanel.Controls.Add(miLabel);
            //TableRow tr = new TableRow();

            //TableCell tc1 = new TableCell();
            //TableCell tc2 = new TableCell();

            //tc1.Text = NombreParametro;
            //tc1.Width = Unit.Percentage(20);
            //tc1.CssClass = "TableCellBold";
            //tr.Cells.Add(tc1);

            // RANGO DESDE
            TextBox dateSelectorDesde = new TextBox();
            // TODO:Cambiar año
            dateSelectorDesde.Text = DateTime.Now.AddYears(-1).ToShortDateString();
            //dateSelectorDesde.CssClass = "TextBox";
            dateSelectorDesde.Columns = 10;
            dateSelectorDesde.ID = "dtDesde" + Parametro;
            //tc2.Controls.Add(dateSelectorDesde);
            miPanel.Controls.Add(dateSelectorDesde);

            TextBox txtBoxDesde = new TextBox();
            txtBoxDesde.Text = "@Rango" + Parametro.Replace("@", "") + "Inicial";
            //txtBoxDesde.CssClass = "TextBox";
            txtBoxDesde.ID = "TxtBoxDesde" + Parametro;
            txtBoxDesde.Visible = false;
            //tc2.Controls.Add(txtBoxDesde);
            miPanel.Controls.Add(txtBoxDesde);

            Literal ltrl = new Literal();
            ltrl.Text = "  ";
            miPanel.Controls.Add(ltrl);

            Image imgCalendarDesde = new Image();
            imgCalendarDesde.ImageUrl = "~/Imagenes/Calendario.png";
            imgCalendarDesde.ID = "imgCalendarDesde" + Parametro;
            //tc2.Controls.Add(imgCalendarDesde);

            AjaxControlToolkit.CalendarExtender fechaCalendarDesde = new AjaxControlToolkit.CalendarExtender();
            fechaCalendarDesde.Format = "dd/MM/yyyy";
            fechaCalendarDesde.ID = "fechaCalendarDesde" + Parametro;
            fechaCalendarDesde.TargetControlID = dateSelectorDesde.ID;
            fechaCalendarDesde.PopupButtonID = imgCalendarDesde.ClientID;
            miPanel.Controls.Add(fechaCalendarDesde);

            Literal ltrl2 = new Literal();
            ltrl2.Text = "  ";
            miPanel.Controls.Add(ltrl2);

            // RANGO HASTA
            TextBox dateSelectorHasta = new TextBox();
            dateSelectorHasta.Text = DateTime.Now.AddYears(1).ToShortDateString();
            dateSelectorHasta.Columns = 10;
            //dateSelectorHasta.CssClass = "TextBox";
            dateSelectorHasta.ID = "dtHasta" + Parametro;
            miPanel.Controls.Add(dateSelectorHasta);

            TextBox txtBoxHasta = new TextBox();
            txtBoxHasta.Text = "@Rango" + Parametro.Replace("@", "") + "Final";
            //txtBoxHasta.CssClass = "TextBox";
            txtBoxHasta.ID = "TxtBoxHasta" + Parametro;
            txtBoxHasta.Visible = false;
            miPanel.Controls.Add(txtBoxHasta);

            Literal ltrl1 = new Literal();
            ltrl1.Text = "  ";
            miPanel.Controls.Add(ltrl1);

            Image imgCalendarHasta = new Image();
            imgCalendarHasta.ImageUrl = "~/Imagenes/Calendario.png";
            imgCalendarHasta.ID = "imgCalendarHasta" + Parametro;
            miPanel.Controls.Add(imgCalendarHasta);

            AjaxControlToolkit.CalendarExtender fechaCalendarHasta = new AjaxControlToolkit.CalendarExtender();
            fechaCalendarHasta.Format = "dd/MM/yyyy";
            fechaCalendarHasta.ID = "fechaCalendarHasta" + Parametro;
            fechaCalendarHasta.TargetControlID = dateSelectorHasta.ID;
            fechaCalendarHasta.PopupButtonID = imgCalendarHasta.ID;
            miPanel.Controls.Add(fechaCalendarHasta);

            AjaxControlToolkit.MaskedEditExtender fechaExtenderDesde = new AjaxControlToolkit.MaskedEditExtender();
            fechaExtenderDesde.Mask = "99/99/9999";
            fechaExtenderDesde.ID = "fechaExtenderDesde" + Parametro;
            fechaExtenderDesde.MaskType = AjaxControlToolkit.MaskedEditType.Date;
            fechaExtenderDesde.TargetControlID = dateSelectorDesde.ID;
            //fechaExtenderDesde.CultureName = "es-AR";
            fechaExtenderDesde.UserDateFormat = MaskedEditUserDateFormat.DayMonthYear;
            miPanel.Controls.Add(fechaExtenderDesde);

            AjaxControlToolkit.MaskedEditExtender fechaExtenderHasta = new AjaxControlToolkit.MaskedEditExtender();
            fechaExtenderHasta.Mask = "99/99/9999";
            fechaExtenderHasta.ID = "fechaExtenderHasta" + Parametro;
            fechaExtenderHasta.MaskType = AjaxControlToolkit.MaskedEditType.Date;
            fechaExtenderHasta.TargetControlID = dateSelectorHasta.ID;
            //fechaExtenderHasta.CultureName = "es-AR";
            fechaExtenderHasta.UserDateFormat = MaskedEditUserDateFormat.DayMonthYear;
            miPanel.Controls.Add(fechaExtenderHasta);

            //AjaxControlToolkit.MaskedEditValidator fechaValidatorDesde = new AjaxControlToolkit.MaskedEditValidator();
            //fechaValidatorDesde.ID = "fechaValidatorDesde" + Parametro;
            //fechaValidatorDesde.MaximumValue = "31/12/2100";
            //fechaValidatorDesde.MinimumValue = "01/01/1950";
            //fechaValidatorDesde.Display = ValidatorDisplay.Dynamic;
            //fechaValidatorDesde.EmptyValueMessage = "&nbsp;Fechas obligatorias";
            //fechaValidatorDesde.IsValidEmpty = false;
            //fechaValidatorDesde.ControlExtender = fechaExtenderDesde.ID;
            //fechaValidatorDesde.ControlToValidate = dateSelectorDesde.ID;
            //fechaValidatorDesde.InvalidValueMessage = "&nbsp;Formato inválido";
            //fechaValidatorDesde.MinimumValueMessage = "&nbsp;El valor mínimo es 01/01/1950";
            //fechaValidatorDesde.MaximumValueMessage = "&nbsp;El valor máximo es 31/12/2100";
            //tc2.Controls.Add(fechaValidatorDesde);

            //AjaxControlToolkit.MaskedEditValidator fechaValidatorHasta = new AjaxControlToolkit.MaskedEditValidator();
            //fechaValidatorHasta.ID = "fechaValidatorHasta" + Parametro;
            //fechaValidatorHasta.MaximumValue = "31/12/2100";
            //fechaValidatorHasta.MinimumValue = "01/01/1950";
            //fechaValidatorHasta.Display = ValidatorDisplay.Dynamic;
            //fechaValidatorHasta.EmptyValueMessage = "&nbsp;Fechas obligatorias";
            //fechaValidatorHasta.IsValidEmpty = false;
            //fechaValidatorHasta.ControlExtender = fechaExtenderHasta.ID;
            //fechaValidatorHasta.ControlToValidate = dateSelectorHasta.ID;
            //fechaValidatorHasta.InvalidValueMessage = "&nbsp;Formato inválido";
            //fechaValidatorHasta.MinimumValueMessage = "&nbsp;El valor mínimo es 01/01/1950";
            //fechaValidatorHasta.MaximumValueMessage = "&nbsp;El valor máximo es 31/12/2100";
            //tc2.Controls.Add(fechaValidatorHasta);

            //tc2.CssClass = "TableCell";
            //tc2.Width = Unit.Percentage(80);
            //tc2.ColumnSpan = 3;
            //tr.Cells.Add(tc2);

            return miPanel;
        }

        private Panel AddDateBoxRow(string NombreParametro, string Parametro)
        {
            Panel miPanel = new Panel();
            Label miLabel = new Label();
            miLabel.Text = NombreParametro;
            miPanel.Controls.Add(miLabel);
            //TableRow tr = new TableRow();

            //TableCell tc1 = new TableCell();
            //TableCell tc2 = new TableCell();

            //tc1.Text = NombreParametro;
            ////tc1.Width = Unit.Percentage(20);
            //tc1.CssClass = "TableCellBold";
            //tr.Cells.Add(tc1);

            TextBox dateSelector = new TextBox();
            // TODO:Cambiar año
            //if (Parametro == "FechaDesde")
            //    dateSelector.Text = DateTime.Now.AddDays(-DateTime.Now.Day+1).ToShortDateString();
            //else
            dateSelector.Text = DateTime.Now.ToShortDateString();
            //dateSelector.CssClass = "TextBox";
            dateSelector.Columns = 10;
            dateSelector.ID = Parametro;
            miPanel.Controls.Add(dateSelector);

            Image imgCalendar = new Image();
            imgCalendar.ImageUrl = "~/Imagenes/Calendario.png";
            imgCalendar.ID = "imgCalendar" + Parametro;
            miPanel.Controls.Add(imgCalendar);

            AjaxControlToolkit.CalendarExtender fechaCalendar = new AjaxControlToolkit.CalendarExtender();
            fechaCalendar.Format = "dd/MM/yyyy";
            fechaCalendar.ID = "fechaCalendar" + Parametro;
            fechaCalendar.TargetControlID = dateSelector.ID;
            fechaCalendar.PopupButtonID = imgCalendar.ClientID;
            miPanel.Controls.Add(fechaCalendar);

            Literal ltrl2 = new Literal();
            ltrl2.Text = "  ";
            miPanel.Controls.Add(ltrl2);

            AjaxControlToolkit.MaskedEditExtender fechaExtender = new AjaxControlToolkit.MaskedEditExtender();
            fechaExtender.Mask = "99/99/9999";
            fechaExtender.ID = "fechaExtender" + Parametro;
            fechaExtender.MaskType = AjaxControlToolkit.MaskedEditType.Date;
            fechaExtender.TargetControlID = dateSelector.ID;
            fechaExtender.CultureName = "es-AR";
            //fechaExtender.UserDateFormat = MaskedEditUserDateFormat.DayMonthYear;
            miPanel.Controls.Add(fechaExtender);

            //AjaxControlToolkit.MaskedEditValidator fechaValidator = new AjaxControlToolkit.MaskedEditValidator();
            //fechaValidator.ID = "fechaValidator" + Parametro;
            //fechaValidator.MaximumValue = "31/12/2100";
            //fechaValidator.MinimumValue = "01/01/1950";
            //fechaValidator.Display = ValidatorDisplay.Dynamic;
            //fechaValidator.EmptyValueMessage = "&nbsp;Fechas obligatorias";
            //fechaValidator.IsValidEmpty = false;
            //fechaValidator.ControlExtender = fechaExtender.ID;
            //fechaValidator.ControlToValidate = dateSelector.ID;
            //fechaValidator.InvalidValueMessage = "&nbsp;Formato inválido";
            //fechaValidator.MinimumValueMessage = "&nbsp;El valor mínimo es 01/01/1950";
            //fechaValidator.MaximumValueMessage = "&nbsp;El valor máximo es 31/12/2100";
            //tc2.Controls.Add(fechaValidator);

            //tc2.CssClass = "TableCell";
            //tc2.Width = Unit.Percentage(80);
            //tc2.ColumnSpan = 3;
            //tr.Cells.Add(tc2);
            return miPanel;
        }

        private Panel AddYearMonthCombo(string NombreParametro, string Parametro)
        {
            Panel miPanel = new Panel();
            Label miLabel = new Label();
            miLabel.Text = NombreParametro;
            miPanel.Controls.Add(miLabel);
            //TableRow tr = new TableRow();

            //TableCell tc1 = new TableCell();
            //TableCell tc2 = new TableCell();

            //tc1.Text = NombreParametro;
            //tc1.Width = Unit.Percentage(20);
            //tc1.CssClass = "TableCellBold";
            //tr.Cells.Add(tc1);

            //tc2.Width = Unit.Percentage(80);
            //tc2.ColumnSpan = 3;
            //tc2.CssClass = "TableCell";

            YearMonthCombo YMCYear = new YearMonthCombo();
            YMCYear.ID = "Year" + Parametro;

            for (int i = 1900; i < 2100; i++)
            {
                YMCYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            YearMonthCombo YMCMonth = new YearMonthCombo();
            YMCMonth.ID = "Month" + Parametro;

            YMCMonth.Items.Add(new ListItem("Enero", "01"));
            YMCMonth.Items.Add(new ListItem("Febrero", "02"));
            YMCMonth.Items.Add(new ListItem("Marzo", "03"));
            YMCMonth.Items.Add(new ListItem("Abril", "04"));
            YMCMonth.Items.Add(new ListItem("Mayo", "05"));
            YMCMonth.Items.Add(new ListItem("Junio", "06"));
            YMCMonth.Items.Add(new ListItem("Julio", "07"));
            YMCMonth.Items.Add(new ListItem("Agosto", "08"));
            YMCMonth.Items.Add(new ListItem("Septiembre", "09"));
            YMCMonth.Items.Add(new ListItem("Octubre", "10"));
            YMCMonth.Items.Add(new ListItem("Noviembre", "11"));
            YMCMonth.Items.Add(new ListItem("Diciembre", "12"));

            YMCYear.Text = DateTime.Now.Year.ToString();
            YMCMonth.Text = (DateTime.Now.Month < 10 ? ("0" + DateTime.Now.Month.ToString()) : DateTime.Now.Month.ToString());

            TextBox Text = new TextBox();
            //Text.CssClass = "TextBox";
            Text.ID = "TxtBox" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;

            miPanel.Controls.Add(YMCYear);
            miPanel.Controls.Add(YMCMonth);
            miPanel.Controls.Add(Text);
            //tr.Cells.Add(tc2);

            return miPanel;
        }

        #endregion
    }
}