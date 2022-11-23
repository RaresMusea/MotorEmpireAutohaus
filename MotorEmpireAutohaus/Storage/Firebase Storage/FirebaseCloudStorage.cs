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
        public static void AddFileToFirebaseCloudStorage(FileResult file)
        {
            string key = System.IO.File.ReadAllText("key.txt");
            //FirebaseStorage firebaseStorage = await new FirebaseStorage(key).Child(file.FileName).PutAsync(() => { file.OpenReadAsync(); });
        }
    }
}
