namespace BeachMobile.BillableSubscription

open System

module Language =

    type ErrorDescription = string
    type SubscriptionId   = string
    type PlanId           = string

    [<CLIMutable>]
    type RegistrationRequest = {
        FirstName : string
        LastName  : string
        Phone     : string
        Email     : string
    }

    [<CLIMutable>]
    type RegistrationRequestEntity = {
        id : string
        RegistrationRequest : RegistrationRequest
    }

    [<CLIMutable>]
    type Plan = {
        PlanId : PlanId
        Name   : string
    }

    [<CLIMutable>]
    type BillablePlan = {
        Plan      : Plan
        Price     : double
        Frequency : string
    }

    [<CLIMutable>]
    type Subscription = {
        SubscriptionId : SubscriptionId
        Registration   : RegistrationRequest
        Plan           : BillablePlan
        Started        : DateTime
    }

    [<CLIMutable>]
    type GetSubscriptionRequest = {
        SubscriptionId : SubscriptionId
    }

    [<CLIMutable>]
    type PaymentRequest = {
        Subscription : Subscription
        Amount       : double
    }

    [<CLIMutable>]
    type SuccessfulPayment = {
        Payment   : PaymentRequest
        Timestamp : DateTime
    }

    [<CLIMutable>]
    type PaymentHistoryRequest = {
        SubscriptionId : SubscriptionId
    }

    [<CLIMutable>]
    type PaymentHistory = {
        SubscriptionId    : SubscriptionId
        SuccessfulPayment : SuccessfulPayment seq
    }