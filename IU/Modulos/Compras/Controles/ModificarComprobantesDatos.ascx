<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModificarComprobantesDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.ModificarComprobantesDatos" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="AUGE" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<div class="ModificarComprobantes">


    <asp:Label ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
    <asp:TextBox ID="txtFechaDesde" runat="server"></asp:TextBox>
    <div class="Calendario">
        <asp:Image ID="imgFechaDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
        <asp:CalendarExtender ID="cdFechaDesde" runat="server" Enabled="True" TargetControlID="txtFechaDesde" PopupButtonID="imgFechaDesde" Format="dd/MM/yyyy"></asp:CalendarExtender>
        <asp:RequiredFieldValidator CssClass="Validador"  ID="rfvFechaDesde" ControlToValidate="txtFechaDesde" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>

    <asp:Label ID="lblPeriodoContable" runat="server" Text="Periodo Contable"></asp:Label>
    <asp:TextBox ID="txtPeriodoContable" runat="server" ReadOnly="true"></asp:TextBox>
    <div class="Espacio"></div>

    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />

<%--
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
--%>
<br />
<%--<asp:TabPanel runat="server" ID="tpDatos" HeaderText="Datos" >
        <ContentTemplate>--%>
        <br />
        <br />
        <asp:UpdatePanel ID="upGlobal" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="True" AllowSorting="True" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="False" ShowFooter="True"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" EnableModelValidation="True"
            >
                <Columns>
                    <asp:TemplateField HeaderText="Número Solicitud" SortExpression="NumeroSolicitud">
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
                            <asp:Label ID="lblImporteSinIVA" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Iva" SortExpression="ImporteTotal">
                        <ItemTemplate>
                            <%# Eval("IvaTotal", "{0:C2}")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblIvaTotal" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" SortExpression="ImporteTotal">
                        <ItemTemplate>
                            <%# Eval("ImporteTotal", "{0:C2}")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Contable" SortExpression="FechaContable">
                    <ItemTemplate>
                        <asp:TextBox ID="txtFechaContable" runat="server" Text='<%#Bind("FechaContable") %>'></asp:TextBox>
                        <div class="Calendario">
                        <asp:Image ID="imgFechaContable" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                        <asp:CalendarExtender ID="ceFechaContable" runat="server" Enabled="True" TargetControlID="txtFechaContable" PopupButtonID="imgFechaContable" Format="dd/MM/yyyy"></asp:CalendarExtender>
                        </div>
                    </ItemTemplate>
                        <%--<FooterStyle HorizontalAlign="Right" Wrap="False" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Wrap="False" />--%>
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
                                <asp:Label ID="lblGrillaTotalRegistros" runat="server" Text=""></asp:Label>
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
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="AfiliadosModificarDatosAceptar" />
                <asp:Button ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
                </center>
            </ContentTemplate>
        </asp:UpdatePanel> 
</div>

<script type="text/javascript">
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