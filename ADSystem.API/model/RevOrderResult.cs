using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.API
{
    public class RevOrderResult
    {
        public RevOrderResult()
        {
            this.Status = false;
        }
        public string OrderNo { get; set; }
        public string ResultMsg { get; set; }
        public bool Status { get; set; }
        public string SyncUrl { get; set; }
        public string AsyncUrl { get; set; }
        public string AppId { get; set; }
        public int AccountType { get; set; }
        public decimal PayFee { get; set; }
    }
}