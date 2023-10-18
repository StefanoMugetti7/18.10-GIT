<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FirmaDigital.ascx.cs" Inherits="IU.Modulos.Comunes.FirmaDigital" %>

<style type="text/css">
    
    /* CLASS CANVAS*/
    .jSignature 
    {
    	
    	}
    /*
    div {
		margin-top:1em;
		margin-bottom:1em;
	}
	*/
	#signatureparent {
		color:darkblue;
		background-color:darkgrey;
		/*max-width:600px;*/
		padding:20px;
	}
	
	/*This is the div within which the signature canvas is fitted*/
	#signature {
		border: 2px dotted black;
		background-color:lightgrey;
	}

	/* Drawing the 'gripper' for touch-enabled devices */ 
	html.touch #content {
		float:left;
		width:92%;
	}
	html.touch #scrollgrabber {
		float:right;
		width:4%;
		margin-right:2%;
		background-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAAFCAAAAACh79lDAAAAAXNSR0IArs4c6QAAABJJREFUCB1jmMmQxjCT4T/DfwAPLgOXlrt3IwAAAABJRU5ErkJggg==)
	}
	html.borderradius #scrollgrabber {
		border-radius: 1em;
	}
	 
</style>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="EVOL - SIM" Style='display:none' CssClass="modalPopupComprobantes" >

<div id="signatureparent">
<div id="signature"></div>
<button type="button" onclick="$('#signature').jSignature('clear')">Limpiar</button>
<%--<button type="button" id="btnSave">Save</button>--%>
</div>
<input type="hidden" id="hiddenSigData" runat="server"  name="hiddenSigData" />
<div id="scrollgrabber"></div>
<script src="<%=ResolveClientUrl("~/assets/js/jSignature.js")%>"></script>
<script src="<%=ResolveClientUrl("~/assets/js/plugins/jSignature.CompressorBase30.js")%>"></script>
<script src="<%=ResolveClientUrl("~/assets/js/plugins/jSignature.CompressorSVG.js")%>"></script>
<script src="<%=ResolveClientUrl("~/assets/js/plugins/jSignature.UndoButton.js")%>"></script> 
<script>
    $(document).ready(function () {
        var $sigdiv = $("#signature").jSignature({ 'UndoButton': false });

        // -- i explain from here...
        $('#btnSave').click(function () {
            var sigData = $('#signature').jSignature('getData', 'base30');
            //var sigData = $('#signature').jSignature('getData');
            $('#<%= hiddenSigData.ClientID %>').val(sigData);
            $('#<%=btnAceptar.ClientID %>').click();
        });
        // -- ... to here.

    })
</script>

<center>
        <button type="button" id="btnSave">Aceptar</button>
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" Style="display: none;" CausesValidation="false" runat="server" Text="Aceptar" />
        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" Text="Volver" />
    </center>
</asp:Panel>
<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<asp:ModalPopupExtender 
    ID="mpePopUp" runat="server" 
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground" 
    CancelControlID="btnVolver"
    >
</asp:ModalPopupExtender>