using common;
using System;
using System.Collections.Generic;

namespace wServer.networking.packets.outgoing
{
    public class ForgeListResult : OutgoingMessage
    {
        public override PacketId ID => PacketId.FORGE_LIST_RESULT;

        public string Result { get; set; }
        public List<string> Recipes { get; set; }

        public override Packet CreateInstance() => new ForgeListResult();

        protected override void Read(NReader rdr)
        {
            Result = rdr.ReadString();

            Recipes = new List<string>();
            for (var i = 0; i < rdr.ReadInt32(); i++)
                Recipes.Add(rdr.ReadString());
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Result);
            wtr.Write(Recipes.Count);

            foreach (var r in Recipes)
                wtr.WriteUTF(r);
        }
    }
}
