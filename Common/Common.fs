namespace BeachMobile.BillableSubscription

open System
open System.Text
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Threading
open System.Diagnostics
open Newtonsoft.Json

module WebGateway =

    let toResult task = task |> Async.AwaitTask |> Async.RunSynchronously

    let httpClient baseAddress =

        let client = new HttpClient()
        client.Timeout     <- TimeSpan(0,1,0)
        client.BaseAddress <- Uri(baseAddress)
        client.DefaultRequestHeaders.Accept.Clear()
        client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue("application/json"))
        client

    let get baseAddress (resource:string) =

        task {
            let client = httpClient baseAddress
            return! client.GetAsync(resource) |> Async.AwaitTask
        }

    let getWithToken baseAddress (resource:string) =

        task {
            let client = httpClient baseAddress
            return! client.GetAsync(resource) |> Async.AwaitTask
        }

    let postTo (baseAddress:string) (resource:string) (payload:Object) =

        let tokenSource = new CancellationTokenSource(TimeSpan(0,3,0));
        let token       = tokenSource.Token;

        task {
                use  client   = httpClient baseAddress
                let  json     = JsonConvert.SerializeObject(payload)
                let  content  = new StringContent(json, Encoding.UTF8, "application/json")
                let! response = client.PostAsync(resource, content, token) |> Async.AwaitTask

                Debug.WriteLine $"\n\n{baseAddress}{resource}\nSuccess: {response.IsSuccessStatusCode}\n\n" 
 
                return response

        }