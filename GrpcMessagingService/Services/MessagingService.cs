using System.Text;
using Google.Api;
using Grpc.Core;
using Newtonsoft.Json;

namespace GrpcMessagingService.Services
{
    public class MessagingService : Messaging.MessagingBase
    {
        private readonly ILogger<MessagingService> _logger;
        public MessagingService(ILogger<MessagingService> logger)
        {
            _logger = logger;
        }

        public override Task<SendResponse> Send(SendRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"RequestHeaders: [{BuildRequestHeaders(context)}]");
#if DEBUG
            _logger.LogDebug($"Request: {JsonConvert.SerializeObject(request)}");
#endif
            // TODO: дописать отправку сообщения в брокер сообщений:
            var response = new SendResponse()
            {
                Result = "Сообщение успешно отправлено",
                Number = 1000,
            };

            return Task.FromResult(response);

        }

        public override Task<ReceiveResonse> Receive(ReceiveRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"RequestHeaders: [{BuildRequestHeaders(context)}]");
#if DEBUG
            _logger.LogDebug($"Request: {JsonConvert.SerializeObject(request)}");
#endif

            var resonse = new ReceiveResonse()
            {
                Message = $"Рандомное сообщение по номеру: {request.Number}",
            };

            return Task.FromResult(resonse);
        }

        private static string BuildRequestHeaders(ServerCallContext context)
        {
            var sb = new StringBuilder(Environment.NewLine);

            foreach (var header in context.RequestHeaders)
            {
                sb.AppendLine($"{header.Key}: {header.Value}");
            }

            return sb.ToString();
        }
    }
}
