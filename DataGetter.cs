using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace StudentDataGetterApp {
    internal class DataGetter {
        public FileInfo CookieFile { get; set; } = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\User Data\\Default\\Network\\Cookies");

        public FileInfo StateFileOrigin { get; set; } = new FileInfo("C:\\Users\\Username\\AppData\\Local\\Google\\Chrome\\User Data\\Local State");
        public int? QueryYearLower { get; set; }
        public int? QueryYearUpper { get; set; }
        public string Cookie { get; set; }

        public Dictionary<Department, SortedSet<Student>> StudentSet { get; } = new();

        public async Task StartFetchingAsync(Action action) {
            //GetCookieKey();
            await FetchingAsync(action);
        }
        /*
        private void GetCookieKey() {
            FileInfo StateFile = CopyFile(StateFileOrigin, "Local State");
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
        }
        */
        private async Task FetchingAsync(Action action) {
            var query = new Queryer(Cookie, StudentSet);
            for (int year = (int)QueryYearLower; year <= (int)QueryYearUpper; year++) {
                foreach (var department in Department.日間部學士班) {
                    string departmentId = department.Id;
                    if (departmentId.Length == 2) {
                        string studentId7 = $@"s4{year}{departmentId}";
                        bool alreadyGetAllStudent = false;
                        for (int i = 0; i < 10; i++) {
                            if (alreadyGetAllStudent) {
                                break;
                            }
                            string studentId8 = $"{studentId7}{i}";
                            bool remain = await query.GetStudentData(studentId8, 100);
                            if (!remain) {
                                alreadyGetAllStudent = true;
                                continue;
                            }
                            for(int j = 5; j <= 9; j++) {
                                string studentId9 = $"{studentId8}{j}";
                                bool res = await query.GetStudentData(studentId9, 10);
                                if (!res) {
                                    break;
                                }
                            }
                        }
                    } 
                    else {
                        string studentId8 = $@"s4{year}{departmentId}";
                        await query.GetStudentData(studentId8, 100);
                    }
                    //增加進度條
                    action();
                }
            }
        }
        
        public void SaveData() {
            string json = JsonConvert.SerializeObject(StudentSet, Formatting.Indented);
            Directory.CreateDirectory(".\\result");
            File.Create(".\\result\\data.json").Close();
            File.WriteAllText(".\\result\\data.json", json);
        }
#if null
        private string GetCookie() {
            using (var connection = new SqlConnection()) {
                connection.ConnectionString = $@"Data Source={CookieFile.FullName};Read Only=True;";
                connection.Open();
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
            }
            return "";
        }
        */

        private FileInfo CopyFile(FileInfo file, string fileName) {
            string tempPath = $@"{Directory.GetCurrentDirectory()}\temp";
            if (!Directory.Exists(tempPath)) {
                Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\temp");
            }
            if (file.Exists) {
                file.CopyTo(tempPath + $@"\{fileName}", true);
            }
            return new FileInfo(tempPath + $@"\{fileName}");
        }
#endif
    }


}
