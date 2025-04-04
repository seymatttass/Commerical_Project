using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Settings
{
    public static class RabbitMQSettings
    {
        public const string StateMachineQueue = "state-machine-queue";

        public const string Stock_CheckStockQueue = "stock-check-stock-queue";
        public const string Stock_RollbackMessageQueue = "stock-rollback-message-queue";
        public const string Stock_ReductionQueue = "stock-reduction-queue"; 


        public const string Order_OrderCreatedQueue = "order-order-created-queue";
        public const string Order_OrderFailedQueue = "order-order-failed-queue";
        public const string Order_OrderCompletedQueue = "order-order-completed-queue";


        public const string Payment_PaymentStartedQueue = "payment-payment-started-queue"; 

        public const string Basket_ProductAddedToBasketQueue = "basket-add-to-basket-queue"; 
        public const string Basket_BasketFailedEventQueue = "basket-basket-failed-item-added-queue"; 


















        public const string GetAddressDetailRequestEvent = "address-get-address-request-queue";
        public const string GetAddressDetailResponseEvent = "address-get-address-response-queue";

        public const string GetUserDetailRequestEvent = "user-get-user-request-queue";
        public const string GetUserDetailResponseEvent = "user-get-user-response-queue";

        public const string Invoice_CreateInvoiceQueue = "invoice-create-invoice-queue";
        public const string Invoice_InvoiceCreatedQueue = "invoice-invoice-created-queue";

        public const string Category_CategoryEventQueue = "category-event-queue";

        public const string Shipping_CreateShippingQueue = "shipping-create-shipping-queue";
        public const string Shipping_CompletedQueue = "shipping-completed-queue";
        public const string Shipping_FailedQueue = "shipping-failed-queue";

    }
}
    