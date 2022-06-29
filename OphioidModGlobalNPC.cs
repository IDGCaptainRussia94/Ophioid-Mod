using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OphioidMod
{
    public class OphioidNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public int fallthrough = -10;

        public override void AI(NPC npc)
        {
            if (fallthrough > -5)
            {

                fallthrough -= 1;
                if (fallthrough > -1 && fallthrough < 1)
                {

                    fallthrough = 1;
                    if (npc.velocity.Y > 0)
                    {
                        fallthrough = -5000;
                        npc.noTileCollide = true;
                        npc.netUpdate = true;
                    }
                }
            }
        }
    }
}