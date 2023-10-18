<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanesIpsDatos.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PlanesIpsDatos" %>
<%@ Register src="~/Modulos/Prestamos/Controles/PlanesIpsDatosPopUp.ascx" tagname="ParametrosPopUp" tagprefix="auge" %>

<asp:UpdatePanel ID="upParametrosValores" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <AUGE:ParametrosPopUp ID="ctrParametrosPopUp" runat="server" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" onclick="btnAgregar_Click" CausesValidation="false" Visible="false" runat="server" Text="Agregar Plan IPS" />
            <div class="table-responsive">
            <asp:GridView ID="gvParametrosValores"
OnRowDataBound="gvParametrosValores_RowDataBound" DataKeyNames="IndiceColeccion" AllowPaging="true" 
onpageindexchanging="gvParametrosValores_PageIndexChanging" 
        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="FechaDesde" HeaderText="Fecha Desde" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaDesde" />
            <asp:BoundField DataField="ImporteTotal" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" HeaderText="Importe Total" SortExpression="ImporteTotal" />
            <asp:BoundField DataField="CantidadCuotas" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" HeaderText="Cantidad Cuotas" SortExpression="CantidadCuotas" />
            <asp:BoundField DataField="ImporteCuota" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" HeaderText="Importe Cuota" SortExpression="ImporteCuota" />
        </Columns>
    </asp:GridView>   </div>
        </ContentTemplate>
    </asp:UpdatePanel>   