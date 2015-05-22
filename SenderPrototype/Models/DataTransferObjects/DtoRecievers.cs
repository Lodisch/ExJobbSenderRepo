using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenderPrototype.Models.DataTransferObjects
{
    public class DtoRecievers
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsSelected { get; set; }
        public string GroupId { get; set; }
        public string UserId { get; set; }
       
    }
}