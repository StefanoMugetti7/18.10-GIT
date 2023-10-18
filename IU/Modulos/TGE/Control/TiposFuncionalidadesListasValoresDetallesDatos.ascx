<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TiposFuncionalidadesListasValoresDetallesDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.TiposFuncionalidadesListasValoresDetallesDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<div class="ParametrosDatos">
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTiposFuncionalidades" runat="server" Text="Tipo Funcionalidad"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTiposFuncionalidades" runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvTiposFuncionalidades" ControlToValidate="ddlTiposFuncionalidades" runat="server" ValidationGroup="Aceptar" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListasValores" runat="server" Text="Lista de Valor"></asp:Label>
                <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlListasValores" runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvListasValores" ControlToValidate="ddlListasValores" runat="server" ValidationGroup="Aceptar" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
                <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click"/>
            </div>
                </div>
            <div class="form-group row">
            <asp:CheckBoxList ID="chkListasValoresDetalles" Enabled="false" RepeatColumns="3" RepeatDirection="Horizontal" runat="server" >
                </asp:CheckBoxList> 
                    
            </div>
             <div class="row justify-content-md-center">
                <div class="col-md-auto">
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" Visible="false" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </div>
                 </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>