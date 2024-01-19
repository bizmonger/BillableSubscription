namespace BeachMobile.DataGateway.Redis

open System.Collections.Generic
open Newtonsoft.Json
open StackExchange.Redis
open BeachMobile.BillableSubscription.Language

module Msg =

    let noConnectionExists = "No Redis server connection"
    let invalidCacheState  = "Invalid cache state"
    let failedToCache key  = $"Failed to cache: {key}"

type Cache(connectionString:string) =

    let mutable connection : ConnectionMultiplexer option = None
    let mutable cache : IDatabase option = None

    member x.Connect() : Async<Result<unit, ErrorDescription>> =
        
        async {
            match! ConnectionMultiplexer.ConnectAsync(connectionString) |> Async.AwaitTask with
            | conn when conn <> null -> connection <- Some conn
                                        cache <- Some <| conn.GetDatabase()
                                        return Ok()

            | _ -> return Error Msg.noConnectionExists
        }

    member x.Disconnect() : Async<Result<unit, ErrorDescription>> =
        
        async {
            match connection with
            | None -> return Ok()
            | Some conn -> do! conn.CloseAsync() |> Async.AwaitTask
                           connection <- None
                           cache      <- None
                           return Ok()
        }

    member x.Get<'result> (key:Key) : Async<Result<'result, ErrorDescription>> =

        async {
            
            match connection,cache with
            | None,None   -> return Error Msg.noConnectionExists
            | Some _, Some v ->
                let! result = v.StringGetAsync(key) |> Async.AwaitTask
                let hydated = JsonConvert.DeserializeObject<'result> result

                return Ok hydated

            | _ -> return Error Msg.invalidCacheState
        }

    member x.Post<'value> (kv:KeyValuePair<Key,'value>) : Async<Result<unit, ErrorDescription>> =

        async {
        
            match cache with
            | None   -> return Error Msg.noConnectionExists
            | Some v ->

                let value = JsonConvert.SerializeObject(kv.Value)
                let kv'   = KeyValuePair<RedisKey,RedisValue>(RedisKey(kv.Key), RedisValue(value))

                match! v.StringSetAsync([|kv'|]) |> Async.AwaitTask with
                | false -> return Error (Msg.failedToCache kv.Key)
                | true  -> return Ok()
        }