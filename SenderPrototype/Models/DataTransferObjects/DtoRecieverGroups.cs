using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenderPrototype.Models.DataTransferObjects
{
    public class DtoRecieverGroups
    {
        public string Id { get; set; }
        public string Groupname { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsSelected { get; set; }
    }
}