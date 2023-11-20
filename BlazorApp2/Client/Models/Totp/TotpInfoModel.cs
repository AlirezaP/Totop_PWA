using IndexedDB.Blazor;
using Microsoft.JSInterop;
using System;

namespace BlazorApp2.Client.Models.Totp
{
    public class TotpDBContext : IndexedDb
    {
        public TotpDBContext(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }
        public IndexedSet<TotpInfo> Data { get; set; }
        public IndexedSet<TotpInfoSec> DataSec { get; set; }
        public class TotpInfoSec
        {
          
            [System.ComponentModel.DataAnnotations.Key]
            public long Id { get; set; }
            public string SecData { get; set; }
        }
        public class TotpInfo
        {
            [System.ComponentModel.DataAnnotations.Key]
            public long Id { get; set; }
            public string LoginCode { get; set; }
            public int Step { get; set; }
            public int Time { get; set; }
            public string KeySec { get; set; }
        }
    }
}


