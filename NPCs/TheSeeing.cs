using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace OphioidMod.NPCs
{
    public class TheSeeing : ModNPC
    {
        public override string Texture
        {
            get { return ("OphioidMod/NPCs/TheSeeing"); }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Seeing");
            Main.npcFrameCount[NPC.type] = 1;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 60;
            NPC.defense = 15;
            NPC.lifeMax = 3000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.3f;
            NPC.aiStyle = -1;
            NPC.boss = false;
            AIType = NPCID.Wraith;
            AnimationType = 0;
            NPC.behindTiles = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            Music = MusicID.Boss2;
            NPC.value = 90000f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            int associatedNPCType = ModContent.NPCType<Ophiofly>();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Dislodged Ophiopede eyes")
            });
        }

        public override bool CheckActive()
        {
            return !Main.npc[(int)NPC.ai[3]].active;
        }

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(BuffID.Darkness, 60 * 4, true);
        }



        public override void AI()
        {

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)NPC.ai[3]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                }
            }

            NPC.TargetClosest(true);
            Player ply = Main.player[NPC.target];
            float ownerspeed = 0.25f + Main.npc[(int)NPC.ai[3]].velocity.Length() / 25f;
            NPC.ai[0] += 1;
            if (NPC.ai[0] % 300 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[2] = NPC.ai[2] + Main.rand.Next(-30, 30);
                NPC.ai[1] = Main.rand.Next(90, 180);
                NPC.netUpdate = true;
            }
            Vector2 capvelo = NPC.velocity;
            capvelo.Normalize();
            Vector2 diff = new Vector2(Main.npc[(int)NPC.ai[3]].Center.X - NPC.Center.X, Main.npc[(int)NPC.ai[3]].Center.Y - NPC.Center.Y);
            if (diff.Length() > NPC.ai[1])
            {
                Vector2 newdif = diff; newdif.Normalize();
                NPC.velocity += (newdif * ((diff.Length() - 160f) / 50f)) * (1.4f * ownerspeed);
            }

            if (ply.active)
            {
                Vector2 diff4 = new Vector2(ply.Center.X - NPC.Center.X, ply.Center.Y - NPC.Center.Y);
            }
            double angle = 2.0 * Math.PI * ((NPC.ai[2] + ((float)(Main.npc[(int)NPC.ai[3]].rotation / Math.PI) * 360f)) / 360f);
            Vector2 diff3 = new Vector2((float)Math.Cos(angle) * 5f, (float)Math.Sin(angle) * 5f);//new Vector2(ply.Center.X-NPC.Center.X,ply.Center.Y-NPC.Center.Y);
            diff3.Normalize();
            NPC.velocity += (diff3) * (1.8f * ownerspeed);

            NPC.rotation = (float)Math.Atan2((double)diff3.Y, (double)diff3.X) + 1.57f;
            NPC.velocity *= 0.93f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            IDGHelper.DrawTether("OphioidMod/NPCs/tether", Main.npc[(int)NPC.ai[3]].Center, NPC.Center, screenPos);
            return true;
        }
    }
}