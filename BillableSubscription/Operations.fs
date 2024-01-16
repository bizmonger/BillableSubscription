namespace BeachMobile.BillableSubscription

open Language

module Operations =

    type RequestRegistration   = RegistrationRequest    -> Async<Result<SubscriptionId      ,ErrorDescription>>
    type GetSubscriptionStatus = GetSubscriptionRequest -> Async<Result<Option<Subscription>,ErrorDescription>>

    type SubmitPayment     = PaymentRequest -> Async<Result<SuccessfulPayment,ErrorDescription>>
    type GetPaymentHistory = SubscriptionId -> Async<Result<PaymentHistory   ,ErrorDescription>>