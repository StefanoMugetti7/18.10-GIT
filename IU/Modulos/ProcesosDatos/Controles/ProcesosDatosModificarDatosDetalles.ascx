<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProcesosDatosModificarDatosDetalles.ascx.cs" Inherits="IU.Modulos.ProcesosDatos.Controles.ProcesosDatosModificarDatosDetalles" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        SetTabIndexInput();
    });
    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
</script>

<div class="Datos">
    <asp:UpdatePanel ID="upDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProcesos" runat="server" Text="Procesos"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFiltro" runat="server" Enabled="false"></asp:DropDownList>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigo" Visible="true" runat="server" Text="Codigo"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCodigo" Enabled="false" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="card-header">
                PARAMETROS UTILIZADOS
            </div>
            <Evol:EvolGridView ID="gvDatosLote" AllowPaging="false"
                OnRowDataBound="gvDatosLote_RowDataBound" DataKeyNames="IdProcesoProcesamientoParametroValor"
                runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="false"
                Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="Tipo">
                        <ItemTemplate>
                            <%# Eval("Tipo")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre">
                        <ItemTemplate>
                            <%# Eval("NombreParametro")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor">
                        <ItemTemplate>
                            <%# Eval("Valor")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>