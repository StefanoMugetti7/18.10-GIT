<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="AfiliadosEstadoCuenta.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadosEstadoCuenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
            <div class="card-header" id="TituloCuotasSociales">
        CUOTAS SOCIALES
    </div>
     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvCuotasSociales" OnRowCommand="gvCuotasSociales_RowCommand"
                OnRowDataBound="gvCuotasSociales_RowDataBound"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Cuota Mes">
                        <ItemTemplate>
                            <%# Eval("CuotaMes")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vencimiento">
                        <ItemTemplate>
                            <%# Eval("FechaVencimiento", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe">
                        <ItemTemplate>
                            <%# string.Concat("$", Eval("Importe", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Cuotas Adeud." ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat("$", Eval("ImporteDeuda", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pago">
                        <ItemTemplate>
                            <%# Eval("Abreviatura")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Metodo de Pago">
                        <ItemTemplate>
                            <%# Eval("FormaCobro")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
             <div class="col-sm-12">
                    <hr widht="20%" />
                </div>
        <div class="card-header" id="TituloCuotasCentroMedico">
        CUOTAS CENTRO MEDICO
    </div>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvCuotasCentroMedico" OnRowCommand="gvCuotasCentroMedico_RowCommand" 
                OnRowDataBound="gvCuotasCentroMedico_RowDataBound"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Cuota Mes">
                        <ItemTemplate>
                            <%# Eval("CuotaMes")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Vencimiento">
                        <ItemTemplate>
                            <%# Eval("FechaVencimiento", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <%# Eval("Estado")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat(Eval("Moneda"), Eval("Importe", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Punitorio" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat(Eval("Moneda"), Eval("ImportePunitorio", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat(Eval("Moneda"), Eval("ImporteDeuda", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pago">
                        <ItemTemplate>
                            <%# Eval("Abreviatura")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>






             <div class="col-sm-12">
                    <hr widht="20%" />
                </div>
        <div class="card-header" id="TituloAyudasEconomicas">
        AYUDAS ECONOMICAS
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvAyudasEconomicas" OnRowCommand="gvAyudasEconomicas_RowCommand"
                OnRowDataBound="gvAyudasEconomicas_RowDataBound"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>

                    <asp:TemplateField HeaderText="Descripcion">
                        <ItemTemplate>
                            <%# Eval("PrestamoPlanDescripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ayuda N°">
                        <ItemTemplate>
                            <%# Eval("IdPrestamo")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="C. Total">
                        <ItemTemplate>
                            <%# Eval("CantidadCuotas")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imp. Cuota" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat(Eval("moneda"), Eval("ImporteCuota", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat(Eval("moneda"), Eval("ImporteSolicitado", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P">
                        <ItemTemplate>
                            <%# Eval("Abreviatura")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Deuda Cant.">
                        <ItemTemplate>
                            <%# Eval("CantidadCuotasDeuda")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe Deuda" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat(Eval("moneda"), Eval("Deuda", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Intereses" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat(Eval("moneda"), Eval("Interes", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Forma Cobro">
                        <ItemTemplate>
                            <%# Eval("FormaCobroAfiliadoFormaCobroFormaCobro")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Referencia">
                        <ItemTemplate>
                            <%# Eval("NroDeIdentificacion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
           <div class="col-sm-12">
                    <hr widht="20%" />
                </div>
        <div class="card-header" id="TituloCodeudor">
        CODEUDOR
    </div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvCodeudor" OnRowCommand="gvCodeudor_RowCommand" 
                OnRowDataBound="gvCodeudor_RowDataBound"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Socio Codeudor">
                        <ItemTemplate>
                            <%# Eval("ApellidoNombre")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ayuda N°">
                        <ItemTemplate>
                            <%# Eval("IdPrestamo")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <%# Eval("Estado")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cuota" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat("$", Eval("ImporteCuota", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe Deuda"  ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat("$", Eval("Deuda", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
                 <div class="col-sm-12">
                    <hr widht="20%" />
                </div>
            <div class="card-header" id="TituloAhorro">
        AHORROS COMUN Y A TERMINO / PUNTOS
    </div>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvAhorros" OnRowCommand="gvAhorros_RowCommand"
                OnRowDataBound="gvAhorros_RowDataBound"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Descripcion">
                        <ItemTemplate>
                            <%# Eval("Denominacion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Numero">
                        <ItemTemplate>
                            <%# Eval("Numero")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat("$", Eval("Importe", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Vencimiento">
                        <ItemTemplate>
                            <%# Eval("FechaVencimiento", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Intereses" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat("$", Eval("ImporteInteres", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total"  ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat("$", Eval("ImporteTotal", "{0:N2}"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
           <div class="col-sm-12">
                    <hr widht="20%" />
                </div>
</asp:Content>

