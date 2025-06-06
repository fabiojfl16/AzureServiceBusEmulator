using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PublisherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AzureServiceBusController(ServiceBusClient client) : ControllerBase
{
    private readonly ServiceBusClient _client = client;

    [HttpPost]
    public async Task<IActionResult> PostEvent([FromBody][Required] string message = "Hello World")
    {
        var serviceBusMsg = new ServiceBusMessage(message);
        var sender = _client.CreateSender("topic");
        await sender.SendMessageAsync(serviceBusMsg);

        return Ok("Event sent successfully.");
    }
}
