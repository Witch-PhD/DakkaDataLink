using Comms_Core;
using Grpc.Core;

namespace Comms_Core.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
       // private readonly ILogger<GreeterService> _logger;
        public GreeterService()
        {
            
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
