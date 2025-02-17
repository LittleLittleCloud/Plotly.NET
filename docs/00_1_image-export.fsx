(**
---
title: Static image export
category: General
categoryindex: 1
index: 2
---
*)


(*** hide ***)

(*** condition: prepare ***)
#r "nuget: Newtonsoft.JSON, 12.0.3"
#r "nuget: DynamicObj"
#r "nuget: PuppeteerSharp"
#r "../bin/Plotly.NET/netstandard2.0/Plotly.NET.dll"
#r "../bin/Plotly.NET.ImageExport/netstandard2.0/Plotly.NET.ImageExport.dll"

(*** condition: ipynb ***)
#if IPYNB
#r "nuget: Plotly.NET, {{fsdocs-package-version}}"
#r "nuget: Plotly.NET.Interactive, {{fsdocs-package-version}}"
#r "nuget: Plotly.NET.ImageExport, {{fsdocs-package-version}}"
#endif // IPYNB


(**
[![Binder]({{root}}img/badge-binder.svg)](https://mybinder.org/v2/gh/plotly/Plotly.NET/gh-pages?filepath={{fsdocs-source-basename}}.ipynb)&emsp;
[![Script]({{root}}img/badge-script.svg)]({{root}}{{fsdocs-source-basename}}.fsx)&emsp;
[![Notebook]({{root}}img/badge-notebook.svg)]({{root}}{{fsdocs-source-basename}}.ipynb)

# Static image export

### Table of contents

- [Saving static images](#Saving-static-images)
- [Generating URIs for static chart images](#Generating-URIs-for-static-chart-images)
- [Including static images in dotnet interactive notebooks](#Including-static-images-in-dotnet-interactive-notebooks)

As Plotly.NET generates static html pages that contain charts rendered by plotly.js, static image export needs a lot more overhead under the hood 
than you might expect. The underlying renderer needs to execute javascript, leading to the usage of headless browsers.

The package `Plotly.NET.ImageExport` contains extensions for Plotly.NET to render static images. It is designed with extensibility in mind and
it is very easy to add a new rendering engine. The current engines are provided:

| Rendering engine | Type | Prerequisites |
|-|-|-|
| [PuppeteerSharp](https://github.com/hardkoded/puppeteer-sharp) | headless browser | [read more here](https://github.com/hardkoded/puppeteer-sharp#prerequisites) |

## Saving static images

By referencing the `Plotly.NET.ImageExport` package, you get access to:

 - jpg via `Chart.SaveJPG`
 - png via `Chart.SavePNG`
 - svg via `Chart.SaveSVG`

(and Extensions for C# style fluent interfaces by opening the `GenericChartExtensions` namespace)

The parameters for all three functions are exactly the same. 
*)

open Plotly.NET
open Plotly.NET.ImageExport

let exampleChart = 
    Chart.Histogram2DContour(
        [1.;2.;2.;4.;5.],
        [1.;2.;2.;4.;5.]
    )

(***do-not-eval***)
exampleChart
|> Chart.saveJPG(
    "/your/path/without/extension/here",
    Width=300,
    Height=300
)

(*** condition: ipynb ***)
#if IPYNB
let imgString = $"""<img
    src= "{exampleChart|> Chart.toBase64JPGString(Width=300,Height=300)}"
/>"""
DisplayExtensions.DisplayAs(imgString,"text/html")
#endif // IPYNB

(***hide***)
$"""<img
    src= "{exampleChart|> Chart.toBase64JPGString(Width=300,Height=300)}"
/>"""
(***include-it-raw***)

(**
## Generating URIs for static chart images

By referencing the `Plotly.NET.ImageExport` package, you get access to:

 - jpg via `Chart.toBase64JPGString`
 - png via `Chart.toBase64PNGString`
 - svg via `Chart.toSVGString`

(and Extensions for C# style fluent interfaces by opening the `GenericChartExtensions` namespace)

*)

let base64JPG =
    exampleChart
    |> Chart.toBase64JPGString(
        Width=300,
        Height=300
    )

(**
It is very easy to construct a html tag that includes this image via a base64 uri. For SVGs, 
not even that is necessary and just the SVG string can be used.
*)

(***do-not-eval***)
$"""<img
    src= "{base64JPG}"
/>"""

(*** condition: ipynb ***)
#if IPYNB
let imgString = $"""<img
    src= "{base64JPG}"
/>"""
DisplayExtensions.DisplayAs(imgString,"text/html")
#endif // IPYNB

(***hide***)
$"""<img
    src= "{base64JPG}"
/>"""

(***include-it-raw***)

(**
SVGs can be included without the image tag:
*)

let svgString =
    exampleChart
    |> Chart.toSVGString(
        Width=300,
        Height=300
    )

svgString.Substring(0,300)
|> printfn "%s"

(***include-output***)

(**
In fact, the images shown on this site are included just the same way.

## Including static images in dotnet interactive notebooks

To include the images in dotnet interactive, convert them to html tags as above and include them via 
dotnet interactive's `DisplayAs` function. The content type for PNG/JPG is "text/html", and "image/svg+xml" for SVG.
*)

let base64PNGTag =
    let base64 =
        exampleChart
        |> Chart.toBase64PNGString(
            Width=300,
            Height=300
        )
    $"""<img src= "{base64JPG}"/>"""

let svgString2 =
    exampleChart
    |> Chart.toSVGString(
        Width=300,
        Height=300
    )

// DisplayExtensions.DisplayAs(base64PNG,"text/html")
// DisplayExtensions.DisplayAs(svgString2,"image/svg+xml")

(*** condition: ipynb ***)
#if IPYNB
DisplayExtensions.DisplayAs(base64PNG,"text/html")
DisplayExtensions.DisplayAs(svgString,"image/svg+xml")
#endif // IPYNB