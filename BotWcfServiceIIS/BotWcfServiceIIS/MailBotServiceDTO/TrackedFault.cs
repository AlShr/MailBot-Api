using System;
using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract]
    public class TrackedFault
    {
        public TrackedFault()
        {
        }

        public TrackedFault(Guid trackingId, string message, DateTime dateTime)
        {
            TrakingId = trackingId;
            Message = message;
            DateAndTime = dateTime;
        }

        [DataMember]
        public Guid TrakingId { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DateTime DateAndTime { get; set; }
    }
}