<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListasPreciosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Compras.Controles.ListasPreciosBuscarPopUp" %>

<script lang="javascript" type="text/javascript">
   function ShowModalPopUp() {
        $("[id$='modalPopUp']").modal('show');
    }

    function HideModalPopUp() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalPopUp']").modal('hide');
    }
    </script>
<div class="modal" id="modalPopUp" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content">
                             <div class="modal-header">
                                <h5 class="modal-title">Sistema de gestión para mutuales</h5>
                                   <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                      <span aria-hidden="true">&times;</span>
                                   </button>
                             </div>
                         <div class="modal-body">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
   <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-4 col-sm-4 col-form-label" ID="lblCodigoLista" runat="server" Text="Codigo" ></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtCodigoLista" runat="server" Enabled="true"></asp:TextBox>

      </div>
            <asp:Label CssClass="col-lg-1 col-md-4 col-sm-4 col-form-label" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9"><asp:DropDownList CssClass="form-control select2" ID="ddlEstados"  runat="server">
            </asp:DropDownList></div>

   <div class="col-lg-3 col-md-3 col-sm-9">

        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
                onclick="btnBuscar_Click" />
            </div>
    </div>   <div class="form-group row">
        
        <asp:Label CssClass="col-lg-1 col-md-4 col-sm-4 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
          <div class="col-lg-3 col-md-3 col-sm-9">   <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
           </div>
            <asp:Label CssClass="col-lg-1 col-md-4 col-sm-4 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
           <div class="col-lg-3 col-md-3 col-sm-9">  <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
        
               </div></div>   <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-4 col-sm-4 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
    <div class="col-lg-7 col-md-7 col-sm-9">     <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" TextMode="MultiLine" Enabled="true"></asp:TextBox>
      </div></div>
        <%--<auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />--%>
            <div class="table-responsive">
         <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" 
            >
                <Columns>
                    <asp:TemplateField HeaderText="Codigo" SortExpression="IdListaPrecio">
                            <ItemTemplate>
                                <%# Eval("IdListaPrecio")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                            <ItemTemplate>
                                <%# Eval("Descripcion")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha" SortExpression="FechaAlta">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Inicio Vigencia" SortExpression="FechaInicio">
                        <ItemTemplate>
                            <%# Eval("FechaInicioVigencia", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Fin Vigencia" SortExpression="FechaFin">
                        <ItemTemplate>
                            <%# Eval("FechaFinVigencia", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("Estado.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>        
                     <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                                
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnSeleccionar"
                         AlternateText="Seleccionar" ToolTip="Seleccionar" />
                            
                        </ItemTemplate>
                     </asp:TemplateField>
                    </Columns>
            </asp:GridView>
                </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</div>
                    <div class="modal-footer">

   
   <div class="row justify-content-md-center">
            <div class="col-md-auto">
        <asp:Button CssClass="botonesEvol" ID="btnVolver" OnClick="btnCancelar_Click"  CausesValidation="false" runat="server" Text="Volver" />
  </div></div>
   

                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>