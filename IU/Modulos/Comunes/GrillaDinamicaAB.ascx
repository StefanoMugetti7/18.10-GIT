<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GrillaDinamicaAB.ascx.cs" Inherits="IU.Modulos.Comunes.GrillaDinamicaAB" %>
<asp:PlaceHolder ID="phAlta" runat="server">
  <script type="text/javascript">  
    function InitCombo(sp, ph) {
        var control = $("select[name$='ddlCombo']");
        control.select2({
            placeholder: ph,
            selectOnClose: true,
            theme: 'bootstrap4',
            minimumInputLength: 4,
            width: '100%',
            language: 'es',
            tags: true,
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica',
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        sp: sp,
                        value: control.val(), // search term");
                        filtro: params.term // search term");
                    });
                },
                processResults: function (data, params) {
                    //return { results: data.items };
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                text: item.text,
                                id: item.id
                            }
                        })
                    };
                    cache: true
                }
            }
        });

        control.on('select2:select', function (e) {
            var newOption = new Option(e.params.data.text, e.params.data.id, false, true);
            $("select[id$='ddlCombo']").append(newOption).trigger('change');
        });

        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
        });
    }
</script>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblComboEtiqueta" runat="server" Text=""></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlCombo" runat="server">
        </asp:DropDownList>
        <div class="col-sm-4">
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
            onclick="btnAgregar_Click" />
        </div>
    </div>
    </div>
</asp:PlaceHolder>
<div class="form-group row">
<div class="table-responsive">
        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" SkinID="GrillaResponsive" 
            OnRowDataBound="gvDatos_RowDataBound" ShowHeaderWhenEmpty="true"
            runat="server" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
        </asp:GridView>
    </div>
</div>
