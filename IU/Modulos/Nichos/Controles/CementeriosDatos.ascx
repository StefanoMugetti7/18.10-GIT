<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CementeriosDatos.ascx.cs" Inherits="IU.Modulos.Nichos.Controles.CementeriosDatos" %>


<div class="CementerioPanteones">
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Cementerio"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
               <asp:RequiredFieldValidator ID="rfvValidador" ValidationGroup="Aceptar" ControlToValidate="txtDescripcion" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
 
        </div>
         <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvFilial" ControlToValidate="ddlFilial" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>      
    </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>    
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigo" runat="server" Text="Codigo"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
           <asp:TextBox CssClass="form-control" ID="txtCodigo" runat="server"></asp:TextBox>          
        <asp:RequiredFieldValidator ID="rfvCodigo" ValidationGroup="Aceptar" ControlToValidate="txtCodigo" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>
          <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDomicilio" runat="server" Text="Domicilio"></asp:Label>
        <div class="col-sm-3">
           <asp:TextBox CssClass="form-control" ID="txtDomicilio" runat="server"></asp:TextBox>               
        </div>
        </div>     
</div>
<d>   
    <div class="row justify-content-md-center">
        <div class="col-md-auto">
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
        </div>
    </div>
    </d iv>
<br />
 



    