using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rubberduck.Model.Abstract;

namespace Rubberduck.ContentServices.Model
{
    public enum SynchronisationStatusCode
    {
        Processing = 0,
        Success = 1,
        Failed = 2
    }

    public class Synchronisation : Entity
    {
        public string ApiVersion { get; set; }
        public string RequestIP { get; set; }
        public string UserAgent { get; set; }
        public string Message { get; set; }
        public DateTime TimestampStart { get; set; }
        public DateTime? TimestampEnd { get; set; }
        public int StatusCode { get; set; }
    }
}
