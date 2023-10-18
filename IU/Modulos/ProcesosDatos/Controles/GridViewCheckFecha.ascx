<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridViewCheckFecha.ascx.cs" Inherits="IU.Modulos.ProcesosDatos.Controles.GridViewCheckFecha" %>

<script language="javascript" type="text/javascript">

    function RepetirFecha() {
        var fecha = $('#<%=gvDatos.ClientID%> tr:first').find('[id*="txtRepetirFecha"]').val();
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            $(this).find('[id*="txtFecha"]').val(fecha);
        });
    }

    function CheckRow(objRef) {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;
        //Get the reference of GridView
        var GridView = row.parentNode;
        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            //The First element is the Header Checkbox
            var headerCheckBox = inputList[0];
            //Based on all or none checkboxes
            //are checked check/uncheck Header Checkbox
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i]
                                               != headerCheckBox) {
                if (!inputList[i].checked) {
                    checked = false;
                    break;
                }
            }
        }
        headerCheckBox.checked = checked;
    }

    function checkAllRow(objRef) {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            //Get the Cell To find out ColumnIndex
            var row = inputList[i].parentNode.parentNode;
            if (inputList[i].type == "checkbox" && objRef
                                                != inputList[i]) {
                if (objRef.checked) {
                    inputList[i].checked = true;
                }
                else {
                    inputList[i].checked = false;
                }
            }
        }
    }

</script>

            <asp:GridView ID="gvDatos" AllowPaging="false" AllowSorting="false" 
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            OnRowDataBound="gvDatos_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Descripcion" >
                            <ItemTemplate>
                                <%# Eval("Descripcion")%>
                                <asp:HiddenField ID="hdfIdValor" Value='<%#Bind("IdValor") %>' runat="server" />
                                <asp:HiddenField ID="hdfDescripcion" Value='<%#Bind("Descripcion") %>' runat="server" />
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha" >
                            <ItemTemplate>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:TextBox CssClass="form-control datepicker" ID="txtRepetirFecha" runat="server" ></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" runat="server"></asp:TextBox>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Incluir" >
                            <ItemTemplate>
                            </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this); CalcularItem();" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" />
                        </ItemTemplate>
                     </asp:TemplateField>
                    </Columns>
            </asp:GridView>