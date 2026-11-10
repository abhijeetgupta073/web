using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class OfflineEncryptionDecryptionController : Controller
    {
        // Secret key used for encryption and decryption (Ensure this is consistent with the JS secretKey)
        private static readonly string SecretKey = "HSE8TSLNTR5NI99212";
        private readonly object ms;

        // Encrypt Method
        [HttpPost]
        public JsonResult Encrypt(string plainText)
        {
            try
            {
                string encryptedData = EncryptData(plainText, GetMs());
                return Json(new { success = true, encryptedData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Encryption failed: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Decrypt Method
        [HttpPost]
        public JsonResult Decrypt(string cipherText)
        {
            try
            {
                string decryptedData = DecryptData(cipherText);
                return Json(new { success = true, decryptedData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Decryption failed: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private object GetMs()
        {
            return ms;
        }

        // AES Encryption Helper
        private string EncryptData(string plainText, object ms)
        {
            var key = Encoding.UTF8.GetBytes(SecretKey);
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = new byte[16]; // Zero IV, matches the JS setup
                aes.Padding = PaddingMode.PKCS7;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var mss = new MemoryStream())
                using (var cs = new CryptoStream(mss, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                    sw.Close(); // Ensure data is written
                }
                return Convert.ToBase64String((byte[])ms); // Return the encrypted data as Base64 string
            }
        }

        // AES Decryption Helper
        private string DecryptData(string cipherText)
        {
            var key = Encoding.UTF8.GetBytes(SecretKey);
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = new byte[16]; // Zero IV, matches the JS setup
                aes.Padding = PaddingMode.PKCS7;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd(); // Return the decrypted data as plain text
                }
            }
        }
    }
}
