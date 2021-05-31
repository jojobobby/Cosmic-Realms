#region

using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.terrain;
using wServer.realm.worlds;

#endregion

namespace wServer.realm.setpieces
{
    internal class LightningGod : ISetPiece
    {
        private static readonly string Floor = "1EligGround";

    

        private readonly Random rand = new Random();

        public int Size
        {
            get { return 60; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
           

            Entity lord = Entity.Resolve(world.Manager, "Legendary Dragon Kakashi");
            lord.Move(pos.X + 30f, pos.Y + 30f);
            world.EnterWorld(lord);
        }
    }
}