<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ProveedoresPorcentajesComisionesListar.aspx.cs" Inherits="IU.Modulos.Proveedores.ProveedoresPorcentajesComisionesListar" %>

<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        InitApellidoSelect2();
    });


    function InitApellidoSelect2() {
        var control = $("select[name$='ddlNumeroProveedor']");
        //var lblCUIT = $(this).find("input:text[id*='lblCUIT']");
        control.select2({
            placeholder: 'Ingrese el codigo o Razón Social',
            selectOnClose: true,
            theme: 'bootstrap4',
            minimumInputLength: 1,
            //width: '100%',
            language: 'es',
            //tags: true,
            //allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Proveedores/ProveedoresWS.asmx/ProveedoresCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: control.val(), // search term");
                        filtro: params.term // search term");
                    });
                },
                beforeSend: function (xhr, opts) {
                    var algo = JSON.parse(this.data); // this.data.split('"');
                    //console.log(algo.filtro);
                    if (isNaN(algo.filtro)) {
                        if (algo.filtro.length < 4) {
                            xhr.abort();
                        }
                    }
                    else {
                    }
                },
                processResults: function (data, params) {
                    //return { results: data.items };
                    return {
                        results: $.map(data.d, function (item) {
                            return {

                                id: item.IdProveedor,
                                text: item.CodigoProveedor,
                                Cuit: item.TipoDocumentoDescripcion,
                                NumeroDocumento: item.NumeroDocumento,
                                RazonSocial: item.RazonSocial,
                                IdCondicionFiscal: item.IdCondicionFiscal,
                                Estado: item.EstadoDescripcion,
                                Beneficiario: item.Beneficiario


                                //Apellido: item.Apellido,
                                //Nombre: item.Nombre,
                                //IdTipoDocumento: item.IdTipoDocumento,
                                //TipoDocumento: item.DescripcionTipoDocumento,
                                //NumeroDocumento: item.NumeroDocumento,
                                //IdAfiliadoTipo: item.IdAfiliadoTipo,
                                //IdCondicionFiscal: item.IdCondicionFiscal,
                                //CondicionFiscalDescripcion: item.CondicionFiscalDescripcion
                            }
                        })
                    };
                    cache: true
                }
            }
        });

        control.on('select2:select', function (e) {
            $("input[id*='hdfIdProveedor']").val(e.params.data.id);//.trigger("change");
            $("input[id*='hdfNumeroProveedor']").val(e.params.data.text);

        });

        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
            $("input[id*='hdfIdProveedor']").val('');//.trigger("change")
            $("input[id*='hdfNumeroProveedor']").val('');
        });

    }

</script>

<div class="ProveedoresPorcentajesComisionesListar">
<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroProveedor" runat="server" Text="Proveedor"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroProveedor" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvNumeroProveedor" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroProveedor" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdProveedor" runat="server" />
                        <asp:HiddenField ID="hdfNumeroProveedor" runat="server" />

                    </div>
           
        

        <asp:Label CssClass="col-form-label col-sm-1" ID="lblTipoOperacion"  runat="server" Text="Tipo Operacion"></asp:Label>
         <div class="col-sm-3">    <asp:DropDownList CssClass="form-control select2" ID="ddlTiposOperaciones"  runat="server">
        </asp:DropDownList>
             </div>  
                    
                    <div class="col-sm-3">
        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
        <div class="Espacio"></div>
        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
        onclick="btnAgregar_Click" />
   
                 </div>
                         </div>
            <div class="form-group row">
                 <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma de Cobro" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro" runat="server"  />
         
            </div>
            </div>
        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                        runat="server" onclick="btnExportarExcel_Click" Visible="false" />
    <br />
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Número Proveedor" SortExpression="Proveedor.IdProveedor">
                    <ItemTemplate>
                        <%# Eval("Proveedor.IdProveedor")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Razón Social" SortExpression="Proveedor.RazonSocial">
                    <ItemTemplate>
                        <%# Eval("Proveedor.RazonSocial")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CUIT" SortExpression="Proveedor.CUIT">
                <ItemTemplate>
                    <%# Eval("Proveedor.CUIT")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacion.TipoOperacion">
                <ItemTemplate>
                    <%# Eval("TipoOperacion.TipoOperacion")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%--      <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacion.TipoOperacion">
                <ItemTemplate>
                    <%# Eval("FormaCobro.FormaCobro")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="Fecha Inicio desde" SortExpression="FechaInicioVigencia">
                <ItemTemplate>
                    <%# Eval("FechaInicioVigencia", "{0:dd/MM/yyyy}")%>
                </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField HeaderText="Forma Cobro" SortExpression="FormaCobro.FormaCobro">
                <ItemTemplate>
                    <%# Eval("FormaCobro.FormaCobro")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Porcentaje" SortExpression="Porcentaje" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%# Eval("Porcentaje", "{0:N4}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("Estado.Descripcion")%>
                </ItemTemplate>
            </asp:TemplateField>            
             <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Baja" ID="btnBaja" 
                                AlternateText="Baja de Comision" ToolTip="Baja de Comision"  Visible="false"/>
                      <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                        AlternateText="Consultar" ToolTip="Consultar" />
                </ItemTemplate>
             </asp:TemplateField>
            </Columns>
    </asp:GridView>
    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>
</div>
</asp:Content>


