using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class LGAbility3 : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.LGAbility3;

        public void OnEquip(Player player)
        {
            //using wServer.networking.packets.outgoing;
            player.Client.SendPacket(new Notification()
            {
                ObjectId = player.Id,
                Color = new ARGB(0x0000FF),
                Message = "{\"key\": \"Hydro's Assistance\"}"
            }, PacketPriority.Low);
        }

        public void OnHit(Player player, int dmg)
        {


            player.Client.SendPacket(new Notification()
            {
                ObjectId = player.Id,
                Color = new ARGB(0x0000FF),
                Message = "+[" + (10 + (player.Stats[7] / 5)) +"MP] +[" +(10 + (player.Stats[6] / 5)) +"HP]"
            }, PacketPriority.Low);

            player.MP += 10 + (player.Stats[7]/5);
            player.HP += 10 + (player.Stats[6]/5);

        }


        public void OnTick(Player player, RealmTime time)
        {
        }

        public void Unequip(Player player)
        {
        }
    }
}
