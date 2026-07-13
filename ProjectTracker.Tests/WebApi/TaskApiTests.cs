using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProjectTracker.Application.Tasks.Commands;

namespace ProjectTracker.Tests.WebApi;

public class TaskApiTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TaskApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async System.Threading.Tasks.Task PostTask_WithValidData_ReturnsCreatedId()
    {
        //arrange: make http client connected in-memory api
        var client = _factory.CreateClient();

        // DTO send via body req
        var command = new CreateTaskCommand("Setup WebApi", "Wire up DI and MediatR");

        // act: do http post to endpoint
        var response = await client.PostAsJsonAsync("/api/tasks", command);

        // assert: confirm succeed and return ID
        response.EnsureSuccessStatusCode();

        var taskId = await response.Content.ReadFromJsonAsync<Guid>();
        Assert.NotEqual(Guid.Empty, taskId);

    }
}