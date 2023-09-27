using CommandAPI.Interfaces;
using CommandAPI.Models;
using CommandAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Confluent.Kafka;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IKafkaPublisher<string,CommandMessage> _kafkaPublisher;
        IExecutor _executor;

        public HomeController(IKafkaPublisher<string, CommandMessage> kafkaPublisher, IExecutor executor)
        //public HomeController(IExecutor executor)
        {
            _kafkaPublisher = kafkaPublisher;
            _executor = executor;
        }

        [HttpPost]
        public async Task<ActionResult> ExecuteCommand(CommandRequest command)
        {
            try
            {
                var result = _executor.Execute(command.Command, command.Params);
                
                var message = new CommandMessage
                {
                    Command = command.Command,
                    Params = command.Params,
                    Result= result
                };


                var cancellationTokenSource = new CancellationTokenSource();
                
                var ret = await _kafkaPublisher.SendMessage(
                                    "AsyncMessaging", 
                                    "key1", 
                                    message, 
                                    cancellationTokenSource.Token);

                return Ok(ret);
            }
            catch
            {
                return BadRequest("Undefined command [" + command.Command + "]");
            }
        }
    }
}
