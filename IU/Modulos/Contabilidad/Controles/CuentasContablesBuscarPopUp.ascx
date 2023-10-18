<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CuentasContablesBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.CuentasContablesBuscarPopUp" %>

<script lang="javascript" type="text/javascript">
    function InitControl() {
        $("[id$='modalBuscarCuentaContable']").modal('show');

        $("input:text[id$='txtBuscar']").keypress(function (event) {
            var key = (event.keyCode ? event.keyCode : event.which); //e.which;
            if (key == 13) // the enter key code
            {
                event.preventDefault();
                $('#btnBuscarCuentas').click();
                return false;
            }
        });
    }

    function HideModalBuscarCuentaContable() {
        $("[id$='modalBuscarCuentaContable']").modal('hide');
    }


    function BuscarCuentasContables() {
        var tree, onSuccessFunc, idEjer, desc;
        idEjer = $("select[id$='ddlEjercicioContable']").val();
        desc = $("input:text[id$='txtBuscar']").val();

        onSuccessFunc = function (response) {
            //you can modify the response here if needed
            tree.render(response.d);
        };
        tree = $("[id$='tvCuentasContables']").tree({
            uiLibrary: 'bootstrap4',
            dataSource: { url: '<%=ResolveClientUrl("~")%>Modulos/Contabilidad/ContabilidadWS.asmx/CuentasContablesArmarArbol2', 
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                success: onSuccessFunc,
            },
            primaryKey: 'id',
            textField: 'text',
            imageUrlField: 'flagUrl',
            autoLoad: false,
            select: function (e, node, id) {
                var record = tree.getDataById(id);
                if (record.imputable) {
                    //popupBuscarCuentaContable(id);
                    $("[id$='modalBuscarCuentaContable']").modal('hide');
                    var btn = $("[id$='btnSeleccionar']");
                    var hdf = $("input[id$='hdfPopUpCtaCbleBuscarIdCuentacontable']");
                    hdf.val(id);
                    btn.click();
                }
                else
                    tree.expand(node);
            }
        });
        tree.reload({ idEjercicio: idEjer, descripcion: desc });
    }
</script>
<div class="modal" id="modalBuscarCuentaContable" runat="server" tabindex="-1" role="dialog" >
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
    <div class="modal-content modal-minHeight85 ">
        <div class="modal-header">
        <h5 class="modal-title">Buscar Cuenta Contable</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
        <div class="modal-body">
            <div class="form-group row">
            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblEjercicioContable" runat="server" Text="Seleccione el ejercicio contable:" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server" ></asp:DropDownList>
            </div>
        <asp:Label CssClass="col-sm-2 col-form-label" ID="lblBuscar" runat="server" Text="Filtrar Cuentas" />
        <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtBuscar" runat="server" />
        </div>
        <div class="col-sm-2">
            <button class="botonesEvol" id="btnBuscarCuentas" type="button" onclick="BuscarCuentasContables()">Filtrar</button>
        </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-12">
                <asp:Button ID="btnSeleccionar" OnClick="btnSeleccionar_Click" style="visibility:hidden" runat="server" Text="" />
                <asp:HiddenField ID="hdfPopUpCtaCbleBuscarIdCuentacontable" runat="server" />
                <div id="tvCuentasContables" ></div>
                    </div>
                </div>
        </div>
    </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
<script type="text/javascript" lang="javascript">

</script>