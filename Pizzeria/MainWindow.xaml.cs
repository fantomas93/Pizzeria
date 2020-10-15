using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Pizzeria.Base;
using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pizzeria
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                BaseModel baseModel = new BaseModel();
                baseModel.Window = this;
                baseModel.LoadConfiguration();
                baseModel.Initialize();
                DataContext = baseModel;
            }
            catch (Exception ex)
            {
                ShowMessage("Error", ex.Message);
            }
        }

        internal void ShowMessage(string title, string message)
        {
            this.ShowMessageAsync(title, message);
        }

        private void ConfirmOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                var baseModel = (this.DataContext as BaseModel);
                if (baseModel != null && baseModel.NewOrder != null)
                {
                    if (baseModel.HistoryOrders == null)
                        baseModel.HistoryOrders = new System.Collections.ObjectModel.ObservableCollection<Models.Order>();

                    baseModel.HistoryOrders.Add(baseModel.NewOrder);
                    baseModel.NewOrder.Date = DateTime.Now;
                    baseModel.SaveHistoryOrders();

                    if (!string.IsNullOrEmpty(baseModel.Configuration.Email) && !string.IsNullOrEmpty(baseModel.NewOrder.Recipient))
                        SendEmail(baseModel);

                    baseModel.NewOrder = new Models.Order();
                    ShowMessage("Order compleated", "");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Sending email error", ex.Message);
            }
        }

        private void SendEmail(BaseModel baseModel)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient(baseModel.Configuration.SMTP);
                SmtpServer.Port = baseModel.Configuration.Port;
                SmtpServer.Credentials =
                new System.Net.NetworkCredential(baseModel.Configuration.Email, Password.Password);
                SmtpServer.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(baseModel.Configuration.Email);
                mail.To.Add(baseModel.NewOrder.Recipient);
                mail.Subject = "Pizzeria Mail";
                mail.Body = baseModel.GetEmailBody();

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
