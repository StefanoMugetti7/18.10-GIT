<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TiposFuncionalidadesPorEstadosModificarDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.TiposFuncionalidadesPorEstadosModificarDatos" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>

<div class="ParametrosDatos">
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <div class="form-group row">
            <asp:Label CssClass="col-form-label col-sm-1" ID="lblEstados" runat="server" Text="Estados"></asp:Label>
            <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddlEstados_SelectedIndexChanged">
            </asp:DropDownList></div>
           </div>
            <asp:CheckBoxList ID="chkEstados" RepeatColumns="3" RepeatDirection="Horizontal" runat="server" >
                </asp:CheckBoxList> 
            <br />
            <center>
                <%--<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" Visible="false" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>