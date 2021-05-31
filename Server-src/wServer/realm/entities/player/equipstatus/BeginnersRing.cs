using common.resources;
using wServer.networking.packets.outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using common.resources;
using common;
using wServer.logic;
using wServer.networking.packets;
using wServer.networking;
using wServer.realm.terrain;
using log4net;
using wServer.networking.packets.outgoing;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;
using DiscordWebhook;


namespace wServer.realm.entities.player.equipstatus
{
    class BeginnersRing : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.BeginnersRing;

        public void OnEquip(Player player)
        {
        }

        public void OnHit(Player player, int dmg)
        {
            if (player.HP <= player.MaximumHP / 4)
            {
                //using wServer.networking.packets.outgoing;
                if (RandomUtil.RandInt(1, 10) == 1)
                {
                    //using wServer.networking.packets.outgoing;
                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0x636262),
                        Message = "{\"key\": \"Beginner's Shield\"}"
                    }, PacketPriority.Low);

                    player.ApplyConditionEffect(ConditionEffectIndex.Invincible, 3000);
                    player.ApplyConditionEffect(ConditionEffectIndex.Paralyzed, 3000);
                    player.ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 4500);
                    player.ApplyConditionEffect(ConditionEffectIndex.Healing, 6000);
                }
            }
        }

        public void OnTick(Player player, RealmTime time)
        {
        }

        public void Unequip(Player player)
        {
        }
    }
}
