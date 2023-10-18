<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaquinasDatos.ascx.cs" Inherits="IU.Modulos.LavaYa.Controles.MaquinasDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>


<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        SetTabIndexInput();

    });
    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function UpdPanelUpdate() {
        __doPostBack("<%=button.UniqueID %>", "");
    }
</script>

<div class="Maquinas">
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblMarca" runat="server" Text="Marca"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlMarca" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMarca_SelectedIndexChanged"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMarca" ControlToValidate="ddlMarca" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblModelo" runat="server" Text="Modelo"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlModelo" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvModelo" ControlToValidate="ddlModelo" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoMaquina" runat="server" Text="Tipo"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoMaquina" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoMaquina" ControlToValidate="ddlTipoMaquina" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <%--            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaVencimiento" runat="server" Text="Fecha Alta"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAlta" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaAlta" ControlToValidate="txtFechaAlta" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>--%>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNroSerie" runat="server" Text="Numero Serie"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtNroSerie" runat="server"></asp:TextBox>
            <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvNroSerie" ControlToValidate="txtNroSerie" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblImagen" Visible="false" runat="server" Text="Codigo QR:"></asp:Label>
        <contenttemplate>
            <asp:Image ID="imgLogo" runat="server" Width="128px" Visible="false" />
        </contenttemplate>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblManual" runat="server" Text="Manual:"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:AsyncFileUpload ID="afuPdf" OnUploadedFileError="afuPdf_UploadedFileError" OnClientUploadComplete="UpdPanelUpdate" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField" ToolTip="Seleccione archivo" runat="server"
                UploadingBackColor="#CCFFFF" ThrobberID="imgUploadFile" UploaderStyle="Traditional" />
            <asp:Button CssClass="botonesEvol" ID="button" runat="server" OnClick="button_Click" Style="display: none;" />
        </div>
    </div>
    <asp:UpdatePanel ID="upImagen" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPdfCargado" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:Button CssClass="botonesEvol" ID="btnVerManual" runat="server" Text="Ver" OnClick="btnVerManual_Click" Visible="false" />
                    <asp:Button CssClass="botonesEvol" ID="btnEliminarManual" runat="server" Text="Eliminar" OnClick="btnEliminarManual_Click" Visible="false" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
