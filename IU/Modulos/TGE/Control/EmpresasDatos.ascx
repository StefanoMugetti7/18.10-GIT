<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpresasDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.EmpresasDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>



<script type="text/javascript">

    function UpdPanelUpdate() {

        __doPostBack("<%= button.ClientID %>", "");

    }
</script>




<div class="EmpresasDatos">
    <div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEmpresa"  runat="server" Text="Empresa" />
    <div class="col-lg-3 col-md-3 col-sm-9"><asp:TextBox CssClass="form-control" ID="txtEmpresa"  runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEmpresa" runat="server" ControlToValidate="txtEmpresa" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
  </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion"  runat="server" Text="Descripcion" />
  <div class="col-lg-3 col-md-3 col-sm-9">  <asp:TextBox CssClass="form-control" ID="txtDescripcion"  runat="server" TextMode="MultiLine"/>
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" runat="server" ControlToValidate="txtDescripcion" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
  </div></div>
    
    <asp:Panel ID="pnlDireccion" GroupingText="Datos Empresa" runat="server">
    <div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCalle"  runat="server" Text="Calle" />
   <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtCalle"  runat="server" />
   </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumero"  runat="server" Text="Numero" />
   <div class="col-lg-3 col-md-3 col-sm-9"> <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero"  runat="server" />
</div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPiso"  runat="server" Text="Piso" />
    <div class="col-lg-3 col-md-3 col-sm-9"><AUGE:NumericTextBox CssClass="form-control" ID="txtPiso"  runat="server" />
    </div>
  </div><div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblOficina"  runat="server" Text="Oficina" />
 <div class="col-lg-3 col-md-3 col-sm-9">   <asp:TextBox CssClass="form-control" ID="txtOficina"  runat="server" />
  </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTelefono"  runat="server" Text="Telefono" />
   <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtTelefono"  runat="server" />
 </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigoPostal"  runat="server" Text="Codigo Postal" />
  <div class="col-lg-3 col-md-3 col-sm-9">  <asp:TextBox CssClass="form-control" ID="txtCodigoPostal" MaxLength="8" runat="server" />
 </div>

 </div><div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCUIT"  runat="server" Text="CUIT" />
   <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtCUIT"  runat="server" />
  </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumeroIIBB"  runat="server" Text="Numero IIBB" />
   <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtNumeroIIBB"  runat="server" />
   </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblMatriculaINAES"  runat="server" Text="Matricula INAES" />
    <div class="col-lg-3 col-md-3 col-sm-9"><asp:TextBox CssClass="form-control" ID="txtMatriculaINAES"  runat="server" />
    </div>

   </div><div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFechaInicioActividad" runat="server" Text="Inicio Actividad"></asp:Label>
   <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control datepicker" ID="txtFechaInicioActividad" runat="server"></asp:TextBox>
  </div>  <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCondicionFiscal"  runat="server" Text="Condicion Fiscal" />
    <div class="col-lg-3 col-md-3 col-sm-9"><asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal"  runat="server" />
 </div>
  </div>
        <div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblLogo" runat="server" Visible="true" Text="Logo - Hasta 200k. 300px X 125px Maximo"></asp:Label>
       
        <asp:AsyncFileUpload ID="afuLogo" Visible="true" OnClientUploadComplete="UpdPanelUpdate" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField"  runat="server" 
        UploadingBackColor="#CCFFFF" ThrobberID="imgUploadFile" UploaderStyle="Traditional" />
        <asp:ImageButton ID="imgUploadFile" Visible="false" ImageUrl="~/Imagenes/updateprogress.gif" runat="server" />
            </div> <div class="form-group row">
        <asp:UpdatePanel ID="upImagen" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
         <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblImagen" runat="server" Visible="true" Text="Muestra Logo:"></asp:Label>
         <asp:Image ID="imgLogo" runat="server" Visible = "true"/>
         </ContentTemplate>
    </asp:UpdatePanel>
        <asp:Button CssClass="botonesEvol" ID="button" runat="server" OnClick="button_Click" style="display:none;"/>
   </div>
        <div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCantidadUsuarios"  runat="server" Text="Cant. Usuarios" />
  <div class="col-lg-3 col-md-3 col-sm-9">  <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadUsuarios"  runat="server" />
 </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCantidadFiliales"  runat="server" Text="Cant. Filiales" />
    <div class="col-lg-3 col-md-3 col-sm-9"><AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadFiliales"  runat="server" />
 </div></div>
    
    </asp:Panel>
    
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
        <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios" >
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria" >
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

<br />

 <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>