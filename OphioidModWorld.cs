using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.GameContent.ItemDropRules;
using System;
using System.Linq;
using static Terraria.GameContent.ItemDropRules.Conditions;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Audio;
using OphioidMod.NPCs;

namespace OphioidMod
{
    public class OphioidWorld : ModSystem
    {
        #region vars
        public static bool downedOphiopede = false;
        public static bool downedOphiopede2 = false;
        #endregion

        static public bool OphioidBoss
        {
            get
            {
                return NPC.CountNPCS(ModContent.NPCType<OphiopedeHead>()) + NPC.CountNPCS(ModContent.NPCType<Ophiofly>()) + NPC.CountNPCS(ModContent.NPCType<Ophiocoon>()) + NPC.CountNPCS(ModContent.NPCType<OphiopedeHead2>()) > 0;
            }
        }

        public override void OnWorldLoad()
        {
            downedOphiopede = false;
            downedOphiopede2 = false;
        }

        public override void OnWorldUnload()
        {
            downedOphiopede = false;
            downedOphiopede2 = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (downedOphiopede)
            {
                tag["downedOphiopede"] = true;
            }
            if (downedOphiopede2)
            {
                tag["downedOphiopede2"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            //var Ophioidsavedata = tag.GetList<string>("Ophioidsavedata");
            downedOphiopede = tag.GetBool("downedOphiopede");
            downedOphiopede2 = tag.GetBool("downedOphiopede2");
        }
        public override void NetSend(BinaryWriter writer)
        {
            var bossdeaths = new BitsByte();
            bossdeaths[0] = downedOphiopede;
            bossdeaths[1] = downedOphiopede2;
            writer.Write(bossdeaths);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte bossdeaths = reader.ReadByte();
            downedOphiopede = bossdeaths[0];
            downedOphiopede2 = bossdeaths[1];
        }
    }
}