using System;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ionic.Zlib;
using Newtonsoft.Json;

namespace wServer.networking.handlers
{
    class UpgradeHandler : PacketHandlerBase<Upgrade>
    {
        public override PacketId ID => PacketId.UPGRADE;

        protected override void HandlePacket(Client client, Upgrade packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client, packet, t));
            getRandom();
        }

        int getRandom()
        {
            Random rand = new Random();
            return rand.Next(0, 9);
        }

        private void Handle(Client client, Upgrade packet, RealmTime time)
        {
            var plr = client.Player;
            var acc = client.Account;
            var chr = client.Character;

            if (packet.Lunar == true)
            {
                if (acc.Fame >= 1000)
                {
                    if (acc.PotionStorageLunarLevel < 25)
                    {
                        acc.Fame -= 1000;
                        acc.PotionStorageLunarLevel++;
                        plr.ResolveStorageSize();
                        acc.FlushAsync();
                        plr.SendInfo("Your storage has been upgraded, you can now store " + acc.PotionStorageLunarSize + " celestial enhancers");
                    }
                    else
                    {
                        plr.SendError("You can't upgrade this storage anymore");
                    }
                }
                else
                {
                    plr.SendError("You don't have enought fame to buy this upgrade.");
                }
                return;
            }
            if (packet.Earth == true)
            {
                if (acc.Fame >= 500)
                {
                    if (acc.PotionStorageLevel < 10)
                    {
                        acc.Fame -= 500;
                        acc.PotionStorageLevel++;
                        plr.ResolveStorageSize();
                        acc.FlushAsync();
                        plr.SendInfo("Your storage has been upgraded, you can now store " + acc.PotionStorageSize + " potions of each type.");
                    }
                    else
                    {
                        plr.SendError("You can't upgrade this storage anymore");
                    }
                }
                else
                {
                    plr.SendError("You don't have enought fame to buy this upgrade.");
                }
                return;
            }
            if (packet.Enhancer == true)
            {
                if (chr.MoonPrimed == true)
                {
                    switch (getRandom())
                    {
                        case 0:
                            if (chr.LifePotsMoon < 50)
                            {
                                plr.Stats.Base[0] += 5;
                                chr.LifePotsMoon += 5;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 5 extra Health. Total [{ chr.LifePotsMoon }] / 50");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                        case 1:
                            if (chr.ManaPotsMoon < 50)
                            {
                                plr.Stats.Base[1] += 5;
                                chr.ManaPotsMoon += 5;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 5 extra Mana. Total [{ chr.ManaPotsMoon }] / 50");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                        case 2:
                            if (chr.AttackStatsMoon < 10)
                            {
                                plr.Stats.Base[2] += 1;
                                chr.AttackStatsMoon += 1;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 1 extra Attack. Total [{ chr.AttackStatsMoon }] / 10");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                        case 3:
                            if (chr.DefensePotsMoon < 10)
                            {
                                plr.Stats.Base[3] += 1;
                                chr.DefensePotsMoon += 1;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 1 extra Defense. Total [{ chr.DefensePotsMoon }] / 10");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                        case 4:
                            if (chr.SpeedPotsMoon < 10)
                            {
                                plr.Stats.Base[4] += 1;
                                chr.SpeedPotsMoon += 1;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 1 extra Speed. Total [{ chr.SpeedPotsMoon }] / 10");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                        case 5:
                            if (chr.DexterityPotsMoon < 10)
                            {
                                plr.Stats.Base[5] += 1;
                                chr.DexterityPotsMoon += 1;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 1 extra Dexterity. Total [{ chr.DexterityPotsMoon }] / 10");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                        case 6:
                            if (chr.VitalityPotsMoon < 10)
                            {
                                plr.Stats.Base[6] += 1;
                                chr.VitalityPotsMoon += 1;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 1 extra Vitality. Total [{ chr.VitalityPotsMoon }] / 10");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                        case 7:
                            if (chr.WisdomPotsMoon < 10)
                            {
                                plr.Stats.Base[7] += 1;
                                chr.WisdomPotsMoon += 1;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 1 extra Wisdom. Total [{ chr.WisdomPotsMoon }] / 10");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                        case 8:
                            if (chr.CritDmgPotsMoon < 10)
                            {
                                plr.Stats.Base[11] += 1;
                                chr.CritDmgPotsMoon += 1;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 1 extra Critical Damage. Total [{ chr.CritDmgPotsMoon }] / 10");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                        case 9:
                            if (chr.CritHitPotsMoon < 10)
                            {
                                plr.Stats.Base[12] += 1;
                                chr.CritHitPotsMoon += 1;
                                acc.FlushAsync();
                                plr.SendInfo($"You have gained 1 extra Health. Total [{ chr.CritHitPotsMoon }] / 10");
                            }
                            else
                            {
                                plr.SendInfo("This enhancer has failed.");
                            }
                            return;
                    }
                }
                else
                {
                    plr.SendInfo("Your character is unable to use these!");
                }
                return;
            }
        }
    }
}
