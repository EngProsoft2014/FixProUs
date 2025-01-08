
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace FixProUs.Models
{
    public class ScheduleMaterialReceiptModel 
    {

        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public int? ScheduleId { get; set; }
        public int? ScheduleDateId { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? TechnicianId { get; set; }
        public decimal? Cost { get; set; }
        public string Notes { get; set; }
        public string ReceiptPhoto { get; set; }

        public string ReceiptPhotoView { get { return string.Format("https://FixProUs.engprosoft.net/ScheduleMaterialReceipt/{0}", ReceiptPhoto); } }
        //public ImageSource ReceiptPhotoView { get; set; }

        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
