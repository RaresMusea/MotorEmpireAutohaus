using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Models.Post_Model;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using MVVM.Services.Car_Post_Services;
using MVVM.View_Models.Base;
using RestSharp;
using System.Net;
using System.Net.Mail;
using Tools.Handlers;
using Tools.Utility.Messages;

namespace MVVM.View_Models.Post_Info
{
    [QueryProperty(nameof(Car), nameof(Car))]
    [QueryProperty(nameof(LoggedInUser), nameof(LoggedInUser))]
    public partial class PostInfoViewModel : BaseViewModel
    {

        private readonly CarPostService postService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ContactOwnerText))]
        private CarPost post;

        [ObservableProperty]
        private string loggedInUser;

        public string ContactOwnerText => $"Contact {post.Owner.Name} via ";

        [ObservableProperty]
        private string contactViaEmail;

        [ObservableProperty]
        private string contactViaPhone;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotOwner))]
        private bool isOwner;

        public bool IsNotOwner => !isOwner;

        public PostInfoViewModel(CarPostService postService)
        {
            this.postService = postService;
            post = SelectedCarPost.Selected;
            contactViaEmail = ContactOwnerText + "email";
            contactViaPhone = ContactOwnerText + "phone number";
            isOwner = Logger.CurrentlyLoggedInUuid.Equals(post.Owner.Uuid);

        }

        [RelayCommand]
        private void ToggleContactOptions()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                MouseHandler.mouse_event(MouseHandler.MouseeventRightDown | MouseHandler.MouseeventRightUp,
                    0, 0, 0, 0);
            }
        }

        [RelayCommand]
        public void SendEmailToOwner()
        {
            try
            {
                /*var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("username", "password"),
                    EnableSsl = true,
                };

                var mailConfig = new MailMessage
                {
                    From = new MailAddress(Logger.CurrentlyLoggedInEmail),
                    Subject = $"Motor Empire Autohaus - Post: {post.HeadingTitle}",
                    Body = $"<div style='font-family:Arial'>" +
                           $"<h3>Hello, {post.Owner.Name}</h3>" +
                           $"<p>I just saw the ad posted by you on Motor Empire Autohaus about " +
                           $"your vehicle(a {post.Car.Manufacturer} {post.Car.Model}) and all I can say is that" +
                           $" I was pleasantly surprised and I would really like to know more about it, to meet" +
                           $" and if everything goes well, event to buy it.</p>" +
                           $"<p>If the ad is still available I would like to get in touch with you as soon as possible.</p>" +
                           $"<br>" +
                           $"<p>Thank you in advance!</p>" +
                           $"<p>Best regards,</p>" +
                           $"<p style='text-align:right>{Logger.CurrentlyLoggedInName}</p>" +
                           $"</div>",
                    IsBodyHtml = true
                };

                mailConfig.To.Add(post.Owner.EmailAddress);
                smtpClient.Send(mailConfig);*/

                /* var client = new SmtpClient("smtp.mailtrap.io", 2525)
                 {
                     Credentials = new NetworkCredential("94a1973b017036", "4e28c270dd7878"),
                     EnableSsl = true
                 };
                 client.Send("from@example.com", "to@example.com", "Test", "testbody");*/


                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress(Logger.CurrentlyLoggedInEmail);
                    mail.To.Add(post.Owner.EmailAddress);
                    mail.Subject = "Test Mail - 1";
                    mail.Body = "This is a test";

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential();
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);

            }
            catch (Exception)
            {
                CrossPlatformMessageRenderer.RenderMessages("Unable to process your request! This feature is still in development!", "I understand", 3);
            }
        }

        [RelayCommand]
        private void DialOwner()
        {

                if (post.Owner.PhoneNumber is not null)
                {
                    if (PhoneDialer.Default.IsSupported)
                    {
                        PhoneDialer.Default.Open($"{post.Owner.PhoneNumber}");
                        return;
                    }
                    CrossPlatformMessageRenderer.RenderMessages("Unfortunately, your device does not support dialing! " +
                        "Please try again on other platform such as a mobile device!", "Ok", 3);
                    return;
                }

                CrossPlatformMessageRenderer.RenderMessages($"{post.Owner.Name} has not provided any phone number within the app!", "Ok", 3);

        }

    }

}
