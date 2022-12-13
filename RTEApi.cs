using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace RTEApi
{
    public class Authentication
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

    }

    public class ConsumptionForecast
    {
        [JsonProperty("short_term")]
        public List<ShortTermConsumptionForecast> ShortTerm { get; set; }

        public override string ToString()
        {
            string returnString = "";

            if (ShortTerm == null)
            {
                returnString = "null";
            }
            else
            {
                foreach (var item in ShortTerm)
                {
                    returnString += item.ToString();
                }
            }

            return returnString;
        }
    }

    public class ShortTermConsumptionForecast
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("values")]
        public List<ForecastData> Values { get; set; }

        public override string ToString()
        {
            string returnString = "type : " + Type + " startDate : " + StartDate.ToString() + " end_date : " + EndDate.ToString();
            returnString = returnString + "forecastData : " + "\n";
            foreach (var item in Values)
            {
                returnString = returnString + item.ToString() + "\n";
            }
            return returnString;
        }

    }
    
    public class ForecastData


    {
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }
        [JsonProperty("updated_date")]
        public DateTime UpdatedDate { get; set; }
        [JsonProperty("value")]
        public int Value { get; set; }

        public override string ToString()
        {
            string returnString = "start_date : " + StartDate.ToString() + " end_date : " + EndDate.ToString() + " updated_date : " + UpdatedDate.ToString();
            returnString = returnString + " value : ";

            returnString = returnString + Value.ToString();

            return returnString;
        }

    }
    public class ForecastApplication
    {

        public static ConsumptionForecast GetConsumptionForecastExample()
        {

            // Credentials (username and password) encoded in base64 to be used for authentication. These are found on your RTE API user profile.
            string credentials = "MTkyOTVjN2QtNjMyMS00OTE3LTlhZTMtMWFlZmQ2NmQ5NGM5OjBkZTdhNTgyLTc4N2YtNGNmZi05YjM1LTA1MGY2NTVjOTVjYw==";

            // Forecast type : "REALISED" (realised consumption), "D-1" (forecast computed at D-1), "D-2" (forecast computed at D-2), "ID" (Infraday forecast).
            string forecastType = "D-2";

            // Construction of the start date and end date.
            int startYear = 2022;
            int startMonth = 10;
            int startDay = 31;
            int startHour = 0;
            int startMinute = 0;
            int startSecond = 0;

            int endYear = 2022;
            int endMonth = 11;
            int endDay = 01;
            int endHour = 0;
            int endMinute = 0;
            int endSecond = 0;

            DateTime startDate = new DateTime(startYear, startMonth, startDay, startHour, startMinute, startSecond);
            DateTime endDate = new DateTime(endYear, endMonth, endDay, endHour, endMinute, endSecond);

            ConsumptionForecast forecast = ForecastApplication.GetConsumptionForecast(credentials, forecastType, startDate, endDate);
            return forecast;
        }


        public static ConsumptionForecast GetConsumptionForecast(string credentials, string forecastType, DateTime startDate, DateTime endDate)
        {
            // Initializes the HTTP client to communicate with the REST API.
            var client = new HttpClient();

            // Authenticates and retrieves the access token to be used in next step.
            Console.WriteLine("Authenticating...");
            var authentication = Authenticate(client, credentials);
            Console.WriteLine("Authentication access token : " + authentication.AccessToken);

            // Retrieves data from the REST API using the access token.
            Console.WriteLine("Getting forecasts from REST API...");
            ConsumptionForecast forecast = GetShortTermConsumptionForecast(client, authentication, forecastType, startDate, endDate);
            Console.WriteLine(forecast.ToString());

            return forecast;
        }


        public static Authentication Authenticate(HttpClient client, string credentials)
        {

            // Configures the HTTP headers and address.
            client.DefaultRequestHeaders.Add("Host", "digital.iservices.rte-france.com");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
            client.BaseAddress = new Uri("https://digital.iservices.rte-france.com");

            // Sends a POST request to the authentication server.
            var parameters = new Dictionary<string, string>();
            var responseTask = client.PostAsync("/token/oauth/", new FormUrlEncodedContent(parameters));
            responseTask.Wait();
            var response = responseTask.Result;
            // Reads the server response content.
            var contentsTask = response.Content.ReadAsStringAsync();
            contentsTask.Wait();
            var contents = contentsTask.Result;

            // Deserialize the received authentication data (here it is just an access token) inside an Authentication class object.
            //Authentication authentication = System.Text.Json.JsonSerializer.Deserialize<Authentication>(contents);
            Authentication authentication = JsonConvert.DeserializeObject<Authentication>(contents);
            // Clears the HTTP headers (for cleanliness, because we'll use the same client object afterwards w/ other headers)
            client.DefaultRequestHeaders.Clear();

            // Returns the authentication object we've just serialized.
            return authentication;
        }

        public static ConsumptionForecast GetShortTermConsumptionForecast(HttpClient client, Authentication authentication, string forecastType, DateTime startDate, DateTime endDate)
        {
            // Configures the HTTP headers and address.
            client.DefaultRequestHeaders.Add("Host", "digital.iservices.rte-france.com");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authentication.AccessToken);

            // Format the input dateTime object as strings suitably formatted for the HTTP request (so-called "sortable formatting").
            string endDateString = endDate.ToString("s") + endDate.ToString("zzz");
            string startDateString = startDate.ToString("s") + startDate.ToString("zzz");

            string path = "/open_api/consumption/v1/short_term" + "?type=" + forecastType + "&start_date=" + startDateString + "&end_date=" + endDateString;

            // Sends a GET request to the data server.
            var streamTask = client.GetAsync(path);
            streamTask.Wait();
            var streamResult = streamTask.Result;
            // Reads the server response content.
            var contentTask = streamResult.Content.ReadAsStringAsync();
            var content = contentTask.Result;

            // Deserialize the received data as a ConsumptionForecast class object.
            ConsumptionForecast consumptionForecast = JsonConvert.DeserializeObject<ConsumptionForecast>(content);
            //var consumptionForecast = System.Text.Json.JsonSerializer.Deserialize<ConsumptionForecast>(content);

            return consumptionForecast;
        }


    }


}