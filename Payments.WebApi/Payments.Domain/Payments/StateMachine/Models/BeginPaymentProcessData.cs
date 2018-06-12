using System;

namespace Payments.Domain.Payments.StateMachine.Models
{
    public class BeginPaymentProcessData
    {
        public BeginPaymentProcessData(Guid orderId, string username, decimal totalPrice)
        {
            OrderId = orderId;
            Username = username;
            TotalPrice = totalPrice;
        }

        public Guid OrderId { get; }
        public string Username { get; }
        public decimal TotalPrice { get; }

    }
}