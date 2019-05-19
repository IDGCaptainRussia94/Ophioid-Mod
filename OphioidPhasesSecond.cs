using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using System;
using Idglibrary;



namespace Ophioid
{

        [AutoloadBossHead]
    public class OphiopedeHead2 : OphiopedeHead
    {
        public override string Texture
        {
            get { return("Ophioid/wormmiscparts"); }
        }
        
        public override void StartPhaseTwo()
        {
        if (Main.netMode!=1){
        if (npc.life<(npc.lifeMax*0.50)){
        npc.active=false;
        }}
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 70;
            npc.height = 70;
            npc.damage = 200;
            npc.defense = 25;
            npc.lifeMax = 72000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.boss=true;
            aiType = NPCID.Wraith;
            animationType = 0;
            npc.behindTiles = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            music = MusicID.Boss2;
            npc.value = 90000f;
        }
    }



}
