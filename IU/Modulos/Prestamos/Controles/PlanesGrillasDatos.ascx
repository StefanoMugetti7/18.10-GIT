<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanesGrillasDatos.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PlanesGrillasDatos" %>
<%@ Register src="~/Modulos/Comunes/ImportarArchivo.ascx" tagname="popUpImportarArchivo" tagprefix="auge" %>

<asp:UpdatePanel ID="upPlanesGrillas" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
        <asp:Button CssClass="botonesEvol" ID="btnDescargarPlantilla" runat="server" Text="Descargar Plantilla" onclick="btnDescargarPlantilla_Click" CausesValidation="false" />
        <br /><br />
        <auge:popUpImportarArchivo ID="ctrImportarArchivo" runat="server" />
        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                                runat="server" onclick="btnExportarExcel_Click" Visible="false" />
        <asp:GridView ID="gvDatos"  AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" OnRowDataBound="gvDatos_RowDataBound"
            AllowPaging="true" runat="server" SkinID="GrillaBasicaFormal">
            <Columns>
                        <asp:BoundField DataField="CantidadCuotas" HeaderText="CantidadCuotas" SortExpression="CantidadCuotas" />
                        <asp:BoundField DataField="Neto" HeaderText="Monto Solicitado" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="Neto" />
                        <asp:BoundField DataField="Monto" HeaderText="Importe Prestamo" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="Monto" />
                        <asp:BoundField DataField="ImporteCuota" HeaderText="Importe Cuota" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="ImporteCuota" />
                        <asp:BoundField DataField="TasaAdm" HeaderText="Cargo Adm." HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="TasaAdm" />
                        <asp:BoundField DataField="Sellado" HeaderText="Sellado" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="Sellado" />
                        <asp:BoundField DataField="CFT" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" HeaderText="CFT" SortExpression="TasaEfectivaMensual" />
                        <asp:BoundField DataField="TEA" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" HeaderText="TEA" SortExpression="TasaEfectivaAnual" />
                        <asp:TemplateField HeaderText="">
                            <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblRegistros" runat="server" Text="Registros: "/>
                        </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
        </asp:GridView>   
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnDescargarPlantilla" />
        <asp:PostBackTrigger ControlID="btnExportarExcel" />
    </Triggers>
</asp:UpdatePanel>