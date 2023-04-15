open ColorEnumerations

let colors = RandomColors.Create () |> Seq.cache 

printfn $"%A{Seq.take 10 colors}"
