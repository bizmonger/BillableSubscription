namespace BeachMobile.BillableSubscription

open Language

module Operations =

    type RequestRegistration   = RegistrationRequest    -> Result<SubscriptionId      ,ErrorDescription>
    type GetSubscriptionStatus = GetSubscriptionRequest -> Result<Option<Subscription>,ErrorDescription>

    type SubmitPayment     = PaymentRequest -> Result<SuccessfulPayment,ErrorDescription>
    type GetPaymentHistory = SubscriptionId -> Result<PaymentHistory   ,ErrorDescription>