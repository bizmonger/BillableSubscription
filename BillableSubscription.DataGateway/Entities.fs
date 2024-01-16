namespace BeachMobile.BillableSubscription

open Language

module Entities =

    [<CLIMutable>]
    type RegistrationRequestEntity = {
        id : string
        RegistrationRequest : RegistrationRequest
    }

    [<CLIMutable>]
    type PlanEntity = {
        id   : string
        Plan : Plan
    }

    [<CLIMutable>]
    type SubscriptionEntity = {
        id : string
        Subscription : Subscription
    }

    [<CLIMutable>]
    type PaymentRequestEntity = {
        id : string
        PaymentRequest : PaymentRequest
    }

    [<CLIMutable>]
    type SuccessfulPaymentEntity = {
        id : string
        SuccessfulPayment : PaymentRequestEntity
    }