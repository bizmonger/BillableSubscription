namespace BeachMobile.BillableSubscription

open Language

module Entities =

    [<CLIMutable>]
    type RegistrationRequestEntity = {
        id : string
        PartitionKey: string
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
        PartitionKey: string
        Subscription : Subscription
    }

    [<CLIMutable>]
    type PaymentRequestEntity = {
        id : string
        PartitionKey: string
        PaymentRequest : PaymentRequest
    }

    [<CLIMutable>]
    type SuccessfulPaymentEntity = {
        id : string
        PartitionKey: string
        SuccessfulPayment : PaymentRequestEntity
    }