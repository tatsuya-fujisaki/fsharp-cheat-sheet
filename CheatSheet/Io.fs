﻿module Io

open Process1
open System
open System.IO

// F# cannot define a variadic function. (A variadic function is a function of indefinite arity.
let desktopize path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop, path))

let printMap path map = File.WriteAllLines (path, map |> Map.toSeq |> Seq.map (fun (k, v) -> sprintf "%A = %A" k v))

let rec readName () = 
    printf "Enter name: "
    match Console.ReadLine().Trim() with
    | "" -> readName ()
    | s -> s

let rec readAge () = 
    printf "Enter age: "
    match Int32.TryParse(Console.ReadLine()) with
    | true, n when n > 0 -> n
    | _ -> readAge ()

let readValues () = 
    let rec readValues' vs = 
        printf "Enter a value, or press enter to end: "
        match Console.ReadLine() with
        | "" -> List.rev vs
        | s -> 
            match Double.TryParse s with
            | true, v -> v :: vs
            | _ -> vs
            |> readValues'
    readValues' []

let rec readFilePath () = 
    printf "Enter file path: "
    let filePath = Console.ReadLine()
    match File.Exists filePath with
    | true -> filePath
    | false -> readFilePath ()

let rec print = function
    | [] -> ()
    | x :: tail -> 
        printfn "%A" x
        print tail

let rec reversePrint = function
    | [] -> ()
    | x :: tail -> 
        reversePrint tail
        printfn "%A" x

let deleteDirectory path = 
    // Avoid DirectoryNotFoundException
    match Directory.Exists path with
    | true -> Directory.Delete(path, true)
    | false -> ()

let deleteFile path = 
    // Avoid ArgumentNullException by File.Delete("")
    // Avoid ArgumentException by File.Delete(null)
    // Avoid DirectoryNotFoundException by File.Delete(@"C:\NonExistingDirectory\Foo.txt")
    match File.Exists path with
    | true -> File.Delete path
    | false -> ()

let copyDirectory sourceDirectory destinationDirectory = 
    deleteDirectory destinationDirectory
    startAndForget "xcopy" (sprintf "%A %A /e /i /y" sourceDirectory destinationDirectory)

let rec walk dir pattern = 
    seq { yield! Directory.EnumerateFiles(dir, pattern)
          for d in Directory.EnumerateDirectories dir do
              yield! walk d pattern }