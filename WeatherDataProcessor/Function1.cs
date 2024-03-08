using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace WeatherDataProcessor
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([ServiceBusTrigger("weather-forecast-added", "send-email", Connection = "WeatherDataConnection")]
        string mySbMsg, ILogger logger)
        {
            if (mySbMsg.Contains("2020")) throw new Exception("Cannot process for year 2020");
            logger.LogInformation($"SEND EMAIL: {mySbMsg}");
        }

        [FunctionName("UpdateReport")]
        public static void Run1([ServiceBusTrigger("weather-forecast-added", "update-report", Connection = "WeatherDataConnection")]
        ServiceBusReceivedMessage mySbMsg, ILogger logger)
        {
            logger.LogInformation($"UPDATING REPORT: {mySbMsg}");
        }
    }
}
