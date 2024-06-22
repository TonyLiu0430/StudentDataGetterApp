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

        public async Task StartFetchingAsync(Action action = null) {
            //GetCookieKey();
            await FetchingAsync(action);
        }
        private async Task FetchingAsync(Action action) {
            using var query = new Queryer(Cookie, StudentSet);
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
                                    alreadyGetAllStudent = true;
                                    break;
                                }
                            }
                        }
                    } 
                    else {
                        string studentId8 = $@"s4{year}{departmentId}";
                        await query.GetStudentData(studentId8, 100);
                        for (int j = 5; j <= 9; j++) {
                            string studentId9 = $"{studentId8}{j}";
                            bool res = await query.GetStudentData(studentId9, 10);
                            if (!res) {
                                break;
                            }
                        }
                    }
                    //增加進度條
                    action?.Invoke();
                }
            }
        }
        
        public void SaveData() {
            string json = JsonConvert.SerializeObject(StudentSet, Formatting.Indented);
            Directory.CreateDirectory(".\\result");
            File.Create(".\\result\\data.json").Close();
            File.WriteAllText(".\\result\\data.json", json);
        }
    }


}
