<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListasValoresDetalles.ascx.cs" Inherits="IU.Modulos.TGE.Control.ListasValoresDetalles" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/TGE/Control/ListasValoresDetallesDatosPopUp.ascx" tagname="DatosPopUp" tagprefix="auge" %>
<asp:UpdatePanel ID="upParametrosValores" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListaValor" runat="server" Text="Lista Valor"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtListaValor" Enabled="false" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvListaValor" runat="server" ValidationGroup="AceptarLista" ControlToValidate="txtListaValor" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoValor" runat="server" Text="Codigo Valor"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtCodigoValor" Enabled="false" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvCodigoValor" Enabled="false" runat="server" ValidationGroup="AceptarLista" ControlToValidate="txtCodigoValor" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDepende"  runat="server" Text="Depende de" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlDepende" Enabled="false" runat="server" />
    </div>
            </div>
        


<asp:MultiView ID="MultiView1" runat="server">
<asp:View ID="vwValoresItem" runat="server">
    <AUGE:DatosPopUp ID="ctrDatosPopUp" runat="server" />
</asp:View>
    <asp:View ID="vwLista" runat="server">

        <div class="form-group row">
            <div class="col-sm-4">
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" OnClick="btnAgregar_Click" CausesValidation="false" runat="server" Text="Agregar Valor" />
            </div>
            <div class="col-sm-8"></div>
        </div>
        <div class="form-group row">
            <div class="col-sm-4">
                <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                    runat="server" OnClick="btnExportarExcel_Click" Visible="true" />
            </div>
            <div class="col-sm-8"></div>
        </div>
        <asp:GridView ID="gvParametrosValores" OnRowCommand="gvParametrosValores_RowCommand"
            AllowPaging="true" AllowSorting="true" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" 
            OnRowDataBound="gvParametrosValores_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="CodigoValor" HeaderText="Codigo Valor" SortExpression="CodigoValor" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
                <asp:TemplateField HeaderText="Valor Dependiente" SortExpression="DescripcionRef">
                    <ItemTemplate>
                        <%# Eval("DescripcionRef")%>
                    </ItemTemplate>
                </asp:TemplateField>    
                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                    <ItemTemplate>
                        <%# Eval("Estado.Descripcion")%>
                    </ItemTemplate>
                </asp:TemplateField>     
                
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                    AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
<%--                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Eliminar" ID="btnEliminar"
                                AlternateText="Eliminar Registro" ToolTip="Eliminar Registro" />--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>   
  
          <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" ValidationGroup="AceptarLista" onclick="btnAceptar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
        </asp:View>
</asp:MultiView>

         </ContentTemplate>
   <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
</asp:UpdatePanel>   
