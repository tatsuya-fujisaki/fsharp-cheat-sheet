﻿module Bool

open System

// Explanatory wrapper
let isSome (x: 'T option) = x.IsSome

// Explanatory wrapper
let isEmpty xs = Array.isEmpty xs

let isNumeric x = Double.TryParse x |> fst

let caseInsensitiveContains (s : string) findMe =
    -1 < s.IndexOf(findMe, StringComparison.OrdinalIgnoreCase)

let isCommentLine (s : string) =
    s.TrimStart().[0] = '#'

let areSameLists f (xs : 'a list) (ys : 'a list) =
    xs.Length = ys.Length &&
    List.forall2 f xs ys

let excludeNone (xs : 'a option[]) =
    Array.filter (fun (x : 'a option) -> x.IsSome) xs