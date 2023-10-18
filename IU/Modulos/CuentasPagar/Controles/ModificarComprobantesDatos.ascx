<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModificarComprobantesDatos.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.ModificarComprobantesDatos" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="AUGE" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<script>
    $("[id*='txtFechaContable']").datepicker({
         modal: true,
         header: true,
         footer: true,
         //change: function (e) {
         //    alert('Change is fired');
         //},
         select: function (e, type) {
             var row = $(this).closest('tr').find('input:checkbox[id*="chkIncluir"]').prop("checked", true);
         }
    });

     function CheckRow(objRef) {
         //Get the Row based on checkbox
         var row = objRef.parentNode.parentNode;
         //Get the reference of GridView
         var GridView = row.parentNode;
         //Get all input elements in Gridview
         var inputList = GridView.getElementsByTagName("input");
         for (var i = 0; i < inputList.length; i++) {
             //The First element is the Header Checkbox
             var headerCheckBox = inputList[0];
             //Based on all or none checkboxes
             //are checked check/uncheck Header Checkbox
             var checked = true;
             if (inputList[i].type == "checkbox" && inputList[i]
                                                != headerCheckBox) {
                 if (!inputList[i].checked) {
                     checked = false;
                     break;
                 }
             }
         }
         headerCheckBox.checked = checked;
     }
     function checkAllRow(objRef) {
         var GridView = objRef.parentNode.parentNode.parentNode;
         var inputList = GridView.getElementsByTagName("input");
         for (var i = 0; i < inputList.length; i++) {
             //Get the Cell To find out ColumnIndex
             var row = inputList[i].parentNode.parentNode;
             if (inputList[i].type == "checkbox" && objRef
                                                 != inputList[i]) {
                 if (objRef.checked) {
                     inputList[i].checked = true;
                 }
                 else {
                     inputList[i].checked = false;
                 }
             }
         }
     }
 </script>

<div class="ModificarComprobantes">

<asp:UpdatePanel ID="upGlobal" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodoContable" runat="server" Text="Periodo Contable"></asp:Label>
    <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlPeriodoContable" runat="server"></asp:DropDownList>
        </div>
            <div class="col-sm-3">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
                </div>
            </div>
        

            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" EnableModelValidation="true"
            >
                <Columns>
                    <asp:TemplateField HeaderText="Nro. Solicitud" SortExpression="NumeroSolicitud">
                            <ItemTemplate>
                                <%# Eval("IdSolicitudPago")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                            <ItemTemplate>
                                <%# Eval("Entidad.Nombre")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacion" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("TipoOperacion.TipoOperacion")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Solicitud" SortExpression="FechaAlta">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                            <ItemTemplate>
                                <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Nro Factura" SortExpression="NumeroFacturaCompleto">
                            <ItemTemplate>
                                <%# Eval("TipoNumeroFacturaCompleto")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" SortExpression="ImporteSinIVA">
                        <ItemTemplate>
                            <%# Eval("ImporteSinIVA", "{0:C2}")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteSinIVA" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" Wrap="false" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Wrap="false" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Iva" SortExpression="ImporteTotal">
                        <ItemTemplate>
                            <%# Eval("IvaTotal", "{0:C2}")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblIvaTotal" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" Wrap="false" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Wrap="false" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" SortExpression="ImporteTotal">
                        <ItemTemplate>
                            <%# Eval("ImporteTotal", "{0:C2}")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" Wrap="false" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Wrap="false" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Contable" SortExpression="FechaContable" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaContable" runat="server" Text='<%#Bind("FechaContable", "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                    </ItemTemplate>
                        <%--<FooterStyle HorizontalAlign="Right" Wrap="false" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Wrap="false" />--%>
                    </asp:TemplateField>                          
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Incluir" >
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckRow(this);" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblGrillaTotalRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField> 
                    </Columns>
            </asp:GridView>
   <%-- </ContentTemplate>
</asp:TabPanel>--%>

    <%--<asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria" >
        <ContentTemplate>
            <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
</asp:TabContainer>--%>

    </ContentTemplate>
</asp:UpdatePanel>
<br />

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
            <ContentTemplate>
                <center>
                <auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="AfiliadosModificarDatosAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
                </center>
            </ContentTemplate>
        </asp:UpdatePanel> 
</div>
