
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

    //    public override Task<MsgStatus> notifySubscribe(subscribeNotification subData, ServerCallContext context)
    //    {
    //        // Todo: Handle subscribe.
    //        Spotter.Instance.AddNewSubscriber(context.Peer);
    //        return Task.FromResult(new MsgStatus { StatusCode = 0 });
    //    }
    //
    //    public override Task<MsgStatus> notifyUnsubscribe(unsubscribeNotification unsubData, ServerCallContext context)
    //    {
    //        // Todo: Handle subscribe.
    //        
    //        return Task.FromResult(new MsgStatus { StatusCode = 0 });
    //    }
    //
    //    public override Task<MsgStatus> receiveNewCoords(Coords coords, ServerCallContext context)
    //    {
    //        GunBattery.Instance.receiveNewCoords(coords.Az, coords.Dist);
    //        return Task.FromResult(new MsgStatus { StatusCode = 0 });
    //    }

        public override async Task openStream(IAsyncStreamReader<Coords> requestStream, IServerStreamWriter<Coords> responseStream, ServerCallContext context)
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
