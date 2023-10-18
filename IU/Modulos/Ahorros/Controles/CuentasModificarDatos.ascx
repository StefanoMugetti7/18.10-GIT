<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CuentasModificarDatos.ascx.cs" Inherits="IU.Modulos.Ahorros.Controles.CuentasModificarDatos" %>
<%@ Register src="~/Modulos/Comunes/Archivos.ascx" tagname="Archivos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" tagname="popUpAfiliadosBuscar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<div class="CuentasModificarDatos">
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCuenta"  runat="server" Text="Tipo de Cuenta" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoCuenta"  runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoCuenta" runat="server" InitialValue="" ControlToValidate="ddlTipoCuenta" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda"  runat="server" Text="Moneda" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda"  runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMoneda" runat="server" InitialValue="" ControlToValidate="ddlMoneda" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado"  runat="server" Text="Estado" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado"  runat="server" />
    </div>
        </div>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1"  runat="server" Text="Denominación" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtDenominacion" MaxLength="250" TextMode="MultiLine" runat="server"></asp:TextBox>
    </div>
        <div class="col-sm-8"></div>
        </div>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumero"  runat="server" Text="Número" />
    <div class="col-sm-3">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero"  runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaldo"  runat="server" Text="Saldo Actual" />
    <div class="col-sm-3">
        <AUGE:CurrencyTextBox CssClass="form-control" ID="txtSaldo"  runat="server" />
    </div>
        <div class="col-sm-3"></div>
        </div>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAlta"  runat="server" Text="Fecha Alta" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtFechaAlta"  runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblUsuarioAlta"  runat="server" Text="Usuario Alta" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtUsuarioAlta"  runat="server" />
    </div>
        <div class="col-sm-3"></div>
    </div>
      <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
        <asp:TabPanel runat="server" ID="tpCotitulares" HeaderText="Cotitulares" >
            <ContentTemplate>
                <asp:UpdatePanel ID="upCotitulares" UpdateMode="Conditional" runat="server" >
                    <ContentTemplate>
                        <AUGE:popUpAfiliadosBuscar ID="ctrAfiliados" runat="server" />
                        <div class="form-group row">
                            <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnAgregarCotitular" runat="server" Text="Agregar Cotitular" 
                                onclick="btnAgregarCotitular_Click" Visible="false" CausesValidation="false" />
                                </div>
                            <div class="col-sm-9"></div>
                            </div>
                            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" 
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Apellido" SortExpression="Afiliado.Apellido">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.Apellido")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nombre" SortExpression="Afiliado.Nombre">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.Nombre")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo" SortExpression="Afiliado.TipoDocumento.TipoDocumento">
                                            <ItemTemplate>
                                                <%# Eval("Afiliado.TipoDocumento.TipoDocumento")%>
                                            </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Número" SortExpression="Afiliado.NumeroDocumento" ItemStyle-Wrap="false" >
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.NumeroDocumento")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Categoria" SortExpression="Afiliado.Categoria.Categoria">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.Categoria.Categoria")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Parentesco" SortExpression="Afiliado.Parentesco.Parentesco">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.Parentesco.Parentesco")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>            
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                 AlternateText="Elminiar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
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

  

    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
           <div class="row justify-content-md-center">
                <div class="col-md-auto">
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>