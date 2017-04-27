using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Pyramid.Tools
{
    public static class Cryptography
    {
        private static AesCryptoServiceProvider aesCrypt = null;

        static Cryptography()
        {
            aesCrypt = new AesCryptoServiceProvider();
        }

        /// <summary>
        /// Вычислить хеш от входного слова.
        /// </summary>
        /// <param name="open">Открытый текст</param>
        /// <returns></returns>
        public static string GetHash(string open)
        {
            var sha1 = new SHA512CryptoServiceProvider();
            var pswBytes = sha1.ComputeHash(Encoding.Unicode.GetBytes(open));
            var hash = Convert.ToBase64String(pswBytes);
            return hash;
        }

        /// <summary>
        /// Сгенерировать случайную последовательность заданной длины.
        /// </summary>
        /// <param name="length">Длина выходной последовательности</param>
        /// <returns></returns>
        public static string GenerateCode(int length = 10)
        {
            var dict = "abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()-_+=<>:;";
            var rnd = new Random();
            var res = new StringBuilder();
            for (int i = 0; i < length; i++)
                res.Append(dict[rnd.Next(0, dict.Length)]);
            return res.ToString();
        }

        /// <summary>
        /// Получить зашифрованное сообщение.
        /// </summary>
        /// <param name="open">Текст для шифрования</param>
        /// <param name="key">Ключ шифрования</param>
        /// <param name="salt">Соль</param>
        /// <returns></returns>
        public static string GetUserCode(string open, string key, string salt)
        {
            int temp = aesCrypt.KeySize;
            string text = DateTime.Now.Ticks + " " + open;
            var encryptor = aesCrypt.CreateEncryptor(UnicodeEncoding.Unicode.GetBytes(key),
                UnicodeEncoding.Unicode.GetBytes(salt));
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                    swEncrypt.Write(text);
                var array = msEncrypt.ToArray();
                string result = "";
                foreach (byte item in array)
                    result += item.ToString("x2");
                return result;
            }
        }

        /// <summary>
        /// Получить расшифрованное сообщение.
        /// </summary>
        /// <param name="cipherText">Шифротекст</param>
        /// <param name="key">Ключ</param>
        /// <param name="salt">Соль</param>
        /// <returns></returns>
        public static string ValidateUserCode(string cipherText, string key, string salt)
        {
            try
            {
                string text;
                if (cipherText.Length % 2 == 1)
                    return null;
                var array = new byte[cipherText.Length / 2];
                for (int i = 0; i < cipherText.Length; i += 2)
                    array[i / 2] = byte.Parse(cipherText.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                var decryptor = aesCrypt.CreateDecryptor(UnicodeEncoding.Unicode.GetBytes(key),
                    UnicodeEncoding.Unicode.GetBytes(salt));
                using (MemoryStream msDecrypt = new MemoryStream(array))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader swDecrypt = new StreamReader(csDecrypt))
                    text = swDecrypt.ReadToEnd();
                int spacePos = text.IndexOf(' ');
                if (spacePos == -1)
                    return null;
                long ticks = long.Parse(text.Substring(0, spacePos));
                DateTime time = new DateTime(ticks);
                if (time.AddHours(24).CompareTo(DateTime.Now) < 0)
                    return null;
                string email = text.Substring(spacePos + 1);
                return email;
            }
            catch
            {
                return null;
            }
        }
    }
}