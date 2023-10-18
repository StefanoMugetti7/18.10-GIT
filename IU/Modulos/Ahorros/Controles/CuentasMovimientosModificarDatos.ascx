<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CuentasMovimientosModificarDatos.ascx.cs" Inherits="IU.Modulos.Ahorros.Controles.CuentasMovimientosModificarDatos" %>
<%@ Register src="~/Modulos/Comunes/Archivos.ascx" tagname="Archivos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<div class="CuentasMovimientosModificarDatos">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuenta" runat="server" Text="Cuenta" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlCuenta" runat="server" OnSelectedIndexChanged="ddlCuenta_SelectedIndexChanged" 
                AutoPostBack="true"/>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuenta" runat="server" InitialValue="" ControlToValidate="ddlCuenta" 
                ErrorMessage="*" ValidationGroup="Aceptar"/>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaldoActual" runat="server" Text="Saldo Actual" />
            <div class="col-sm-3">
                <AUGE:CurrencyTextBox CssClass="form-control" ID="txtSaldoActual" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtFechaAlta" runat="server" />
                </div>
                </div>

    <div class="form-group row">
<%--    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoValor" runat="server" Text="Tipo de Valor" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoValor" runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoValor" runat="server" ControlToValidate="ddlTipoValor" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
    </div>--%>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacion" runat="server" ControlToValidate="ddlTipoOperacion" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
    </div>
     <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaMovimiento" runat="server" Text="Fecha Movimiento"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaMovimiento" Enabled="false" runat="server"></asp:TextBox>
        </div>
             <asp:Label CssClass="col-sm-1 col-form-label" ID="lblConceptoContable" runat="server" Text="Concepto"></asp:Label>
            <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlConceptoContable" runat="server"></asp:DropDownList>
        </div>

        </div>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" InitialValue="" ControlToValidate="txtImporte" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
</div>

        
        </div>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
                    
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
        <asp:TabPanel runat="server" ID="tpCotitulares" Visible="false" HeaderText="Cotitulares" >
            <ContentTemplate>
                <div class="form-group row">
                 <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCotitulares"  runat="server" Text="Cotitulares" />
        <div class="col-sm-3"> <asp:DropDownList CssClass="form-control select2" ID="ddlCotitulares"  runat="server" /></div>

                    <div class="col-sm-1"> <asp:Button CssClass="botonesEvol" ID="btnAgregarCotitular" runat="server" Text="Agregar" onclick="btnAgregarCotitular_Click"/></div>

                    </div>
                  <div class="table-responsive">
            <asp:GridView ID="gvCotitulares" OnRowCommand="gvCotitulares_RowCommand"
                OnRowDataBound="gvCotitulares_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                <Columns>

                    <asp:TemplateField HeaderText="Usuario" SortExpression="ApellidoNombre">
                        <ItemTemplate> <%# Eval("ApellidoNombre")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo de Documento" SortExpression="TipoDocumento">
                        <ItemTemplate><%# Eval("TipoDocumento.TipoDocumento")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Numero de Documento" SortExpression="NumeroDocumento">
                        <ItemTemplate><%# Eval("NumeroDocumento")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                                  <asp:HiddenField ID="hdfIdAfiliado" runat="server" Value='<%#Eval("IdAfiliado") %>' />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' Visible="false" ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Eliminar" ID="btnEliminar"
                                AlternateText="Eliminar" ToolTip="Eliminar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
            </ContentTemplate>
        </asp:TabPanel> 
        <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios" >
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos" >
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria" >
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
            </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                   <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>