<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemesasControlDatos.ascx.cs" Inherits="IU.Modulos.Haberes.Controles.RemesasControlDatos" %>
<%--<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>--%>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" tagname="popUpAfiliadosBuscar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/EnviarMails.ascx" tagname="EnviarMails" tagprefix="auge" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<div class="CuentasModificarDatos">
    <asp:Label CssClass="labelEvol" ID="lblPeriodo"  runat="server" Text="Periodo" />
    <asp:TextBox CssClass="textboxEvol" ID="txtPeriodo" ReadOnly="true" runat="server"></asp:TextBox>
    <asp:HiddenField ID="hdfIdRemesaTipo" runat="server" />
    <div class="Espacio"></div>
    
    <asp:Label CssClass="labelEvol" ID="lblCantidadRegistro"  runat="server" Text="Cantidad Registros" />
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCantidadRegistros" ReadOnly="true" runat="server"></AUGE:NumericTextBox>
    <div class="Espacio"></div>
    
    <asp:Label CssClass="labelEvol" ID="lblImporteTotal"  runat="server" Text="Importe Total" />
    <AUGE:CurrencyTextBox CssClass="textboxEvol" ID="txtImporteTotal" ReadOnly="true" runat="server"></AUGE:CurrencyTextBox>
    
    <br />
    
    <asp:Label CssClass="labelEvol" ID="lblCantidadDepositar"  runat="server" Text="Cantidad a Depositar" />
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCantidadDepositar" ReadOnly="true" runat="server"></AUGE:NumericTextBox>
    <div class="Espacio"></div>
    
    <asp:Label CssClass="labelEvol" ID="lblImporteDepositar"  runat="server" Text="Importe Total" />
    <AUGE:CurrencyTextBox CssClass="textboxEvol" ID="txtImporteDepositar" ReadOnly="true" runat="server"></AUGE:CurrencyTextBox>
    
    <br />
    <br />
    <asp:UpdatePanel ID="upDetalles" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            
            <asp:Panel ID="pnlFiltros" GroupingText="Filtros sobre grilla" runat="server">
                <asp:Label CssClass="labelEvol" ID="lblNumeroDocumento"  runat="server" Text="Numero Documento IAF" />
                <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
                <div class="Espacio"></div>    
                <asp:Label CssClass="labelEvol" ID="lblEstadoAfiliado"  runat="server" Text="Estados Socio" />
                <asp:DropDownList CssClass="selectEvol" ID="ddlEstadosAfiliados" runat="server">
                </asp:DropDownList>
                <br />
                <asp:Label CssClass="labelEvol" ID="lblEstado"  runat="server" Text="Estados" />
                <asp:DropDownList CssClass="selectEvol" ID="ddlEstados" runat="server">
                </asp:DropDownList>
                <div class="Espacio"></div>
                <asp:Button CssClass="botonesEvol" ID="btnFiltrar" runat="server" onclick="btnFiltrar_Click" Text="Filtrar Datos" />
            </asp:Panel>

            <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                                runat="server" onclick="btnExportarExcel_Click" Visible="false" />

            <AUGE:popUpAfiliadosBuscar ID="ctrAfiliados" runat="server" />
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="True" AllowSorting="True" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="False" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Tipo" SortExpression="RemesaTipo.CodigoValor">
                            <ItemTemplate>
                                <%# Eval("RemesaTipo.CodigoValor")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Nombre" SortExpression="Afiliado.ApellidoNombre">
                            <ItemTemplate>
                                <%# Eval("Afiliado.ApellidoNombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="Nombre" SortExpression="Afiliado.Nombre">
                            <ItemTemplate>
                                <%# Eval("Afiliado.Nombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Tipo Nro Doc" SortExpression="Afiliado.TipoDocumento.TipoDocumento">
                            <ItemTemplate>
                                <%# string.Concat( Eval("Afiliado.TipoDocumento.TipoDocumento"), " ", Eval("Afiliado.NumeroDocumento")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Número Doc" SortExpression="Afiliado.NumeroDocumento">
                            <ItemTemplate>
                                <%# Eval("Afiliado.NumeroDocumento")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Numero Socio" SortExpression="Afiliado.NumeroSocio">
                            <ItemTemplate>
                                <%# Eval("Afiliado.NumeroSocio")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo Nro Doc IAF" SortExpression="TipoDocumentoIAF">
                                <ItemTemplate>
                                    <%# string.Concat( Eval("TipoDocumentoIAF"), " ",Eval("NumeroDocumentoIAF") ) %>
                                </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Número DOC IAF" SortExpression="NumeroDocumentoIAF" ItemStyle-Wrap="false" >
                            <ItemTemplate>
                                <%# Eval("NumeroDocumentoIAF")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Apellido Nombre IAF" SortExpression="ApellidoNombre">
                            <ItemTemplate>
                                <%# Eval("ApellidoNombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Neto IAF" SortExpression="NetoIAF" ItemStyle-Wrap="false" >
                            <ItemTemplate>
                                <%# Eval("NetoIAF", "{0:C2}")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                    <asp:Label CssClass="labelFooterEvol" ID="lblNetoIAF" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observaciones" SortExpression="Observaciones">
                            <ItemTemplate>
                                <%# Eval("Observaciones")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Categoria" SortExpression="CategoriaAfiliado">
                            <ItemTemplate>
                                <%# Eval("CategoriaAfiliado")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado Socio" SortExpression="EstadoAfiliado">
                            <ItemTemplate>
                                <%# Eval("EstadoAfiliado")%>
                            </ItemTemplate>
                        </asp:TemplateField>      
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>      
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Agregar" ID="btnAgregar" Visible="false"
                                             AlternateText="Agregar pago" ToolTip="Agregar pago" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                             AlternateText="Elminiar" ToolTip="Eliminar" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblGrillaTotalRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        </Columns>
                </asp:GridView>
        </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
    </asp:UpdatePanel>
    
    <br />
    <asp:UpdatePanel ID="upEnviarMails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <auge:EnviarMails ID="ctrEnviarMails" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <%--<asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail" Visible="true"
                          onclick="btnEnviarMail_Click"  AlternateText="Enviar Correos" ToolTip="Enviar Correos" />--%>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Visible="false" Text="Depósito y Generación de Recibos" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCerrarRemesa" runat="server" Visible="false" Text="Cierre de Remesa y Generacion de Solicitud de Pago IAF" onclick="btnCerrarRemesa_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>