using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.API
{
    public class QueryAccountResult
    {
        public string Appid { get; set; }
        public string Mchid { get; set; }
        public string Pubkey { get; set; }
        public string Prikey { get; set; }
        public string CertPath { get; set; }
        public string Ext1 { get; set; }
        public string Ext2 { get; set; }
        public string Ext3 { get; set; }
        public string Ext4 { get; set; }
        public string Ext5 { get; set; }
        public bool Status { get; set; }
        public QueryAccountResult()
        {
            this.Status = false;
        }
    }
}