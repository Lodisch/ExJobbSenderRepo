using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenderPrototype.Models.DataTransferObjects
{
    public class DtoSendTo
    {
        public string Id { get; set; }
        public virtual DtoRecievers Recievers { get; set; }
        public virtual DtoAnnouncements Announcements { get; set; }
    }
}