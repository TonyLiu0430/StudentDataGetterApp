using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace StudentDataGetterApp {

    internal class Queryer : IDisposable {
        public int queryCount = 92;

        private readonly Dictionary<Department, SortedSet<Student>> StudentSet;
        private readonly HttpClientHandler httpHandler;
        private readonly HttpClient httpClient;

        public Queryer(string cookieOrigin, Dictionary<Department, SortedSet<Student>> StudentSet) {
            this.StudentSet = StudentSet;
            var cookie = cookieOrigin.Replace(";", ",");

            httpHandler = new HttpClientHandler();
            httpHandler.CookieContainer = new CookieContainer();
            httpHandler.CookieContainer.SetCookies(new Uri("https://outlook.office365.com"), cookie);
            httpClient = new HttpClient(httpHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-TW"));
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-CN", 0.9));
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh", 0.8));
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US", 0.7));
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.6));
            httpClient.DefaultRequestHeaders.Add("action", "FindPeople");
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue {
                NoCache = true
            };
            httpClient.DefaultRequestHeaders.Add("ms-cv", $@"0OKV+BcJjD4dHxLvwJQ34T.{queryCount}");
            httpClient.DefaultRequestHeaders.Add("origin", "https://outlook.office365.com");
            httpClient.DefaultRequestHeaders.Pragma.Add(new NameValueHeaderValue("no-cache"));
            httpClient.DefaultRequestHeaders.Add("prefer", "exchange.behavior=\"IncludeThirdPartyOnlineMeetingProviders\"");
            httpClient.DefaultRequestHeaders.Add("priority", "u=1, i");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
            string x_owa_canary_value = null;
            string x_owa_canary_debug_value = null;
            foreach (var cookieObj in httpHandler.CookieContainer.GetCookies(new Uri("https://outlook.office365.com"))) {
                var cookieItem = cookieObj as Cookie;
                if (cookieItem.Name == "X-OWA-CANARY") {
                    x_owa_canary_value = cookieItem.Value;
                }
                else if(cookieItem.Name == "X-OWA-CANARY-DEBUG") {
                    x_owa_canary_debug_value = cookieItem.Value;
                }
            }
            if(x_owa_canary_value == null || x_owa_canary_debug_value == null) {
                MessageBox.Show("Cookie 錯誤", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception("Cookie 錯誤");
            }
            httpClient.DefaultRequestHeaders.Add("x-owa-canary", x_owa_canary_value);
            httpClient.DefaultRequestHeaders.Add("x-owa-canary-debug", x_owa_canary_debug_value);
            httpClient.DefaultRequestHeaders.Add("x-owa-correlationid", "457aae00-552f-ce01-bd37-d19b38e57938");
            httpClient.DefaultRequestHeaders.Add("x-owa-hosted-ux", "false");
            httpClient.DefaultRequestHeaders.Add("x-owa-sessionid", "13fd0c10-73ad-4a68-83b6-47d76224c1c7");
            httpClient.DefaultRequestHeaders.Add("x-req-source", "People");
        }
        public async Task<bool> GetStudentData(string studentId8, int queryNum) {
            queryCount += 3;
            string url = $"https://outlook.office365.com/owa/service.svc?action=FindPeople&app=People&n={queryCount}";
            string urlpostdata = $@"%7B%22__type%22%3A%22FindPeopleJsonRequest%3A%23Exchange%22%2C%22Header%22%3A%7B%22__type%22%3A%22JsonRequestHeaders%3A%23Exchange%22%2C%22RequestServerVersion%22%3A%22V2018_01_08%22%2C%22TimeZoneContext%22%3A%7B%22__type%22%3A%22TimeZoneContext%3A%23Exchange%22%2C%22TimeZoneDefinition%22%3A%7B%22__type%22%3A%22TimeZoneDefinitionType%3A%23Exchange%22%2C%22Id%22%3A%22Taipei%20Standard%20Time%22%7D%7D%7D%2C%22Body%22%3A%7B%22__type%22%3A%22FindPeopleRequest%3A%23Exchange%22%2C%22IndexedPageItemView%22%3A%7B%22__type%22%3A%22IndexedPageView%3A%23Exchange%22%2C%22BasePoint%22%3A%22Beginning%22%2C%22Offset%22%3A0%2C%22MaxEntriesReturned%22%3A{Math.Min(queryNum, 150)}%7D%2C%22PersonaShape%22%3A%7B%22__type%22%3A%22PersonaResponseShape%3A%23Exchange%22%2C%22BaseShape%22%3A%22Default%22%2C%22AdditionalProperties%22%3A%5B%7B%22__type%22%3A%22PropertyUri%3A%23Exchange%22%2C%22FieldURI%22%3A%22PersonaAttributions%22%7D%2C%7B%22__type%22%3A%22PropertyUri%3A%23Exchange%22%2C%22FieldURI%22%3A%22PersonaRelevanceScore%22%7D%2C%7B%22__type%22%3A%22PropertyUri%3A%23Exchange%22%2C%22FieldURI%22%3A%22PersonaTitle%22%7D%5D%7D%2C%22ShouldResolveOneOffEmailAddress%22%3Afalse%2C%22QueryString%22%3A%22{studentId8}%22%2C%22SearchPeopleSuggestionIndex%22%3Atrue%2C%22Context%22%3A%5B%7B%22__type%22%3A%22ContextProperty%3A%23Exchange%22%2C%22Key%22%3A%22AppName%22%2C%22Value%22%3A%22OWA%22%7D%2C%7B%22__type%22%3A%22ContextProperty%3A%23Exchange%22%2C%22Key%22%3A%22AppScenario%22%2C%22Value%22%3A%22peopleHubReact%22%7D%2C%7B%22__type%22%3A%22ContextProperty%3A%23Exchange%22%2C%22Key%22%3A%22DisableAdBasedPersonaIdForPersonalContacts%22%2C%22Value%22%3A%22true%22%7D%5D%2C%22QuerySources%22%3A%5B%22Mailbox%22%2C%22Directory%22%5D%7D%7D";
            using var jsonContent = new StringContent("");
            jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            jsonContent.Headers.ContentType.CharSet = "utf-8";
            jsonContent.Headers.Add("x-owa-urlpostdata", urlpostdata);
            // 發送POST請求
            var response = await httpClient.PostAsync(url, jsonContent);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException e) {
                if(response.StatusCode == HttpStatusCode.Unauthorized) {
                    MessageBox.Show("Cookie 錯誤 未登入", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new UnAuthorizedException("Cookie 錯誤 未登入");
                }
                MessageBox.Show($"獲取資料失敗 {e.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            var res = ParseResponse(responseBody);

            return res;
        }
    
        private bool ParseResponse(string responseBody) {
            var response = JObject.Parse(responseBody);
            var resultSet = response["Body"]["ResultSet"];
            var flag = false;
            foreach(var student in resultSet.Children()) {
                string studentId = (string)student["Nickname"];
                string studentName = (string)student["DisplayName"];
                var studentNew = new Student {
                    Id = studentId,
                    Name = studentName,
                    Department = Department.GetDepartmentFromStudent(studentId)
                };
                var studentDepartment = Department.GetDepartmentFromStudent(studentId);
                if (!StudentSet.ContainsKey(studentDepartment)) {
                    StudentSet[studentDepartment] = new SortedSet<Student>(new StudentComparer());
                }
                if(!StudentSet[studentDepartment].Contains(studentNew)) {
                    flag = true;
                    StudentSet[studentDepartment].Add(studentNew);
                }
            }
            return flag;
        }

        public void Dispose() {
            httpHandler.Dispose();
            httpClient.Dispose();
        }
    }

    public class UnAuthorizedException : Exception {
        public UnAuthorizedException() {
        }

        public UnAuthorizedException(string message) : base(message) {
        }

        public UnAuthorizedException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
