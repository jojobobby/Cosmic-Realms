#region

using common.resources;
using System;
using System.Linq;
using wServer.logic.loot;
using wServer.realm.entities;
using wServer.realm.terrain;
using wServer.realm.worlds;

#endregion

namespace wServer.realm.setpieces
{
    internal class LightningGod2 : ISetPiece
    {
        private static readonly string Floor = "1EligGround";

        private readonly Random rand = new Random();

        public int Size
        {
            get { return 30; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity lord = Entity.Resolve(world.Manager, "Legendary Dragon Kakashi");
            lord.Move(pos.X + 15.5f, pos.Y + 15.5f);
            world.EnterWorld(lord);

        }
    }
}