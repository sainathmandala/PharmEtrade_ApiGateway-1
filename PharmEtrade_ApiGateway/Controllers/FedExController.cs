using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using System.Text;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FedExController : ControllerBase
    {
        //[HttpGet]
        //[Route("TrackingInfo")]
        //public async Task<ActionResult<TrackingResponseModel>> TrackingInfo()
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://apis-sandbox.fedex.com/track/v1/trackingnumbers");
        //    var tokenResponse = await TokenCreation();

        //    if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
        //    {
        //        return BadRequest(new { Success = false, Message = "Token creation failed. Please try again." });
        //    }

        //    request.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");

        //    const string requestBody = "{ \"trackingInfo\": [{ \"trackingNumberInfo\": { \"trackingNumber\": \"794843185271\" }}], \"includeDetailedScans\": true }";
        //    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response;

        //    try
        //    {
        //        response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return BadRequest(new { Message = $"Request error: {ex.Message}" });
        //    }

        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    var responseJson = JsonSerializer.Deserialize<Tracking_OutputModel>(responseContent);

        //    if (responseJson == null)
        //    {
        //        return BadRequest(new { Message = "Failed to parse the response from FedEx." });
        //    }


        //    var output = responseJson.output;
        //    if (output == null)
        //    {
        //        return BadRequest(new { Message = "No output found in the FedEx response." });
        //    }


        //    var completeTrackResults = output.completeTrackResults;
        //    if (completeTrackResults == null || completeTrackResults.Count == 0)
        //    {
        //        return BadRequest(new { Message = "No complete tracking results found." });
        //    }


        //    var trackingResult = completeTrackResults.First();
        //    var latestStatus = trackingResult.trackResults.LastOrDefault()?.latestStatusDetail;


        //    var trackingResponse = new TrackingResponseModel
        //    {
        //        TransactionId = responseJson.transactionId,
        //        Alerts = output.alerts,
        //        Status = latestStatus?.description ?? "No status available",
        //        FullResponse = responseJson.output.completeTrackResults
        //    };

        //    return Ok(trackingResponse);
        //}
    }
}
