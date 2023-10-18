<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Comentarios.ascx.cs" Inherits="IU.Modulos.Comunes.Comentarios" %>

<asp:UpdatePanel ID="upComentarios" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
          
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFechaVencimiento" runat="server" Text="Fecha de Vencimiento"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaVencimiento" runat="server"></asp:TextBox>
            </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
             <div class="col-lg-3 col-md-3 col-sm-9">
               <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server">
                    </asp:DropDownList>
             <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Comentario" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                </div>
            <div class="col-sm-2">
                <asp:Button CssClass="botonesEvol" ID="btnAgregarComentario" OnClick="btnAgregarComentario_Click" ValidationGroup="Comentario" runat="server" Text="Agregar" />

                </div>
        </div>
        <div class="form-group row">
              <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAgregarComentario" runat="server" Text="Comentario"></asp:Label>
            <div class="col-sm-7">
                <asp:TextBox CssClass="form-control" ID="txtComentario" Rows="2" TextMode="MultiLine" MaxLength="500" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvComentario" ControlToValidate="txtComentario" ValidationGroup="Comentario" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

            </div>

        </div>
        <div class="table-responsive">
            <asp:GridView ID="gvComentarios" OnRowCommand="gvComentarios_RowCommand"
                OnRowDataBound="gvComentarios_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                    <asp:BoundField ItemStyle-Wrap="true" HeaderText="Comentario" DataField="Comentario" ItemStyle-Width="650px" />
                    <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vencimiento" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaVencimiento" />
                    <asp:BoundField DataField="FechaCumplimiento" HeaderText="Fecha Cumplimiento" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaCumplimiento" />
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlEstados" CssClass="form-control select2" runat="server"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Usuario" SortExpression="UsuarioLoguedo.ApellidoNombre">
                        <ItemTemplate><%# Eval("UsuarioLogueado.ApellidoNombre")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                                  <asp:HiddenField ID="hdfIdComentario" runat="server" Value='<%#Eval("IdComentario") %>' />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' Visible="false" ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Eliminar" ID="btnEliminar"
                                AlternateText="Eliminar Comentario" ToolTip="Eliminar Comentario" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
