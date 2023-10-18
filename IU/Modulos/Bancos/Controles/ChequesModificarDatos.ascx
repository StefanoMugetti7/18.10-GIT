<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChequesModificarDatos.ascx.cs" Inherits="IU.Modulos.Bancos.Controles.ChequesModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<div class="form-group row">
    <asp:label cssclass="col-sm-1 col-form-label" id="lblFecha" runat="server" text="Fecha"></asp:label>
    <div class="col-sm-3">
        <asp:textbox cssclass="form-control datepicker" id="txtFecha" runat="server"></asp:textbox>

        <asp:requiredfieldvalidator cssclass="Validador" id="rfvFecha" runat="server" controltovalidate="txtFecha"
            errormessage="*" enabled="false" validationgroup="IngresarCheque" />
    </div>

    <asp:label cssclass="col-sm-1 col-form-label" id="lblFechaDiferido" runat="server" text="Fecha Diferido"></asp:label>
    <div class="col-sm-3">
        <asp:textbox cssclass="form-control" id="txtFechaDiferido" runat="server"></asp:textbox>

        <asp:requiredfieldvalidator cssclass="Validador" id="rfvChequeDiferido" runat="server" controltovalidate="txtFechaDiferido"
            errormessage="*" enabled="false" validationgroup="IngresarCheque" />
    </div>

    <asp:label cssclass="col-sm-1 col-form-label" id="lblNumeroCheque" runat="server" text="Numero Cheque"></asp:label>
    <div class="col-sm-3">
        <asp:textbox cssclass="form-control" id="txtNumeroCheque" runat="server"></asp:textbox>
        <asp:requiredfieldvalidator cssclass="Validador" id="rfvNumeroCheque" runat="server" controltovalidate="txtNumeroCheque"
            errormessage="*" enabled="false" validationgroup="IngresarCheque" />
    </div>
</div>

<div class="form-group row">
    <asp:label cssclass="col-sm-1 col-form-label" id="Label1" runat="server" text="Importe" />
    <div class="col-sm-3">
        <auge:currencytextbox cssclass="form-control" id="txtImporte" enabled="false" runat="server" />
    </div>

    <asp:label cssclass="col-sm-1 col-form-label" id="lblConcepto" runat="server" text="Concepto"></asp:label>
    <div class="col-sm-3">
        <asp:textbox cssclass="form-control" id="txtConcepto" runat="server"></asp:textbox>
    </div>

    <asp:label cssclass="col-sm-1 col-form-label" id="lblBanco" runat="server" text="Banco"></asp:label>
    <div class="col-sm-3">
        <asp:dropdownlist cssclass="form-control select2" id="ddlBancos" runat="server">
        </asp:dropdownlist>
        <asp:requiredfieldvalidator cssclass="Validador" id="rfvBancos" runat="server" controltovalidate="ddlBancos"
            errormessage="*" enabled="false" validationgroup="IngresarCheque" />
    </div>
</div>
<div class="form-group row">
    <asp:label cssclass="col-sm-1 col-form-label" id="lblCuit" runat="server" text="CUIT"></asp:label>
    <div class="col-sm-3">
        <auge:numerictextbox cssclass="form-control" id="txtCUIT" maxlength="11" runat="server"></auge:numerictextbox>
        <asp:requiredfieldvalidator cssclass="Validador" id="rfvCUIT" runat="server" controltovalidate="txtCUIT"
            errormessage="*" enabled="false" validationgroup="IngresarCheque" />
    </div>

    <asp:label cssclass="col-sm-1 col-form-label" id="lblTitular" runat="server" text="Titular"></asp:label>
    <div class="col-sm-3">
        <asp:textbox cssclass="form-control" id="txtTitular" runat="server"></asp:textbox>
        <asp:requiredfieldvalidator cssclass="Validador" id="rfvTitular" runat="server" controltovalidate="txtTitular"
            errormessage="*" enabled="false" validationgroup="IngresarCheque" />
    </div>

    <asp:label cssclass="col-sm-1 col-form-label" id="lblEstado" runat="server" text="Estado"></asp:label>
    <div class="col-sm-3">
        <asp:dropdownlist cssclass="form-control select2" id="ddlEstados" runat="server">
        </asp:dropdownlist>
    </div>
</div>
<asp:gridview id="gvDatos" allowpaging="true"
    datakeynames="IndiceColeccion"
    runat="server" skinid="GrillaBasicaFormal" autogeneratecolumns="false" showfooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onrowcommand="gvDatos_RowCommand" onrowdatabound="gvDatos_RowDataBound">
    <columns>
        <asp:boundfield headertext="Fecha" datafield="Fecha" dataformatstring="{0:dd/MM/yyyy HH:mm}" />
        <asp:templatefield headertext="Filial">
            <itemtemplate>
                <%# Eval("Filial.Filial")%>
            </itemtemplate>
        </asp:templatefield>
        <asp:boundfield headertext="Descripcion" datafield="Descripcion" />
        <asp:templatefield headertext="Tipo Operación">
            <itemtemplate>
                <%# Eval("TipoOperacion.TipoOperacion")%>
            </itemtemplate>
        </asp:templatefield>
        <asp:templatefield headertext="Usuario">
            <itemtemplate>
                <%# Eval("UsuarioLogueado.Usuario")%>
            </itemtemplate>
        </asp:templatefield>
        <asp:templatefield headertext="Acciones" headerstyle-horizontalalign="Center">
            <itemtemplate>
                <asp:imagebutton commandargument='<%# Container.DisplayIndex%>' imageurl="~/Imagenes/Ver.png" runat="server" commandname="Consultar" id="btnConsultar"
                    alternatetext="Mostrar" tooltip="Mostrar" />
                <asp:imagebutton commandargument='<%# Container.DisplayIndex%>' imageurl="~/Imagenes/print_f2.png" runat="server" visible="false" commandname="Impresion" id="btnImprimir"
                    alternatetext="Imprimir Comprobante" tooltip="Imprimir Comprobante" />
                <asp:hiddenfield id="hdfIdTipoOperacion" runat="server" value='<%#Eval("TipoOperacion.IdTipoOperacion") %>' />
                <asp:hiddenfield id="hdfIdRefTipoOperacion" runat="server" value='<%#Eval("IdRefTipoOperacion") %>' />
            </itemtemplate>
        </asp:templatefield>
    </columns>
</asp:gridview>
<br />
<br />
<asp:updatepanel id="UpdatePanel1" runat="server" updatemode="Conditional">
    <contenttemplate>
        <center>
            <auge:popupmensajespostback id="popUpMensajes" runat="server" />
            <auge:popupcomprobantes id="ctrPopUpComprobantes" runat="server" />
            <asp:button cssclass="botonesEvol" id="btnAceptar" runat="server" text="Aceptar"
                onclick="btnAceptar_Click" validationgroup="IngresarCheque" />
            <asp:button cssclass="botonesEvol" id="btnCancelar" runat="server" text="Volver" onclick="btnCancelar_Click" />
        </center>
    </contenttemplate>
</asp:updatepanel>
