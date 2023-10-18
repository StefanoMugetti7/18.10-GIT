<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnticiposTurismoPopUp.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.AnticiposTurismoPopUp" %>

<script type="text/javascript">

    /*gridViewId va ser la grilla en la que deseo que suceda el selectallrows*/ 
    var gridViewId = '#<%= gvDatos.ClientID %>';

    function checkAllRow(selectAllCheckbox) {
        //get all checkboxes within item rows and select/deselect based on select all checked status
        //:checkbox is jquery selector to get all checkboxes
        $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
        updateSelectionLabel();
    }
    function CheckRow(selectCheckbox) {
        //if any item is unchecked, uncheck header checkbox as well
        if (!selectCheckbox.checked)
            $('th :checkbox', gridViewId).prop("checked", false);
        updateSelectionLabel();
    }
    function updateSelectionLabel() {
        //update the caption element with the count of selected items. 
        //:checked is jquery selector to get list of checked checkboxes
        $('caption', gridViewId).html($('td :checkbox:checked', gridViewId).length + " options selected");
    }

    function ShowModalAnticipoTurismo() {
         $('.modal-backdrop').remove();
        $("[id$='modalAnticipoTurismo']").modal('show');
    }

    function HideModalAnticipoTurismo() {
               $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalAnticipoTurismo']").modal('hide');
    }

</script>

<%--DATOS A FILTRAR--%>
<div class="modal" id="modalAnticipoTurismo" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
        <div class="modal-content modal-minHeight85">
            <div class="modal-header">
                <h5 class="modal-title">Buscar Reservas</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBuscar" runat="server" Text="Buscar" />
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtBuscar" Enabled="true" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-9"></div>
                             <div class="col-sm-3">
                                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                            </div>
                        </div>                        
                         <div class="form-group row">
                            <div class="col-sm-12">
                                <div class="table-responsive">
                        <asp:GridView ID="gvDatos" AllowPaging="false" AllowSorting="false"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdTipoCargoAfiliadoFormaCobro"
                            runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Nro.Reserva" >
                                    <ItemTemplate>
                                        <%# Eval("NumeroReserva")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Detalle" >
                                    <ItemTemplate>
                                        <%# Eval("Detalle")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Socio" >
                                    <ItemTemplate>
                                        <%# Eval("NumeroSocio")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Apellido y Nombre" >
                                    <ItemTemplate>
                                        <%# Eval("ApellidoNombre")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Importe" >
                                    <ItemTemplate>
                                        <%# Eval("Importe", "{0:C2}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckRow(this);" Checked='<%# Eval("Incluir")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                                    </div>
                                </div>
                             </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="row justify-content-md-center">
                    <div class="col-md-auto">
                        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" OnClientClick="HideModalAnticipoTurismo();" />
                        <asp:Button CssClass="botonesEvol" ID="btnVolver" OnClientClick="HideModalAnticipoTurismo();" CausesValidation="false" runat="server" Text="Volver" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>