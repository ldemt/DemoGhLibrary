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
         

            // Authenticates and retrieves the access token to be used in next step.

            // Retrieves data from the REST API using the access token.

            return forecast;
        }


        public static Authentication Authenticate(HttpClient client, string credentials)
        {

            // Configures the HTTP headers and address.

            // Sends a POST request to the authentication server.
                   // Reads the server response content.
  
            // Deserialize the received authentication data (here it is just an access token) inside an Authentication class object.
            //Authentication authentication = System.Text.Json.JsonSerializer.Deserialize<Authentication>(contents);
                  // Clears the HTTP headers (for cleanliness, because we'll use the same client object afterwards w/ other headers)
   
            // Returns the authentication object we've just serialized.
            return authentication;
        }

        public static ConsumptionForecast GetShortTermConsumptionForecast(HttpClient client, Authentication authentication, string forecastType, DateTime startDate, DateTime endDate)
        {
            // Configures the HTTP headers and address.

            // Format the input dateTime object as strings suitably formatted for the HTTP request (so-called "sortable formatting").

            // Sends a GET request to the data server.
           
            // Reads the server response content.

            // Deserialize the received data as a ConsumptionForecast class object.

            return consumptionForecast;
        }


    }


}