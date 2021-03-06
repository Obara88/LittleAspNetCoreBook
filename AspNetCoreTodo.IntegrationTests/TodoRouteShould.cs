﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreTodo.IntegrationTests
{
    public class TodoRouteShould : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;

        public TodoRouteShould(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task ChallengeAnonymouseUser()
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/todo");

            //Act: request the /todo route
            var response = await _client.SendAsync(request);

            //Assert: anonymous user is redirected to the login page
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("http://localhost/Account/Login?ReturnUrl=%2Ftodo", response.Headers.Location.ToString());

        }
    }
}
