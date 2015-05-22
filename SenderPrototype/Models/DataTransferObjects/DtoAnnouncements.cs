using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenderPrototype.Models.DataTransferObjects
{
    public class DtoAnnouncements
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Announcement { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string SendToId { get; set; }
    }
}