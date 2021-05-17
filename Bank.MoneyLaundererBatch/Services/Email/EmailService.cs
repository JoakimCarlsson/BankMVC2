using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Bank.MoneyLaundererBatch.ReportObjects;
using MailKit.Net.Smtp;
using MimeKit;

namespace Bank.MoneyLaundererBatch.Services.Email
{
    class EmailService : IEmailService
    {
        public async Task SendReportEmailAsync(string country, IEnumerable<CustomerReport> reports)
        {
            //testbanken

            var message = FormateMessage(country, reports);
            
            var mimeMessage = new MimeMessage
            {
                From = {new MailboxAddress("NoReply", "Automated@testbanken.se")},
                To = {new MailboxAddress($"{country}Bank", $"{country}.testbanken.se")},
                Subject = "Suspicious Customers",
                Body = message
            };

            using var client = new SmtpClient ();
            await client.ConnectAsync ("smtp.mailtrap.io", 2525, false);
            await client.AuthenticateAsync ("11a87d096f4a2b", "799ee125cfea76");
            await client.SendAsync (mimeMessage);
            await client.DisconnectAsync (true);
        }

        private TextPart FormateMessage(string country, IEnumerable<CustomerReport> reports)
        {
            var body = new TextPart("plain");
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"{DateTime.Now:D} | {country}");
            stringBuilder.AppendLine();
            
            foreach (CustomerReport customerReport in reports)
            {
                stringBuilder.AppendLine("\n");
                stringBuilder.AppendLine($"Customer ID: {customerReport.Id}, Name: {customerReport.Name}");
                stringBuilder.AppendLine("\tAccounts:");
                
                foreach (var accountReport in customerReport.Accounts)
                {
                    stringBuilder.AppendLine($"\t\t Account ID: {accountReport.Id}");
                    stringBuilder.AppendLine("\tTransactions:");
                    foreach (var transactionReport in accountReport.Transactions)
                    {
                        stringBuilder.AppendLine($"\t\tTransaction ID {transactionReport.Id}, Amount: {transactionReport.Amount}, Date: {transactionReport.Date:D}");
                    }
                }
            }


            body.Text = stringBuilder.ToString();
            return body;
        }
    }
}