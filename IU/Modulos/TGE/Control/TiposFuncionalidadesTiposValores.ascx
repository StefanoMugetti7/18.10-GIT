<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TiposFuncionalidadesTiposValores.ascx.cs" Inherits="IU.Modulos.TGE.Control.TiposFuncionalidadesTiposValores" %>
<div class="PlazosFijosDatos">
    <asp:UpdatePanel ID="upSaldoActual" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
               
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoValor" runat="server" Text="Tipo Valor" />
                <div class="col-sm-3">
                  
                 <asp:DropDownList CssClass="form-control select2" ID="ddlTipoValor" runat="server">
                        </asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoValor" runat="server" ControlToValidate="ddlTipoValor"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
        
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFuncionalidad" runat="server" Text="Tipo Funcionalidad" Visible="true" />
               
                <div class="col-sm-3">
                  
                   <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFuncionalidad" runat="server">
                        </asp:DropDownList>
                         <asp:RequiredFieldValidator CssClass="Validador" ID="rfvIdTipoFuncionalidad" runat="server" ControlToValidate="ddlTipoFuncionalidad"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
               
             
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
          
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
