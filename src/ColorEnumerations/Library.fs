// Copyright Y56380X https://github.com/Y56380X/ColorEnumerations.
// Licensed under the MIT License.

namespace ColorEnumerations

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Core

type RGB =
    { R: byte; G: byte; B: byte }
    member color.ToHexCode () = $"#{color.R:X2}{color.G:X2}{color.B:X2}"

type AlternatingColors =
    
    static member Create (limit, [<ParamArray>] colors) : RGB seq =
        let rec enumerateLimit limit = seq {
            if limit < Array.length colors
            then yield! Array.take limit colors
            else yield! colors
                 yield! enumerateLimit (limit - Array.length colors)
        }
        enumerateLimit limit
    
    static member Create ([<ParamArray>] colors) : RGB seq =
        let rec enumerateInfinite () = seq {
            yield! colors
            yield! enumerateInfinite ()
        }
        enumerateInfinite ()

type RandomColors =
    
    static member Create ([<Optional>] seed: int Nullable) =
        let random =
            if seed.HasValue
            then Random seed.Value
            else Random ()
        let rec enumerateInfinite () = seq {
            let colorBuffer = Array.zeroCreate 3
            random.NextBytes colorBuffer
            yield { R = colorBuffer[0]; G = colorBuffer[1]; B = colorBuffer[2] }
            yield! enumerateInfinite ()
        }
        enumerateInfinite ()
    
    static member Create (limit, [<Optional>] seed: int Nullable) =
        let random =
            if seed.HasValue
            then Random seed.Value
            else Random ()
        let rec enumerateLimit limit = seq {
            if limit > 0
            then
                let colorBuffer = Array.zeroCreate 3
                random.NextBytes colorBuffer
                yield { R = colorBuffer[0]; G = colorBuffer[1]; B = colorBuffer[2] }
                yield! enumerateLimit (limit - 1)
            else ()
        }
        enumerateLimit limit

type ColorGradient =
    
    static member Create (steps: int, fromColor, toColor) =
        let stepR = (decimal toColor.R - decimal fromColor.R) / decimal steps
        let stepG = (decimal toColor.G - decimal fromColor.G) / decimal steps
        let stepB = (decimal toColor.B - decimal fromColor.B) / decimal steps
        let stepColor step =
            {
                R = byte (decimal fromColor.R + Math.Ceiling(stepR * decimal step))
                G = byte (decimal fromColor.G + Math.Ceiling(stepG * decimal step))
                B = byte (decimal fromColor.B + Math.Ceiling(stepB * decimal step))
            }
        Seq.unfold (function step when step <= steps -> Some (stepColor step, step + 1) | _ -> None ) 0
