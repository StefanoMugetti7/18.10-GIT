<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NomencladoresDatos.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.NomencladoresDatos" %>
<script type="text/javascript">
</script>

<div class="Nomencladores">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Codigo"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtCodigo" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigo" ControlToValidate="txtCodigo" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPrestacion" runat="server" Text="Prestacion"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtPrestacion" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPrestacion" ControlToValidate="txtPrestacion" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoPrestacion" runat="server" Text="Tipo Prestacion"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEspecializacion" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoPrestacion" ControlToValidate="ddlEspecializacion" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>