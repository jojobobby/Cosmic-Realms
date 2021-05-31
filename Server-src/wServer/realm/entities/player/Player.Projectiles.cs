using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common.resources;
using wServer.logic;

namespace wServer.realm.entities
{
    public partial class Player
    {
        internal Projectile PlayerShootProjectile(
            byte id, ProjectileDesc desc, ushort objType,
            int time, Position position, float angle)
        {
            projectileId = id;
            var dmg = (int) Stats.GetAttackDamage(Stats[8], Stats[9]);
            return CreateProjectile(desc, objType, dmg,
                C2STime(time), position, angle);
        }
    }
}
