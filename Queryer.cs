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

    internal class Queryer {

        //private HttpClient httpClient;
        //private readonly HttpClientHandler httpHandler;
        public int queryCount = 92;

        private readonly Dictionary<Department, SortedSet<Student>> StudentSet;
        private readonly string cookie;

        public Queryer(string cookie, Dictionary<Department, SortedSet<Student>> StudentSet) {
            /*
            httpHandler = new HttpClientHandler();
            httpHandler.CookieContainer = new CookieContainer();
            // 添加你的cookie到CookieContainer
            httpHandler.CookieContainer.SetCookies(new Uri("https://outlook.office365.com"), cookie.Replace(";", ",").ToString());
            //httpClient = new HttpClient(handler);
            */
            this.StudentSet = StudentSet;
            this.cookie = cookie.Replace(";", ",");
        }
        public async Task<bool> GetStudentData(string studentId8) {
            queryCount += 3;
            string url = $"https://outlook.office365.com/owa/service.svc?action=FindPeople&app=People&n={queryCount}";
            string urlpostdataOrigin = $@"%7B%22__type%22%3A%22FindPeopleJsonRequest%3A%23Exchange%22%2C%22Header%22%3A%7B%22__type%22%3A%22JsonRequestHeaders%3A%23Exchange%22%2C%22RequestServerVersion%22%3A%22V2018_01_08%22%2C%22TimeZoneContext%22%3A%7B%22__type%22%3A%22TimeZoneContext%3A%23Exchange%22%2C%22TimeZoneDefinition%22%3A%7B%22__type%22%3A%22TimeZoneDefinitionType%3A%23Exchange%22%2C%22Id%22%3A%22Taipei%20Standard%20Time%22%7D%7D%7D%2C%22Body%22%3A%7B%22__type%22%3A%22FindPeopleRequest%3A%23Exchange%22%2C%22IndexedPageItemView%22%3A%7B%22__type%22%3A%22IndexedPageView%3A%23Exchange%22%2C%22BasePoint%22%3A%22Beginning%22%2C%22Offset%22%3A0%2C%22MaxEntriesReturned%22%3A100%7D%2C%22PersonaShape%22%3A%7B%22__type%22%3A%22PersonaResponseShape%3A%23Exchange%22%2C%22BaseShape%22%3A%22Default%22%2C%22AdditionalProperties%22%3A%5B%7B%22__type%22%3A%22PropertyUri%3A%23Exchange%22%2C%22FieldURI%22%3A%22PersonaAttributions%22%7D%2C%7B%22__type%22%3A%22PropertyUri%3A%23Exchange%22%2C%22FieldURI%22%3A%22PersonaRelevanceScore%22%7D%2C%7B%22__type%22%3A%22PropertyUri%3A%23Exchange%22%2C%22FieldURI%22%3A%22PersonaTitle%22%7D%5D%7D%2C%22ShouldResolveOneOffEmailAddress%22%3Afalse%2C%22QueryString%22%3A%22{studentId8}%22%2C%22SearchPeopleSuggestionIndex%22%3Atrue%2C%22Context%22%3A%5B%7B%22__type%22%3A%22ContextProperty%3A%23Exchange%22%2C%22Key%22%3A%22AppName%22%2C%22Value%22%3A%22OWA%22%7D%2C%7B%22__type%22%3A%22ContextProperty%3A%23Exchange%22%2C%22Key%22%3A%22AppScenario%22%2C%22Value%22%3A%22peopleHubReact%22%7D%2C%7B%22__type%22%3A%22ContextProperty%3A%23Exchange%22%2C%22Key%22%3A%22DisableAdBasedPersonaIdForPersonalContacts%22%2C%22Value%22%3A%22true%22%7D%5D%2C%22QuerySources%22%3A%5B%22Mailbox%22%2C%22Directory%22%5D%7D%7D";

            using var httpHandler = new HttpClientHandler();
            httpHandler.CookieContainer = new CookieContainer();
            // 添加你的cookie到CookieContainer
            httpHandler.CookieContainer.SetCookies(new Uri("https://outlook.office365.com"), cookie);
            using var httpClient = new HttpClient(httpHandler);
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
            httpClient.DefaultRequestHeaders.Add("ms-cv", $@"YQwjnlMJzAK9pZx/pUa9Sf.{queryCount}");
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
            httpClient.DefaultRequestHeaders.Add("x-owa-canary", "bdvoV1qYFd8AAAAAAAAAAAA_0LWKkNwYIAdbuhx8e78VAqYhswIpN6eacpgksnWcy2nE556TJWk.");
            httpClient.DefaultRequestHeaders.Add("x-owa-canary-debug", "(T:2024-06-20T14:26:56.8793104Z)(S:S-1-5-21-86608550-739683862-1502845539-6761089)(I:57e8db6d985adf150000000000000000)(H:A6AA39E73A0BC8548676D35BA1CBDE80D820CC64007B7380822D2BE420719623)(K:CertConstKeyHmac)(L:bdvoV1qYFd8AAAAAAAAAABAktN9rkNwY)");
            httpClient.DefaultRequestHeaders.Add("x-owa-correlationid", "457aae00-552f-ce01-bd37-d19b38e57938");
            httpClient.DefaultRequestHeaders.Add("x-owa-hosted-ux", "false");
            httpClient.DefaultRequestHeaders.Add("x-owa-sessionid", "1140b4ae-a8bb-46c3-a5a1-c55fafd4edd1");
            httpClient.DefaultRequestHeaders.Add("x-owa-urlpostdata", /*WebUtility.UrlEncode(*/urlpostdataOrigin);
            httpClient.DefaultRequestHeaders.Add("x-req-source", "People");

            using var jsonContent = new StringContent("");
            jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            jsonContent.Headers.ContentType.CharSet = "utf-8";
            // 發送POST請求
            var response = await httpClient.PostAsync(url, jsonContent);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException e) {
                MessageBox.Show($"獲取資料失敗 {e.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // 讀取響應內容
            var responseBody = await response.Content.ReadAsStringAsync();

            var res = Parseresponse(responseBody);

            return res;
        }
    
        private bool Parseresponse(string responseBody) {
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
    }
}
