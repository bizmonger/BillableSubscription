namespace BeachMobile.BillableSubscription

open System

module Language =

    type Key              = string
    type ErrorDescription = string
    type SubscriptionId   = string

    [<CLIMutable>]
    type RegistrationRequest = {
        FirstName : string
        LastName  : string
        Phone     : string
        Email     : string
        Plan      : string
    }

    [<CLIMutable>]
    type Plan = {
        Name        : string
        Description : string
    }

    [<CLIMutable>]
    type BillablePlan = {
        Plan      : Plan
        Price     : double
        Frequency : string
    }

    [<CLIMutable>]
    type RegistrationReceipt = {
        id : string
        Request : RegistrationRequest
        Timestamp : DateTime
    }

    [<CLIMutable>]
    type Subscription = {
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