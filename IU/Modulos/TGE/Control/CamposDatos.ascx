<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CamposDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.CamposDatos" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<AUGE:popupbotonconfirmar id="popUpConfirmar" runat="server" />

<div class="CuentasModificarDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTitulo" runat="server" Text="Titulo" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtTitulo" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTitulo" runat="server" ControlToValidate="txtTitulo"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <asp:UpdatePanel ID="upTablaParametro" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTablaValor" runat="server" Text="Tabla Asociada" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTablaValor" OnSelectedIndexChanged="ddlTablaValor_OnSelectedIndexChanged" AutoPostBack="true" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTablaValor" runat="server" ControlToValidate="ddlTablaValor"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTabla" runat="server" Text="Tabla Parametro" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTabla" AutoPostBack="true" OnSelectedIndexChanged="ddlTabla_SelectedIndexChanged" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIdRefTabla" runat="server" Text="Valor Parametro" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlIdRefTabla" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRequerido" runat="server" Text="Es obligatorio" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkRequerido" CssClass="form-control" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblOrden" runat="server" Text="Orden" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtOrden" runat="server" />
        </div>
        <div class="col-sm-3"></div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTamanioMinimo" runat="server" Text="Tamanio Mínimo" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtTamanioMinimo" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTamanioMaximo" runat="server" Text="Tamanio Máximo" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtTamanioMaximo" runat="server" />
        </div>
        <div class="col-sm-3"></div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCampoTipo" runat="server" Text="CampoTipo" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlCampoTipo" AutoPostBack="true" OnSelectedIndexChanged="ddlCampoTipo_SelectedIndexChanged" runat="server" />
                </div>
                <div class="col-sm-8"></div>
            </div>
            <asp:Panel ID="pnlListaValor" Visible="false" runat="server">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListaValor" runat="server" Text="Lista de Valores" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlListaValores" runat="server" />
                    </div>
                    <div class="col-sm-8"></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlStoredProcedure" Visible="false" runat="server">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblStoredProcedure" runat="server" Text="StoredProcedure" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtStoredProcedure" runat="server" />
                    </div>

                    <div class="col-sm-8"></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlConsultaDinamica" Visible="false" runat="server">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblConsultaDinamica" runat="server" Text="Consulta Dinamica (Campos: IdListaValorDetalle, Descripcion)" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtConsultaDinamica" TextMode="MultiLine" Rows="3" runat="server" />
                    </div>
                    <div class="col-sm-8"></div>
                </div>
            </asp:Panel>
     
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblStoredProcedureValidaciones" runat="server" Text="StoredProcedure Validaciones" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtStoredProcedureValidaciones" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblStoredProcedureLeyenda" runat="server" Text="StoredProcedure Leyenda" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtStoredProcedureLeyenda" runat="server" />
        </div>
        <br />
        </div>
              <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaltoLinea" runat="server" Text="Salto de Linea" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkSaltoLinea" CssClass="form-control" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDeshabilitado" runat="server" Text="Deshabilitado" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkDeshabilitado" CssClass="form-control" runat="server" />
        </div>
    </div>
        <div class="col-sm-3"></div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRefCampo" runat="server" Text="Ref Campo" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlRefCampo" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMostrarWordpress" runat="server" Text="Mostrar Wordpress" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkMostrarWordpress" CssClass="form-control" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCampoDependiente" runat="server" Text="Campo Dependiente" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlCampoDependiente" runat="server" />
        </div>
    </div>
               </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEventoJavaScript" runat="server" Text="Evento JavaScript" />
        <div class="col-sm-7">
            <asp:TextBox CssClass="form-control" ID="txtEventoJavaScript" TextMode="MultiLine" Rows="3" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblClase" runat="server" Text="Clase" />
                <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtClase" runat="server" />
        </div>
    </div>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
            <ContentTemplate>
                <AUGE:comentarios id="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
            <ContentTemplate>
                <AUGE:auditoriadatos id="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <AUGE:popupmensajespostback id="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
