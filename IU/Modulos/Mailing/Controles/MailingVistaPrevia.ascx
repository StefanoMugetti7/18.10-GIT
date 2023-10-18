<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailingVistaPrevia.ascx.cs" Inherits="IU.Modulos.Mailing.Controles.MailingVistaPrevia" %>

<script type="text/javascript" lang="javascript">

    function ShowModalVistaPrevia()
    {
        $("[id$='modalVistaPrevia']").modal('show')
    }

    //function HideModaVistaPrevia() {
    //    $('body').removeClass('modal-open');
    //    $('.modal-backdrop').remove();
    //    $("[id$='modalVistaPrevia']").modal('hide');
    //}

</script>


<div class="modal" id="modalVistaPrevia" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Vista previa</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="form-group row">
                    <div class="col-sm-1"></div>
                    <asp:Label CssClass="col-form-label col-sm-11" Text="" ID="txtDe" runat="server" />
                </div>
                <div class="form-group row">
                    <div class="col-sm-1"></div>
                    <asp:Label CssClass="col-form-label col-sm-11" Text="" ID="txtPara" runat="server" />
                </div>
                <div class="form-group row">
                    <div class="col-sm-1"></div>
                    <asp:Label CssClass="col-form-label col-sm-11" Text="" ID="txtAsunto" runat="server" />
                </div>
                <div class="w100"></div>
                <div class="row">
                    <div class="col-sm-1"></div>
                    <div class="col-sm-11">
                        <asp:Literal ID="ltrDatos" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <%--<div class="modal-footer">
                        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" data-dismiss="modal" />
            </div>--%>
        </div>
    </div>
</div>
