using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PitBoss
{
    public static class Constants
    {
        public enum ConnectionStatus
        {
            Disconnected,
            Connected_As_Gunner,
            Connected_As_Spotter,
            Error
        }
    }
}
