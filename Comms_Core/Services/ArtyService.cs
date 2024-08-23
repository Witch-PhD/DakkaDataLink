
using Grpc.Core;

namespace Comms_Core.Services
{
    public class ArtyService : Arty.ArtyBase
    {
        //private readonly ILogger<GreeterService> _logger;
        public ArtyService()
        {
            //_logger = logger;
        }


        public override async Task openStream(IAsyncStreamReader<ArtyMsg> requestStream, IServerStreamWriter<ArtyMsg> responseStream, ServerCallContext context)
        {
           //
           //PitBoss.DataManager.
           //while (await requestStream.MoveNext(context.CancellationToken))
           //{
           //    Coords newCoords = requestStream.Current;
           //}
        }
    }
}
