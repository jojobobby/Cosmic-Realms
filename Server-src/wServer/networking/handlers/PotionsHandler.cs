using System;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm;

namespace wServer.networking.handlers
{
    class PotionsHandler : PacketHandlerBase<Potions>
    {
        public override PacketId ID => PacketId.POTIONS;

        protected override void HandlePacket(Client client, Potions packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client, packet, t));
        }

        private void Handle(Client client, Potions packet, RealmTime time)
        {
            var plr = client.Player;
            var acc = client.Account;
            var potionStoragePotions = acc.PotionStoragePotions;
            var statInfo = plr.Manager.Resources.GameData.Classes[plr.ObjectType].Stats;

            int potion = packet.Type;
            bool max = packet.Max;
            int consumeAmount = (potion == 0 || potion == 1) ? 5 : 1;
            int leftToMax = statInfo[potion].MaxValue - plr.Stats.Base[potion];

            if (plr.Stats.Base[potion] >= statInfo[potion].MaxValue)
            {
                plr.SendInfo("This stat is already maxed!");
                return;
            } 
            else
            {
                if (max == true)
                {
                    if (potionStoragePotions[potion] < statInfo[potion].MaxValue - plr.Stats.Base[potion])
                    {
                        plr.SendInfo("You don't have enough potions to max!");
                        return;
                    }
                    else
                    {
                        plr.Stats.Base[potion] += consumeAmount * leftToMax;
                        potionStoragePotions[potion] -= leftToMax;
                    }
                }
                else
                {
                    plr.Stats.Base[potion] += consumeAmount;
                    potionStoragePotions[potion]--;
                    acc.PotionStoragePotions = potionStoragePotions;
                    acc.FlushAsync();
                }
            }
        }
    }
}
