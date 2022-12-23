using Firebase.Storage;

namespace MotorEmpireAutohaus.Storage.Firebase_Storage
{
    public static class FirebaseCloudStorage
    {
        static readonly string endpoint = "motor-empire-autohaus.appspot.com";

        public static async Task<string> AddFileToFirebaseCloudStorageAsync(FileResult file, string firebaseCloudPath)
        {
            var fileToUpload = await file.OpenReadAsync();
            var firebase = await new FirebaseStorage(endpoint)
                .Child($"{firebaseCloudPath}/{file.FileName}").PutAsync(fileToUpload);
            return firebase;
        }

        public static async Task<string> RetrieveUrlFromFirebaseCloudStorageAsync(string path)
        {
            FirebaseStorage storage = new FirebaseStorage(endpoint);
            var reference = storage.Child(path);
            string link = await reference.GetDownloadUrlAsync();
            return link;
        }
    }
}