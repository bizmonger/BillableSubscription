namespace BeachMobile.BillableSubscription

open Language

module Operations =

    type RequestRegistration   = RegistrationRequest -> Async<Result<RegistrationReceipt       ,ErrorDescription>>
    type GetRegistrationStatus = RegistrationStatus  -> Async<Result<Option<RegistrationStatus>,ErrorDescription>>

    type SubmitPayment     = PaymentRequest -> Async<Result<SuccessfulPayment    ,ErrorDescription>>
    type GetPaymentHistory = SubscriptionId -> Async<Result<SuccessfulPayment seq,ErrorDescription>>