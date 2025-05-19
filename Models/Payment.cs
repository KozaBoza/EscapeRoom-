using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

//OBSŁUGA PŁATNOŚCI (potem wszystko opisze dokladniej) !!!BD MUSIALA TO POZMIENAIC BO TO SA VIEWMODELE
namespace EscapeRoom.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum PaymentMethod
    {
        CreditCard,
        BankTransfer,
        Cash,
        OnlinePayment
    }

    public class Payment 
    {
        private int _id;
        private int _reservationId;
        private decimal _amount;
        private PaymentStatus _status;
        private PaymentMethod _method;
        private DateTime _paymentDate;
        private string _transactionId;
        private string _notes;
    }

    //tutaj idk czy cos dodac jeszcze strukturalnie
}