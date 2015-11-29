//using System;
//using System.Runtime.Remoting.Messaging;

//namespace AccessControl.Data.Session
//{
//    public class UnitOfWork
//    {
//        private readonly ISessionFactoryHolder _sessionFactoryHolder;

//        public UnitOfWork(ISessionFactoryHolder sessionFactoryHolder)
//        {
//            _sessionFactoryHolder = sessionFactoryHolder;
//        }
//    }
//    public static class UnitOfWorkManager
//    {

//        private const string ConversationId = "C48782BB-711E-4b4e-8BDF-95F3885A8823";

//        private static UnitOfWork GetFromStorage()
//        {
//            return CallContext.GetData(ConversationId) as UnitOfWork;
//        }

//        private static void RemoveFromStorage()
//        {
//            System.Runtime.Remoting.Messaging.CallContext.FreeNamedDataSlot(ConversationId);
//        }

//        /// <summary>
//        /// Creates a unit of work, then insert into the current context, finally create the disposable
//        /// action that will dispose the unit of work and remove it from the current context
//        /// </summary>
//        /// <returns></returns>
//        private static IDisposable CreateUnitOfWork()
//        {
//            UnitOfWork newUnitOfWork = new UnitOfWork();
//            System.Runtime.Remoting.Messaging.CallContext.SetData(ConversationId, newUnitOfWork);
//            DisposableAction action = new DisposableAction(
//                delegate () {
//                    newUnitOfWork.Dispose();
//                    RemoveFromStorage();
//                });
//            return action;
//        }

//        /// <summary>
//        /// Get current conversation, if no conversation is active it return a 
//        /// new conversation that will span for the caller use of the conversation itself.
//        /// This means you get a unit of work and you will use it for the time you need.
//        /// If a Unit Of Work is already active, then simply return a weak reference of the 
//        /// unit of work, a weak reference does not dispose the unit of work.
//        /// </summary>
//        /// <returns></returns>
//        public static UnitOfWork GetUnitOfWork()
//        {
//            UnitOfWork current = GetFromStorage();
//            return current == null ? new UnitOfWork() : new UnitOfWorkWeakReference(current);
//        }

//        /// <summary>
//        /// Start a conversation and return a delegate used to automatically close the
//        /// conversation.
//        /// </summary>
//        /// <returns></returns>
//        public static IDisposable BeginContext()
//        {
//            if (IsInConversation)
//                throw new ApplicationException("BeginContext is called but a conversation was already active");
//            return CreateUnitOfWork();
//        }

//        internal static Boolean IsInConversation
//        {
//            get { return GetFromStorage() != null; }
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        public static void EndConversation()
//        {
//            GetUnitOfWork().Close();
//            RemoveFromStorage();
//        }

//        public static void MarkCurrentConversationDirty()
//        {
//            GetFromStorage().MarkDirty();
//        }
//    }

//}