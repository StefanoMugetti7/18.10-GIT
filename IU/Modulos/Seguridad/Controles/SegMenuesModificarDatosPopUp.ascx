<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegMenuesModificarDatosPopUp.ascx.cs" Inherits="IU.Modulos.Seguridad.Controles.SegMenuesModificarDatosPopUp" %>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <div class="form-group row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblMenu" runat="server" Text="Menu"></asp:Label>
                    <div class="col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtMenu" runat="server"></asp:TextBox>
                    </div>
                    </div>
                <div class="form-group row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="Label4" runat="server" Text="URL"></asp:Label>
                <div class="col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtURL" runat="server"></asp:TextBox>
                    </div>
               </div>
                <div class="form-group row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="Label5" runat="server" Text="Nro. Orden"></asp:Label>
                <div class="col-sm-9">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtOrden" runat="server"></auge:NumericTextBox>
                </div>
                </div>
                <div class="form-group row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblMenuPadre" runat="server" Text="Menu Padre"></asp:Label>
                <div class="col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlMenuPadre" runat="server">
                </asp:DropDownList>
                    </div>
               </div>
                <div class="form-group row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="Label3" runat="server" Text="Tiene hijos"></asp:Label>
                <div class="col-sm-9">
                    <asp:CheckBox ID="chkHijos" Enabled="false" runat="server" />
                </div>
                </div>
                <div class="form-group row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="Label2" runat="server" Text="Mostrar Menu en Pantalla"></asp:Label>
                <div class="col-sm-9">
                    <asp:CheckBox ID="chkMostrar" runat="server" />
                </div>
                </div>
                <div class="form-group row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="Label1" runat="server" Text="Baja logica"></asp:Label>
                <div class="col-sm-9">
                    <asp:CheckBox ID="chkBajaLogica" runat="server" />
                </div>
                </div>
                <div class="form-group row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="Label6" runat="server" Text="Funcionalidad"></asp:Label>
                <div class="col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFuncionalidades" runat="server">
                </asp:DropDownList>
                </div>
                </div>

                         <asp:UpdatePanel ID="upGrillaControles" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>                        
                        <asp:Panel ID="pnlGrillaControles" runat="server">
                            <div class="card">
                        <div class="card-header">
                            Controles No Visibles
                        </div>
                        <div class="card-body">
                            <div class="form-group row" runat="server" id="dvAgregarControl">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblControlOcultar" runat="server" Text='Control' ></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control" ID="txtControl" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvControl" ControlToValidate="txtControl" ValidationGroup="Control" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-3">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarGrilla" runat="server" Text="Agregar Control" ValidationGroup="Control" OnClick="btnAgregarGrilla_Click" />
                                    </div>
                                <div class="col-sm-3"></div>
                                    </div>
                            <div class="form-group row">
                                <div class="col-sm-12">
                            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="Control"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                >
                                <Columns>
                                     <asp:TemplateField HeaderText="Control">
                                                    <ItemTemplate>
                                                        <%# Eval("Control")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="5%" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                                            AlternateText="Eliminar" ToolTip="Eliminar Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                 </asp:UpdatePanel>
                <center>
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                        onclick="btnAceptar_Click" ValidationGroup="MenuPopUp" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar"  runat="server" Text="Volver" 
                        onclick="btnCancelar_Click" CausesValidation="false" />
                    </center>
        </ContentTemplate>
    </asp:UpdatePanel>
