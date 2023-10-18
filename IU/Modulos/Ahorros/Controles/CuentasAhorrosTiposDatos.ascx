<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CuentasAhorrosTiposDatos.ascx.cs" Inherits="IU.Modulos.Ahorros.Controles.CuentasAhorrosTiposDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);

        intiGridDetalle();
    });

    function intiGridDetalle() {
        var rowindex = 0;
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlCuentaContable = $(this).find('[id*="ddlCuentaContable"]');
            var hdfIdCuentaContable = $(this).find("input[id*='hdfIdCuentaContable']");
            var hdfCuentaContable = $(this).find("input[id*='hdfCuentaContable']");
            //if (hdfIdCuentaContable.val() > 0) {
            //    var newOption = new Option(hdfCuentaContableDetalle.val(), hdfIdCuentaContable.val(), false, true);
            //    ddlCuentaContable.append(newOption).trigger('change');
            //}
            ddlCuentaContable.select2({
                placeholder: 'Ingrese el numero o nombre de la cuenta',
                selectOnClose: true,
                theme: 'bootstrap4',
                width: '100%',
                //theme: 'bootstrap',
                minimumInputLength: 1,
                language: 'es',
                //tags: true,
                allowClear: true,
                ajax: {
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Contabilidad/ContabilidadWS.asmx/ContabilidadSeleccionarAjaxComboCuentas', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                      delay: 500,
                      data: function (params) {
                          return JSON.stringify({
                              value: ddlCuentaContable.val(), // search term");
                              filtro: params.term,
                              idEjercicioContable: '0'
                          });
                          //var Productos = ObtenerProductosSeleccionadas();
                          //console.log(" array " + Productos);
                          //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
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
                                      text: item.text,
                                      id: item.id,
                                      numeroCuenta: item.numeroCuenta,
                                      descripcion: item.text,
                                      //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                                  }
                              })
                          };
                          cache: true
                      }
                  },
              });
            ddlCuentaContable.on('select2:select', function (e) {
                hdfCuentaContable.val(e.params.data.descripcion);
                hdfIdCuentaContable.val(e.params.data.id);
            });
            ddlCuentaContable.on('select2:unselect', function (e) {
                hdfCuentaContable.val('');
                hdfCuentaContableDetalle.val('');
            });
            rowindex++;
        });
    }
</script>

<div class="CuentasTiposModificarDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigo" runat="server" Text="Codigo BNRA"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtCodigo" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigo" ControlToValidate="txtCodigo" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCuentaTipo" runat="server" Text="Cuenta Tipo"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtCuentaTipo" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvtxtCuentaTipo" ControlToValidate="txtCuentaTipo" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
        </div>
    </div>
    <br />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpConfiguracionCuentasContables" HeaderText="Configuracion de Cuentas Contables">
            <ContentTemplate>
                <asp:UpdatePanel ID="items" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregar" runat="server" Text="Cantidad"></asp:Label>
                            <div class="col-sm-1">
                                <asp:TextBox CssClass="form-control" ID="txtCantidadAgregar" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvDatos" DataKeyNames="IdCuentaTipoCuentaContable" AllowPaging="false" AllowSorting="false"
                                runat="server" SkinID="GrillaResponsive"
                                AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvDatos_RowDataBound" OnRowCommand="gvDatos_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Cuenta Contable">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlCuentaContable" runat="server"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfIdCuentaContable" Value='<%#Eval("IdCuentaContable") %>' runat="server" />
                                            <asp:HiddenField ID="hdfCuentaContable" Value='<%#Eval("CuentaContableDescripcion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Moneda">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" SortExpression="">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                                AlternateText="Elminiar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
