<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NichosDatos.ascx.cs" Inherits="IU.Modulos.Nichos.Controles.NichosDatos" %>


<div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigo" runat="server" Text="Codigo"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:TextBox CssClass="form-control" ID="txtCodigo" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvCodigo" ValidationGroup="Aceptar" ControlToValidate="txtCodigo" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoNicho" runat="server" Text="Tipo"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoNicho" OnSelectedIndexChanged="ddlTipoNicho_OnSelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvTipo" ControlToValidate="ddlTipoNicho" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>

    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Cementerio"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:DropDownList CssClass="form-control select2" ID="ddlCementerio" OnSelectedIndexChanged="ddlCementerio_OnSelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvCementerio" ControlToValidate="ddlCementerio" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPanteon" runat="server" Text="Panteon"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlPanteon" Enabled="false" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvPanteon" ControlToValidate="ddlPanteon" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNichoCapacidad" runat="server" Text="Capacidad"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlNichoCapacidad" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvNichoCapacidad" ControlToValidate="ddlNichoCapacidad" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblUbicacion" runat="server" Text="Ubicacion"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlUbicacion" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvUbicacion" ControlToValidate="ddlUbicacion" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label select2" ID="lblSubUbicacion" runat="server" Text="SubUbicacion"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlSubUbicacion" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvSubUbicacion" ControlToValidate="ddlSubUbicacion" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
</div>


    <div class="row justify-content-md-center">
        <div class="col-md-auto">
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
        </div>
    </div>





