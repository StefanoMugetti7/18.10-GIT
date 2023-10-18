<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChequesModificarListar.ascx.cs" Inherits="IU.Modulos.Bancos.Controles.ChequesModificarListar" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>

<div class="ChequesListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              <asp:Panel ID="pnlDatosDeposito" Visible="false" runat="server">
            <div class="form-group row">

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
                <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTiposOperaciones" runat="server">
            </asp:DropDownList>
            </div>
          
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBoletaDeposito" runat="server" Text="Boleta Deposito"></asp:Label>
                    <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtBoletaDeposito" runat="server"></asp:TextBox>
               </div>
            </div>
                  <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle"></asp:Label>
                             <div class="col-sm-7">
                <asp:TextBox CssClass="form-control" ID="txtDetalle" MaxLength="100" TextMode="MultiLine" runat="server"></asp:TextBox>
               </div>
                      </div>
                  <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancosCuentas" runat="server" Text="Cuenta Bancaria"></asp:Label>
                   <div class="col-sm-7">
                <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentas" runat="server">
                </asp:DropDownList>

                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentas" runat="server" ErrorMessage="*" 
                    ControlToValidate="ddlBancosCuentas" Enabled="false" ValidationGroup="Aceptar"/>
               </div></div>
                  <div class="form-group row">
                      <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialesTraspaso" runat="server" Visible="false" Text="Filial a Traspasar"></asp:Label>
                   <div class="col-sm-7">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFilialesTraspaso" Visible="false" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilialesTraspaso" runat="server" ErrorMessage="*" 
                    ControlToValidate="ddlFilialesTraspaso" Enabled="false" ValidationGroup="Aceptar"/>
                      </div>
                      </div>
            </asp:Panel>
            <asp:Panel ID="Panel1" GroupingText="Filtros" runat="server">
                     <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                         <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
               
              </div>
                
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                          <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
               
              </div>
               <div class="col-sm-1">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
                  </div> </div>
                     <div class="form-group row">
                
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDiferidoDesde" runat="server" Text="Fecha Dif. Desde"></asp:Label>
                          <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferidoDesde" runat="server"></asp:TextBox>
              
             </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDiferidoHasta" runat="server" Text="Fecha Dif. Hasta"></asp:Label>
                          <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferidoHasta" runat="server"></asp:TextBox>
            </div>
                           </div>
                     <div class="form-group row">
                
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCheque" runat="server" Text="Numero Cheque" />
                          <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtNumeroCheque" runat="server" />
              </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBanco" runat="server" Text="Banco" />
                          <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server" />
            </div>
                           </div>
                     <div class="form-group row">
            
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
                          <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />  
                              </div>
             
                <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <asp:DropDownList CssClass="selectEvol" ID="ddlEstado" runat="server" />
                <br />--%>
                
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTitularCheque" runat="server" Text="Titular Cheque" />
                 <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtTitularCheque" runat="server" />
                       </div>
                     
                         </div>
            </asp:Panel>
            <br />
            <br />
            <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                        <asp:BoundField  HeaderText="Fecha Diferido" DataField="FechaDiferido" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaDiferido" />
                        <asp:BoundField  HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                        <asp:BoundField  HeaderText="Número Cheque" ItemStyle-HorizontalAlign="Right" DataField="NumeroCheque" SortExpression="NumeroCheque" />
                        <asp:TemplateField HeaderText="Banco" SortExpression="Banco.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Banco.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filial" SortExpression="Filial.Filial">
                            <ItemTemplate>
                                <%# Eval("Filial.Filial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField  HeaderText="Importe" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" DataField="Importe" SortExpression="Importe" />
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
<%--                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Incluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckRow(this);" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblGrillaTotalRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>    
                        </Columns>
                </asp:GridView>
                <br />
                <br />
                <center>
                    <%--<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" OnClick="btnImprimir_Click" AlternateText="Imprimir" visible="false" ToolTip="Imprimir" />
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
