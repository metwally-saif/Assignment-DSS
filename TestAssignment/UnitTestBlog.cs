using Assignment_DSS;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TestAssignment
{
    public class UnitTestBlog : IClassFixture<WebApplicationFactory<Assignment_DSS.Program>>
    {
        private readonly WebApplicationFactory<Assignment_DSS.Program> _factory;   
        private readonly HttpClient _httpClient;

        public UnitTestBlog(WebApplicationFactory<Assignment_DSS.Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }



        [Theory]
        [InlineData("/")]
        [InlineData("Login/Login")]
        [InlineData("Reg/Reg")]
        public async Task TestAllPages(string URL)
        {
            var client = _factory.CreateClient();

            var res = await client.GetAsync(URL);
            int code = (int)res.StatusCode;

            Assert.Equal(200, code);
        }
    }
}