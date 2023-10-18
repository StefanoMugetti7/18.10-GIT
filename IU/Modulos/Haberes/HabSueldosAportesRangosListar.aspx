<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="HabSueldosAportesRangosListar.aspx.cs" Inherits="IU.Modulos.Haberes.HabSueldosAportesRangosListar" %>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ListasValoresListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--<asp:Label CssClass="labelEvol" ID="lblParametro" runat="server" Text="Codigo Relacion" />
                <asp:TextBox CssClass="textboxEvol" ID="txtParametro" runat="server"></asp:TextBox>
           <div class="Espacio"></div>



                <asp:Label CssClass="labelEvol" ID="lblCodigoDetalle" runat="server" Text="Codigo Sistema Detalle" />
                <asp:TextBox CssClass="textboxEvol" ID="txtCodigoSistema" runat="server"></asp:TextBox>
             <div class="Espacio"></div>--%>

               <div class="form-group row">

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIngresoDesde" runat="server" Text="Ingreso Desde"></asp:Label>
                <div class=" col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control datepicker" ID="txtIngresoDesde" runat="server"></Evol:CurrencyTextBox>

              
                </div>

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIngresoHasta" runat="server" Text="Ingreso Hasta"></asp:Label>
                <div class=" col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control datepicker" ID="txtIngresoHasta" runat="server"></Evol:CurrencyTextBox>

                
                </div>
                     <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstados" runat="server" Text="Estado"></asp:Label>
     <div class=" col-sm-3"><asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
            </div>
                   </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAnioMinimo" runat="server" Text="Año minimo"></asp:Label>
                <div class=" col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtAnioMinimo" runat="server"></Evol:CurrencyTextBox>

            
                </div>
                     
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAnioMaximo" runat="server" Text="Año maximo"></asp:Label>
                <div class=" col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtAnioMaximo" runat="server"></Evol:CurrencyTextBox>
                    </div>
           
                </div>
              <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPorcentajeMinimo" runat="server" Text="Porcentaje Aporte Minimo"></asp:Label>
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajeMinimo" runat="server" NumberOfDecimals="4" Prefix=""></Evol:CurrencyTextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPorcentajeMaximo" runat="server" Text="Porcentaje Aporte Maximo"></asp:Label>
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajeMaximo" runat="server" NumberOfDecimals="4" Prefix=""></Evol:CurrencyTextBox>
                </div>
         
                    <div class="col-sm-2">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />

                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                  </div>

                <%-- <asp:Label CssClass="labelEvol" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
            <asp:DropDownList CssClass="selectEvol" ID="ddlEstados"  runat="server">
            </asp:DropDownList>
              <br /> --%>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdSueldoAporteRango"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:BoundField HeaderText="IdSueldoAporteRango" DataField="IdSueldoAporteRango" ItemStyle-Wrap="false" SortExpression="IdSueldoAporteRango" />
                        <asp:BoundField HeaderText="Fecha Ingreso Desde" DataField="FechaIngresoDesde" ItemStyle-Wrap="false" SortExpression="FechaIngresoDesde" />
                        <asp:BoundField HeaderText="Fecha Ingreso Hasta" DataField="FechaIngresoHasta" ItemStyle-Wrap="false" SortExpression="FechaIngresoHasta" />
                        <asp:BoundField HeaderText="Años Minimos" DataField="AniosMinimos" ItemStyle-Wrap="false" SortExpression="AniosMinimos" />
                        <asp:BoundField HeaderText="Años Maximos" DataField="AniosMaximos" ItemStyle-Wrap="false" SortExpression="AniosMaximos" />
                        <asp:BoundField HeaderText="Porcentaje Aporte Minimo" DataField="PorcentajeAporteMinimo" ItemStyle-Wrap="false" SortExpression="PorcentajeAporteMinimo" />
                        <asp:BoundField HeaderText="Porcentaje Aporte Maximo" DataField="PorcentajeAporteMaximo" ItemStyle-Wrap="false" SortExpression="PorcentajeAporteMaximo" />
                        <asp:BoundField HeaderText="Estado" DataField="EstadoDescripcion" ItemStyle-Wrap="false" SortExpression="EstadoDescripcion" />


                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>





            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

