<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="IU.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">

        //var graficos = [];
        //google.charts.load('current', { 'packages': ['corechart'] });
        //google.charts.load('current', { 'packages': ['Table'] });
        
        google.charts.load('current', {
		  callback: drawChart,
		  packages: ['bar', 'corechart', 'table']
		});

        $(document).ready(function () {
            /*Cards*/
            $.ajax({
                type: "POST",
                url: 'Dashboard.aspx/ObteneCards',
                data: null,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (resultado) {
                    $.each(resultado.d, function () {
                        $("#dvCards").append(this.Html);
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
            /*Graficos*/
            $.ajax({
                type: "POST",
                url: 'Dashboard.aspx/ObteneGraficos',
                data: null,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (resultado) {
                    $.each( resultado.d, function() {
                        $.ajax({
                            type: "POST",
                            url: 'Dashboard.aspx/ObtenerGraficoDatos',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            data: JSON.stringify({ 'grafico': this}),
                            success: function (resultado) {
                                drawChart(resultado.d);
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert(textStatus);
                            },
                        });

                        var options = {
                            allowHtml: true, showRowNumber: true, title: this.Titulo, width: '100%', 
                            animation: {
                                duration: 1000,
                                startup: true
                            },
                            legend: { position: 'top' },
                        };
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        });

        function drawChart(grafico) {
            var chart;
            var data = new google.visualization.DataTable();
            var columns = grafico.Columns
            for (var j = 0; j < columns.length; j++)
            {
                var tipo = (columns[j][0].substr(7)).toLowerCase();
                if (tipo == "decimal")
                    tipo = "number"
                data.addColumn(tipo, columns[j][1]);
            }
            data.addRows(grafico.Rows);

            //Default options
            var options = { allowHtml: true, showRowNumber: true, title: grafico.Titulo, width: '100%',
                            animation: {
                                duration: 1000,
                                startup: true
                            },
                            legend: { position: 'top' },
            };
            if (grafico.opciones!=null && grafico.opciones.length>0)
                options = grafico.opciones;

            $("#dvGraficos").append("<div class=\"col-12 col-md-6 col-lg-6 mb-3\" id=\"Grafico" + grafico.IdGrafico + "\"></div>");
            var chartDiv = document.getElementById('Grafico' + grafico.IdGrafico);
            switch (grafico.TipoGrafico)
            {
                case 'Line':
                    chart = new google.visualization.LineChart(chartDiv);
                    break;
                case 'Bar':
                    chart = new google.visualization.ColumnChart(chartDiv);
                    break;
                case 'Table':
                    formatter = new google.visualization.NumberFormat(
                        { negativeColor: 'red', decimalSymbol: ',', groupingSymbol: '.' });
                    formatter.format(data,3);
                    chart = new google.visualization.Table(chartDiv);
                    break;
                case 'Pie':
                    chart = new google.visualization.PieChart(chartDiv);
                    break;
            }

            if (grafico.TipoGrafico != "Table")
            {
                //$("#Grafico" + i).append("<a id=\"descarga" + i.toString() + "\" class=\"btn btn-success\">boton</a>")
                var button = document.getElementById('descarga' + grafico.IdGrafico)

                /*button.onclick = function () {
                    console.log("Hola")
                }*/

                /*google.visualization.events.addListener(chart, 'ready', function ()
                {
                    chartDiv.innerHTML = '<img id="chart" src=' + chart.getImageURI() + '>';
                    document.getElementById("descarga" + i).setAttribute("href", chart.getImageURI())
                });*/
            }

            chart.draw(data, options);

        }

    </script>
<div class="cards"><div class="card-group" id="dvCards"></div></div>
<div class="form-group row" id="dvGraficos"></div>

</asp:Content>
