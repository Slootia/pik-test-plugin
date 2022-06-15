using Autodesk.Revit.DB;
using PikTestPlugin.Models;

namespace PikTestPlugin
{
    public static class PluginTransaction
    {
        private static readonly Transaction _transaction = new Transaction(RevitStaticData.Document);
        private static bool _isStarted = false;
        private const string _transactionMessage = "Изменение тона у прилежащих типов помещений";

        public static Transaction RevitTransaction => _transaction;
        public static bool IsStarted 
        { 
            get
            {
                _isStarted = IsTransactionStarted();
                return _isStarted;
            }
        }

        public static string TransactionMessage => _transactionMessage;

        private static bool IsTransactionStarted()
        {
            if (_transaction.GetStatus() == TransactionStatus.Started ||
                _transaction.GetStatus() == TransactionStatus.Committed)
            {
                _isStarted = true;
            }
            else
                _isStarted = false;

            return _isStarted;
        }

        public static void Start()
        {
            _transaction.Start(_transactionMessage);
            _isStarted = IsTransactionStarted();
        }

        public static void Start(string transactionMessage)
        {
            _transaction.Start(transactionMessage);
            _isStarted = IsTransactionStarted();
        }

        public static void Commit()
        {
            _transaction.Commit();
            _isStarted = IsTransactionStarted();
        }
    }
}
