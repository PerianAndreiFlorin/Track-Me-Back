using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TrackMeBack_Mobile.ClaseUtilitare
{
    internal class Servicii_Criptare
    {
        public static string Criptare(string parola)
        {
            //Generare Hash
            SHA256 hash_sha = SHA256.Create();
            byte[] bytes = hash_sha.ComputeHash(Encoding.UTF8.GetBytes(parola));

            StringBuilder hash_string = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                hash_string.Append(bytes[i].ToString("x2"));
            }
            return hash_string.ToString();
        }
    }
}
