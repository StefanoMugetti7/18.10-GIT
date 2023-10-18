<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AsientosContablesCuentasContablesParametrosDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.AsientosContablesCuentasContablesParametrosDatos" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" TagName="buscarCuentasContables" TagPrefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<div class="AsientosContablesCuentasContablesParametrosDatos">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoValor" runat="server" Text="Tipo Valor" />
          <div class="col-sm-3">   <asp:DropDownList CssClass="form-control select2" ID="ddlTipoValor" runat="server" OnSelectedIndexChanged="ddlEntidadContable_SelectedIndexChanged" AutoPostBack="true" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoValor" runat="server" ErrorMessage="*" 
                ControlToValidate="ddlTipoValor" ValidationGroup="Aceptar"/>
       </div>
            
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
          <div class="col-sm-3">   <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilial" runat="server" ErrorMessage="*" 
                ControlToValidate="ddlFilial" ValidationGroup="Aceptar"/>
            </div>
         
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
             <div class="col-sm-3"><asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="frvMoneda" runat="server" ErrorMessage="*" 
                ControlToValidate="ddlMoneda" ValidationGroup="Aceptar"/>
                </div></div>
            <asp:Panel ID="pnlBancoCuenta" Visible="false" runat="server">
                <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBanco" runat="server" Text="Banco" />
           <div class="col-sm-3">  <asp:DropDownList CssClass="form-control select2" ID="ddlBanco" runat="server" OnSelectedIndexChanged="ddlBanco_SelectedIndexChanged" AutoPostBack="true" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBanco" runat="server" ErrorMessage="*" 
                ControlToValidate="ddlBanco" ValidationGroup="Aceptar"/>
     </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoCuenta" runat="server" Text="Cuenta" />
         <div class="col-sm-3">    <asp:DropDownList CssClass="form-control select2" ID="ddlBancoCuenta" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancoCuenta" runat="server" ErrorMessage="*" 
                ControlToValidate="ddlBancoCuenta" ValidationGroup="Aceptar"/>
                    </div></div>
            </asp:Panel>

            <AUGE:buscarCuentasContables ID="buscarCuenta" MostrarEtiquetas="true" runat="server" />
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
            <div class="col-sm-3"> <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                </div></div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>