using BAL.BusinessLogic.Interface;
using BAL.Models.FedEx;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Helper
{
    public class FedExHelper : IFedExHelper
    {
        private readonly string grantType = "";
        private readonly string clientId = "";
        private readonly string clientSecret = "";
        private readonly string fedExBaseUrl = "";

        public FedExHelper(IConfiguration configuration)
        {
            grantType = configuration?.GetSection("FedExSettings")["grant_type"] ?? "";
            clientId = configuration?.GetSection("FedExSettings")["client_id"] ?? "";
            clientSecret = configuration?.GetSection("FedExSettings")["client_secret"] ?? "";
            fedExBaseUrl = configuration?.GetSection("FedExSettings")["FedExBaseUrl"] ?? "";
        }
        public async Task<TokenResponse> GenerateToken()
        {
            var requestUrl = fedExBaseUrl + "oauth/token";

            var content = new FormUrlEncodedContent(new[]
            {
                 new KeyValuePair<string, string>("grant_type", grantType),
                 new KeyValuePair<string, string>("client_id", clientId),
                 new KeyValuePair<string, string>("client_secret", clientSecret)
            });

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(requestUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        switch (response.StatusCode)
                        {
                            case System.Net.HttpStatusCode.ServiceUnavailable:
                                return new TokenResponse();

                            case System.Net.HttpStatusCode.Unauthorized:
                                return new TokenResponse();

                            case System.Net.HttpStatusCode.InternalServerError:
                                return new TokenResponse();

                            default:
                                return new TokenResponse();
                        }
                    }
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(responseContent);

                    if (string.IsNullOrEmpty(tokenResponse?.AccessToken))
                    {
                        return new TokenResponse();
                    }

                    return tokenResponse;
                }
                catch (HttpRequestException e)
                {
                    return new TokenResponse();
                }
            }
        }

        public async Task<TrackingResponseModel> GetTrackingInfo(string trackingNumber)
        {
            var trackingInforesponse = new TrackingResponseModel();

            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, fedExBaseUrl + "track/v1/trackingnumbers");
            var tokenResponse = await GenerateToken();

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                trackingInforesponse.Status = "401::Token creation failed. Please try again.";
                return trackingInforesponse;
            }

            request.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");

            string requestBody = "{ \"trackingInfo\": [{ \"trackingNumberInfo\": { \"trackingNumber\": \"{{TRACKING_NUMBER}}\" }}], \"includeDetailedScans\": true }";
            requestBody = requestBody.Replace("{{TRACKING_NUMBER}}", trackingNumber);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                trackingInforesponse.Status = $"500::Request error: {ex.Message}";
                return trackingInforesponse;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Tracking_OutputModel>(responseContent);

            if (responseJson == null)
            {
                trackingInforesponse.Status = $"400::Failed to parse the response from FedEx.";
                return trackingInforesponse;
            }


            var output = responseJson.output;
            if (output == null)
            {
                trackingInforesponse.Status = $"400::No output found in the FedEx response.";
                return trackingInforesponse;
            }


            var completeTrackResults = output.completeTrackResults;
            if (completeTrackResults == null || completeTrackResults.Count == 0)
            {
                trackingInforesponse.Status = $"400::No complete tracking results found.";
                return trackingInforesponse;
            }


            var trackingResult = completeTrackResults.First();
            var latestStatus = trackingResult.trackResults.LastOrDefault()?.latestStatusDetail;


            var trackingResponse = new TrackingResponseModel
            {
                TransactionId = responseJson.transactionId,
                Alerts = output.alerts,
                Status = latestStatus?.description ?? "No status available",
                FullResponse = responseJson.output.completeTrackResults
            };

            return trackingResponse;
        }
    }
}
