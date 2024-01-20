namespace BeachMobile.BillableSubscription

open Language

module Entities =

    [<CLIMutable>]
    type RegistrationRequestEntity = {
        id : string
        PartitionId : string
        RegistrationRequest : RegistrationRequest
    }

    [<CLIMutable>]
    type RegistrationStatusEntity = {
        id : string
        Status : RegistrationStatus
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
        PartitionId : string
        PaymentRequest : PaymentRequest
    }

    type PaymentHistoryEntity = {
        id : string
        Payments : SuccessfulPayment seq
    }

    [<CLIMutable>]
    type SuccessfulPaymentEntity = {
        id : string
        SuccessfulPayment : PaymentRequestEntity
    }