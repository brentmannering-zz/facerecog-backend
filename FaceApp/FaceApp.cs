using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace FaceApp
{
    public static class FaceApp
    {
        [FunctionName("FaceFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (String.IsNullOrEmpty(requestBody))
            {
                log.LogInformation("Body is empty");
            }

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var client = new HttpClient();

            string baseUrl = Environment.GetEnvironmentVariable("faceApiBaseUrl");

            string faceApiKey = Environment.GetEnvironmentVariable("faceApiKey");

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", faceApiKey);

            JsonContent payload = new JsonContent(new { url = data.url });

            string reqUrl = $"{baseUrl}/face/v1.0/detect?returnFaceId=true&returnFaceAttributes=gender,smile,age,emotion&recognitionModel=recognition_01&detectionModel=detection_01";

            var response = await client.PostAsync(reqUrl, payload);

            var content = await response.Content.ReadAsStringAsync();

            return new OkObjectResult(content);
        }

        public class JsonContent : StringContent
        {
            public JsonContent(object obj) :
                base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }
    }
}
