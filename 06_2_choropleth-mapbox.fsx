(**
// can't yet format YamlFrontmatter (["title: ChoroplethMapbox"; "category: Mapbox map charts"; "categoryindex: 7"; "index: 3"], Some { StartLine = 2 StartColumn = 0 EndLine = 6 EndColumn = 8 }) to pynb markdown

# ChoroplethMapbox

[![Binder](https://plotly.net/img/badge-binder.svg)](https://mybinder.org/v2/gh/plotly/Plotly.NET/gh-pages?filepath=06_2_choropleth-mapbox.ipynb)&emsp;
[![Script](https://plotly.net/img/badge-script.svg)](https://plotly.net/06_2_choropleth-mapbox.fsx)&emsp;
[![Notebook](https://plotly.net/img/badge-notebook.svg)](https://plotly.net/06_2_choropleth-mapbox.ipynb)

*Summary:* This example shows how to create choropleth maps using Mapbox layers in F#.

Choropleth Maps display divided geographical areas or regions that are coloured, shaded or patterned in relation to 
a data variable. This provides a way to visualise values over a geographical area, which can show variation or 
patterns across the displayed location.

This choropleth map version uses [Mapbox Layers](https://plotly.net//6_0_geo-vs-mapbox.html). For the Geo variant, head over [here](https://plotly.net//5_2_choropleth-map.html)

ChoroplethMapbox charts need GeoJSON formatted data.

[GeoJSON](https://en.wikipedia.org/wiki/GeoJSON) is an open standard format designed for representing simple geographical features, along with their non-spatial attributes.

GeoJSON, or at least the type of GeoJSON accepted by plotly.js are `FeatureCollection`s. A feature has for example the `geometry` field, which defines e.g. the corrdinates of it (think for example the coordinates of a polygon on the map)
and the `properties` field, a key-value pair of properties of the feature. 

If you want to use GeoJSON with Plotly.NET (or any plotly flavor really), you have to know the property of the feature you are mapping your data to. In the following example this is simply the `id` of a feature, but you can access any property by `property.key`.

Consider the following GeoJSON:

*)
// we are using the awesome FSharp.Data project here to perform a http request
#r "nuget: FSharp.Data"

open FSharp.Data
open Newtonsoft.Json

let geoJson = 
    Http.RequestString "https://raw.githubusercontent.com/plotly/datasets/master/geojson-counties-fips.json"
    |> JsonConvert.DeserializeObject // the easiest way to use the GeoJSON object is deserializing the JSON string.
(**
it looks like this:

```JSON
{
    "type": "FeatureCollection", 
    "features": [{
        "type": "Feature", 
        "properties": {
            "GEO_ID": "0500000US01001", 
            "STATE": "01", 
            "COUNTY": "001", 
            "NAME": "Autauga", 
            "LSAD": "County", 
            "CENSUSAREA": 594.436
        }, 
        "geometry": {
            "type": "Polygon", 
            "coordinates": [[[-86.496774, 32.344437], [-86.717897, 32.402814], [-86.814912, 32.340803], [-86.890581, 32.502974], [-86.917595, 32.664169], [-86.71339, 32.661732], [-86.714219, 32.705694], [-86.413116, 32.707386], [-86.411172, 32.409937], [-86.496774, 32.344437]]]
        },
        "id": "01001"
    }, ... MANY more features.
```

It basically contains all US counties as polygons on the map. Note that the `id` property corresponds to the [**fips code**](https://en.wikipedia.org/wiki/FIPS_county_code).

To visualize some data using these counties as locations on a choropleth map, we need some exmaple data:

*)
// we use the awesome Deedle data frame library to parse and extract our location and z data
#r "nuget: Deedle"
open Deedle

let data = 
    Http.RequestString "https://raw.githubusercontent.com/plotly/datasets/master/fips-unemp-16.csv"
    |> fun csv -> Frame.ReadCsvString(csv,true,separators=",",schema="fips=string,unemp=float")
(**
The data looks like this:

*)
data.Print()(* output: 
fips  unemp 
0    -> 01001 5.3   
1    -> 01003 5.4   
2    -> 01005 8.6   
3    -> 01007 6.6   
4    -> 01009 5.5   
5    -> 01011 7.2   
6    -> 01013 7.1   
7    -> 01015 6.7   
8    -> 01017 5.5   
9    -> 01019 5.2   
10   -> 01021 5.6   
11   -> 01023 8.9   
12   -> 01025 11.1  
13   -> 01027 6.2   
14   -> 01029 6.1   
:       ...   ...   
3204 -> 72125 15.1  
3205 -> 72127 7.9   
3206 -> 72129 12.6  
3207 -> 72131 18.9  
3208 -> 72133 16.8  
3209 -> 72135 8.4   
3210 -> 72137 8.9   
3211 -> 72139 7.2   
3212 -> 72141 15.5  
3213 -> 72143 11.6  
3214 -> 72145 13.9  
3215 -> 72147 10.6  
3216 -> 72149 20.2  
3217 -> 72151 16.9  
3218 -> 72153 18.8*)
(**
As the data contains the fips code and associated unemployment data, we can use the fips codes as locations and the unemployment as z data:

*)
let locations: string [] = 
    data
    |> Frame.getCol "fips"
    |> Series.values
    |> Array.ofSeq

let z: int [] = 
    data
    |> Frame.getCol "unemp"
    |> Series.values
    |> Array.ofSeq
(**
And finally put together the chart using GeoJSON:

*)
open Plotly.NET
open Plotly.NET.LayoutObjects

let choroplethMapbox =
    Chart.ChoroplethMapbox(
        locations = locations,
        z = z,
        geoJson = geoJson,
        FeatureIdKey="id"
    )
    |> Chart.withMapbox(
        Mapbox.init(
            Style=StyleParam.MapboxStyle.OpenStreetMap, // Use the free open street map base map layer
            Center=(-104.6,50.45)
        ) 
    )(* output: 
<div id="684b9cbf-e07a-4459-a1db-7acde637dc0b"><!-- Plotly chart will be drawn inside this DIV --></div>
<script type="text/javascript">

            var renderPlotly_684b9cbfe07a4459a1db7acde637dc0b = function() {
            var fsharpPlotlyRequire = requirejs.config({context:'fsharp-plotly',paths:{plotly:'https://cdn.plot.ly/plotly-2.4.2.min'}}) || require;
            fsharpPlotlyRequire(['plotly'], function(Plotly) {

            var layout = {"width":600,"height":600,"template":{"layout":{"title":{"x":0.05},"font":{"color":"rgba(42, 63, 95, 1.0)"},"paper_bgcolor":"rgba(255, 255, 255, 1.0)","plot_bgcolor":"rgba(229, 236, 246, 1.0)","autotypenumbers":"strict","colorscale":{"diverging":[[0.0,"#8e0152"],[0.1,"#c51b7d"],[0.2,"#de77ae"],[0.3,"#f1b6da"],[0.4,"#fde0ef"],[0.5,"#f7f7f7"],[0.6,"#e6f5d0"],[0.7,"#b8e186"],[0.8,"#7fbc41"],[0.9,"#4d9221"],[1.0,"#276419"]],"sequential":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]],"sequentialminus":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]},"hovermode":"closest","hoverlabel":{"align":"left"},"coloraxis":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}},"geo":{"showland":true,"landcolor":"rgba(229, 236, 246, 1.0)","showlakes":true,"lakecolor":"rgba(255, 255, 255, 1.0)","subunitcolor":"rgba(255, 255, 255, 1.0)","bgcolor":"rgba(255, 255, 255, 1.0)"},"mapbox":{"style":"light"},"polar":{"bgcolor":"rgba(229, 236, 246, 1.0)","radialaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""},"angularaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","ticks":""}},"scene":{"xaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"yaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true},"zaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","gridwidth":2.0,"zerolinecolor":"rgba(255, 255, 255, 1.0)","backgroundcolor":"rgba(229, 236, 246, 1.0)","showbackground":true}},"ternary":{"aaxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"baxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"caxis":{"ticks":"","linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)"},"bgcolor":"rgba(229, 236, 246, 1.0)"},"xaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"yaxis":{"title":{"standoff":15},"ticks":"","automargin":true,"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","zerolinecolor":"rgba(255, 255, 255, 1.0)","zerolinewidth":2.0},"annotationdefaults":{"arrowcolor":"#2a3f5f","arrowhead":0,"arrowwidth":1},"shapedefaults":{"line":{"color":"rgba(42, 63, 95, 1.0)"}},"colorway":["rgba(99, 110, 250, 1.0)","rgba(239, 85, 59, 1.0)","rgba(0, 204, 150, 1.0)","rgba(171, 99, 250, 1.0)","rgba(255, 161, 90, 1.0)","rgba(25, 211, 243, 1.0)","rgba(255, 102, 146, 1.0)","rgba(182, 232, 128, 1.0)","rgba(255, 151, 255, 1.0)","rgba(254, 203, 82, 1.0)"]},"data":{"bar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}},"errorx":{"color":"rgba(42, 63, 95, 1.0)"},"errory":{"color":"rgba(42, 63, 95, 1.0)"}}],"barpolar":[{"marker":{"line":{"color":"rgba(229, 236, 246, 1.0)","width":0.5},"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"carpet":[{"aaxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"},"baxis":{"linecolor":"rgba(255, 255, 255, 1.0)","gridcolor":"rgba(255, 255, 255, 1.0)","endlinecolor":"rgba(42, 63, 95, 1.0)","minorgridcolor":"rgba(255, 255, 255, 1.0)","startlinecolor":"rgba(42, 63, 95, 1.0)"}}],"choropleth":[{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}],"contour":[{"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]],"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}],"contourcarpet":[{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}],"heatmap":[{"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]],"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}],"heatmapgl":[{"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]],"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}],"histogram":[{"marker":{"pattern":{"fillmode":"overlay","size":10,"solidity":0.2}}}],"histogram2D":[{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"histogram2Dcontour":[{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}],"mesh3d":[{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}],"parcoords":[{"line":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"pie":[{"automargin":true}],"scatter":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"scatter3d":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}},"line":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"scattercarpet":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"scattergeo":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"scattergl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"scattermapbox":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"scatterpolar":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"scatterpolargl":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"scatterternary":[{"marker":{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}}}}],"surface":[{"colorbar":{"outlinewidth":0.0,"ticks":{"Case":"Empty"}},"colorscale":[[0.0,"#0d0887"],[0.1111111111111111,"#46039f"],[0.2222222222222222,"#7201a8"],[0.3333333333333333,"#9c179e"],[0.4444444444444444,"#bd3786"],[0.5555555555555556,"#d8576b"],[0.6666666666666666,"#ed7953"],[0.7777777777777778,"#fb9f3a"],[0.8888888888888888,"#fdca26"],[1.0,"#f0f921"]]}]}},"mapbox":{"style":"open-street-map","center":{"lon":-104.6,"lat":50.45}}};
            var config = {"responsive":true};
            Plotly.newPlot('684b9cbf-e07a-4459-a1db-7acde637dc0b', data, layout, config);
});
            };
            if ((typeof(requirejs) !==  typeof(Function)) || (typeof(requirejs.config) !== typeof(Function))) {
                var script = document.createElement("script");
                script.setAttribute("src", "https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.6/require.min.js");
                script.onload = function(){
                    renderPlotly_684b9cbfe07a4459a1db7acde637dc0b();
                };
                document.getElementsByTagName("head")[0].appendChild(script);
            }
            else {
                renderPlotly_684b9cbfe07a4459a1db7acde637dc0b();
            }
</script>
*)
