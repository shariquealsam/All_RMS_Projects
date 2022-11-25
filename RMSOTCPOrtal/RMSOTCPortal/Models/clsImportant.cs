using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RMSOTCPortal.Models
{
    public class clsImportant
    {
        //Function to create a log file
        public string CreateLogFiles()
        {
            try
            {
                //sLogFormat used to create log files format dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
                string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                string sErrorTime = string.Empty;

                //this variable used to create log filename format for example filename : ErrorLogYYYYMMDD
                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();

                if (sMonth.Length == 1)
                {
                    sMonth = "0" + sMonth;
                }
                return sErrorTime = sYear + sMonth + sDay + ".txt";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Capture Error Log
        public void ErrorLog(string sPathName, string sErrMsg)
        {
            try
            {
                //Variables Declaration
                string sErrorTime = CreateLogFiles();
                //Create a instance of stream writer
                StreamWriter srWriter = new StreamWriter(sPathName + sErrorTime, true);
                //Write error message
                srWriter.WriteLine(sErrMsg);
                //Clear Writer
                srWriter.Flush();
                //Close Stream Writer
                srWriter.Close();
            }
            catch (Exception exErrorLog)
            {
                //throw;
            }
        }

        //Encrypt Password
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        //Decrypt Password
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        //Function to Send Email
        public bool SendMailMessage(string from, string to, string bcc, string cc, string subject, string body)
        {
            bool bMessageSent = false;
            try
            {
               // bool bMessageSent = false;

                // Instantiate a new instance of MailMessage
                MailMessage mMailMessage = new MailMessage();

                // Set the sender address of the mail message
                mMailMessage.From = new MailAddress(from);

                // Set the recepient address of the mail message
                mMailMessage.To.Add(new MailAddress(to));

                // Check if the bcc value is null or an empty string
                if ((bcc != null) && (bcc != string.Empty))
                {
                    // Set the Bcc address of the mail message
                    mMailMessage.Bcc.Add(new MailAddress(bcc));
                }

                // Check if the cc value is null or an empty value
                if ((cc != null) && (cc != string.Empty))
                {
                    // Set the CC address of the mail message
                    mMailMessage.CC.Add(new MailAddress(cc));
                }

                // Set the subject of the mail message
                mMailMessage.Subject = subject;

                // Set the body of the mail message
                mMailMessage.Body = body;

                // Set the format of the mail message body as HTML
                mMailMessage.IsBodyHtml = true;

                // Set the priority of the mail message to normal
                mMailMessage.Priority = MailPriority.Normal;

                // Instantiate a new instance of SmtpClient
                SmtpClient mSmtpClient = new SmtpClient("mail.sisprosegur.com", 587);

                //Enable SSl Certificate
                mSmtpClient.EnableSsl = true;

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("support@sisprosegur.com", "India@321");

                //Assigning credentials in mSmtpClient
                mSmtpClient.Credentials = credentials;

                // Send the mail message
                mSmtpClient.Send(mMailMessage);

                bMessageSent = true;

                return bMessageSent;
            }
            catch (Exception exSendMailMessage)
            {
               // throw;
            }
            return bMessageSent;
        }
    }
}