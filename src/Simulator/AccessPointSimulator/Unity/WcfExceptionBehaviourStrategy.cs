using System;
using System.ServiceModel;
using ChannelAdam.ServiceModel;
using log4net;

namespace AccessPointSimulator.Unity
{
    public class WcfExceptionBehaviourStrategy : IServiceConsumerExceptionBehaviourStrategy
    {
        private const string NullPhrase = "Exception is null";

        private static readonly ILog Log = LogManager.GetLogger(typeof(WcfExceptionBehaviourStrategy));

        public static WcfExceptionBehaviourStrategy Instance { get; } = new WcfExceptionBehaviourStrategy();

        public void PerformAbortExceptionBehaviour(Exception exception)
        {
            LogException(exception);
        }

        public void PerformCloseCommunicationExceptionBehaviour(CommunicationException exception)
        {
            LogException(exception);
        }

        public void PerformCloseTimeoutExceptionBehaviour(TimeoutException exception)
        {
            LogException(exception);
        }

        public void PerformCloseUnexpectedExceptionBehaviour(Exception exception)
        {
                
        }

        public void PerformCommunicationExceptionBehaviour(CommunicationException exception)
        {
            LogException(exception);
        }

        public void PerformFaultExceptionBehaviour(FaultException exception)
        {
            LogException(exception);
        }

        public void PerformTimeoutExceptionBehaviour(TimeoutException exception)
        {
            LogException(exception);
        }

        public void PerformUnexpectedExceptionBehaviour(Exception exception)
        {
            LogException(exception);
        }

        public void PerformDestructorExceptionBehaviour(Exception exception)
        {
            LogException(exception);
        }

        private void LogException(Exception exception)
        {
            if (exception != null)
                Log.Error(exception);
            else
                Log.Error(NullPhrase);
        }
    }
}