﻿using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API;
using Xunit;

namespace TaskManager.APITests
{
    [Trait("Category", "API")]
    public class HelloWorldTests
    {

        [Fact]
        public async Task HelloWorld_CanGet()
        {

            var url = "http://localhost:8989";

            using (WebApp.Start<Startup>(url))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    var response = await client.GetAsync("api/hello");

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    var obj = (JObject)JsonConvert.DeserializeObject(content);

                    Assert.Equal("hello world", obj["message"].ToString());

                }
            }

        }
        

    }
}