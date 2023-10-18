<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GestionarReportesDatos.ascx.cs" Inherits="IU.Modulos.Reportes.GestionarReportesDatos" %>
<%@ Register Src="~/Modulos/Reportes/GestionarReportesDatosParametros.ascx" TagPrefix="AUGE" TagName="ReportesDatosParametros" %>

<div class="GestionarReportesDatos">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>   
    <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label4" runat="server" Text="Modulo Sistema"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9"> 
                <asp:DropDownList CssClass="form-control select2" ID="ddlModulosSistemaIdModulosSistema" runat="server"> </asp:DropDownList>

            </div>
        </div>

            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label1" runat="server" Text="Descripcion"></asp:Label>
             <div class="col-lg-3 col-md-3 col-sm-9">   <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
</div>
           <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9"> 
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstadoIdEstado" runat="server">
                </asp:DropDownList>
</div></div>     <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Detalle" runat="server" Text="Detalle"></asp:Label>
              <div class="col-lg-7 col-md-9 col-sm-11">   <asp:TextBox CssClass="form-control" ID="txtDetalle" TextMode="MultiLine" runat="server"></asp:TextBox>
</div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblStored" runat="server" Text="Procedimiento Almacenado"></asp:Label>
       <div class="col-lg-3 col-md-3 col-sm-9">         <asp:TextBox CssClass="form-control" ID="txtStoredProcedure" runat="server" ></asp:TextBox>
           </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNombreCrystal" runat="server" Text="Nombre Reporte"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">        <asp:TextBox CssClass="form-control" ID="txtNombreCrystal" runat="server" ></asp:TextBox>
</div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNombreArchivo" runat="server" Text="Nombre Archivo"></asp:Label>
           <div class="col-lg-3 col-md-3 col-sm-9">     <asp:TextBox CssClass="form-control" ID="txtNombreArchivo" runat="server" ></asp:TextBox>
       </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNombreSolapa" runat="server" Text="Nombre Solapa"></asp:Label>
             <div class="col-lg-3 col-md-3 col-sm-9">   <asp:TextBox CssClass="form-control" ID="txtNombreSolapa" runat="server" ></asp:TextBox>
</div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label2" runat="server" Text="Exportar Excel"></asp:Label>
              <div class="col-lg-3 col-md-3 col-sm-9">  <asp:CheckBox ID="chkExcel" runat="server" CssClass="form-control"  />
       </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label5" runat="server" Text="Exportar TXT"></asp:Label>
             <div class="col-lg-3 col-md-3 col-sm-9">   <asp:CheckBox ID="chkTexto" runat="server" CssClass="form-control"/>
      </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label6" runat="server" Text="Exportar DBF"></asp:Label>
             <div class="col-lg-3 col-md-3 col-sm-9">   <asp:CheckBox ID="chkDBF" runat="server" CssClass="form-control"/>
         </div></div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label7" runat="server" Text="Incluir Nombre de Campos"></asp:Label>
              <div class="col-lg-3 col-md-3 col-sm-9">  <asp:CheckBox ID="chkIncluirNombreCampos" runat="server" CssClass="form-control"/>
          </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label8" runat="server" Text="Incluir Separador"></asp:Label>
               <div class="col-lg-3 col-md-3 col-sm-9"> <asp:CheckBox ID="chkSeparador" runat="server" CssClass="form-control"/>
          </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label9" runat="server" Text="Separador"></asp:Label>
               <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtSeparador" runat="server" ></asp:TextBox>
                   </div>
  </div> <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label3" runat="server" Text="Seguridad"></asp:Label>
             <div class="col-lg-3 col-md-3 col-sm-9">  <asp:CheckBox ID="chkSeguridad" runat="server" CssClass="form-control" />
</div>
      <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPlantilla" runat="server" Text="Plantilla"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9"> 
                <asp:DropDownList CssClass="form-control select2" ID="ddlPlantilla" runat="server">
                </asp:DropDownList>
         </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCortePDF" runat="server" Text="Corte Control PDF"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">        <asp:TextBox CssClass="form-control" ID="txtCortePDF" runat="server" ></asp:TextBox>
</div>
         </div>
               <asp:Panel ID="pnlParametros" GroupingText="Parametros" runat="server">
                    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
                            CausesValidation="false" onclick="btnAgregar_Click" />
                  
                    <AUGE:ReportesDatosParametros ID="RepDatosParametros" runat="server" />
                    <asp:GridView ID="gvParametros" runat="server" AutoGenerateColumns="false"  
                        DataKeyNames="IndiceColeccion" onrowcommand="gvParametros_RowCommand" 
                               SkinID="GrillaBasicaFormal" >
                        <Columns>
                            <asp:BoundField  HeaderText="Parametro" DataField="Parametro" />
                            <asp:BoundField  HeaderText="Orden" DataField="Orden" />
                            <asp:BoundField  HeaderText="Nombre Parametro" DataField="NombreParametro" />
                            <asp:TemplateField HeaderText="Tipo Parametro" SortExpression="TipoParametro.Descripcion">
                                <ItemTemplate><%# Eval("TipoParametro.Descripcion")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField  HeaderText="Parmetro Dependiente" DataField="ParamDependiente" />
                            <asp:BoundField  HeaderText="SP Parametros" DataField="StoredProcedure" />
                            <asp:ButtonField ButtonType="Image" CommandName="Modificar" ImageUrl="~/Imagenes/Modificar.png" />
                        </Columns>
                    </asp:GridView>           
                </asp:Panel>      
<br />
            <center>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="GestionarReportesDatos"/>
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" 
                    CausesValidation="false" onclick="btnCancelar_Click" />
            </center>
          </ContentTemplate>

          </asp:UpdatePanel>
</div>