using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using SenderPrototype.Models.DataTransferObjects;
using SenderPrototype.Models.EntityModel;

namespace SenderPrototype.Controllers
{
    public class AnnouncementsController : Controller
    {
   
        public SiolSenderPrototype_dbEntities context = new SiolSenderPrototype_dbEntities();
        public List<DtoAnnouncements> announcements = new List<DtoAnnouncements>();
        public List<DtoRecievers> recievers = new List<DtoRecievers>();
        public List<DtoRecieverGroups> groups = new List<DtoRecieverGroups>();

        //public void AddDefaults()
        //{
        //    var defaultClient = new DtoClients
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        Firstname = "Nils",
        //        Lastname = "Karlsson",
        //        GroupId = Guid.NewGuid().ToString(),
        //    };

        //    context.Clients.Add(new Client
        //    {
        //        Id = defaultClient.Id,
        //        Firstname = defaultClient.Firstname,
        //        Lastname = defaultClient.Lastname,
        //        ClientGroupId = defaultClient.GroupId
        //    });

        //    context.SaveChanges();

        //    var clientInGroup = context.Clients.FirstOrDefault(q => q.ClientGroupId == defaultClient.GroupId);
        //    var defaultGroup = new DtoClientGroups
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        CreatedAt = DateTimeOffset.UtcNow,
        //        Groupname = "Test Group",
        //    };

        //    context.ClientGroups.Add(new ClientGroup
        //    {
        //        Id = defaultGroup.Id,
        //        CreatedAt = defaultGroup.CreatedAt,
        //        GroupName = defaultGroup.Groupname,
        //        Client = clientInGroup
        //    });

        //    context.SaveChanges();                      
        //}

        public ActionResult Index()
        {
            //AddDefaults();
            return View();
        }

        public ActionResult DisplayClients()
        {
            foreach (var reciever in context.Recievers)
            {
                var cl = new DtoRecievers
                {
                    Id = reciever.Id,
                    Firstname = reciever.Firstname,
                    Lastname = reciever.Lastname,
                    GroupId = reciever.RecieverGroup.Id,
                    UserId = reciever.UserId
                };

                recievers.Add(cl);
            }

            return View(recievers);
        }

        public ActionResult DisplayGroups()
        {
            foreach (var group in context.RecieverGroups)
            {
                var gr = new DtoRecieverGroups
                {
                    Id = group.Id,
                    Groupname = group.Groupname,
                    CreatedAt = group.CreatedAt
                };

                groups.Add(gr);
            }

            return View(groups);
        }

        public List<string> selectedIds = new List<string>();
        public List<DtoRecievers> clientRecievers = new List<DtoRecievers>();
        public List<DtoRecieverGroups> groupRecievers = new List<DtoRecieverGroups>();
        public static List<DtoSendTo> recieverList = new List<DtoSendTo>();
        public ActionResult GetSelected(IEnumerable<string> selected)
        {
            foreach (var id in selected)
            {
                selectedIds.Add(id);
            }

            var selectedRecievers = selectedIds;
            GetRecieverGroup(selectedRecievers);

            var recievers = new List<DtoSendTo>();

            if (clientRecievers != null)
            {
                GetClientRecievers(recievers);
            }
            if (groupRecievers != null)
            {
                GetGroupRecievers(recievers);
            }

            recieverList = recievers;

            return PartialView(recievers);
        }

        private void GetRecieverGroup(List<string> selected)
        {
            foreach (var id in selected)
            {
                var isClientMatch = context.Recievers.FirstOrDefault(q => q.Id == id);
                var isGroupMatch = context.Recievers.FirstOrDefault(q => q.RecieverGroup.Id == id);

                if (isClientMatch != null)
                {
                    foreach (var reciever in context.Recievers.Where(q => q.Id == isClientMatch.Id))
                    {
                        clientRecievers.Add(new DtoRecievers
                        {
                            Id = reciever.Id,
                            Firstname = reciever.Firstname,
                            Lastname = reciever.Lastname,
                            GroupId = reciever.RecieverGroup.Id,
                            UserId = reciever.UserId
                        });
                    }
                }

                if (isGroupMatch != null)
                {
                    foreach (var group in context.RecieverGroups.Where(q => q.Id == isGroupMatch.Id))
                    {
                        groupRecievers.Add(new DtoRecieverGroups
                        {
                            Id = @group.Id,
                            CreatedAt = @group.CreatedAt,
                            Groupname = @group.Groupname,
                        });
                    }
                }
            }
        }

        private void GetGroupRecievers(List<DtoSendTo> recievers)
        {
            foreach (var reciever in groupRecievers)
            {
                var recieversInGroup = context.Recievers.Where(q => q.RecieverGroup.Id == reciever.Id)
                    .Select(rec => new DtoSendTo
                    {
                        Id = reciever.Id,
                        Recievers = new DtoRecievers
                        {
                            Id = rec.Id,
                            Firstname = rec.Firstname,
                            Lastname = rec.Lastname,
                            GroupId = rec.RecieverGroup.Id,
                            UserId = rec.UserId
                        }
                    });

                foreach (var group in recieversInGroup)
                {
                    recievers.Add(new DtoSendTo
                    {
                        Id = reciever.Id,
                        Recievers = @group.Recievers
                    });
                }
            }
        }

        private void GetClientRecievers(List<DtoSendTo> recievers)
        {
            foreach (var reciever in clientRecievers)
            {
                recievers.Add(new DtoSendTo
                {
                    Id = reciever.Id,
                    Recievers = new DtoRecievers
                    {
                        Id = reciever.Id,
                        Firstname = reciever.Firstname,
                        Lastname = reciever.Lastname,
                        GroupId = reciever.GroupId,
                        UserId = reciever.UserId
                    }
                });
            }
        }

        public ActionResult Announcement()
        {
            var announcement = new DtoSendTo();

            return View(announcement);
        }

        [HttpPost]
        public ActionResult Announcement(DtoSendTo message)
        {
            var selectedRecievers = recieverList;
            var announcement = new DtoAnnouncements
            {
                Id = Guid.NewGuid().ToString(),
                Title = message.Announcements.Title,
                Announcement = message.Announcements.Announcement,
                CreatedAt = DateTimeOffset.Now,
            };
            context.Announcements.Add(new Announcement
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Message = announcement.Announcement,
                CreatedAt = announcement.CreatedAt,
            });
            context.SaveChanges();

            var currentMessage = context.Announcements.FirstOrDefault(q => q.Id == announcement.Id);
            foreach (var reciever in selectedRecievers)
            {
                var currentReciever = context.Recievers.FirstOrDefault(q => q.Id == reciever.Id);
                var sendTo = new Sender { Announcement = currentMessage, Reciever = currentReciever };
                context.Senders.Add(sendTo);
            }
                                
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return View();
        }
    }
}