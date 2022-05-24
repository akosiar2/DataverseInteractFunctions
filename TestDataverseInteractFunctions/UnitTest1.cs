using System;
using Xunit;
using DataverseInteractFunctions;
using DataverseInteractFunctions.dto;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IO;
using System.Text;

namespace TestDataverseInteractFunctions
{
    public class UnitTest1
    {
        public UnitTest1()
        {

        }

        [Fact]
        public void WhenTestDataverseHasRequest_Success()
        {
            var jsonpost = @"{
                          ""$schema"": ""http://json-schema.org/draft-04/schema#"",
                                    ""type"": ""object"",
                          ""properties"": {
                                        ""StartOn"": {
                                            ""type"": ""string"",
                              ""format"": ""date""
                                        },
                            ""EndOn"": {
                                            ""type"": ""string"",
                              ""format"": ""date""
                            }
                                    },
                          ""required"": [
                            ""05 /23/2022"",
                            ""05 /26/2022""
                          ]
                        }
                        ";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonpost));
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = stream,
                ContentLength = stream.Length
            };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = Function1.Run(request, logger);
            response.Wait();

            // Check that the response is an "OK" response
            Assert.IsAssignableFrom<OkObjectResult>(response.Result);

            // Check that the contents of the response are the expected contents
            var result = (OkObjectResult)response.Result;
            string watchInfo = $"Time Entries Interacted";
            Assert.Equal(watchInfo, result.Value);
        }

        [Fact]
        public void WhenTestDataverseHasRequest_NoRequestBody()
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = Function1.Run(request, logger);
            response.Wait();

            // Check that the response is an "Bad" response
            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);

            // Check that the contents of the response are the expected contents
            var result = (BadRequestObjectResult)response.Result;
            Assert.Equal("Please provide a model in the request body", result.Value);
        }
    }
}
