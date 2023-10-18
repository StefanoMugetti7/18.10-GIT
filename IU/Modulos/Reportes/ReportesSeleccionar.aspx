<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false"  AutoEventWireup="true" EnableTheming="true" Theme="com_1"
    CodeBehind="ReportesSeleccionar.aspx.cs" Inherits="IU.Modulos.Reportes.ReportesSeleccionar" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .fieldSet100 {
            width:100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../../assets/js/gridviewscroll.js" type="text/javascript"></script>
<script lang="javascript" type="text/javascript">
    function IniciarScroll (ctrl) {
        //var gridViewScroll = new GridViewScroll({
        //    elementID: ctrl.id,
        //    width: screen.width-100,
        //    height: 500,
        //    //freezeColumn: true,
        //    //freezeFooter: true,
        //    //freezeColumnCssClass: "GridViewScrollItemFreeze",
        //    //freezeFooterCssClass: "GridViewScrollFooterFreeze",
        //    //freezeHeaderRowCount: 2,
        //    //freezeColumnCount: 3,
        //});
        //gridViewScroll.enhance();
    }

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CheckBoxListStyle);
        CheckBoxListStyle();
    });

    function CheckBoxListStyle() {
        $('input:checkbox').each(function () {
            if (!$(this).hasClass("form-check-input")) {
                $(this).addClass("form-check-input");
            }
        });
        $('label').each(function () {
            if (!$(this).hasClass("form-check-label")) {
                $(this).addClass("form-check-label");
            }
        });
    }
</script>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <div class="col-sm-1"></div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblModulo" runat="server" Text="Módulo"></asp:Label>
                <div class="col-sm-4">
                <asp:DropDownList CssClass="form-control select2" ID="ddlModulosSistema" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlModulosSistema_SelectedIndexChanged">
                </asp:DropDownList>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblReporte" runat="server" Text="Reporte"></asp:Label>
                <div class="col-sm-4">
                <asp:DropDownList CssClass="form-control select2" ID="ddlReportes" runat="server" AutoPostBack="true"
                    onselectedindexchanged="ddlReportes_SelectedIndexChanged">
                </asp:DropDownList>
                    </div>
             </div>
            <div class="form-group row">
                <div class="col-sm-1"></div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle"></asp:Label>
                <div class="col-sm-9">
                <asp:TextBox CssClass="form-control" ID="txtDetalle" Enabled="false" Rows="4" Visible="false" TextMode="MultiLine" runat="server"></asp:TextBox>
                 </div>
                <div class="col-sm-1"></div>
            </div>
            <div class="form-group row">
                <div class="col-sm-2"></div>
                <div class="col-sm-9">
            <asp:Panel ID="pnlParametros" class="form-group fieldSet100" GroupingText="Parameters" Visible="false" runat="server">
            </asp:Panel>
                    </div>
                <div class="col-sm-1"></div>
            </div>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                        <asp:CheckBox ID="chkNombreCampos" Visible="false" runat="server"
                                Text="Incluir Nombres de Campos" />
                            <asp:CheckBox ID="chkSeparador" Visible="false" runat="server"
                                Text="Incluir Separador de Campos" />
                            <br /><br />
                            <asp:ImageButton ID="btnPantalla" Visible="false" ImageUrl="~/Imagenes/pantalla.png" 
                            runat="server" onclick="btnPantalla_Click" ToolTip="Mostrar en Pantalla" AlternateText="Mostrar en Pantalla"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnExportarExcel" Visible="false" ImageUrl="~/Imagenes/Excel-icon.png" 
                            runat="server" onclick="btnExportarExcel_Click" ToolTip="Descargar en formato Excel" AlternateText="Descargar en formato Excel"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnExportarTxt" CausesValidation="true" Visible="false" ImageUrl="~/Imagenes/txt-icon.png" 
                            runat="server" onclick="btnExportarTxt_Click" ToolTip="Descargar en formato Texto" AlternateText="Descargar en formato Texto" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnExportarPDF" Visible="false" ImageUrl="~/Imagenes/PDF-icon.png" 
                            runat="server" onclick="btnExportarPDF_Click" ToolTip="Descargar en formato PDF" AlternateText="Descargar en formato PDF"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnExportarDBF" Visible="false" ImageUrl="~/Imagenes/dbf_icon_chico.png" 
                            runat="server" onclick="btnExportarDBF_Click" ToolTip="Descargar en formato DBF" AlternateText="Descargar en formato DBF"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
               </div>
            </div>
            <asp:UpdatePanel ID="upGrillasPantalla" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                    <asp:Label CssClass="col-sm-12 col-form-label" ID="lblMensajeFilas" runat="server" Visible="false" Text="Se muestran las primeras 1.000 filas de cada tabla. Para obtener el reporte completo, descargue el Excel."></asp:Label>
                    </div>
                    <asp:Panel ID="pnlGrillas" CssClass="table-responsive" runat="server">
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
            <asp:PostBackTrigger ControlID="btnExportarTxt" />
            <%--<asp:PostBackTrigger ControlID="btnExportarPDF" />--%>
            <asp:PostBackTrigger ControlID="btnExportarDBF" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
