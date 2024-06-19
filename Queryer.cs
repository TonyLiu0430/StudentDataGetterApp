using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudentDataGetterApp {

    internal class Queryer {

        private readonly HttpClient httpClient;

        public int queryCount = 30;

        public Queryer(string cookie) {
            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            // 添加你的cookie到CookieContainer
            //handler.CookieContainer.Add(new Uri("https://outlook.office365.com"), cookie);
            handler.CookieContainer.SetCookies(new Uri("https://outlook.office365.com"), cookie.ToString());
            // 如果有更多的cookie，重複上面的Add方法來添加

            httpClient = new HttpClient(handler);
        }
        public async Task<List<Student>> getStudentList(string studentId8) {
            string url = $"https://outlook.office365.com/owa/service.svc?action=FindPeople&app=People&n={queryCount}";
            queryCount += 3;
            List<Student> studentList = new List<Student>();
            string urlpostdataOrigin = """
                {"__type":"FindPeopleJsonRequest:#Exchange","Header":{"__type":"JsonRequestHeaders:#Exchange","RequestServerVersion":"V2018_01_08","TimeZoneContext":{"__type":"TimeZoneContext:#Exchange","TimeZoneDefinition":{"__type":"TimeZoneDefinitionType:#Exchange","Id":"Taipei Standard Time"}}},"Body":{"__type":"FindPeopleRequest:#Exchange","IndexedPageItemView":{"__type":"IndexedPageView:#Exchange","BasePoint":"Beginning","Offset":0,"MaxEntriesReturned":100},"PersonaShape":{"__type":"PersonaResponseShape:#Exchange","BaseShape":"Default","AdditionalProperties":[{"__type":"PropertyUri:#Exchange","FieldURI":"PersonaAttributions"},{"__type":"PropertyUri:#Exchange","FieldURI":"PersonaRelevanceScore"},{"__type":"PropertyUri:#Exchange","FieldURI":"PersonaTitle"}]},"ShouldResolveOneOffEmailAddress":false,"QueryString":"
                """;
            urlpostdataOrigin += studentId8;
            urlpostdataOrigin += """
                ","SearchPeopleSuggestionIndex":true,"Context":[{"__type":"ContextProperty:#Exchange","Key":"AppName","Value":"OWA"},{"__type":"ContextProperty:#Exchange","Key":"AppScenario","Value":"peopleHubReact"},{"__type":"ContextProperty:#Exchange","Key":"DisableAdBasedPersonaIdForPersonalContacts","Value":"true"}],"QuerySources":["Mailbox","Directory"]}}
                """;



            var jsonRequest = new {
                __type = "FindPeopleJsonRequest:#Exchange",
                Header = new {
                    __type = "JsonRequestHeaders:#Exchange",
                    RequestServerVersion = "V2018_01_08",
                    TimeZoneContext = new {
                        __type = "TimeZoneContext:#Exchange",
                        TimeZoneDefinition = new {
                            __type = "TimeZoneDefinitionType:#Exchange",
                            Id = "Taipei Standard Time"
                        }
                    }
                },
                Body = new {
                    __type = "FindPeopleRequest:#Exchange",
                    IndexedPageItemView = new {
                        __type = "IndexedPageView:#Exchange",
                        BasePoint = "Beginning",
                        Offset = 0,
                        MaxEntriesReturned = 10
                    },
                    PersonaShape = new {
                        __type = "PersonaResponseShape:#Exchange",
                        BaseShape = "Default",
                        AdditionalProperties = new[]
                        {
                            new { __type = "PropertyUri:#Exchange", FieldURI = "PersonaAttributions" },
                            new { __type = "PropertyUri:#Exchange", FieldURI = "PersonaRelevanceScore" },
                            new { __type = "PropertyUri:#Exchange", FieldURI = "PersonaTitle" }
                        }
                    },
                    ShouldResolveOneOffEmailAddress = false,
                    QueryString = "s411285",
                    SearchPeopleSuggestionIndex = true,
                    Context = new[]
                    {
                        new { __type = "ContextProperty:#Exchange", Key = "AppName", Value = "OWA" },
                        new { __type = "ContextProperty:#Exchange", Key = "AppScenario", Value = "peopleHubReact" },
                        new { __type = "ContextProperty:#Exchange", Key = "DisableAdBasedPersonaIdForPersonalContacts", Value = "true" }
                    },
                    QuerySources = new[] { "Mailbox", "Directory" }
                }
            };

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonRequest);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            // 設置請求標頭
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            httpClient.DefaultRequestHeaders.Add("accept-language", "zh-TW,zh-CN;q=0.9,zh;q=0.8,en-US;q=0.7,en;q=0.6");
            httpClient.DefaultRequestHeaders.Add("action", "FindPeople");
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            httpClient.DefaultRequestHeaders.Add("ms-cv", "/ZW8KkHwg8OZxy/rtcOqko.114");
            httpClient.DefaultRequestHeaders.Pragma.Clear();
            httpClient.DefaultRequestHeaders.Pragma.Add(new NameValueHeaderValue("no-cache"));
            httpClient.DefaultRequestHeaders.Add("prefer", "exchange.behavior=IncludeThirdPartyOnlineMeetingProviders");
            httpClient.DefaultRequestHeaders.Add("priority", "u=1, i");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
            httpClient.DefaultRequestHeaders.Add("x-owa-canary", "bdvoV1qYFd8AAAAAAAAAAGBXTxd-j9wY2KxchpC-ZlTAtKBBLN94465zGSHMIstEFmnEaBQVvzo.");
            httpClient.DefaultRequestHeaders.Add("x-owa-canary-debug", $"({DateTime.UtcNow.ToString()})(S:S-1-5-21-86608550-739683862-1502845539-6761089)(I:57e8db6d985adf150000000000000000)(H:D8AC5C8690BE6654C0B4A0412CDF78E3AE731921CC22CB441669C4681415BF3A)(K:CertConstKeyHmac)(L:bdvoV1qYFd8AAAAAAAAAAGBXTxd-j9wY)");
            httpClient.DefaultRequestHeaders.Add("x-owa-correlationid", "71ab4b87-f45f-da9b-643c-dac00351d5d1");
            httpClient.DefaultRequestHeaders.Add("x-owa-hosted-ux", "false");
            httpClient.DefaultRequestHeaders.Add("x-owa-sessionid", "e0f67514-4f1e-423d-bba9-a9e199c76896");
            httpClient.DefaultRequestHeaders.Add("x-owa-urlpostdata", WebUtility.UrlEncode(urlpostdataOrigin));
            httpClient.DefaultRequestHeaders.Add("x-req-source", "People");


            // 發送請求並獲取響應
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            // 確保響應成功
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException e) {
                Console.WriteLine($"獲取資料失敗 {e.Message}");
                Thread.Sleep(10000);
                return new List<Student>();
            }

            // 讀取響應內容
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);

            return new List<Student>();
        }

    }
}


/*
namespace StudentDataGetterApp {

    internal class Queryer {

        private readonly HttpClient httpClient;

        public Queryer() {
            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            // 添加你的cookie到CookieContainer
            handler.CookieContainer.Add(new Uri("https://outlook.office365.com"), new Cookie("cookie名稱", "cookie值"));
            // 如果有更多的cookie，重複上面的Add方法來添加

            httpClient = new HttpClient(handler);
        }

        public int queryCount = 30;
        public async Task<List<Student>> getStudentList(string studentId8) {
            // 你的方法實現...
        }
    }
}*/

