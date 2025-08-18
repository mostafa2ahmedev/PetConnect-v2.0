using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using PetConnect.BLL.Services.Interfaces;

namespace PetConnect.BLL.Services.Classes
{
    public class FaceComparisonService : IFaceComparisonService
    {
        private const string ApiKey = "J9O6SgEsVrYWryhhaybWN8TBxat6OiJg";
        private const string ApiSecret = "L67IaiOAk6YyoCx7iORbJRETNDKTW1lt";
        private const string FaceApiUrl = "https://api-us.faceplusplus.com/facepp/v3/compare";

   
        public async Task<bool> AreFacesMatchingAsync(Stream image1, Stream image2)
        {
            if (image1.CanSeek) image1.Position = 0;
            if (image2.CanSeek) image2.Position = 0;

            using var client = new HttpClient();
            using var form = new MultipartFormDataContent();

            var apiKeyContent = new StringContent(ApiKey);
            apiKeyContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"api_key\""
            };
            form.Add(apiKeyContent);

            var apiSecretContent = new StringContent(ApiSecret);
            apiSecretContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"api_secret\""
            };
            form.Add(apiSecretContent);

       
            using var ms1 = new MemoryStream();
            await image1.CopyToAsync(ms1);
            var image1Content = new ByteArrayContent(ms1.ToArray());
            image1Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            image1Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"image_file1\"",
                FileName = "\"image1.jpg\""
            };
            form.Add(image1Content);

      
            using var ms2 = new MemoryStream();
            await image2.CopyToAsync(ms2);
            var image2Content = new ByteArrayContent(ms2.ToArray());
            image2Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            image2Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"image_file2\"",
                FileName = "\"image2.jpg\""
            };
            form.Add(image2Content);

     
            var response = await client.PostAsync(FaceApiUrl, form);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[FaceComparison] Raw API Response: {jsonResponse}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("[FaceComparison] HTTP Error: " + response.StatusCode);
                return false;
            }

            try
            {
                using var doc = JsonDocument.Parse(jsonResponse);
                var root = doc.RootElement;

                if (root.TryGetProperty("error_message", out var errorMessage))
                {
                    Console.WriteLine($"[FaceComparison] API Error: {errorMessage.GetString()}");
                    return false;
                }

                if (root.TryGetProperty("confidence", out var confidenceElement) &&
                    root.TryGetProperty("thresholds", out var thresholdsElement) &&
                    thresholdsElement.TryGetProperty("1e-3", out var thresholdElement))
                {
                    var confidence = confidenceElement.GetDouble();
                    var threshold = thresholdElement.GetDouble();

                    Console.WriteLine($"[FaceComparison] Confidence: {confidence}");
                    Console.WriteLine($"[FaceComparison] Threshold (1e-3): {threshold}");

                    return confidence > threshold;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FaceComparison] JSON Parse Error: {ex.Message}");
            }

            return false;
        }
    }
}
