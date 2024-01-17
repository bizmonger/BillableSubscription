namespace BeachMobile.DataGateway.Redis

open System.Collections.Generic
open Newtonsoft.Json
open StackExchange.Redis
open BeachMobile.BillableSubscription.Language

module Msg =

    let noConnectionExists = "No Redis server connection"

type Cache(connectionString:string) =

    let mutable cache : IDatabase option = None

    member x.Connect() : Async<Result<unit, ErrorDescription>> =
        
        async {
            let! connection = ConnectionMultiplexer.ConnectAsync(connectionString) |> Async.AwaitTask
            cache <- Some <| connection.GetDatabase()
            return Ok()
        }

    member x.Get<'result> (key:Key) : Async<Result<'result, ErrorDescription>> =

        async {
            
            match cache with
            | None   -> return Error Msg.noConnectionExists
            | Some v ->
                let! result = v.StringGetAsync(key) |> Async.AwaitTask
                let hydated = JsonConvert.DeserializeObject<'result> result

                return Ok hydated
        }

    member x.Post<'value> (kv:KeyValuePair<Key,'value>) : Async<Result<unit, ErrorDescription>> =

        async {
        
            match cache with
            | None   -> return Error Msg.noConnectionExists
            | Some v ->

                let value = JsonConvert.SerializeObject(kv.Value)
                let kv'   = KeyValuePair<RedisKey,RedisValue>(RedisKey(kv.Key), RedisValue(value))

                match! v.StringSetAsync([|kv'|]) |> Async.AwaitTask with
                | false -> return Error $"Failed to cache: {kv.Key}"
                | true  -> return Ok()
        }