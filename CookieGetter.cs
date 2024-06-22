//using Devart.Data.SQLite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StudentDataGetterApp {
    internal class CookieGetter {
        public static string GetKey(FileInfo file) {
            FileInfo StateFile = CopyFile(file, "Local State");
            string cookieKey = "";
            try {
                string stateJsonStr = File.ReadAllText(StateFile.FullName);
                cookieKey = JObject.Parse(stateJsonStr)["os_crypt"]["encrypted_key"].ToString();
                if (cookieKey == null || cookieKey == "") {
                    throw new Exception("Cannot find cookie key");
                }
            } finally {
                if (File.Exists(StateFile.FullName)) {
                    File.Delete(StateFile.FullName);
                }
            }
            return cookieKey;
        }

        public static async Task<string> GetCookieAsync(FileInfo CookieFile, string key) {
            using var db = new SQLiteConnection();
            /*connection.ConnectionString = $@"Data Source={CookieFile.FullName};";
            await connection.OpenAsync();*/
            Console.WriteLine(CookieFile.FullName);
            db.ConnectionString = $@"Data Source={CookieFile.FullName};Read Only=True;FailIfMissing=True;";
            db.Open();
            /*
            SqliteCommand command = new SqliteCommand("select host_key,name,encrypted_value from cookies where name='public-token' and host_key='.cponline.cnipa.gov.cn'", connection);
            SqliteDataReader dataReader = command.ExecuteReader();
            dataReader.Read();
            byte[] encryptedValue = (byte[])dataReader["encrypted_value"];
            int keyLength = 256 / 8;
            int nonceLength = 96 / 8;
            String kEncryptionVersionPrefix = "v10";
            int GCM_TAG_LENGTH = 16;
            byte[] encryptedKeyBytes = Convert.FromBase64String(cookieKey);
            encryptedKeyBytes = encryptedKeyBytes.Skip("DPAPI".Length).Take(encryptedKeyBytes.Length - "DPAPI".Length).ToArray();
            var keyBytes = System.Security.Cryptography.ProtectedData.Unprotect(encryptedKeyBytes, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
            var nonce = encryptedValue.Skip(kEncryptionVersionPrefix.Length).Take(nonceLength).ToArray();
            encryptedValue = encryptedValue.Skip(kEncryptionVersionPrefix.Length + nonceLength).Take(encryptedValue.Length - (kEncryptionVersionPrefix.Length + nonceLength)).ToArray();
            return WebUtility.UrlDecode(A);*/
            return "";
        }

        private static FileInfo CopyFile(FileInfo file, string fileName) {
            string tempPath = @".\temp";
            if (!Directory.Exists(tempPath)) {
                Directory.CreateDirectory(@".\temp");
            }
            if (file.Exists) {
                file.CopyTo($@"{tempPath}\{fileName}", true);
            }
            return new FileInfo($@"{tempPath}\{fileName}");
        }
    }
}
