<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegUsuariosNotificacionesModificarPopUp.ascx.cs" Inherits="IU.Modulos.Seguridad.Controles.SegUsuariosNotificacionesModificarPopUp" %>

<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        IniciarAfiliadosWS();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CambiarImagen);
        CambiarImagen();
    });

    function CambiarImagen() {
        var notificaciones = $("input:hidden[id$='hdfNotificaciones']").val();
        if (notificaciones == "1") {
            $("#imgNotificaciones").attr("src", "<%=ResolveClientUrl("~")%>/Imagenes/campana-noti.png");
        } else {
            $("#imgNotificaciones").attr("src", "<%=ResolveClientUrl("~")%>/Imagenes/campana.png");
        }
    }

    function IniciarAfiliadosWS() {
        $.ajax({
            url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/IniciarWS',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            success: function (response) {
                //do whatever your thingy..
            }
        });
        }
    
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

    function ShowModalNotificaciones() {
        $("[id$='modalNotificaciones']").modal('show');
        $("input[id$='btnProcesar']").click(function () {
            var incluidos = ObtenerChecked();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/NotificacionesGuardar',
                async: true,
                data: JSON.stringify({ 'filtro': incluidos }),
                beforeSend: function (xhr, opts) {
                    if (incluidos.length < 1) {
                        xhr.abort();
                        HideModalNotificaciones();
                    }
                },
                success: function (msg) {
                    HideModalNotificaciones();
                    $("input[id$='btnRecargarNotificaciones']").click();
                }
            });
        }); 
    }

    function HideModalNotificaciones() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalNotificaciones']").modal('hide');
    }

    function ObtenerChecked() {
        //var contadorRegistros = 0;
        var incluidos = [];
        var pendiente = 0;
        //incluidos.val("");
        $('#<%=gvDatos.ClientID%> tr').not(':first').each(function () {
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var idNotificacion = $(this).find('input:hidden[id*="hdfIdNotificacion"]').val();
            if (incluir) {
                incluidos.push(idNotificacion);
            } else {
                pendiente = 1;
            }
        });
        if (pendiente == 0) {
            $("input:hidden[id$='hdfNotificaciones']").val("0");
        }
        return incluidos.join(',');
    }

</script>
 <style>
      img.pointer {
        cursor: pointer;
      }
    </style>
<asp:HiddenField ID="hdfNotificaciones" runat="server" />
<img id="imgNotificaciones" onclick="ShowModalNotificaciones();" class="pointer" alt="Notificaciones" style="border:none" />
<div class="modal" id="modalNotificaciones" tabindex="-1" role="dialog">
         <div class="modal-dialog modal-dialog-scrollable modal-lg" role="document">
        <div class="modal-content modal-minHeight85">
                    <div class="modal-header">
                        <h5 class="modal-title">Notificaciones</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="upModalNotificaciones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                         <div class="form-group row">
                            <div class="col-sm-12">
                            <asp:GridView ID="gvDatos" DataKeyNames="IdNotificacion"
                                runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <%# Eval("Fecha", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Descripcion")%>
                                            <asp:HiddenField ID="hdfIdNotificacion" Value='<%# Eval("IdNotificacion")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leidos">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkIncluirTodos" runat="server" onclick="checkAllRow(this);" Visible="true" Text="Leidos" TextAlign="Left" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIncluir" onclick="CheckRow(this);" Visible="true" Checked="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Label ID="lblMensaje" runat="server" Text="No se encontraron mensajes"></asp:Label>
                        </div>
                            </div>
                             </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <input type="button" runat="server" class="botonesEvol" id="btnProcesar" value="Aceptar" />
                        <input type="button" runat="server" class="botonesEvol" id="Volver" value="Volver" data-dismiss="modal"/>
                        <asp:Button CssClass="botonesEvol" ID="btnRecargarNotificaciones" runat="server" Text="Recargar Notificaciones" OnClick="btnRecargarNotificaciones_Click" Style="display: none" />

                    </div>
                           
                </div>
            </div>
</div>
