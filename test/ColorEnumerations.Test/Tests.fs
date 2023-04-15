module Tests

open ColorEnumerations
open Xunit
open FsUnit.Xunit

[<Fact>]
let ``Enumerate no alternating colors`` () =
    let testColor1 = { R = 10uy; G = 15uy; B = 20uy }
    let colors = AlternatingColors.Create [|testColor1|]
    
    let color1 = Seq.item 0 colors
    let color2 = Seq.item 1 colors
    let color3 = Seq.item 2 colors
    
    color1 |> should equal testColor1
    color2 |> should equal testColor1
    color3 |> should equal testColor1

[<Fact>]
let ``Enumerate one alternating color`` () =
    let testColor1 = { R = 10uy; G = 15uy; B = 20uy }
    let testColor2 = { R = 20uy; G = 15uy; B = 10uy }
    let colors = AlternatingColors.Create [|testColor1; testColor2|]
    
    let color1 = Seq.item 0 colors
    let color2 = Seq.item 1 colors
    let color3 = Seq.item 2 colors
    let color4 = Seq.item 3 colors
    
    color1 |> should equal testColor1
    color2 |> should equal testColor2
    color3 |> should equal testColor1
    color4 |> should equal testColor2

[<Fact>]
let ``Enumerate two alternating colors`` () =
    let testColor1 = { R = 10uy; G = 15uy; B = 20uy }
    let testColor2 = { R = 20uy; G = 15uy; B = 10uy }
    let testColor3 = { R = 15uy; G = 10uy; B = 20uy }
    let colors = AlternatingColors.Create [|testColor1; testColor2; testColor3|]
    
    let color1 = Seq.item 0 colors
    let color2 = Seq.item 1 colors
    let color3 = Seq.item 2 colors
    let color4 = Seq.item 3 colors
    let color5 = Seq.item 4 colors
    let color6 = Seq.item 5 colors
    
    color1 |> should equal testColor1
    color2 |> should equal testColor2
    color3 |> should equal testColor3
    color4 |> should equal testColor1
    color5 |> should equal testColor2
    color6 |> should equal testColor3

[<Theory>]
[<InlineData 10>]
[<InlineData 11>]
let ``Enumerate alternating colors with limit`` limit =
    let testColor1 = { R = 10uy; G = 15uy; B = 20uy }
    let testColor2 = { R = 20uy; G = 15uy; B = 10uy }
    let testColor3 = { R = 15uy; G = 10uy; B = 20uy }
    let colors = AlternatingColors.Create (limit, [|testColor1; testColor2; testColor3|])
    
    let length = Seq.length colors
    
    length |> should equal limit

[<Fact>]
let ``Enumerate alternating colors without limit`` () =
    let testColor1 = { R = 10uy; G = 15uy; B = 20uy }
    let testColor2 = { R = 20uy; G = 15uy; B = 10uy }
    let testColor3 = { R = 15uy; G = 10uy; B = 20uy }
    let colors = AlternatingColors.Create [|testColor1; testColor2; testColor3|]
    
    let length = Seq.take 1000 colors |> Seq.length
    
    length |> should equal 1000

[<Theory>]
[<InlineData 10>]
[<InlineData 11>]
let ``Enumerate random colors with limit`` limit =
    let colors = RandomColors.Create (limit = limit)
    
    let length = Seq.length colors
    
    length |> should equal limit

[<Fact>]
let ``Enumerate random colors without limit`` () =
    let colors = RandomColors.Create ()
    
    let length = Seq.take 1000 colors |> Seq.length
    
    length |> should equal 1000

[<Fact>]
let ``Enumerate random colors without seed`` () =
    let colors1 = RandomColors.Create (limit = 1000)
    let colors2 = RandomColors.Create (limit = 1000)
    
    colors1 |> should not' (equalSeq colors2)

[<Fact>]
let ``Enumerate random colors with seed`` () =
    let colors1 = RandomColors.Create (limit = 1000, seed = 10)
    let colors2 = RandomColors.Create (limit = 1000, seed = 10)
    
    let colors1 = Array.ofSeq colors1
    let colors2 = Array.ofSeq colors2
    
    colors1 |> should equalSeq colors2

[<Theory>]
[<InlineData 1>]
[<InlineData 2>]
[<InlineData 3>]
[<InlineData 1000>]
let ``Enumerate color gradient check enumeration lenght`` steps =
    let black = { R = 0uy;   G = 0uy;   B = 0uy }
    let white = { R = 255uy; G = 255uy; B = 255uy }
    let colors = ColorGradient.Create (steps, black, white)
    
    let length = Seq.length colors
    
    length |> should equal (steps + 1)

[<Theory>]
[<InlineData (1, 0, 255)>]
[<InlineData (1000, 0, 255)>]
[<InlineData (1, 255, 0)>]
[<InlineData (1000, 255, 0)>]
let ``Enumerate color gradient check boundary colors`` steps fromValue toValue =
    let fromColor = { R = fromValue; G = fromValue; B = fromValue }
    let toColor   = { R = toValue  ; G = toValue  ; B = toValue }
    let colors = ColorGradient.Create (steps, fromColor, toColor)
    
    Seq.head colors |> should equal fromColor
    Seq.last colors |> should equal toColor
