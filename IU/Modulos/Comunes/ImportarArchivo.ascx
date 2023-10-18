<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportarArchivo.ascx.cs" Inherits="IU.Modulos.Comunes.ImportarArchivo" %>

<script language="javascript" type="text/javascript">
    function ClientUploadComplete_Action() {
        __doPostBack("<%=button.UniqueID %>", "");
    }
</script>

<div class="ImportarArchivo">
    <asp:Label CssClass="labelEvol" ID="lblColumnas" runat="server" Text="Nombre de Columnas Obligatorias" Width="100%"></asp:Label>
    <asp:Label CssClass="labelEvol" ID="lblColumnasDetalles" runat="server" Text="" Width="100%"></asp:Label>
    <br />
    <br />
    <asp:Label CssClass="labelEvol" ID="lblArchivo" runat="server" Text="Adjuntar archivo"></asp:Label>
    <asp:AsyncFileUpload ID="afuArchivo" OnClientUploadComplete="ClientUploadComplete_Action" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField" ToolTip="Seleccione archivo" runat="server" 
        UploadingBackColor="#CCFFFF" ThrobberID="imgUploadFile" UploaderStyle="Traditional" />
            <asp:ImageButton ID="imgUploadFile" Visible="false" ImageUrl="~/Imagenes/updateprogress.gif" runat="server" />
    <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" style="display:none;"/>
<br />
    <asp:Label CssClass="labelEvol" ID="lblCantidad" Visible="false" runat="server" Text="{0} filas importadas"></asp:Label>
</div>