<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AsientosListar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script lang="javascript" type="text/javascript">
    
    function InitControlFecha(desde, hasta) {
        var minDate = new Date(1970, 0, 1);
        minDate.setMilliseconds(desde);
        var maxDate = new Date(1970, 0, 1);
        maxDate.setMilliseconds(hasta);
        //txtFechaAsiento.destroy();
        var txtFechaAsientoDesde = $('input:text[id$=txtFechaAsientoDesde]').datepicker({
            showOnFocus: true,
            uiLibrary: 'bootstrap4',
            locale: 'es-es',
            format: 'dd/mm/yyyy',
            minDate: minDate,
            maxDate: maxDate
        });
        var txtFechaAsientoHasta = $('input:text[id$=txtFechaAsientoHasta]').datepicker({
            showOnFocus: true,
            uiLibrary: 'bootstrap4',
            locale: 'es-es',
            format: 'dd/mm/yyyy',
            minDate: minDate,
            maxDate: maxDate
        });
    }

</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTextoNumeroAsiento" runat="server" Text="Número Asiento " />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroAsiento" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                </div>
            </div>
            <asp:UpdatePanel ID="upEjercicioContable" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Ejercicio:" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" OnSelectedIndexChanged="ddlEjercicioContable_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAsientoDesde" runat="server" Text="Fecha Desde" />
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAsientoDesde" runat="server" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaSeintoHasta" runat="server" Text="Fecha Hasta" />
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAsientoHasta" runat="server" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="form-group row">
                 <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server" />
                        </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaRealizado" runat="server" Text="Fecha Realizado" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaRealizado" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCopiativo" runat="server" Text="Numero Copiativo" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroCopiativo" runat="server" />
                </div>
                <div class="col-sm-4"></div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operación" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRefTipoOperacion" runat="server" Text="Tipo Referencia" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlRefTipoOperacion" runat="server" />
                </div>
                <div class="col-sm-4">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-12">
                    <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdAsientoContable"
                        runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                        <Columns>
                            <asp:BoundField HeaderText="Número Asiento" DataField="NumeroAsiento" ItemStyle-Wrap="false" SortExpression="NumeroAsiento" />
                            <asp:BoundField HeaderText="Detalle" DataField="DetalleGeneral" ItemStyle-Wrap="true" SortExpression="DetalleGeneral" />
                            <asp:BoundField HeaderText="N° Copiativo" DataField="NumeroAsientoCopiativo" ItemStyle-Wrap="false" SortExpression="NumeroAsientoCopiativo" />
                            <asp:BoundField HeaderText="Fecha" DataField="FechaAsiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaAsiento" />
                            <asp:BoundField HeaderText="Debe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataField="TotalDebe" DataFormatString="{0:C2}" />
                            <asp:BoundField HeaderText="Haber" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataField="TotalHaber" DataFormatString="{0:C2}" />
                            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("EstadoDescripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                        AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Evol:EvolGridView>
                </div>
            </div>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" Visible="false" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

