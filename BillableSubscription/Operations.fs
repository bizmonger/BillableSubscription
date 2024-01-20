namespace BeachMobile.BillableSubscription

open System.Threading.Tasks
open Language

module Operations =

    type RequestRegistration   = RegistrationRequest -> Task<Result<RegistrationStatus        ,ErrorDescription>>
    type GetRegistrationStatus = RegistrationReceipt -> Task<Result<Option<RegistrationStatus>,ErrorDescription>>

    type SubmitPayment     = PaymentRequest -> Task<Result<SuccessfulPayment             ,ErrorDescription>>
    type GetPaymentHistory = SubscriptionId -> Task<Result<option<seq<SuccessfulPayment>>,ErrorDescription>>