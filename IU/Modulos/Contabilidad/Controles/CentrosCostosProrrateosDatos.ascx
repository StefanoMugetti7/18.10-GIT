<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CentrosCostosProrrateosDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.CentrosCostosProrrateosDatos" %>
<%--<%@ Register src="~/Modulos/Comunes/Archivos.ascx" tagname="Archivos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>
--%>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>

<script language="javascript" type="text/javascript">
    function CentrosCostosCalcularItem() {
        var totalDebe = 0.00;
        $('#<%=gvDatos.ClientID%> tr').each(function () {
            var importeDebe = $(this).find('input:text[id*="txtPorcentaje"]').maskMoney('unmasked')[0];
            if (importeDebe) {
                totalDebe += parseFloat(importeDebe);
            }
        });
        $("#<%=gvDatos.ClientID %> [id*=lblPorcentajeTotal]").text(accounting.formatMoney(totalDebe, "", 2, "."));
    }

</script>
     
<div class="CentrosCostosProrrateosDatos">   <div class="form-group row">
<asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCentrosCostoProrrateo" runat="server" Text="Escenario de Centros de Costos"></asp:Label>
 <div class="col-lg-3 col-md-3 col-sm-9"><asp:TextBox CssClass="form-control" ID="txtCentrosCostosProrrateo" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator CssClass="Validador" ID="rfvCentrosCostosProrrateo" runat="server" ErrorMessage="*" ControlToValidate="txtCentrosCostosProrrateo" ValidationGroup="CCPDAceptar"/>
</div>
<asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
 <div class="col-lg-3 col-md-3 col-sm-9"><asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" Enabled="false" runat="server" />
<asp:RequiredFieldValidator CssClass="Validador" ID="rfvFiliales" runat="server" ErrorMessage="*" ControlToValidate="ddlFiliales" ValidationGroup="CCPDAceptar"/>
</div>
<asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
 <div class="col-lg-3 col-md-3 col-sm-9"><asp:DropDownList CssClass="form-control select2" ID="ddlEstado" Enabled="false" runat="server" />
</div></div>
 
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
        <asp:TabPanel runat="server" ID="TabPanel1" HeaderText="Detalle de Centros de Costos" >
            <ContentTemplate>
    <asp:UpdatePanel ID="upDetalleCentrosCostos" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <br />
                <asp:Button CssClass="botonesEvol" ID="btnAgregarDetalle" runat="server" Text="Agregar" onclick="btnAgregarCuenta_Click" CausesValidation="false" />
            <br />
            <br />
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion" 
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                     <asp:TemplateField HeaderText ="Centro de Costo" ItemStyle-Wrap="false" >
                        <ItemTemplate>
                          <div class="col-lg-9 col-md-9 col-sm-9">   <asp:DropDownList CssClass="form-control select2" ID="ddlCentrosCostos" runat="server" /></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Porcentaje" ItemStyle-Wrap="false" >
                        <ItemTemplate>
                          <div class="col-lg-9 col-md-9 col-sm-9">   <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentaje" Text='<%# Eval("Porcentaje")%>' Prefix="" NumberOfDecimals="2" MaxLength="6" runat="server"/>
                        </ItemTemplate>
                        <FooterTemplate>    
                            <asp:Label CssClass="labelFooterEvol" ID="lblPorcentajeTotal" runat="server"  Text="0" Style="text-align: right"/>
                        </FooterTemplate> 
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblRegistros" runat="server" Text="Registros: "/>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <%--<asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios" >
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos" >
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>--%>
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
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="CCPDAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>