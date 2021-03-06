﻿using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API;
using TaskManager.API.Models;
using Xunit;

namespace TaskManager.APITests
{
    [Trait("Category", "API")]
    public class TaskTests
    {

        [Fact]
        public async Task Task_Get()
        {

            using (var serverAndClient = new HttpServerAndClient<Startup>())
            {

                var response = await serverAndClient.Client.GetAsync("api/task");

                response.EnsureSuccessStatusCode();

            }

        }

        [Fact]
        public async Task<Guid> Task_Post()
        {

            using (var serverAndClient = new HttpServerAndClient<Startup>())
            {
                
                var response = await serverAndClient.Client.PostAsJsonAsync<TaskItem>(
                    "api/task",
                    new TaskItem()
                    {
                        Name = "Task 1"
                    });

                response.EnsureSuccessStatusCode();

                var task = await response.Content.ReadAsAsync<TaskItem>();

                return task.ID;

            }

        }

        [Fact]
        public async Task Task_Patch()
        {

            var id = await Task_Post();

            using (var serverAndClient = new HttpServerAndClient<Startup>())
            {
                
                var response = await serverAndClient.Client.PatchAsJsonAsync(
                    "api/task/" + id,
                    new TaskItem()
                    {
                        ID = id,
                        Name = "Task 1"
                    });

                response.EnsureSuccessStatusCode();
                
            }

        }

        [Fact]
        public async Task Task_GetSingle()
        {

            var id = await Task_Post();

            using (var serverAndClient = new HttpServerAndClient<Startup>())
            {

                var response = await serverAndClient.Client.GetAsync("api/task/" + id);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

            }

        }
        
    }

    public static class Extensons
    {

        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            //Ensure.Argument.NotNull(client, "client");
            //Ensure.Argument.NotNullOrEmpty(requestUri, "requestUri");
            //Ensure.Argument.NotNull(value, "value");

            var content = new ObjectContent<T>(value, new JsonMediaTypeFormatter());
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content };

            return client.SendAsync(request);
        }

    }

}
