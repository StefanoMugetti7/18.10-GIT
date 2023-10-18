<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="EjerciciosContablesListar.aspx.cs" Inherits="IU.Modulos.Contabilidad.EjerciciosContablesListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="EjerciciosContablesListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripción" />
           <div class="col-sm-3">  <asp:TextBox CssClass="form-control" ID="txtDescripcion"  runat="server" />
    </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
          <div class="col-sm-3">   <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
            </div>
          <div class="col-sm-3">   <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" /></div>
         </div>
           <%-- <asp:Label CssClass="labelEvol" ID="lblFechaInicioDesde" runat="server" Text="Fecha Inicio Desde" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaInicioDesde"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaInicioDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaInicioDesde" runat="server" Enabled="true" 
                    TargetControlID="txtFechaInicioDesde" PopupButtonID="imgFechaInicioDesde" Format="dd/MM/yyyy" />
            </div>
            <asp:Label CssClass="labelEvol" ID="lblFechaInicioHasta" runat="server" Text="Fecha Inicio Hasta" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaInicioHasta"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaInicioHasta" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaInicioHasta" runat="server" Enabled="true" 
                    TargetControlID="txtFechaInicioHasta" PopupButtonID="imgFechaInicioHasta" Format="dd/MM/yyyy" />
            </div>
            <br />
            <asp:Label CssClass="labelEvol" ID="lblFechaFinDesde" runat="server" Text="Fecha Fin Desde" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaFinDesde"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaFinDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaFinDesde" runat="server" Enabled="true" 
                    TargetControlID="txtFechaFinDesde" PopupButtonID="imgFechaFinDesde" Format="dd/MM/yyyy" />
            </div>
            <asp:Label CssClass="labelEvol" ID="lblFechaFinHasta" runat="server" Text="Fecha Fin Hasta" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaFinHasta"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaFinHasta" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaFinHasta" runat="server" Enabled="true" 
                    TargetControlID="txtFechaFinHasta" PopupButtonID="imgFechaFinHasta" Format="dd/MM/yyyy" />
            </div>
            <br />
            <asp:Label CssClass="labelEvol" ID="lblFechaCierreDesde" runat="server" Text="Fecha Cierre Desde" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaCierreDesde"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaCierreDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaCierreDesde" runat="server" Enabled="true" 
                    TargetControlID="txtFechaCierreDesde" PopupButtonID="imgFechaCierreDesde" Format="dd/MM/yyyy" />
            </div>
            <asp:Label CssClass="labelEvol" ID="lblFechaCierreHasta" runat="server" Text="Fecha Cierre Hasta" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaCierreHasta"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaCierreHasta" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaCierreHasta" runat="server" Enabled="true" 
                    TargetControlID="txtFechaCierreHasta" PopupButtonID="imgFechaCierreHasta" Format="dd/MM/yyyy" />
            </div>
            <br />
            <asp:Label CssClass="labelEvol" ID="lblFechaCopiativoDesde" runat="server" Text="Fecha Copiativo Desde" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaCopiativoDesde"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaCopiativoDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaCopiativoDesde" runat="server" Enabled="true" 
                    TargetControlID="txtFechaCopiativoDesde" PopupButtonID="imgFechaCopiativoDesde" Format="dd/MM/yyyy" />
            </div>
            <asp:Label CssClass="labelEvol" ID="lblFechaCopiativoHasta" runat="server" Text="Fecha Copiativo Hasta" />
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaCopiativoHasta"  runat="server" />
            <div class="Calendario">
                <asp:Image ID="imgFechaCopiativoHasta" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaCopiativoHasta" runat="server" Enabled="true" 
                    TargetControlID="txtFechaCopiativoHasta" PopupButtonID="imgFechaCopiativoHasta" Format="dd/MM/yyyy" />
            </div>--%>
            <br />
            <br />
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                <Columns>
                    <asp:BoundField  HeaderText="Descripción" DataField="Descripcion" SortExpression="Descripcion" />
                    <asp:BoundField  HeaderText="Fecha Inicio" DataField="FechaInicio" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaInicio" />
                    <asp:BoundField  HeaderText="Fecha Cierre" DataField="FechaCierre" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaCierre" />
                    <asp:BoundField  HeaderText="Fecha Fin" DataField="FechaFin" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaFin" />
                    <asp:BoundField  HeaderText="Fecha Copiativo" DataField="FechaCopiativo" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaCopiativo" />
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                        <ItemTemplate>
                            <%# Eval("Estado.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                AlternateText="Mostrar" ToolTip="Mostrar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                AlternateText="Modificar" ToolTip="Modificar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <center>
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" Visible="false"  onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
