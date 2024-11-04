using BAL.Models;
using BAL.RequestModels.Customer;
using BAL.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Extensions;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Security.Cryptography.X509Certificates;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaxController : ControllerBase
    {
        private readonly ITaxRepo _itaxRepo;
        private readonly JwtAuthenticationExtensions _jwtTokenService;
        private readonly IConfiguration _configuration;

        public TaxController(ITaxRepo itaxRepo, JwtAuthenticationExtensions jwtTokenService, IConfiguration configuration)
        {
            _itaxRepo = itaxRepo;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllTaxInformation")]
        public async Task<IActionResult> GetAllTaxInformation()
        {
            try
            {
                var response = await _itaxRepo.GetAllTaxInformation();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving Tax Information details.");
            }
        }

        [HttpPost]
        [Route("AddTaxInformation")]
        public async Task<IActionResult> AddTaxInformation(TaxInformation taxInformation)
        {
          
            try
            {
                var response = await _itaxRepo.AddTaxInformationDetails(taxInformation);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Adding Tax Information details.");
            }
        }

        [HttpGet]
        [Route("GetByStateName")]
        public async Task<IActionResult> GetByStateName(string stateName)
        {
            try
            {
                var response = await _itaxRepo.GetTaxInformationByStateName(stateName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving Tax Information details.");
            }
        }

        [HttpGet]
        [Route("GetByCategorySpecificationId")]
        public async Task<IActionResult> GetByCategorySpecificationId(int categorySpecificationId)
        {
            try
            {
                var response = await _itaxRepo.GetTaxInformationByCategorySpecificationId(categorySpecificationId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving Tax Information details.");
            }
        }

        [HttpPost]
        [Route("UpdateTaxInformation")]
        public async Task<IActionResult> UpdateTaxInformation(TaxInformation taxInformation)
        {
            try
            {
                var response = await _itaxRepo.UpdateTaxInformation(taxInformation);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating Tax Information  details.");
            }
        }
    }
}
