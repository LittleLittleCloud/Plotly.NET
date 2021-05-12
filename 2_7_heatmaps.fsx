(**
// can't yet format YamlFrontmatter (["title: Heatmaps"; "category: Simple Charts"; "categoryindex: 3"; "index: 8"], Some { StartLine = 2 StartColumn = 0 EndLine = 6 EndColumn = 8 }) to pynb markdown

# Heatmaps

[![Binder](https://plotly.net/img/badge-binder.svg)](https://mybinder.org/v2/gh/plotly/Plotly.NET/gh-pages?filepath=2_7_heatmaps.ipynb)&emsp;
[![Script](https://plotly.net/img/badge-script.svg)](https://plotly.net/2_7_heatmaps.fsx)&emsp;
[![Notebook](https://plotly.net/img/badge-notebook.svg)](https://plotly.net/2_7_heatmaps.ipynb)

*Summary:* This example shows how to create heatmap charts in F#.

let's first create some data for the purpose of creating example charts:

*)
open Plotly.NET 
 
let matrix =
    [[1.;1.5;0.7;2.7];
    [2.;0.5;1.2;1.4];
    [0.1;2.6;2.4;3.0];]

let rownames = ["p3";"p2";"p1"]
let colnames = ["Tp0";"Tp30";"Tp60";"Tp160"]

let colorscaleValue = 
    StyleParam.Colorscale.Custom [(0.0,"#3D9970");(1.0,"#001f3f")]

// Generating the Heatmap 
let heat1 =
    Chart.Heatmap(
        matrix,colnames,rownames,
        Colorscale=colorscaleValue,
        Showscale=true
    )
    |> Chart.withSize(700.,500.)
    |> Chart.withMarginSize(Left=200.)(* output: 
<div id="a18aad1f-1175-4259-be5b-8a19911f8b94" style="width: 700px; height: 500px;"><!-- Plotly chart will be drawn inside this DIV --></div>
<script type="text/javascript">

            var renderPlotly_a18aad1f11754259be5b8a19911f8b94 = function() {
            var fsharpPlotlyRequire = requirejs.config({context:'fsharp-plotly',paths:{plotly:'https://cdn.plot.ly/plotly-latest.min'}}) || require;
            fsharpPlotlyRequire(['plotly'], function(Plotly) {

            var data = [{"type":"heatmap","z":[[1.0,1.5,0.7,2.7],[2.0,0.5,1.2,1.4],[0.1,2.6,2.4,3.0]],"x":["Tp0","Tp30","Tp60","Tp160"],"y":["p3","p2","p1"],"colorscale":[[0.0,"#3D9970"],[1.0,"#001f3f"]],"showscale":true}];
            var layout = {"width":700.0,"height":500.0,"margin":{"l":200.0}};
            var config = {};
            Plotly.newPlot('a18aad1f-1175-4259-be5b-8a19911f8b94', data, layout, config);
});
            };
            if ((typeof(requirejs) !==  typeof(Function)) || (typeof(requirejs.config) !== typeof(Function))) {
                var script = document.createElement("script");
                script.setAttribute("src", "https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js");
                script.onload = function(){
                    renderPlotly_a18aad1f11754259be5b8a19911f8b94();
                };
                document.getElementsByTagName("head")[0].appendChild(script);
            }
            else {
                renderPlotly_a18aad1f11754259be5b8a19911f8b94();
            }
</script>
*)
(**
A heatmap chart can be created using the `Chart.HeatMap` functions.
When creating heatmap charts, it is usually desirable to provide the values in matrix form, rownames and colnames.
## Styling Colorbars

All charts that contain colorbars can be styled by the `Chart.withColorBarStyle` function.
Here is an example that adds a title to the colorbar:
*)
let heat2 =
    heat1
    |> Chart.withColorBarStyle(
        "Im the Colorbar",
        TitleSide = StyleParam.Side.Right,
        TitleFont = Font.init(Size=20.)
    )(* output: 
<div id="fce63daa-5fe6-418a-8be9-9b9c19110c57" style="width: 700px; height: 500px;"><!-- Plotly chart will be drawn inside this DIV --></div>
<script type="text/javascript">

            var renderPlotly_fce63daa5fe6418a8be99b9c19110c57 = function() {
            var fsharpPlotlyRequire = requirejs.config({context:'fsharp-plotly',paths:{plotly:'https://cdn.plot.ly/plotly-latest.min'}}) || require;
            fsharpPlotlyRequire(['plotly'], function(Plotly) {

            var data = [{"type":"heatmap","z":[[1.0,1.5,0.7,2.7],[2.0,0.5,1.2,1.4],[0.1,2.6,2.4,3.0]],"x":["Tp0","Tp30","Tp60","Tp160"],"y":["p3","p2","p1"],"colorscale":[[0.0,"#3D9970"],[1.0,"#001f3f"]],"showscale":true,"colorbar":{"title":"Im the Colorbar","titlefont":{"size":20.0},"titleside":"right"}}];
            var layout = {"width":700.0,"height":500.0,"margin":{"l":200.0}};
            var config = {};
            Plotly.newPlot('fce63daa-5fe6-418a-8be9-9b9c19110c57', data, layout, config);
});
            };
            if ((typeof(requirejs) !==  typeof(Function)) || (typeof(requirejs.config) !== typeof(Function))) {
                var script = document.createElement("script");
                script.setAttribute("src", "https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js");
                script.onload = function(){
                    renderPlotly_fce63daa5fe6418a8be99b9c19110c57();
                };
                document.getElementsByTagName("head")[0].appendChild(script);
            }
            else {
                renderPlotly_fce63daa5fe6418a8be99b9c19110c57();
            }
</script>
*)

