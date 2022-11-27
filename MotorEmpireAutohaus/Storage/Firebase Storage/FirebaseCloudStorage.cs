using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Storage.Firebase_Storage
{
    public static class FirebaseCloudStorage
    {
        static readonly string key = System.IO.File.ReadAllText(@"\MotorEmpireAutohaus\Storage\Firebase Storage\key.txt");

        public static async Task<string> AddFileToFirebaseCloudStorageAsync(FileResult file, string firebaseCloudPath)
        {
            var fileToUpload = await file.OpenReadAsync();
            var firebase = await new FirebaseStorage("gs://motor-empire-autohaus.appspot.com").Child($"{firebaseCloudPath}/{file.FileName}").PutAsync(fileToUpload);
            return firebase;
        }

        public static async Task<string> RetrieveURLFromFirebaseCloudStorageAsync(string path)
        {
            FirebaseStorage storage = new FirebaseStorage(key);
            var reference = storage.Child(path);
            string link = await reference.GetDownloadUrlAsync();
            return link;
        }
    }
}
