<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TurnerasModificarDatos.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.TurnerasModificarDatos" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<%@ Register Src="~/Modulos/Medicina/Controles/TurnosModificarDatosPopUp.ascx" TagPrefix="AUGE" TagName="Turnos" %>

<script type="text/javascript" lang="javascript">
    function ShowModalBuscarProducto() {
        $("[id$='modalBuscarProducto']").modal('show');
    }
    function HideModalBuscarProducto() {
        $("[id$='modalBuscarProducto']").modal('hide');
    }
</script>
<asp:UpdatePanel ID="upTurneras" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
           <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                    <div class="col-sm-3">
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilial" runat="server" ControlToValidate="ddlFilial"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
              <div class="col-sm-1">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" ValidationGroup="Aceptar" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPrestador" runat="server" Text="Prestador"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlPrestador" OnSelectedIndexChanged="ddlPrestadores_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPrestador" runat="server" ControlToValidate="ddlPrestador"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
              <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEspecializacion" runat="server" Text="Especialidad"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEspecializacion" runat="server"></asp:DropDownList>
            </div>
            <%--            <asp:Label CssClass="labelEvol" ID="lblEspecializacion" runat="server" Text="Especialización"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtEspecializacion" Enabled="false" runat="server"></asp:TextBox>--%>
        </div>
        <AUGE:Turnos ID="ctrTurnos" runat="server" />
        <DayPilot:DayPilotCalendar ID="dpcDias" runat="server" OnBeforeEventRender="dpcDias_OnBeforeEventRender"
            ViewType="Days" Days="7" OnEventClick="dpcDias_OnEventClick" Visible="false"
            EventClickHandling="PostBack" OnCommand="dpcDias_Command" EventClickJavaScript="LevantarPopup"
            OnTimeRangeSelected="dpcDias_TimeRangeSelected"/>
    </ContentTemplate>
</asp:UpdatePanel>
