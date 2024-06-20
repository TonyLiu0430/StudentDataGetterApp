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

        public async Task StartFetchingAsync() {
            //GetCookieKey();
            MessageBox.Show("START", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            await FetchingAsync();
            MessageBox.Show("END", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private async Task FetchingAsync() {
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
                            bool remain = await query.GetStudentData(studentId8);
                            if (!remain) {
                                alreadyGetAllStudent = true;
                            }
                        }
                    } 
                    else {
                        string studentId8 = $@"s4{year}{departmentId}";
                        await query.GetStudentData(studentId8);
                    }
                }
            }

            MessageBox.Show(StudentSet.Count.ToString(), "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
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
#endif
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
    }

    
}
/*
using (SqliteConnection connection = new SqliteConnection()) {
    //1、获取google Chrome浏览器目录
    var userprofilePath = Environment.GetEnvironmentVariable("USERPROFILE");
    var sourceFile = $@"{userprofilePath}\AppData\Local\Google\Chrome\User Data\Default\Network\Cookies";
    //var sourceFile = $@"{userprofilePath}\Desktop\11111111.txt";
    var targetFile = $@"{Directory.GetCurrentDirectory()}\GoogleFile\Cookies";

    //2、拷贝文件到本地目录，防止文件被占用
    FileInfo file = new FileInfo(sourceFile);
    if (!Directory.Exists($@"{Directory.GetCurrentDirectory()}\GoogleFile"))
        Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\GoogleFile");
    if (file.Exists) {
        file.CopyTo(targetFile, true);
    }

    //3、链接sqlite数据库
    connection.ConnectionString = $@"DataSource=" + targetFile;
    connection.Open();
    //4、通过select查询相关的语句
    SqliteCommand command = new SqliteCommand("select host_key,name,encrypted_value from cookies where name='public-token' and host_key='.cponline.cnipa.gov.cn'", connection);
    SqliteDataReader dataReader = command.ExecuteReader();
    dataReader.Read();
    byte[] encryptedValue = (byte[])dataReader["encrypted_value"];

    //5、解密数据
    int keyLength = 256 / 8;
    int nonceLength = 96 / 8;
    String kEncryptionVersionPrefix = "v10";
    int GCM_TAG_LENGTH = 16;
    //字符串内容取自C:\Users\用户名\AppData\Local\Google\Chrome\User Data\Local State文件的encrypted_key
    byte[] encryptedKeyBytes = Convert.FromBase64String("RFBBUEkBAAAA0Iyd3wEV0RGMegDAT8KX6wEAAACVAmSA/y7+TLs+3WWdDv1ZAAAAAAIAAAAAABBmAAAAAQAAIAAAABV7dDMB8p+vKnLEjnrhnWB4DAbB/k5XAtjWGFnci/3qAAAAAA6AAAAAAgAAIAAAAH/pnc+fF6dhG8Fpw6yQezIXtMw48xNvuyRub/cZ62XaMAAAAP1pl5QqRJmd1J4V++dhE63MEA9F4NzCHb1aOMgTnFCo1+xSHYovSTzCoYFvoDfIFUAAAAAZzDzWwwpUm6yZG9tpYu/ioRSO8V16MetQy2s7L9HHO03Q6bO8Nr05Erl1QbjCVoSgSOU4krcerUsngMwIYFyb");
    encryptedKeyBytes = encryptedKeyBytes.Skip("DPAPI".Length).Take(encryptedKeyBytes.Length - "DPAPI".Length).ToArray();
    var keyBytes = System.Security.Cryptography.ProtectedData.Unprotect(encryptedKeyBytes, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
    var nonce = encryptedValue.Skip(kEncryptionVersionPrefix.Length).Take(nonceLength).ToArray();
    encryptedValue = encryptedValue.Skip(kEncryptionVersionPrefix.Length + nonceLength).Take(encryptedValue.Length - (kEncryptionVersionPrefix.Length + nonceLength)).ToArray();
    var str = System.Web.HttpUtility.UrlDecode(AesGcmDecrypt(keyBytes, nonce, encryptedValue));

    //6、获得值
    Console.WriteLine($"{dataReader["host_key"]}-{dataReader["name"]}-{str}");
    //7、关闭数据
    connection.Close();
}
*/