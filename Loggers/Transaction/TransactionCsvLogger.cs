using CreditKiosk.Exceptions;
using CreditKiosk.Loggers.Transaction.LogEntries;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CreditKiosk.Loggers.Transaction
{
    /// <summary>
    /// Supplies the Log-method to be used as a delegate for transaktionManager to log transactions.
    /// </summary>
    public class TransactionCsvLogger
    {
        private string logDirFullPath;

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="logFullPath">Full path to folder where log files should be stored.</param>
        /// <exception cref="ArgumentNullException">logFullPath was null.</exception>
        public TransactionCsvLogger(string logFullPath)
        {
            try
            {
                Directory.CreateDirectory(logFullPath);  // Makes sure directory exist.
            }
            catch (Exception ex)
            {
                throw new CouldNotCreateLogDirException($"Could not create \"{logFullPath}\"");
            }

            this.logDirFullPath = logFullPath ?? throw new ArgumentNullException(nameof(logFullPath));
        }

        /// <summary>
        /// Get full path of csv file from log directory and the persons name.
        /// </summary>
        /// <param name="firstName">First name of person.</param>
        /// <param name="lastName">Last name of person.</param>
        /// <returns>Full path of csv file</returns>
        private string getLogFileFullPathFromNames(string firstName, string lastName) =>
            Path.Join(logDirFullPath, $"{firstName}{lastName}.csv");

        /// <summary>
        /// Writes a record to Csv
        /// </summary>
        /// <typeparam name="T">Type of object to write.</typeparam>
        /// <param name="fileFullPath">Full path to file.</param>
        /// <param name="logEntry">Object to write.</param>
        private void WriteRecordToCsv<T>(string fileFullPath, T logEntry)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = args => args.Row.Index == 6  // Quote comments in the end
            };
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecord(logEntry);
                writer.WriteLine();
            }
        }

        /// <summary>
        /// Logs a deposit transaction.
        /// </summary>
        /// <param name="deposit">Object to log.</param>
        private void LogDeposit(Models.Deposit deposit)
        {
            using (var context = new KioskDbContext())
            {
                // Get entry as it is in database.
#pragma warning disable CS8604 // Possible null reference argument.
                deposit = context.Deposits
                    .Include("Person")
                    .Where(x => x.Id == deposit.Id)
                    .Single();
#pragma warning restore CS8604 // Possible null reference argument.
            }

            string logFileFullPath = getLogFileFullPathFromNames(deposit.Person.FirstName, deposit.Person.LastName);

            DepositLogEntry depositLogEntry = new(deposit);
            WriteRecordToCsv<DepositLogEntry>(logFileFullPath, depositLogEntry);
        }

        /// <summary>
        /// Logs a purchase transaction.
        /// </summary>
        /// <param name="purchase">Object to log.</param>
        private void LogPurchase(Models.Purchase purchase)
        {
            using (var context = new KioskDbContext())
            {
                // Get entry as it is in database.
#pragma warning disable CS8604 // Possible null reference argument.
                purchase = context.Purchases
                    .Include("Person")
                    .Include("ProductGroup")
                    .Where(x => x.Id == purchase.Id)
                    .Single();
#pragma warning restore CS8604 // Possible null reference argument.
            }

            string logFileFullPath = getLogFileFullPathFromNames(purchase.Person.FirstName, purchase.Person.LastName);

            PurchaseLogEntry purchaseLogEntry = new(purchase);
            WriteRecordToCsv<PurchaseLogEntry>(logFileFullPath, purchaseLogEntry);

        }

        /// <summary>
        /// Logs a credit transaction.
        /// </summary>
        /// <param name="credit">Object to log.</param>
        private void LogCredit(Models.Credit credit)
        {
            using (var context = new KioskDbContext())
            {
                // Get entry as it is in database.
#pragma warning disable CS8604 // Possible null reference argument.
                credit = context.Credits
                    .Include("Purchase")
                    .Include("Purchase.Person")
                    .Include("Purchase.ProductGroup")
                    .Where(x => x.Id == credit.Id)
                    .Single();
#pragma warning restore CS8604 // Possible null reference argument.
            }

            string logFileFullPath = getLogFileFullPathFromNames(credit.Purchase.Person.FirstName, credit.Purchase.Person.LastName);
            
            CreditLogEntry creditLogEntry = new(credit);
            WriteRecordToCsv<CreditLogEntry>(logFileFullPath, creditLogEntry);
        }

        /// <summary>
        /// Logs a transaction
        /// </summary>
        /// <param name="transaction">Transaction to log.</param>
        /// <param name="transactionType">Type of transaction.</param>
        /// <exception cref="ArgumentOutOfRangeException">If type not found in enum.</exception>
        public void Log(Models.Transaction transaction, Enums.TransactionType transactionType)
        {
            switch (transactionType)
            {
                case Enums.TransactionType.Deposit:
                    LogDeposit((Models.Deposit)transaction);
                    break;
                case Enums.TransactionType.Purchase:
                    LogPurchase((Models.Purchase)transaction);
                    break;
                case Enums.TransactionType.Credit:
                    LogCredit((Models.Credit)transaction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(transactionType));
            }
        }
    }
}
