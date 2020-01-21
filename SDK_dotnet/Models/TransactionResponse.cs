using System;
using System.Collections.Generic;

namespace Gluwa.SDK_dotnet.Models
{
    public sealed class TransactionResponse
    {
        /// <summary>
        /// Transaction hash.
        /// </summary>
        public string TxnHash { get; set; }

        /// <summary>
        /// Transaction amount. Value is negative if the amount was sent from the caller's address.
        /// For GET transaction, this is the total amount in the transaction. 
        /// For GET transactions by address, this is only the amount sent/received by the address.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Transaction fee amount. Value is negative.
        /// For Gluwacoin Ethless transfers only; this is zero for other transactions.
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// Currency of the transaction.
        /// </summary>
        public ECurrency? Currency { get; set; }

        /// <summary>
        /// Addresses that the transaction is sent from. 
        /// For GET transactions by address, this list only contains the address if the address is a source of the transaction.
        /// </summary>
        public IList<string> Sources { get; set; }
        
        /// <summary>
        /// Addresses that the transaction is sent to. 
        /// For GET transactions by address, this list only contains the address if the address is a target of the transaction.
        /// </summary>
        public IList<string> Targets { get; set; }

        /// <summary>
        /// Identifier for the transaction that was provided by the merchant user.
        /// </summary>
        public string MerchantOrderID { get; set; }

        /// <summary>
        /// Additional information provided by a user.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Status of the transaction.
        /// </summary>
        public ETransactionStatus? Status { get; set; }

        /// <summary>
        /// DateTime when the transaction was created.
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// DateTime when the transaction was last updated.
        /// </summary>
        public DateTime? ModifiedDateTime { get; set; }
    }
}
