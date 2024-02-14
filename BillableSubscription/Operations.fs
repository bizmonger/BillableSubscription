namespace BeachMobile.BillableSubscription

open System.Threading.Tasks
open Language

module Operations =

    type RequestRegistration<'connection>   = RegistrationRequest -> 'connection -> Task<Result<RegistrationStatus        ,ErrorDescription>>
    type GetRegistrationStatus<'connection> = RegistrationReceipt -> 'connection -> Task<Result<Option<RegistrationStatus>,ErrorDescription>>

    type SubmitPayment<'connection>     = PaymentRequest -> 'connection -> Task<Result<SuccessfulPayment             ,ErrorDescription>>
    type GetPaymentHistory<'connection> = SubscriptionId -> 'connection -> Task<Result<option<seq<SuccessfulPayment>>,ErrorDescription>>