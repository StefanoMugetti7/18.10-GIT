<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Archivos.ascx.cs" Inherits="IU.Modulos.Comunes.Archivos" %>

<asp:UpdatePanel ID="upArchivos" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
    <asp:Panel ID="pnlAgregar" runat="server">
    <div class="form-group row" >
        <asp:Label CssClass="col-sm-2 col-form-label" ID="lblArchivo" runat="server" Text="Adjuntar archivo"></asp:Label>
        <div class="col-sm-4">
        <asp:AsyncFileUpload ID="afuArchivo" OnClientUploadStarted="showPopupProgressBar" OnClientUploadComplete="hidePopupProgressBar" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField" ToolTip="Seleccione archivo" runat="server" 
        UploadingBackColor="#CCFFFF" UploaderStyle="Traditional" />
            <%--<asp:ImageButton ID="imgUploadFile" ImageUrl="~/Imagenes/updateprogress.gif" runat="server" />--%>
        </div>
        <asp:Label CssClass="col-sm-2 col-form-label" ID="lblTipoArchivo" runat="server" Text="Tipo Archivo"></asp:Label>
        <div class="col-sm-2">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTiposArchivos" runat="server">
        </asp:DropDownList>
        </div>
        <div class="col-sm-2"></div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-2 col-form-label" ID="lblArchivoDescripcion" runat="server" Text="Descripcion"></asp:Label>
        <div class="col-sm-8">
        <asp:TextBox CssClass="form-control" Rows="2" ID="txtArchivoDescripcion" TextMode="MultiLine" runat="server"></asp:TextBox>
        </div>
        <div class="col-sm-2">
            <asp:Button CssClass="botonesEvol" ID="btnAgregarArchivo"  OnClick="btnAgregarArchivo_Click" ValidationGroup="Archivo" runat="server" Text="Agregar Archivo" />
        </div>
    </div>
    </asp:Panel>
    <div class="table-responsive">
        <asp:GridView ID="gvArchivos" OnRowCommand="gvArchivos_RowCommand" 
            OnRowDataBound="gvArchivos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                <asp:BoundField  ItemStyle-Wrap="true" HeaderText="Nombre Archivo" DataField="NombreArchivo"/>
                <asp:BoundField  HeaderText="Tamaño" DataField="TamanioFormateado" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="Tipo Archivo" SortExpression="ListaValorDetalle.Descripcion">
                    <ItemTemplate><%# Eval("ListaValorDetalle.Descripcion")%></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  ItemStyle-Wrap="true" HeaderText="Descripcion" DataField="Descripcion" />
                <%--<asp:TemplateField HeaderText="Usuario" SortExpression="UsuarioLoguedo.ApellidoNombre">
                    <ItemTemplate><%# Eval("UsuarioLogueado.ApellidoNombre")%></ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                        AlternateText="Mostrar Archivo" ToolTip="Mostrar Archivo" />
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/descargar.png" runat="server" CommandName="Descargar" ID="btnDescargar"
                                AlternateText="Download file" ToolTip="Download file" />
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' Visible="false" ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Eliminar" ID="btnEliminarArchivo"
                                AlternateText="Borrar archivo" ToolTip="Borrar archivo" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>