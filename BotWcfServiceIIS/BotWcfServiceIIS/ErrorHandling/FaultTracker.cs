using System;
using System.ServiceModel;
using MailBot.Service.MailBotServiceDTO;

namespace MailBot.Service.ErrorHandling
{
    public static class FaultTracker
    {
        public static FaultException<TrackedFault> ReturnTrackedFault(Exception exception, string message)
        {
            var faultException = exception as FaultException<TrackedFault>;
            if (faultException != null)
            {
                return faultException;
            }
            var trackedFault = new TrackedFault( Guid.NewGuid(), exception.Message, DateTime.Now );
            faultException = new FaultException<TrackedFault>( trackedFault,
                new FaultReason( exception.GetType().Name ),
                FaultCode.CreateReceiverFaultCode( new FaultCode( message ) ) );
            return faultException;
        }
    }
}