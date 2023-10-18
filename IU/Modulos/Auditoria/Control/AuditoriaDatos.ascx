<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuditoriaDatos.ascx.cs" Inherits="IU.Modulos.Auditoria.Control.AuditoriaDatos" %>

<asp:GridView ID="gvDatos" AllowPaging="true"
 DataKeyNames="IndiceColeccion" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false"
    onpageindexchanging="gvDatos_PageIndexChanging" >
        <Columns>
            <%--<asp:BoundField  HeaderText="Table" DataField="Tabla" ItemStyle-Wrap="false" SortExpression="Tabla"/>--%>
            <%--<asp:BoundField  HeaderText="Id" DataField="IdRefTabla" ItemStyle-Wrap="false" SortExpression="IdRefTabla"/>--%>
            <asp:BoundField  HeaderText="Accion" DataField="Accion" ItemStyle-Wrap="false" SortExpression="Accion"/>
            <asp:BoundField  HeaderText="Campo Cambiado" DataField="CampoCambiado" ItemStyle-Wrap="false" SortExpression="CampoCambiado"/>
            <asp:BoundField  HeaderText="Valor Viejo" DataField="ValorViejo" ItemStyle-Wrap="true" SortExpression="ValorViejo"/>
            <asp:BoundField  HeaderText="Valor Nuevo" DataField="ValorNuevo" ItemStyle-Wrap="true" SortExpression="ValorNuevo"/>
            <asp:BoundField  HeaderText="Fecha" DataField="FechaEvento" ItemStyle-Wrap="false" SortExpression="FechaEvento"/>
            <asp:BoundField  HeaderText="Usuario" DataField="Usuario" ItemStyle-Wrap="false" SortExpression="Usuario"/>
        </Columns>
    </asp:GridView>