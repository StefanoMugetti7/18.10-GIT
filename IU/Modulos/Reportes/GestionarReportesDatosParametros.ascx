<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GestionarReportesDatosParametros.ascx.cs" Inherits="IU.Modulos.Reportes.GestionarReportesDatosParametros" %>








<div class="modal" id="modalPopUpGestionarReportesDatosParametros" tabindex="-1" role="dialog">
<%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <div class="modal-dialog modal-dialog-scrollable modal-md" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Parámetros</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblParametro" runat="server" Text="Parametro (sin @)"></asp:Label>

                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control" ID="txtParametro" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" runat="server" ErrorMessage="*" ControlToValidate="txtParametro" ValidationGroup="GestionarReportesDatosParametros"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblOrden" runat="server" Text="Orden"></asp:Label>

                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control" ID="txtOrden" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblNombreParametro" runat="server" Text="Nombre Parametro"></asp:Label>

                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control" ID="txtNombreParametro" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombreParametro" runat="server" ErrorMessage="*" ControlToValidate="txtNombreParametro" ValidationGroup="GestionarReportesDatosParametros"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblTipoParametro" runat="server" Text="Tipo de Parametro"></asp:Label>

                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoParametroIdTipoParametro" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="Label1" runat="server" Text="Parametro Dependiente"></asp:Label>

                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control" ID="txtParamDependiente" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="Label2" runat="server" Text="Stored Procedure Parametros"></asp:Label>

                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control" ID="txtStoredProcedure" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Button CssClass="botonesEvol" ID="btnGrabar" runat="server" Text="Aceptar"
                                    ValidationGroup="GestionarReportesDatosParametros"
                                    OnClick="btnGrabar_Click" />
                                &nbsp;&nbsp;
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver"
                    CausesValidation="false" OnClick="btnCancelar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    <%--    </ContentTemplate>
        </asp:UpdatePanel>--%>
</div>
