using Firebase.Storage;

namespace Storage.Firebase_Storage
{
    public static class FirebaseCloudStorage
    {
        private const string Endpoint = "motor-empire-autohaus.appspot.com";

        public static async Task<string> AddFileToFirebaseCloudStorageAsync(FileResult file, string firebaseCloudPath)
        {
            var fileToUpload = await file.OpenReadAsync();
            var firebase = await new FirebaseStorage(Endpoint)
                .Child($"{firebaseCloudPath}/{file.FileName}").PutAsync(fileToUpload);
            return firebase;
        }

        public static async Task<string> RetrieveUrlFromFirebaseCloudStorageAsync(string path)
        {
            FirebaseStorage storage = new FirebaseStorage(Endpoint);
            var reference = storage.Child(path);
            string link = await reference.GetDownloadUrlAsync();
            return link;
        }

        public static async Task DeleteFirebaseDataFromAsync(string path)
        {
            //gs://motor-empire-autohaus.appspot.com/Images/VehiclePosts/Cars
            //FirebaseStorage storage = new FirebaseStorage($"{path}");
            /*var reference = storage.Child(storage.StorageBucket);*/
            //CrossPlatformMessageRenderer.RenderMessages(storage.StorageBucket);

            //gs://motor-empire-autohaus.appspot.com/Images/VehiclePosts/Cars/38c71df8-5ce7-427a-bb36-1d068a5cb238
            var storage = new FirebaseStorage($"motor-empire-autohaus.appspot.com/Images/VehiclePosts/Cars/{path}");
            var reference = storage.Child("");
            await reference.DeleteAsync();
        }
    }
}