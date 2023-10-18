<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AfipPadronTXTBuscar.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.AfipPadronTXTBuscar" %>


<script type="text/javascript" lang="javascript">

    function ShowModalBuscarPadronTXT()
    {
        $("[id$='modalBuscarPadronTXT']").modal('show');
    }

    function HideModalBuscarPadronTXT() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalBuscarPadronTXT']").modal('hide');
    }
</script>

<div class="modal" id="modalBuscarPadronTXT" tabindex="-1" role="dialog" >
    <form class="needs-validation" novalidate>
    <div class="modal-dialog modal-dialog-scrollable modal-xl modal-dialog-centered" role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">Seleccionar Datos Padron Afip</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
        <div class="modal-body">
             <asp:UpdatePanel ID="upSeleccionarPadronTXT" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" 
                DataKeyNames="CUIL"
                runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:BoundField HeaderText="CUIT/CUIL" DataField="CUIL" ItemStyle-Wrap="false" />
                    <asp:TemplateField HeaderText="Apellido Y Nombre">
                        <ItemTemplate>
                            <%# string.Concat(Eval("Apellido"),", ", Eval("Nombre"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Seleccionar" ToolTip="Seleccionar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
            <div class="modal-footer">
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" data-dismiss="modal" />
        </div>
    </div>
        </div>
        </form>
</div>