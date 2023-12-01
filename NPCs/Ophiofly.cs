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
using OphioidMod.Projectiles;
using OphioidMod.Items;

namespace OphioidMod.NPCs
{
    [AutoloadBossHead]
    public class Ophiofly : ModNPC, ISinkyBoss
    {

        public bool poweredup = true;
        public int chargesleft = 0;
        public int spawnminionsat = 0;
        private Vector2[] oldPos = new Vector2[4];
        private float[] oldrot = new float[4];
        float leftright = 0;
        int projectiletrack = 0;
        bool noplayer = false;
        bool glowred = false;
        Player ply = null;


        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * balance);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ophiofly");
            Main.npcFrameCount[NPC.type] = 5;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "OphioidMod/NPCs/BestiaryOphiofly",
                PortraitScale = 0.75f, // Portrait refers to the full picture when clicking on the icon in the bestiary
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BeeSmall);
            NPC.width = 128;
            NPC.height = 96;
            NPC.damage = 150;
            NPC.defense = 75;
            NPC.lifeMax = 75000;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(1, 0, 0, 0);
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Centipede_Mod_-_The_Fly");
            AIType = -1;
            AnimationType = -1;
            //BossBag = ModContent.ItemType<TreasureBagOphioid>(); Removed
            NPC.BossBar = ModContent.GetInstance<OphioidBossBar>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            //int associatedNPCType = ModContent.NPCType<Ophiocoon>();
            //bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("The fight against Ophiofly.")
            });
        }

        public override void SendExtraAI(BinaryWriter writer)   
        {
            writer.Write(chargesleft);
            if (ply==null)
                writer.Write(-1);
            else
                writer.Write(ply.whoAmI);
            writer.Write(poweredup);
            writer.Write(spawnminionsat);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            chargesleft=reader.ReadInt32();
            int ply2=reader.ReadInt32();
            if (ply2>-1)
                ply=Main.player[ply2];
            poweredup=reader.ReadBoolean();
            spawnminionsat=reader.ReadInt32();
        }

        public void Resetattacks()
        {
            if (spawnminionsat == 0)
                spawnminionsat = (int)(NPC.lifeMax * 0.8);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = 0;
                int[] pick = { 0, 1, 2, 2, 3, 4 };
                chargesleft = 2;

                NPC.ai[1] = pick[Main.rand.Next(0, pick.Length)];
                if (NPC.life < spawnminionsat)
                {
                    NPC.ai[1] = 5;
                    spawnminionsat -= (int)(NPC.lifeMax * 0.3);
                }
                NPC.netUpdate = true;
            }
        }

        public void Ichorbeam()
        {
            if (NPC.ai[0] > 300)
            {

                if (NPC.ai[0] == 301)
                {
                    leftright = (ply.Center.X - NPC.Center.X) > 0 ? 1 : -1;
                }

                Vector2 plyloc = ply.Center + new Vector2(leftright * 1200, -240);
                Vector2 plydist = (plyloc - NPC.Center);
                Vector2 plynormal = plydist; plynormal.Normalize();
                NPC.direction = (plynormal.X < 0f).ToDirectionInt();

                if (NPC.ai[0] > 300 && NPC.ai[0] < 400)
                {

                    plyloc = ply.Center + new Vector2(-leftright * 800, -320);
                    plydist = (plyloc - NPC.Center);
                    plynormal = plydist; plynormal.Normalize();
                    NPC.velocity += plynormal * 0.4f;
                    NPC.direction = (plydist.X < 0f).ToDirectionInt();
                }
                else
                {
                    NPC.velocity += plynormal * 0.8f;
                }

                NPC.velocity /= 1.025f;

                if (NPC.ai[0] == 400)
                {
                    List<Projectile> itz = IDGHelper.Shattershots(NPC.GetSource_FromThis(), NPC.Center, NPC.Center + new Vector2(-1 * NPC.direction, 4), new Vector2(0, 0), ModContent.ProjectileType<Ophiobeamichor>(), 15, 1f, 0, 1, true, 0f, false, 150);
                    //Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, NPC.position);
                    Main.projectile[projectiletrack].netUpdate = true;
                    SoundEngine.PlaySound(Deathray, NPC.position);
                    projectiletrack = itz[0].whoAmI;
                }

                if (projectiletrack > 0 && Main.projectile[projectiletrack].type == ModContent.ProjectileType<Ophiobeamichor>())
                {
                    Main.projectile[projectiletrack].position = NPC.Center + new Vector2(-NPC.direction * 20, 10);
                }

                if (NPC.ai[0] == 550) { Resetattacks(); projectiletrack = 0; }


                if (NPC.velocity.Length() > 18f) { NPC.velocity.Normalize(); NPC.velocity *= 18f; }
            }
        }

        public void Bodyslam()
        {
            if (NPC.ai[0] < 300)
                return;

            Vector2 plyloc = ply.Center + new Vector2(0, -320);
            Vector2 plydist = (plyloc - NPC.Center);
            Vector2 plynormal = plydist; plynormal.Normalize();

            int minTilePosX = (int)(NPC.Center.X / 16.0) - 1;
            int minTilePosY = (int)((NPC.Center.Y + 32f) / 16.0) - 1;

            int whereisity;
            whereisity = IDGHelper.RaycastDown(minTilePosX + 1, Math.Max(minTilePosY, 0));

            NPC.velocity /= 1.01f;
            if (NPC.ai[0] < 500)
            {
                NPC.velocity /= 1.025f;
                NPC.velocity += plynormal * 1.0f;
            }
            else
            {
                if (NPC.ai[0] < 600)
                {
                    NPC.ai[0] = 501;
                    NPC.velocity.Y += 0.2f + Math.Abs(NPC.velocity.Y / 14f);
                    if (NPC.velocity.Y > 32f)
                        NPC.velocity.Y = 32f;
                    if (whereisity - minTilePosY < 2 && NPC.Center.Y > ply.Center.Y)
                    {
                        NPC.ai[0] = 600;
                        NPC.AddBuff(BuffID.BrokenArmor, 60 * 10, true);

                        SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
                        SoundEngine.PlaySound(SoundID.Item90, NPC.Center);

                        for (float num315 = -10; num315 < 10; num315 += 2f)
                        {
                            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + Main.rand.Next(-8, 8), NPC.Center.Y - 40f, 0f, 0f, ProjectileID.DD2OgreSpit, 1, 0f, 0);
                            Main.projectile[num54].velocity = new Vector2(Main.rand.Next(-8, 8) * (Main.rand.Next(0, 2) == 0 ? 1 : -1) + (num315 / 4f), Main.rand.Next(-15, -3));
                            Main.projectile[num54].damage = (int)(50);
                            Main.projectile[num54].timeLeft = 800;
                            Main.projectile[num54].tileCollide = true;
                            Main.projectile[num54].netUpdate = true;
                            IdgProjectile.Sync(num54);
                            //IdgProjectile.AddOnHitBuff(num54, BuffID.Stinky, 60 * 15);
                        }

                        for (float num315 = -40; num315 < 40; num315 += 0.2f)
                        {
                            if (Main.rand.Next(0, 100) < 25)
                            {
                                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                                int num316 = Dust.NewDust(NPC.Center + new Vector2(num315 * 3, -30), 0, 80, DustID.ScourgeOfTheCorruptor, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 4f - Math.Abs(num315) / 15f);
                                Main.dust[num316].noGravity = true;
                                Dust dust3 = Main.dust[num316];
                                dust3.velocity = (randomcircle * 2.5f * Main.rand.NextFloat());
                                dust3.velocity.X += (float)num315 / 5f;
                            }
                        }

                    }
                }
            }
            if (NPC.ai[0] > 599)
                NPC.velocity = Vector2.Zero;

            if (NPC.ai[0] == 630) { Resetattacks(); projectiletrack = 0; }


            if (Main.rand.Next(300, 500) < NPC.ai[0])
            {
                glowred = true;
            }
        }

        public void Makeminions()
        {
            NPC.velocity /= 1.01f;

            for (int a = 0; a < 20; a++)
            {
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Vector2 vecr = randomcircle * 512;
                vecr *= (1f - (300f / (NPC.ai[0] % 300)));

                int num622 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y) + vecr, 0, 0, DustID.ScourgeOfTheCorruptor, 0f, 0f, 100, default, 3f);
                Main.dust[num622].velocity = randomcircle * -16f;

                Main.dust[num622].noGravity = true;
                Main.dust[num622].color = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5) % 1, 0.9f, 1f);
                Main.dust[num622].color.A = 10;
                Main.dust[num622].velocity.X = NPC.velocity.X / 3 + (Main.rand.Next(-50, 51) * 0.005f);
                Main.dust[num622].velocity.Y = NPC.velocity.Y / 3 + (Main.rand.Next(-50, 51) * 0.005f);
                Main.dust[num622].alpha = 100; ;
            }


            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[0] > 100 && NPC.ai[0] % 50 == 0)
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

            if (NPC.ai[0] == 300) { Resetattacks(); projectiletrack = 0; }
        }


        public void Sporeclouds()
        {

            if (NPC.ai[0] % 40 == 0 && NPC.ai[0] > 150)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int him = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + (float)(NPC.width / 2) + NPC.velocity.X), (int)(NPC.position.Y + (float)(NPC.height / 2) + NPC.velocity.Y), ModContent.NPCType<OphSporeCloud>(), 0, 0f, 0f, 0f, 0f, 255);
                    Main.npc[him].velocity = new Vector2(Main.rand.Next(10, 18) * (Main.rand.Next(0, 2) == 0 ? 1 : -1), Main.rand.Next(-4, 4));
                    Main.npc[him].netUpdate = true;
                }
                SoundEngine.PlaySound(SoundID.Item111, NPC.Center);

            }

            if (NPC.ai[0] < 300)
                return;

            Vector2 plyloc = ply.Center + new Vector2(0, -240);
            Vector2 plydist = (plyloc - NPC.Center);
            Vector2 plynormal = plydist; plynormal.Normalize();

            NPC.velocity /= 1.015f;
            NPC.velocity += plynormal * 0.5f;

            NPC.direction = (plydist.X < 0f).ToDirectionInt();

            if (NPC.ai[0] == 450) { Resetattacks(); projectiletrack = 0; }
        }

        public void Feralbite()
        {
            Vector2 plyloc = ply.Center + new Vector2(0, 16);
            Vector2 plydist = (plyloc - NPC.Center);
            Vector2 plynormal = plydist; plynormal.Normalize();
            NPC.direction = (ply.Center.X - NPC.Center.X < 0f).ToDirectionInt();
            glowred = true;

            NPC.velocity /= 1.02f;

            if (NPC.ai[0] > 10 && NPC.ai[0] < 80 && NPC.ai[0] % 5 == 0 && Main.expertMode)
            {
                List<Projectile> projectile22 = IDGHelper.Shattershots(NPC.GetSource_FromThis(), NPC.Center, plyloc, new Vector2(0, -16), ProjectileID.Stinger, 30, 20f, 0, 1, true, (float)Main.rand.Next(-100, 100) * 0.004f, false, 240);
                Projectile projectile2 = projectile22[0];
                IdgProjectile.Sync(projectile2.whoAmI);
                SoundEngine.PlaySound(SoundID.Item42, NPC.Center);

                for (int num315 = 1; num315 < 8; num315++)
                {
                    if (Main.rand.Next(0, 100) < 25)
                    {
                        Vector2 randomcircle = new(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                        int num316 = Dust.NewDust(new Vector2(projectile2.position.X - 1, projectile2.position.Y), projectile2.width, projectile2.height, DustID.ScourgeOfTheCorruptor, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 1.00f);
                        Main.dust[num316].noGravity = true;
                        Dust dust3 = Main.dust[num316];
                        dust3.velocity = (randomcircle * 2.5f * Main.rand.NextFloat());
                        dust3.velocity.Normalize();
                        dust3.velocity += (projectile2.velocity * 2f);
                    }
                }

            }

            if (NPC.ai[0] < 200 && NPC.ai[0] > 80)
                NPC.velocity += plynormal * 1.5f;

            if (NPC.ai[0] < 120)
                NPC.velocity /= 1.25f;
            if (NPC.velocity.Length() > 32f) { NPC.velocity.Normalize(); NPC.velocity *= 32f; }

            if (NPC.ai[0] == 240)
            {
                chargesleft -= 1;
                NPC.ai[0] = 60 - (int)((float)NPC.life / (float)NPC.lifeMax) * 100;
                NPC.netUpdate = true;
                if (chargesleft < 1) { Resetattacks(); projectiletrack = 0; }
            }
        }


        public static readonly SoundStyle Deathray = new("Terraria/Sounds/Zombie_104")
        {
            Volume = 1f,
            Pitch = 0f
        };

        public void Sludgefield()
        {

            if (NPC.ai[0] > 300)
            {

                if (NPC.ai[0] == 301)
                {
                    leftright = (ply.Center.X - NPC.Center.X) > 0 ? 1 : -1;
                }

                Vector2 plyloc = ply.Center + new Vector2(0, -320);
                Vector2 plydist = (plyloc - NPC.Center);
                Vector2 plynormal = plydist; plynormal.Normalize();
                if (NPC.ai[0] < 450)
                    NPC.direction = (ply.Center.X - NPC.Center.X < 0f).ToDirectionInt();

                if (NPC.ai[0] < 450)
                {

                    for (int num315 = 1; num315 < 8; num315++)
                    {
                        if (Main.rand.Next(0, 100) < 25)
                        {
                            Vector2 randomcircle = new(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                            int num316 = Dust.NewDust(NPC.Center + new Vector2((-NPC.direction * 20) - 12, 12), 24, 24, DustID.ScourgeOfTheCorruptor, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 2.00f);
                            Main.dust[num316].noGravity = true;
                            Dust dust3 = Main.dust[num316];
                            dust3.velocity = (randomcircle * 1.5f * Main.rand.NextFloat());
                        }
                    }
                }
                if (NPC.ai[0] > 300 && NPC.ai[0] < 400)
                {
                    plyloc = ply.Center + new Vector2(-leftright * 500, -240);
                    plydist = (plyloc - NPC.Center);
                    plynormal = plydist; plynormal.Normalize();
                    NPC.velocity += plynormal * 0.25f;
                    NPC.direction = (plydist.X < 0f).ToDirectionInt();
                }

                NPC.velocity /= 1.025f;
                NPC.velocity += plynormal * 0.075f;

                if (NPC.ai[0] == 450)
                {
                    List<Projectile> itz = IDGHelper.Shattershots(NPC.GetSource_FromThis(), NPC.Center, NPC.Center + new Vector2(-1 * NPC.direction, 5), new Vector2(0, 0), ModContent.ProjectileType<Ophiobeam>(), 80, 1f, 0, 1, true, 0f, false, 140);
                    SoundEngine.PlaySound(Deathray, NPC.position);
                    //Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, NPC.position);
                    projectiletrack = itz[0].whoAmI;
                    Main.projectile[projectiletrack].netUpdate = true;
                }
                if (NPC.ai[0] > 449)
                {
                    NPC.velocity /= 1.15f;
                }

                if (projectiletrack > 0 && Main.projectile[projectiletrack].type == ModContent.ProjectileType<Ophiobeam>())
                {
                    Main.projectile[projectiletrack].position = NPC.Center + new Vector2(-NPC.direction * 20, 10);
                }

                if (NPC.ai[0] == 600) { Resetattacks(); projectiletrack = 0; }


                if (NPC.velocity.Length() > 24f) { NPC.velocity.Normalize(); NPC.velocity *= 24f; }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "Ophioid";
            potionType = ItemID.GreaterHealingPotion;
            if (NPC.downedMoonlord)
                potionType = ItemID.SuperHealingPotion;
        }

        public override void OnKill()
        {
            float NPCVX = 0f; float NPCVY = 0f;
            NPC.velocity += new Vector2(NPCVX, NPCVY) * 0.075f;
            NPC.velocity *= 0.95f;
            if (NPC.velocity.Length() > new Vector2(NPCVX, NPCVY).Length())
            {
                NPC.velocity.Normalize();
                NPC.velocity *= new Vector2(NPCVX, NPCVY).Length();
            }

            if (!OphioidWorld.downedOphiopede2)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    IDGHelper.Chat("The " + (WorldGen.crimson ? "Crimson" : "Corruption") + "'s abomination is no longer felt, Ophioid is defeated", 100, 225, 100);
				//OphioidWorld.downedOphiopede2 = true;
				NPC.SetEventFlagCleared(ref OphioidWorld.downedOphiopede2, -1);
			}

            //if (Main.expertMode)
            //    NPC.DropBossBags();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			// OphiopedeHead.DoThemDrops(npcLoot, true);

			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TreasureBagOphioid>()));

			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.OphioflyRelic>()));

			npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Items.OphioidLarva>(), 4));

			// This code uses LeadingConditionRule to logically nest several rules under it.
			LeadingConditionRule notExpert = new(new Conditions.NotExpert());
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.SoulofMight, 1, 12, 30));
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.SoulofFright, 1, 12, 30));
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.SoulofSight, 1, 12, 30));
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.SoulofNight, 1, 24, 60));
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.SoulofLight, 1, 24, 60));
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.SoulofFlight, 1, 24, 60));
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentSolar, 1, 12, 30));
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentVortex, 1, 12, 30));
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentNebula, 1, 12, 30));
			notExpert.OnSuccess(ItemDropRule.Common(ItemID.FragmentStardust, 1, 12, 30));
			notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<OphiopedeMask>(), 7));
			notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SporeInfestedEgg>(), 10));

			npcLoot.Add(notExpert);

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Ophiopedetrophyitem>(), 10));

			/*float NPCVX = 0f; float NPCVY = 0f;
            NPC.velocity += new Vector2(NPCVX, NPCVY) * 0.075f;
            NPC.velocity *= 0.95f;
            if (NPC.velocity.Length() > new Vector2(NPCVX, NPCVY).Length())
            {
                NPC.velocity.Normalize();
                NPC.velocity *= new Vector2(NPCVX, NPCVY).Length();
            }

            if (!Main.expertMode)
            {
                if (Main.rand.Next(0, 100) < 31)
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, mod.ItemType("Ophiopedetrophyitem"));
                if (Main.rand.Next(0, 100) < 31)
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, mod.ItemType("OphiopedeMask"));

                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, mod.ItemType("SporeInfestedEgg"));

                List<int> types = new List<int>();
                types.Insert(types.Count, ItemID.SoulofMight); types.Insert(types.Count, ItemID.SoulofFright); types.Insert(types.Count, ItemID.SoulofSight); types.Insert(types.Count, ItemID.SoulofNight); types.Insert(types.Count, ItemID.SoulofLight); types.Insert(types.Count, ItemID.SoulofNight); types.Insert(types.Count, ItemID.SoulofLight);
                for (int f = 0; f < (Main.expertMode ? 200 : 120); f = f + 1)
                {
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, types[Main.rand.Next(0, types.Count)]);
                }

                types = new List<int>();
                types.Insert(types.Count, ItemID.SoulofFlight); types.Insert(types.Count, ItemID.SoulofFlight); types.Insert(types.Count, ItemID.FragmentStardust); types.Insert(types.Count, ItemID.FragmentSolar); types.Insert(types.Count, ItemID.FragmentVortex); types.Insert(types.Count, ItemID.FragmentNebula);
                for (int f = 0; f < (Main.expertMode ? 100 : 50); f = f + 1)
                {
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, types[Main.rand.Next(0, types.Count)]);
                }
            }
            else
            {
                NPC.DropBossBags();
            }
            */
		}

        public override string Texture
        {
            get { return ("OphioidMod/NPCs/ophioflyfinalform"); }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = ((int)(NPC.localAI[0] / 30));
            NPC.frame.Y %= 2;
            NPC.frame.Y *= frameHeight;
        }

        public override void AI()
        {
            NPC.localAI[0] += 1;
            glowred = false;

            float anglgo = (float)Math.Pow(Math.Abs(NPC.velocity.X * 0.07f), 0.8);
            NPC.rotation = NPC.rotation.AngleLerp(anglgo * (float)-NPC.direction, Math.Abs(NPC.velocity.X * 0.015f));

            for (int k = oldPos.Length - 1; k > 0; k--)
            {
                oldPos[k] = oldPos[k - 1];
                oldrot[k] = oldrot[k - 1];
            }
            oldPos[0] = NPC.Center;
            oldrot[0] = NPC.rotation;

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
                noplayer = true;
                if (NPC.velocity.Y > 0f)
                    NPC.velocity.Y /= 1.15f;
                NPC.velocity.Y -= 0.15f;
                NPC.velocity.X /= 1.15f;
                return;
            }
            else { noplayer = false; }




            ply = Main.player[NPC.target];

            NPC.ai[0] += 1;

            if (NPC.ai[1] == 0)
                Ichorbeam();
            if (NPC.ai[1] == 1)
                Sludgefield();
            if (NPC.ai[1] == 2)
                Feralbite();
            if (NPC.ai[1] == 3)
                Bodyslam();
            if (NPC.ai[1] == 4)
                Sporeclouds();
            if (NPC.ai[1] == 5)
                Makeminions();

            if (NPC.ai[0] < 300 && NPC.ai[1] != 2 && NPC.ai[1] != 5)
            {
                Vector2 plyloc = ply.Center - new Vector2(0, -16);
                Vector2 plydist = (plyloc - NPC.Center);
                Vector2 plynormal = plydist; plynormal.Normalize();

                NPC.velocity /= 1.01f;
                NPC.velocity += plynormal * 0.25f;

                if (NPC.ai[0] % 150 == 0 && NPC.ai[0] > 0)
                {
                    NPC.velocity += plynormal * 20f;
                }
                if (NPC.ai[0] > 80 && NPC.ai[0] < 220 && NPC.ai[0] % 5 == 0)
                {
                    List<Projectile> itz = IDGHelper.Shattershots(NPC.GetSource_FromThis(), NPC.Center, NPC.Center, new Vector2(0, 16), ProjectileID.HornetStinger, 30, 15f, 180, 2, false, NPC.ai[0] / 35f, false, 240);
                }

                NPC.direction = (plydist.X < 0f).ToDirectionInt();
                if (NPC.velocity.Length() > 16f) { NPC.velocity.Normalize(); NPC.velocity *= 16f; }
            }



        }

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
            if (NPC.ai[1] == 2)
            {
				target.AddBuff(BuffID.Rabies, 60 * 15, true);
            }
        }


        private SpriteEffects Facing
        {
            get
            {
                return (NPC.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            }
        }

        Texture2D MyTex => Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D texture = MyTex;
            Vector2 origin = new Vector2(texture.Width, texture.Height / 5) / 2f;
            if (glowred)
                lightColor = Color.Red;
            for (int k = oldPos.Length - 1; k >= 0; k -= 2)
            {
                float alpha = 1f - (float)(k + 1) / (float)(oldPos.Length + 2);
                spriteBatch.Draw(texture, oldPos[k] - Main.screenPosition, new Rectangle(0, NPC.frame.Y, texture.Width, (texture.Height - 1) / 5), lightColor * alpha, oldrot[k], origin, new Vector2(1f, 1f), Facing, 0f);
            }

            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle(0, NPC.frame.Y, texture.Width, (texture.Height) / 5), lightColor, NPC.rotation, origin, new Vector2(1f, 1f), Facing, 0f);
            return false;
        }


        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D texture = MyTex;
            //Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

            Vector2 origin = new Vector2(texture.Width, texture.Height / 5) / 2f;

            int wingframe = (int)(NPC.localAI[0] / 5f);
            wingframe %= 3; wingframe += 2;
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle(0, wingframe * ((texture.Height) / 5), texture.Width, (texture.Height) / 5), lightColor * 0.75f, NPC.rotation, origin, new Vector2(1f, 1f), Facing, 0f);
        }

        public override void HitEffect(NPC.HitInfo hitInfo)
        {
            if (NPC.life < 1 && Main.netMode != NetmodeID.Server)
            {
                for (int i = 1; i < 8; i += 1)
                {
                    Vector2 Vect = new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)); Vect.Normalize();
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vect, ModContent.Find<ModGore>("OphioidMod/ophiofly_gore_" + i).Type, 1f);
                }
            }
        }
    }
}