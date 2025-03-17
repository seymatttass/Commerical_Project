using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Settings
{
    //sadece kuyruk isimlerini barındıracak. bu yüzden static olabilir.
    public static class RabbitMQSettings
    {
        // **Saga State Machine Kuyruğu**
        public const string StateMachineQueue = "state-machine-queue";

        // **Stock Servisi Kuyrukları**
        public const string Stock_CheckStockQueue = "stock-check-stock-queue"; // Stok kontrol kuyruğu
        public const string Stock_StockReservedQueue = "stock-stock-reserved-queue"; // Stok rezerve edildi
        public const string Stock_StockNotReservedQueue = "stock-stock-not-reserved-queue"; // Stok rezerve edilemedi
        public const string Stock_RollbackMessageQueue = "stock-rollback-message-queue";


        // **Order Servisi Kuyrukları**
        public const string Order_OrderCreatedQueue = "order-order-created-queue"; // Sipariş oluşturma kuyruğu
        public const string Order_OrderFailedQueue = "order-order-failed-queue"; // Sipariş başarısız kuyruğu
        public const string Order_OrderCompletedQueue = "order-order-completed-queue"; 

        // **Payment Servisi Kuyrukları**
        public const string Payment_PaymentStartedQueue = "payment-payment-started-queue"; // Ödeme başlatıldı kuyruğu
        public const string Payment_PaymentCompletedQueue = "payment-payment-completed-queue"; // Ödeme tamamlandı kuyruğu
        public const string Payment_PaymentFailedQueue = "payment-payment-failed-queue"; // Ödeme başarısız kuyruğu

        // **Basket (Sepet) Servisi Kuyrukları**
        public const string Basket_ProductAddedToBasketQueue = "basket-add-to-basket-queue"; // Kullanıcı sepete ekleme kuyruğu
        public const string Basket_BasketItemCompletedQueue = "basket-basket-item-added-queue"; // Ürün sepete eklendi kuyruğu

        public const string Address_GetAddressDetailQueue = "address-get-address-detail-queue";

        public const string User_GetUserDetailQueue = "user-get-user-detail-queue";

    }
}
    