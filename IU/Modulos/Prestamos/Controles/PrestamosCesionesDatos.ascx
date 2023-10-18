<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrestamosCesionesDatos.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PrestamosCesionesDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Archivos.ascx" tagname="Archivos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>

<script language="javascript" type="text/javascript">
    function CalcularTotales() {
        var subTotal = 0.00;
        var intTotal = 0.00;
        var cantidad = 0;
        $('#<%=gvDatos.ClientID%> tr').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importe = $(this).find('input:hidden[id*="hdfImporteAmortizacion"]').val(); //$("td:eq(4)", this).html();
            var interes = $(this).find('input:hidden[id*="hdfImporteInteres"]').val(); //$("td:eq(4)", this).html();
            if (incluir && importe && interes) {
                importe = importe.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                interes = interes.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                subTotal += parseFloat(importe);
                intTotal += parseFloat(interes);
                cantidad++;
            }
        });

        $("input[type=text][id$='txtCantidad']").val(cantidad);
        $("input[type=text][id$='txtTotalAmortizacion']").val(accounting.formatMoney(subTotal, "$ ", 2, "."));
        $("input[type=text][id$='txtTotalInteres']").val(accounting.formatMoney(intTotal, "$ ", 2, "."));
    }

    function CheckRow(objRef) {
        var row = objRef.parentNode.parentNode;
        var GridView = row.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            var headerCheckBox = inputList[0];
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
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
        $('#<%=gvDatos.ClientID%> tr').not(':last').each(function () {
            $(this).find('input:checkbox[id*="chkIncluir"]').prop('checked', objRef.checked);
        });        
    }

</script>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<div class="PrestamosCesionesDatos">
              <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCesionario" runat="server" Text="Cesionario"></asp:Label>
   <div class="col-sm-3"> <asp:DropDownList CssClass="form-control select2" ID="ddlCesionario" Enabled="false" runat="server">
    </asp:DropDownList>
 </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
<div class="col-sm-3">    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" Enabled="false" runat="server">
    </asp:DropDownList></div></div>
              <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPrestamoCesion" runat="server" Text="Descripcion"></asp:Label>
 <div class="col-sm-3">   <asp:TextBox CssClass="form-control" ID="txtDescripcion" Enabled="false" TextMode="MultiLine" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvDescripcion" ControlToValidate="txtDescripcion" CssClass="Validador" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div></div>
    
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
        <asp:TabPanel runat="server" ID="tpPrestamos" HeaderText="Prestamos" >
            <ContentTemplate>
                <asp:UpdatePanel ID="upPrestamos" UpdateMode="Conditional" runat="server" >
                    <ContentTemplate>
                        <asp:Panel ID="pnlBuscar" Visible="false" runat="server">
                               <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                    <div class="col-sm-3">    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                      </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                      <div class="col-sm-3">  <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                       </div>
                      <div class="col-sm-3">  <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" ValidationGroup="Buscar" />
                      </div></div>
                         <div class="form-group row">  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPlan" runat="server" Text="Plan" />
                       <div class="col-sm-3"> <asp:DropDownList CssClass="form-control select2" ID="ddlPlan" runat="server"  />
                      </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRiesgoCrediticio" runat="server" Text="Riesgo Crediticio" />
                      <div class="col-sm-3">  <Evol:CurrencyTextBox CssClass="form-control" ID="txtRiesgoCrediticio" Prefix="" NumberOfDecimals="0" runat="server" Text='<%#Bind("Cantidad") %>'></Evol:CurrencyTextBox>
                    </div></div>
                          <div class="form-group row"> <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadCuotasPendientes" runat="server" Text="Cant. Cuotas Pendientes" />
                       <div class="col-sm-3"> <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadCuotasPendientes" Prefix="" NumberOfDecimals="0" runat="server" Text='<%#Bind("Cantidad") %>'></Evol:CurrencyTextBox>
                    </div></div>
                        </asp:Panel>
                        <div class="table-responsive">

                        <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false" 
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                        <Columns>                            
                            <asp:TemplateField HeaderText="Fecha" SortExpression="FechaConfirmacion">
                                <ItemTemplate>
                                    <%# Eval("FechaConfirmacion", "{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField  HeaderText="#Prestamo" DataField="IdPrestamo" SortExpression="IdPrestamo" />
                            <asp:BoundField  HeaderText="# Cuotas" DataField="CantidadCuotas" SortExpression="CantidadCuotas" />
                            <%--<asp:BoundField  HeaderText="Amortizacion"  DataFormatString="{0:C2}" DataField="ImporteAmortizacion" SortExpression="ImporteAmortizacion" />--%>
                            <asp:TemplateField HeaderText="Amortizacion" SortExpression="ImporteAmortizacion"
                                HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("ImporteAmortizacion", "{0:C2}")%>
                                    <asp:HiddenField ID="hdfImporteAmortizacion" Value='<%# Eval("ImporteAmortizacion")%>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label CssClass="labelFooterEvol" ID="lblTotalAmortizacion" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interes" SortExpression="ImporteInteres"
                                HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("ImporteInteres", "{0:C2}")%>
                                    <asp:HiddenField ID="hdfImporteInteres" Value='<%# Eval("ImporteInteres")%>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label CssClass="labelFooterEvol" ID="lblTotalInteres" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion.TipoOperacion">
                                <ItemTemplate>
                                    <%# Eval("TipoOperacion.TipoOperacion")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("Estado.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right" >
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkTodos" Text="Acciones  " TextAlign="Left" runat="server" onclick="checkAllRow(this); CalcularTotales();" Visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate >
                                    <asp:CheckBox ID="chkIncluir" Visible="false" Checked='<%# Convert.ToBoolean (Eval("Incluir")) %>' onclick="CheckRow(this); CalcularTotales();" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label CssClass="labelFooterEvol" ID="lblGrillaTotalRegistros" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            </Columns>
                    </Evol:EvolGridView>
                            </div>
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

<br />
    <asp:UpdatePanel ID="upCalcularVAN" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
               <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTasa" runat="server" Text="Tasa"></asp:Label>
     <div class="col-sm-3"> <Evol:CurrencyTextBox CssClass="form-control" ID="txtTasa" runat="server" Prefix="" Enabled="false"></Evol:CurrencyTextBox>
    <asp:RequiredFieldValidator ID="rfvTasa" ControlToValidate="txtTasa" ValidationGroup="Aceptar" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
</div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblVAN" runat="server" Text="VAN"></asp:Label>
     <div class="col-sm-3"> <Evol:CurrencyTextBox CssClass="form-control" ID="txtVAN" Enabled="false" runat="server"></Evol:CurrencyTextBox>
   </div>   <div class="col-sm-3"><asp:Button CssClass="botonesEvol" ID="btnCalcularVAN" runat="server" Text="Calcular" onclick="btnCalcularVAN_Click" Visible="false" />
 </div></div>
                   </ContentTemplate>
    </asp:UpdatePanel>
     <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidad" runat="server" Text="Cantidad"></asp:Label>
     <div class="col-sm-3"> <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidad" Enabled="false" runat="server"></AUGE:NumericTextBox>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTotalAmortizacion" runat="server" Text="Total Amortizacion"></asp:Label>
      <div class="col-sm-3"><AUGE:NumericTextBox CssClass="form-control" ID="txtTotalAmortizacion" Enabled="false" runat="server"></AUGE:NumericTextBox>
 </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTotalInteres" runat="server" Text="Total Interes"></asp:Label>
     <div class="col-sm-3"> <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalInteres" Enabled="false" runat="server"></AUGE:NumericTextBox>
    </div></div>

    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
           <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
         </div></div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>