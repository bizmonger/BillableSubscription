namespace BeachMobile.BillableSubscription

open Language

module Operations =

    type RequestRegistration   = RegistrationRequest -> Async<Result<RegistrationStatus        ,ErrorDescription>>
    type GetRegistrationStatus = RegistrationReceipt -> Async<Result<Option<RegistrationStatus>,ErrorDescription>>

    type SubmitPayment     = PaymentRequest -> Async<Result<SuccessfulPayment             ,ErrorDescription>>
    type GetPaymentHistory = SubscriptionId -> Async<Result<option<seq<SuccessfulPayment>>,ErrorDescription>>