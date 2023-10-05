using Helper.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Mail
{
    public class MailHelper
    {
        public static void SendMail(string message, string para = "", string filePath = "", string subject = "")
        {
            string Mail_To = String.IsNullOrEmpty(para) ? ConfigurationManager.AppSettings["Mail_To"].ToString() : para;
            string Mail_From = ConfigurationManager.AppSettings["Mail_From"].ToString();
            string Mail_Server = ConfigurationManager.AppSettings["Mail_Server"].ToString();
            string Mail_Port = ConfigurationManager.AppSettings["Mail_Port"].ToString();
            string Mail_Subjet = "";
          
            if (String.IsNullOrEmpty(subject))
            {
                Mail_Subjet = ConfigurationManager.AppSettings["Mail_Subjet"].ToString();
                Mail_Subjet += " " + IpAddressHelper.HostNameIpAddressToString();
            }
            else 
            {
                Mail_Subjet = subject;
            }
            SendMail(Mail_To, Mail_From, Mail_Server, Mail_Port, Mail_Subjet, message, filePath);
        }
        public static void SendMail_Error_Solicitud(string message, string addSubject)
        {
            string Mail_Error = ConfigurationHelper.GetValue("Configuration", "Mail_Error_Solicitud");
            string Mail_From = ConfigurationHelper.GetValue("Configuration", "Mail_From");
            string Mail_Server = ConfigurationHelper.GetValue("Configuration", "Mail_Server");
            string Mail_Port = ConfigurationHelper.GetValue("Configuration", "Mail_Port");
            string Mail_Subjet = ConfigurationHelper.GetValue("Configuration", "Mail_Subjet");
            Mail_Subjet += " " + addSubject;
            Mail_Subjet += " " + IpAddressHelper.HostNameIpAddressToString();
            SendMail(Mail_Error, Mail_From, Mail_Server, Mail_Port, Mail_Subjet, message);
        }

        private static void SendMail(string Mail_To ,string Mail_From ,string Mail_Host ,string Mail_Port ,string Mail_Subjet ,string Mail_Body, string filePath = "")
        {
            LogHelper.GetInstance().PrintDebug("Send Mail");
            System.Net.Mail.MailMessage Mail = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            try
            {
                if ((Mail_To.ToString().Contains(",")))
                {
                    foreach (string cc in Mail_To.ToString().Split(','))
                    {
                        if ((cc != String.Empty))
                        {
                            Mail.To.Add(cc);
                        }
                    }
                }
                else
                {
                    Mail.To.Add(Mail_To.ToString());
                }
                Mail.IsBodyHtml = true;
                Mail.From = new System.Net.Mail.MailAddress(Mail_From);
                Mail.Subject = Mail_Subjet;
                Mail.Body = "<html>" + Mail_Body + "</html>";
                if (!String.IsNullOrEmpty(filePath))
                {
                    Attachment data = new Attachment(filePath, MediaTypeNames.Application.Octet);
                    Mail.Attachments.Add(data);
                }
                try
                {
                    smtp.Port = Convert.ToInt32(Mail_Port);
                }
                catch (Exception e)
                {
                    LogHelper.GetInstance().PrintError(e);
                    throw e;
                }
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Host = Mail_Host;
                smtp.Send(Mail);
                LogHelper.GetInstance().PrintDebug("Send Mail FIN");
            }
            catch (Exception ex)
            {
                LogHelper.GetInstance().PrintError(ex);
                throw ex;
            }
            finally
            {
                Mail = null;
                smtp = null;
            }

        }

    }
}
