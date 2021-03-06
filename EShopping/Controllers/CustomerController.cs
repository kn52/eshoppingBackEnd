﻿namespace EShopping.Controllers
{
    using EShoppingModel.Dto;
    using EShoppingModel.Response;
    using EShoppingRepository.Infc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Threading.Tasks;

    [Route("customer")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
            Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public class CustomerController : ControllerBase
    {
        public CustomerController(ICustomerService service)
        {
            this.CustomerService = service;
        }
        public ICustomerService CustomerService { get; set; }

        [HttpPost]
        [Route("detail")]
        public async Task<IActionResult> AddCustomerDetails([FromBody] CustomerDto customerDto)
        {
            string CustomerData;
            try
            {
                string userId = null;
                userId = User.FindFirst("userId").Value;
                if (userId == null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, "Invalid Token", userId, ""));
                }
                CustomerData = await Task.FromResult(CustomerService.AddCustomerDetails(customerDto, userId));
                if (!CustomerData.Contains("Not") && CustomerData != null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, CustomerData, customerDto, ""));
                }
            }
            catch
            {
                return this.BadRequest(new ResponseEntity(HttpStatusCode.BadRequest, "Not Added", null, ""));
            }
            return this.Ok(new ResponseEntity(HttpStatusCode.NoContent, CustomerData, null, ""));
        }

        [HttpGet]
        [Route("detail")]
        public async Task<IActionResult> FetchCustomerDetails(int addressType = 0)
        {
            try
            {
                string userId = null;
                userId = User.FindFirst("userId").Value;
                if (userId == null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, "Invalid Token", userId, ""));
                }
                var CustomerData = await Task.FromResult(CustomerService.FetchCustomerDetails(addressType, userId));
                if (CustomerData != null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, "Address Found", CustomerData, ""));
                }
            }
            catch
            {
                return this.BadRequest(new ResponseEntity(HttpStatusCode.BadRequest, "Bad Request", null, ""));
            }
            return this.Ok(new ResponseEntity(HttpStatusCode.NoContent, "Address Not Found", null, ""));
        }

        [HttpPost]
        [Route("comments")]
        public async Task<IActionResult> AddUserFeedBack([FromBody] FeedBackDto feedbackDto)
        {
            string CustomerData;
            try
            {
                string userId = null;
                userId = User.FindFirst("userId").Value;
                if (userId == null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, "Invalid Token", userId, ""));
                }
                CustomerData = await Task.FromResult(CustomerService.AddUserFeedBack(feedbackDto, userId));
                if (!CustomerData.Contains("Not") && CustomerData != null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, CustomerData, feedbackDto, ""));
                }
            }
            catch
            {
                return this.BadRequest(new ResponseEntity(HttpStatusCode.BadRequest, "Bad Request", null, ""));
            }
            return this.Ok(new ResponseEntity(HttpStatusCode.NoContent, CustomerData, null, ""));
        }

        [HttpGet]
        [Route("comments")]
        public async Task<IActionResult> GetBookFeedback(string isbnNumber)
        {
            try
            {
                string userId = null;
                userId = User.FindFirst("userId").Value;
                if (userId == null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, "Invalid Token", userId, ""));
                }
                var CustomerData = await Task.FromResult(CustomerService.getBookFeedback(isbnNumber));
                if (CustomerData != null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, "Feedback Found", CustomerData, ""));
                }
            }
            catch
            {
                return this.BadRequest(new ResponseEntity(HttpStatusCode.BadRequest, "Bad Request", null, ""));
            }
            return this.Ok(new ResponseEntity(HttpStatusCode.NoContent, "No Feedback Found", null, ""));
        }

        [HttpGet]
        [Route("feedback")]
        public async Task<IActionResult> GetUserFeedback(int bookId)
        {
            try
            {
                string userId = null;
                userId = User.FindFirst("userId").Value;
                if (userId == null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, "Invalid Token", userId, ""));
                }
                var CustomerData = await Task.FromResult(CustomerService.getUserFeedback(bookId,userId));
                if (CustomerData != null)
                {
                    return this.Ok(new ResponseEntity(HttpStatusCode.OK, "Feedback Found", CustomerData, ""));
                }
            }
            catch
            {
                return this.BadRequest(new ResponseEntity(HttpStatusCode.BadRequest, "Bad Request", null, ""));
            }
            return this.Ok(new ResponseEntity(HttpStatusCode.NoContent, "No Feedback Found", null, ""));
        }
    }
}
