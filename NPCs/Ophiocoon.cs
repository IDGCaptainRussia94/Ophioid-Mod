using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using System;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;

namespace OphioidMod.NPCs
{
    [AutoloadBossHead]
    public class Ophiocoon : ModNPC, ISinkyBoss
    {

        Player ply;
        bool no2ndphase = false;
        float whichway = 0f;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override bool CheckActive()
        {
            if (NPC.life > 0)
                no2ndphase = true;
            return true;
        }

        public override string Texture
        {
            get { return ("OphioidMod/NPCs/cocoon"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiocoon");
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
            {
                CustomTexturePath = "OphioidMod/NPCs/cocoon",
                PortraitScale = 0.6f, // Portrait refers to the full picture when clicking on the icon in the bestiary
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            int associatedNPCType = ModContent.NPCType<Ophiofly>();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Ophiopede metamorphosizes into Ophiofly!")
            });
        }

        public override void OnKill()
        {
            //SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.position);
            for (int a = 0; a < 500; a++)
            {
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Vector2 vecr = randomcircle;

                int num622 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ScourgeOfTheCorruptor, 0f, 0f, 100, default, 3f);
                Main.dust[num622].velocity = randomcircle * new Vector2((float)Main.rand.Next(-1000, 1000) / 100f, (float)Main.rand.Next(-1000, 1000) / 100f);

                Main.dust[num622].noGravity = true;
                Main.dust[num622].color = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5) % 1, 0.9f, 1f);
                Main.dust[num622].color.A = 10;
                Main.dust[num622].alpha = 100; ;
            }
            for (int i = 1; i < 6; i += 1)
            {
                Vector2 Vect = new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)); Vect.Normalize();
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vect, ModContent.Find<ModGore>("OphioidMod/cocoon_gore_" + i).Type, 1f);
            }
        }

        public void Falldown()
        {
            NPC.velocity.Y += 0.25f;

            bool wallblocking = false;
            int y_top_edge = (int)(NPC.position.Y - 16f) / 16;
            int y_bottom_edge = (int)(NPC.position.Y + (float)NPC.height + 16f) / 16;
            int x_left_edge = (int)(NPC.position.X - 16f) / 16;
            int x_right_edge = (int)(NPC.position.X + (float)NPC.width + 16f) / 16;

            for (int x = x_left_edge; x <= x_right_edge; x++)
            {
                for (int y = y_top_edge; y <= y_bottom_edge; y++)
                {
                    if (Main.tile[x, y].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[x, y].TileType] && !Main.tileSolidTop[(int)Main.tile[x, y].TileType])
                    {
                        wallblocking = true;
                        break;
                    }
                }
                if (wallblocking) break;
            }

            if (wallblocking == true)
            {

                NPC.life = 0;
                NPC.HitEffect(0, 10.0);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    IDGHelper.Chat("The Ophiofly hatches from the Cocoon!", 100, 225, 100);

                    int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
                    int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
                    int num663 = ModContent.NPCType<Ophiofly>();

                    int num664 = NPC.NewNPC(NPC.GetSource_FromAI(), x, y, num663);
                    if (Main.netMode == NetmodeID.Server && num664 < 200)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }
                }

                NPC.active = false;
            }
        }

        public override void SetDefaults()
        {
            NPC.width = 96;
            NPC.height = 256;
            NPC.damage = 0;
            NPC.defense = 75;
            NPC.lifeMax = 2000000;
            NPC.knockBackResist = 0f;
            NPC.value = 0f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.dontTakeDamage = true;
            NPC.immortal = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Centipede_Mod_-_Metamorphosis");
            AIType = -1;
            AnimationType = -1;
            NPC.BossBar = ModContent.GetInstance<OphioidBossBar>();
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void AI()
        {
            NPC.velocity /= 1.05f;
            NPC.ai[0] += 1;
            //ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
            if (NPC.ai[0] == 25)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 7; i += 1)
                    {
                        int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
                        int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
                        int num663 = ModContent.NPCType<FlyMinionCacoon>();

                        //HalfVector2 half=new HalfVector2(Main.rand.Next(-120,120),Main.rand.Next(-380,240));

                        int num664 = NPC.NewNPC(NPC.GetSource_FromAI(), x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                        Main.npc[num664].ai[0] = Main.rand.Next(0, 3000);
                        Main.npc[num664].ai[1] = NPC.whoAmI;
                        //Main.npc[num664].life = (int)(NPC.life*0.007);
                        //Main.npc[num664].lifeMax = (int)(NPC.lifeMax*0.007);
                        Main.npc[num664].netUpdate = true;
                        if (Main.netMode == NetmodeID.Server && num664 < 200)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
                NPC.life = 1;
                NPC.lifeMax = 1;
            }

            if (NPC.ai[1] < 0)
            {
                Falldown();
                return;
            }

            if (NPC.ai[0] > 30)
            {
                NPC.ai[1] -= 3;
                if (NPC.ai[1] > 10)
                    NPC.ai[1] = 10;
            }

            int minTilePosX = (int)(NPC.Center.X / 16.0) - 1;
            int minTilePosY = (int)((NPC.Center.Y + 32f) / 16.0) - 1;

            if (whichway == 0f)
            {
                whichway = minTilePosX > (int)(Main.maxTilesX / 2) ? -1f : 1f;
            }


            if (minTilePosX < 32 || minTilePosX > Main.maxTilesX - 32)
            {
                NPC.active = false;
                return;
            }

            NPC.ai[2] -= 1;

            if (NPC.ai[2] % 10 == 0 && NPC.ai[2] > 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int him = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + (float)(NPC.width / 2) + NPC.velocity.X), (int)(NPC.position.Y + (float)(NPC.height / 2) + NPC.velocity.Y), ModContent.NPCType<EvenMoreVileSpit>(), 0, 0f, 0f, 0f, 0f, 255);
                    //NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, mod.ProjectileType("EvenMoreVileSpit"));
                    if (him >= 0)
                    {
                        Main.npc[him].velocity = new Vector2(Main.rand.Next(5, 18) * (Main.rand.Next(0, 2) == 0 ? 1 : -1), Main.rand.Next(-4, 4));
                        Main.npc[him].timeLeft = 200;
                        Main.npc[him].damage = 100;
                        Main.npc[him].netUpdate = true;
                    }
                    //IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);
                }
            }

            if (NPC.ai[0] > 700)
            {
                NPC.ai[0] = 50;
                NPC.ai[2] = 150;

                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.CountNPCS(ModContent.NPCType<FlyMinion>()) < 5)
                {
                    int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
                    int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
                    int num663 = ModContent.NPCType<FlyMinion>();

                    int num664 = NPC.NewNPC(NPC.GetSource_FromAI(), x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                    Main.npc[num664].ai[1] = NPC.whoAmI;
                    if (Main.netMode == NetmodeID.Server && num664 < 200)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }


            int whereisity;
            whereisity = IDGHelper.RaycastDown(minTilePosX + 1, Math.Max(minTilePosY, 0));

            bool wallblocking = false;
            int y_top_edge = (int)(NPC.position.Y - 16f) / 16;
            int y_bottom_edge = (int)(NPC.position.Y + (float)NPC.height + 16f) / 16;
            int x_left_edge = (int)(NPC.position.X - 16f) / 16;
            int x_right_edge = (int)(NPC.position.X + (float)NPC.width + 16f) / 16;

            for (int x = x_left_edge; x <= x_right_edge; x++)
            {
                for (int y = y_top_edge; y <= y_bottom_edge; y++)
                {
                    if (Main.tile[x, y].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[x, y].TileType] && !Main.tileSolidTop[(int)Main.tile[x, y].TileType])
                    {
                        wallblocking = true;
                        break;
                    }
                }
                if (wallblocking) break;
            }

            if (wallblocking)
            {
                NPC.velocity -= new Vector2(0, 0.3f);
            }
            else
            {
                if (whereisity - minTilePosY > 80)
                    NPC.velocity += new Vector2(0, 1.6f);
                NPC.velocity += new Vector2(0.4f * whichway, 0);
            }
            if (NPC.velocity.Length() > 12f)
            {
                NPC.velocity.Normalize(); NPC.velocity *= 12;
            }
            NPC.rotation = (NPC.velocity.X / 15f);
        }
    }

    public class MetaOphiocoon : ModNPC, ISinkyBoss
    {
        public override string Texture
        {
            get { return ("OphioidMod/NPCs/cocoon"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiocoon");
            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new(0)
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
        }

        public override void AI()
        {
            NPC.ai[0] += 1;

            if (NPC.ai[0] > 10)
            {

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {

                    int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
                    int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
                    int num663 = ModContent.NPCType<Ophiocoon>();

                    int num664 = NPC.NewNPC(NPC.GetSource_FromAI(), x, y, num663);
                    if (Main.netMode == NetmodeID.Server && num664 < 200)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }

                }
                NPC.active = false;
            }

        }

        public override void SetDefaults()
        {
            NPC.width = 96;
            NPC.height = 256;
            NPC.damage = 0;
            NPC.defense = 75;
            NPC.lifeMax = 2000000;
            NPC.knockBackResist = 0f;
            NPC.value = 0f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.dontTakeDamage = true;
            NPC.immortal = true;
            AIType = -1;
            AnimationType = -1;
            NPC.BossBar = ModContent.GetInstance<OphioidBossBar>();
        }
    }
}