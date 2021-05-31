using System.Collections.Generic;
using System.Linq;
using common.resources;
using wServer.realm.terrain;
using wServer.networking;

namespace wServer.realm.worlds.logic
{
    class MoonCastle : World
    {
        private int PlayersEntering;

        public MoonCastle(ProtoWorld proto, Client client = null, int playersEntering = 20) : base(proto)
        {
            PlayersEntering = playersEntering;
        }

        public MoonCastle(ProtoWorld proto, Client client = null) : base(proto)
        {
            // this is to get everyone in one spawn when the Castle comes from quaking
            PlayersEntering = 0;
        }
        
        //later we can add stuff there to be based of players in the instance
        //override virtual from World.cs, and use ut ub
    }
}
