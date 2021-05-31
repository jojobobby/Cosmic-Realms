using common;
using log4net;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using wServer.realm;

namespace wServer.networking.handlers
{
    class HelloHandler : PacketHandlerBase<Hello>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HelloHandler));

        public override PacketId ID => PacketId.HELLO;

        protected override void HandlePacket(Client client, Hello packet)
        {
            //client.Manager.Logic.AddPendingAction(t => Handle(client, packet));
            Handle(client, packet);
        }

        private void Handle(Client client, Hello packet)
        {
            var reconnecting = client.State == ProtocolState.Reconnecting;
            if (!reconnecting)
            {
                // get acc info
                client.Manager.Database.Verify(packet.GUID, packet.Password, out var acc);
                if (acc == null)
                    return;
              //  client.Manager.Database.LogAccountByIp(client.IP, acc.AccountId);
              //  if (client.IP != acc.IP && !(System.String.IsNullOrEmpty(client.IP) || System.String.IsNullOrEmpty(acc.IP)))
              //      wServer.networking.webhooks.Webhooks.SendToDiscordAsLoginLog("New IP", "", $"{acc.Name} logged from new ip.\nnew IP: **{client.IP}**\nold IP: **{acc.IP}**\nall accounts associated to old IP:\n**{client.Manager.Database.GetAccountsByIP(acc.IP)}**\nall accounts associated to new IP:\n**{client.Manager.Database.GetAccountsByIP(client.IP)}**");

                
                acc.IP = client.IP;
                acc.FlushAsync();
                client.Account = acc;
            }

            if (!VerifyConnection(client, packet, client.Account))
                return;

            client.Manager.ConMan.Add(new ConInfo(client, packet, reconnecting));
        }

        private bool VerifyConnection(Client client, Hello packet, DbAccount acc)
        {
            var version = client.Manager.Config.serverSettings.version;
            if (!version.Equals(packet.BuildVersion))
            {
                client.SendFailure(version, Failure.ClientUpdateNeeded);
                return false;
            }

            if (acc.Banned)
            {
                client.SendFailure("Account banned.", Failure.MessageWithDisconnect);
                Log.InfoFormat("{0} ({1}) tried to log in. Account Banned.",
                    acc.Name, client.IP);
                return false;
            }

            if (client.Manager.Database.IsIpBanned(client.IP))
            {
                client.SendFailure("IP banned.", Failure.MessageWithDisconnect);
                Log.InfoFormat("{0} ({1}) tried to log in. IP Banned.",
                    acc.Name, client.IP);
                return false;
            }

            if (!acc.Admin && client.Manager.Config.serverInfo.adminOnly)
            {
                client.SendFailureDialog("Admin Only Server", $"Only admins can play on {client.Manager.Config.serverInfo.name}.");
                return false;
            }

            var minRank = client.Manager.Config.serverInfo.minRank;
            if (acc.Rank < minRank)
            {
                client.SendFailureDialog("Rank Required Server", $"You need a minimum server rank of {minRank} to play on {client.Manager.Config.serverInfo.name}.");
                return false;
            }

            return true;
        }
    }
}