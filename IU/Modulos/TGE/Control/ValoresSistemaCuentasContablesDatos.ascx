<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ValoresSistemaCuentasContablesDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.ValoresSistemaCuentasContablesDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%--<%@ Register src="~/Modulos/TGE/Control/ListasValoresDetallesDatosPopUp.ascx" tagname="DatosPopUp" tagprefix="auge" %>--%>
<%@ Register src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" tagname="CuentasContables" tagprefix="AUGE" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>

<div class="ListasValoresDetallesCuentasContables">
       <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListaValor" runat="server" Text="Lista Valor:"></asp:Label>
                  <div class="col-sm-3">   <asp:DropDownList CssClass="form-control select2" ID="ddlListasValores" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlListasValores_SelectedIndexChanged"/>
    
    </div></div>
    <asp:UpdatePanel ID="upListaDetalle"  UpdateMode="Conditional" runat="server" >
        <ContentTemplate>   <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListaDetalles" runat="server" Text="Lista Valor Detalle:"></asp:Label>
                    <div class="col-sm-3"> <asp:DropDownList CssClass="form-control select2" ID="ddlListaValorDetalles" Enabled="false"  runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvListaValorDetalles" runat="server" ControlToValidate="ddlListaValorDetalles" 
                    ErrorMessage="*" ValidationGroup="Aceptar"/></div>
             <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado:"></asp:Label>
                    <div class="col-sm-3"> <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" Enabled="false"  runat="server" />
    
        </div>
    
    <asp:UpdatePanel ID="upCuentasContables"  UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
    
            <asp:Panel ID="pnlCuentasContables" runat="server" Visible="false">
                <AUGE:CuentasContables ID="ctrCuentasContables" MostrarEtiquetas="true" runat="server" />
                <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuentasContables" runat="server" ControlToValidate="ctrCuentasContables" 
                    ErrorMessage="*" ValidationGroup="Aceptar"/>--%>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
    <br />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
        <asp:TabPanel runat="server" ID="tpItems" HeaderText="Relaciones Valores Cta Cte" >
            <ContentTemplate>
        
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand"
            AllowPaging="true" AllowSorting="true" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="IdListaValorSistemaDetalleCuentaContable" HeaderText="Codigo Relacion" SortExpression="CodigoRelacion" />
                    <asp:TemplateField HeaderText="Valor Sys Det" SortExpression="ListaValorSistemaDetalle.Descripcion">
                    <ItemTemplate>
                        <%# Eval("ListaValorSistemaDetalle.Descripcion")%>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Cuenta Contable" SortExpression="CuentaContable.Descripcion">
                    <ItemTemplate>
                        <%# Eval("CuentaContable.Descripcion")%>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                    <ItemTemplate>
                        <%# Eval("Estado.Descripcion")%>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText ="Eliminar" ItemStyle-Width="5%"  SortExpression ="">
                    <ItemTemplate>
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                AlternateText="Elminiar" ToolTip="Eliminar" />
                    </ItemTemplate>
                </asp:TemplateField>

                </Columns>
            </asp:GridView>
        
        </ContentTemplate>
    </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria" >
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            
        </ContentTemplate>
        </asp:TabPanel>

    </asp:TabContainer>
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" ValidationGroup="Aceptar" onclick="btnAceptar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>