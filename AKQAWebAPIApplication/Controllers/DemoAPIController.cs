using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AKQAWebApplicationTest.Models;
using AKQAWebApplicationTest.Extensions;

namespace AKQAWebApplicationTest.Controllers
{
    public class DemoAPIController : ApiController
    {
        /// <summary>
        /// Perform Conversion of amount to string and gives response along with input name
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Show(InputForm input)
        {
            var result = string.Empty;
            var amountInWords = string.Empty;
            try
            {
                amountInWords = input.Amount.DecimalToWords();
                result = string.Format("{0} , {1}", input.Name, amountInWords);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                //TBD //Log error message
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again!"),
                    ReasonPhrase = "Critical Exception"
                });
            }
        }
    }
}
