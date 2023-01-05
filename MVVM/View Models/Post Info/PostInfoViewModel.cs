using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth;
using MVVM.Models.Post_Model;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using MVVM.Services.Car_Post_Services;
using MVVM.View_Models.Base;
using Tools.Handlers;
using Tools.Utility.Messages;
using EASendMail;
using MVVM.View.Post_Feed;
using MVVM.View.Favorite_Posts;

namespace MVVM.View_Models.Post_Info
{
    [QueryProperty(nameof(Car), nameof(Car))]
    [QueryProperty(nameof(LoggedInUser), nameof(LoggedInUser))]
    [QueryProperty(nameof(UpdateNeeded),nameof(UpdateNeeded))]
    public partial class PostInfoViewModel : BaseViewModel
    {

        private readonly CarPostService postService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ContactOwnerText))]
        private CarPost post;

        [ObservableProperty]
        private bool updateNeeded;

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

        [ObservableProperty]
        private string favoritesText;

        public PostInfoViewModel(CarPostService postService)
        {
            this.postService = postService;
            post = SelectedCarPost.Selected;
            contactViaEmail = ContactOwnerText + "email";
            contactViaPhone = ContactOwnerText + "phone number";
            isOwner = Logger.CurrentlyLoggedInUuid.Equals(post.Owner.Uuid);
            favoritesText = post.AddedToFavoritesByCurrentUser ? "Remove from favorites" : "Add to favorites";

        }

        [RelayCommand]
        private async void ToggleContactOptions()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                MouseHandler.mouse_event(MouseHandler.MouseeventRightDown | MouseHandler.MouseeventRightUp,
                    0, 0, 0, 0);
            }
            else
            {
                string action = await Application.Current!.MainPage!.DisplayActionSheet($"Contact {post.Owner.Name}",
                    "Cancel", null, contactViaEmail, contactViaPhone);

                if (action == contactViaPhone)
                {
                   DialOwner();
                }
                else
                {
                    await SendEmailToOwner();
                }
            }
        }

        private async void AddPostToFavorites()
        {
            UpdateNeeded = true;
            if (postService.AddToFavorites(post.Uuid, Logger.CurrentlyLoggedInUuid))
            {
                CrossPlatformMessageRenderer.RenderMessages("Succesfully added this post to your favorites list!", "Ok", 4);
                FavoritesText = "Remove from favorites";
                if (PostInfoConfigurer.OnFavorites == true)
                {
                    await Shell.Current.GoToAsync($"{nameof(FavoritePosts)}?UpdateNeeded={UpdateNeeded}", true);
                    UpdateNeeded = false;
                }
                return;
            }

            CrossPlatformMessageRenderer.RenderMessages("Could not process your request due to an unexpected error!", "Retry", 4);
        }

        private void RemovePostFromFavorites()
        {
            UpdateNeeded = true;
            if (postService.RemoveFromFavorites(post.Uuid))
            {
                CrossPlatformMessageRenderer.RenderMessages("Sucessfully removed post from your favorites list!", "Ok", 4);
                FavoritesText = "Add to favorites";
                if (PostInfoConfigurer.OnFavorites == true)
                {
                    Shell.Current.GoToAsync($"{nameof(FavoritePosts)}?UpdateNeeded={UpdateNeeded}", true);
                    UpdateNeeded = false;
                }
                return;
            }

            CrossPlatformMessageRenderer.RenderMessages("Cannot remove post from your favorites list due to an unexpected error! " +
                "Please try again later.", "Ok", 4);

        }

        [RelayCommand]
        private async Task SendEmailToOwner()
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


            try {
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = "122973338838-08hsndn2u2fijgkh1pqktufii1bud427.apps.googleusercontent.com",
                        ClientSecret = "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"
                    },
                    new[] { "email", "profile", "https://mail.google.com/" },
                    "user",
                    CancellationToken.None
                );

                var jwtPayload = GoogleJsonWebSignature.ValidateAsync(credential.Token.IdToken).Result;
                var username = jwtPayload.Email;

                SmtpServer oServer = new SmtpServer("smtp.gmail.com");
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
                oServer.Port = 587;

                oServer.AuthType = SmtpAuthType.XOAUTH2;
                oServer.User = username;
                
                oServer.Password = credential.Token.AccessToken;

                SmtpMail oMail = new SmtpMail(oServer.Password);
                oMail.From = username;
                oMail.To = post.Owner.EmailAddress;

                oMail.Subject = "test email from gmail account with OAUTH 2";
                oMail.TextBody = "this is a test email sent from c# project with gmail.";


                EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();
                oSmtp.SendMail(oServer, oMail);

                CrossPlatformMessageRenderer.RenderMessages("Succesfully sent!", "OK", 4);
            }
            catch(Exception)
            {
                CrossPlatformMessageRenderer.RenderMessages("BETA TESTING\n\nThis feature is still under development! It will be soon available.", "Retry", 4);
            }
        }



        [RelayCommand]
        private void DialOwner()
        {

            if (post.Owner.PhoneNumber is not null)
            {
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    CrossPlatformMessageRenderer.RenderMessages($"You can get in touch with " +
                        $"{post.Owner.Name} by dialing {post.Owner.PhoneNumber}", "Ok", 5);
                }
                else
                {

                    if (PhoneDialer.Default.IsSupported)
                    {
                        PhoneDialer.Default.Open($"{post.Owner.PhoneNumber}");

                    }
                    CrossPlatformMessageRenderer.RenderMessages("Unfortunately, your device does not support dialing! " +
                    "Please try again on other platform such as a mobile device!", "Ok", 3);

                }
            }
            else
            {
                CrossPlatformMessageRenderer.RenderMessages($"{post.Owner.Name} has not provided any phone number within the app!", "Ok", 3);                
            }
        }

        [RelayCommand]
        private void AddOrRemoveFavoritePost()
        {

            if (FavoritesText == "Add to favorites")
            {
                AddPostToFavorites();
                return;
            }

            RemovePostFromFavorites();
        }

        [RelayCommand]
        private async void DeletePost()
        {
            bool answerIsPositive = await Application.Current!.MainPage!.DisplayAlert(
                "Motor Empire Autohaus - Delete post",
                "Are you sure that you want to delete this post?\nAll the provided information about this ad will be permanently lost!\n",
                "Proceed to post deletion",
                "Cancel");

            if (!answerIsPositive) return;

            if (postService.DeletePost(post))
            {
                CrossPlatformMessageRenderer.RenderMessages(
                    $"Post deleted succesfully.\n You'll be automatically redirected to the main page of the application...",
                    "Alright", 4);
                await Shell.Current.GoToAsync($"{nameof(PostFeed)}",
                    true, new Dictionary<string, object>
                    {
                        {"UpdateNeeded",true },
                    });
            }
        }

    }

}
