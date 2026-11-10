using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scaffold.Models
{
    public class DeliveryDocketViewModel
    {
        public Nullable<int> TransDeliId { get;set;}
        public Nullable<int> TransId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string MaterialType { get; set; }
        public string  ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<Decimal> ItemWeight { get; set; }
        public Nullable<Decimal> TotalWeight { get; set; }

        public int DelDocId { get; set; }
        public Nullable<int> DeliveryID { get; set; }
        public string DepotPic { get; set; }
        public string DepotVedio { get; set; }
        public string DepotPickedBy { get; set; }
        public string DepotPersonSign { get; set; }
        public string TransportBy { get; set; }
        public string TransportDriverSign { get; set; }
        public string SitePic { get; set; }
        public string SiteReceivedBy { get; set; }
        public string SitePersonSign { get; set; }
        public string AcceptedBy { get; set; }
        public string AcceptorSign { get; set; }
        public Nullable<bool> ReceiptConfirmation { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> PickDate { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public string TransportCompName { get; set; }
        public Nullable<System.DateTime> SiteReceivedDate { get; set; }
        public string ReceivedByPosition { get; set; }
        public string ClientPosition { get; set; }
        public Nullable<System.DateTime> PickedBy { get; set; }
        public Nullable<int> ClientId { get; set; }

      //  public virtual tblClient tblClient { get; set; }

        public string ClientName { get; set; }
        public string SiteLocation { get; set; }
        public string Address { get; set; }
        public Nullable<int> TransferNumber { get; set; }
        public string Comment { get; set; }
        }
}