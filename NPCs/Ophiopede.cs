using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using OphioidMod.Projectiles;
using OphioidMod.Items;

namespace OphioidMod.NPCs
{
    #region OphiopedeHead
    [AutoloadBossHead]
    public class OphiopedeHead : ModNPC, ISinkyBoss
    {

        int framevar = 0;
        bool charge = false;
        public bool collision = false;
        public int phase = 0;

        public virtual void StartPhaseTwo()
        {
            //null
        }

        public override bool CheckActive()
        {
            return (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active);
        }

        public override string Texture
        {
            get { return ("OphioidMod/NPCs/wormmiscparts"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede");
            Main.npcFrameCount[NPC.type] = 4;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
            {
                CustomTexturePath = "OphioidMod/NPCs/BestiaryOphiopede",
                PortraitPositionYOverride = 5f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.width = 70;
            NPC.height = 70;
            NPC.damage = 90;
            NPC.defense = 0;
            NPC.lifeMax = 18000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.boss = true;
            AIType = NPCID.Wraith;
            AnimationType = 0;
            NPC.behindTiles = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            Music = MusicID.Boss2;
            NPC.value = Item.buyPrice(0, 50, 0, 0);
            NPC.BossBar = ModContent.GetInstance<OphioidBossBar>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            /*int associatedNPCType = ModContent.NPCType<OphiopedeBody>();
            int associatedNPCType2 = ModContent.NPCType<OphiopedeTail>();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);*/

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("The first fight against Ophiopede.")
            });
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
            if (NPC.downedMoonlord)
                potionType = ItemID.SuperHealingPotion;
        }

        public override bool PreKill()
        {
            Vector2 where = NPC.position;
            List<Vector2> parts = new List<Vector2>();
            for (int i = 0; i < Main.maxNPCs; i += 1)
            {
                NPC npc2 = Main.npc[i];
                if ((npc2.type == ModContent.NPCType<OphiopedeBody>() || npc2.type == ModContent.NPCType<OphiopedeTail>() ||
                    npc2.type == ModContent.NPCType<OphiopedeHead>() || npc2.type == ModContent.NPCType<OphiopedeHead2>()) && npc2.active && npc2.life > 0)
                {
                    parts.Add(new Vector2(i, (npc2.Center - Main.player[NPC.target].Center).Length()));
                }
            }
            parts = parts.OrderBy((x) => x.Y).ToList();

            NPC.position = Main.npc[(int)parts[0].X].position;

            return true;
        }

        public override void OnKill()
        {

            if (!OphioidWorld.downedOphiopede && Main.netMode != NetmodeID.MultiplayerClient)
                IDGHelper.Chat("The infested worm is defeated, but you can still feel the presence of the " + (WorldGen.crimson ? "Crimson" : "Corruption") + "'s abomination", 100, 225, 100);

            OphioidWorld.downedOphiopede = true;

        }
        public class CrimsonWorld : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return WorldGen.crimson;
            }

            public bool CanShowItemDropInUI()
            {
                return true;
            }

            public string GetConditionDescription()
            {
                return "Only drops in Crimson";
            }
        }

        public class CorruptionWorld : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return !WorldGen.crimson;
            }

            public bool CanShowItemDropInUI()
            {
                return true;
            }

            public string GetConditionDescription()
            {
                return "Only drops in Corruption";
            }
        }

        public class CopyIsExpert : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return Main.expertMode;
            }

            public bool CanShowItemDropInUI()
            {
                return true;
            }

            public string GetConditionDescription()
            {
                return "Expert Only";
            }
        }

        public class CopyIsNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return !Main.expertMode;
            }

            public bool CanShowItemDropInUI()
            {
                return true;
            }

            public string GetConditionDescription()
            {
                return "Not in Expert";
            }
        }

        public class AlwaysTrue : IItemDropRuleCondition, IProvideItemConditionDescription
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return true;
            }

            public bool CanShowItemDropInUI()
            {
                return true;
            }

            public string GetConditionDescription()
            {
                return null;
            }
        }

        public static void DoThemDrops(NPCLoot npcLoot, bool phase2)
        {
            List<int> types = new List<int>();
            types.Insert(types.Count, ItemID.SoulofMight); types.Insert(types.Count, ItemID.SoulofFright); types.Insert(types.Count, ItemID.SoulofSight); types.Insert(types.Count, ItemID.SoulofNight); types.Insert(types.Count, ItemID.SoulofLight); types.Insert(types.Count, ItemID.SoulofNight); types.Insert(types.Count, ItemID.SoulofLight);

            //for (int i = 0; i < 1; i += 1)
            //{
            IItemDropRuleCondition condo = phase2 ? new CopyIsNotExpert() : new AlwaysTrue();

            foreach (int itemType in types)
            {
                IItemDropRule itemtodrop = ItemDropRule.ByCondition(condo, itemType, 1, 15, 50);
                IItemDropRule itemtodropnoexpert = ItemDropRule.ByCondition(condo, itemType, 1, 12, 30);

                //leadingConditionRule.OnSuccess(itemtodrop);
                //leadingConditionRule.OnFailedRoll(new DropNothing());
                //leadingConditionRule.OnFailedConditions(itemtodropnoexpert);

                npcLoot.Add(new DropBasedOnExpertMode(itemtodropnoexpert, itemtodrop));
                //}
            }

            if (phase2)
            {
                types = new List<int>();
                types.Insert(types.Count, ItemID.SoulofFlight); types.Insert(types.Count, ItemID.SoulofFlight); types.Insert(types.Count, ItemID.FragmentSolar); types.Insert(types.Count, ItemID.FragmentVortex); types.Insert(types.Count, ItemID.FragmentNebula); types.Insert(types.Count, ItemID.FragmentStardust);

                IItemDropRule bag = ItemDropRule.BossBag(ModContent.ItemType<TreasureBagOphioid>());
                npcLoot.Add(bag);

                IItemDropRule pet = ItemDropRule.ByCondition(condo, ModContent.ItemType<SporeInfestedEgg>(), 1);
                npcLoot.Add(pet);

                npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.OphioflyRelic>()));

                npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<Items.OphioidLarva>(), 4));

                foreach (int itemType in types)
                {
                    IItemDropRule itemtodrop = ItemDropRule.ByCondition(condo, itemType, 1, 15, 50);
                    IItemDropRule itemtodropnoexpert = ItemDropRule.ByCondition(condo, itemType, 1, 12, 30);

                    //leadingConditionRule.OnSuccess(itemtodrop);
                    //leadingConditionRule.OnFailedRoll(new DropNothing());
                    //leadingConditionRule.OnFailedConditions(itemtodropnoexpert);

                    npcLoot.Add(new DropBasedOnExpertMode(itemtodropnoexpert, itemtodrop));
                }
            }


            IItemDropRule trophy = ItemDropRule.ByCondition(condo, ModContent.ItemType<Ophiopedetrophyitem>(), 10, 1, 1);
            npcLoot.Add(trophy);

            IItemDropRule mask = ItemDropRule.ByCondition(condo, ModContent.ItemType<OphiopedeMask>(), 10, 1, 1);
            npcLoot.Add(mask);

            if (phase2 == false)
            {
                npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.OphiopedeRelic>()));

                IItemDropRule bossitem1 = ItemDropRule.ByCondition(new CrimsonWorld(), ModContent.ItemType<Deadfungusbug>(), 1, 1, 1);
                npcLoot.Add(bossitem1);

                IItemDropRule bossitem2 = ItemDropRule.ByCondition(new CorruptionWorld(), ModContent.ItemType<Livingcarrion>(), 1, 1, 1);
                npcLoot.Add(bossitem2);
            }

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            DoThemDrops(npcLoot, false);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override bool PreAI()
        {

            if (GetType() == typeof(OphiopedeHead2))
            {
                if (NPC.life < (int)(NPC.lifeMax * 0.05f))
                {
                    NPC.dontTakeDamage = true;
                    NPC.life = (int)(NPC.lifeMax * 0.05f);
                }
            }

            int belowground = 0;
            bool outofbounds = false;
            collision = false;
            int minTilePosX = (int)(NPC.position.X / 16.0) - 1;
            int maxTilePosX = minTilePosX + (int)((NPC.width) / 16.0) + 2;
            int minTilePosY = (int)(NPC.position.Y / 16.0) - 1;
            int maxTilePosY = minTilePosY + (int)((NPC.height) / 16.0) + 2;
            if (minTilePosX < 0)
            {
                collision = true;
                minTilePosX = 0;
            }
            if (maxTilePosX > Main.maxTilesX)
            {
                collision = true; outofbounds = true;
                maxTilePosX = Main.maxTilesX;
            }
            if (minTilePosY < 0)
            {
                collision = true; outofbounds = true;
                minTilePosY = 0;
            }
            if (maxTilePosY > Main.maxTilesY)
            {
                collision = true; outofbounds = true;
                maxTilePosY = Main.maxTilesY;
            }

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    if (Main.tile[i, j] != null && (Main.tile[i, j].NactiveButWithABetterName() && (Main.tileSolid[(int)Main.tile[i, j].TileType] || Main.tileSolidTop[(int)Main.tile[i, j].TileType] && (int)Main.tile[i, j].TileFrameY == 0) || (int)Main.tile[i, j].LiquidAmount > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16); ;
                        if (NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && (NPC.position.Y + NPC.height > (double)vector2.Y && NPC.position.Y < vector2.Y + 16.0))
                        {
                            collision = true;
                        }
                    }
                }
            }

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY - 4; j < maxTilePosY - 4; ++j)
                {
                    if (Main.tile[i, j] != null && (Main.tile[i, j].NactiveButWithABetterName() && (Main.tileSolid[(int)Main.tile[i, j].TileType] || Main.tileSolidTop[(int)Main.tile[i, j].TileType] && (int)Main.tile[i, j].TileFrameY == 0) || (int)Main.tile[i, j].LiquidAmount > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16); ;
                        if (NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && (NPC.position.Y + NPC.height > (double)vector2.Y && NPC.position.Y < vector2.Y + 16.0))
                        {
                            belowground += 1;
                        }
                    }
                }
            }

            NPC.TargetClosest(true);
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                collision = false;
                if (NPC.Center.Y > Main.maxTilesY * 16)
                {
                    NPC.active = false;
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.rand.Next(0, 30) == 0)
                    NPC.netUpdate = true;

                if (NPC.ai[0] == 0)
                {
                    int lastnpc = NPC.whoAmI;
                    int latest = lastnpc;
                    int randomWormLength = (Main.expertMode ? 40 : 35);
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latest = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<OphiopedeBody>());
                        Main.npc[(int)latest].realLife = NPC.whoAmI;
                        Main.npc[(int)latest].ai[2] = lastnpc;
                        Main.npc[(int)latest].ai[3] = NPC.whoAmI;
                        Main.npc[(int)latest].ai[0] = Main.rand.Next(0, 80);
                        Main.npc[(int)latest].ai[1] = Main.rand.Next(110, 180);
                        lastnpc = latest;
                        //IdgNPC.AddOnHitBuff((int)latest,BuffID.Stinky,60*15);
                    }
                    latest = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<OphiopedeTail>());
                    Main.npc[(int)latest].realLife = NPC.whoAmI;
                    Main.npc[(int)latest].ai[2] = lastnpc;
                    Main.npc[(int)latest].ai[3] = NPC.whoAmI;
                    Main.npc[(int)latest].ai[0] = Main.rand.Next(0, 80);
                    Main.npc[(int)latest].ai[1] = 250;
                    NPC.ai[0] = 1;
                }
                NPC.netUpdate = true;
            }

            if (phase == 0 && NPC.life < NPC.lifeMax * (this.GetType().Name == "OphiopedeHead2" ? 0.75 : 0.5) && NPC.ai[0] > 0)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                phase = 1;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[0] = 1;
                    int lastnpc = NPC.whoAmI;
                    int latest = lastnpc;
                    int randomWormLength = Main.rand.Next(8, 8);
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latest = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TheSeeing>());
                        Main.npc[(int)latest].ai[2] = Main.rand.Next(0, 360);
                        Main.npc[(int)latest].ai[3] = NPC.whoAmI;
                        Main.npc[(int)latest].ai[1] = Main.rand.Next(90, 180);
                        Main.npc[(int)latest].ai[0] = Main.rand.Next(0, 600);
                        Main.npc[(int)latest].lifeMax = (int)(NPC.lifeMax * 0.15); Main.npc[(int)latest].life = NPC.life;
                        Main.npc[(int)latest].netUpdate = true;
                        lastnpc = latest;
                    }
                }
            }

            if ((NPC.ai[0] - 60f) % 400 > 360)
            {
                NPC.ai[0] += 1;
                collision = false;
            }

            Vector2 npcCenter = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float targetXPos = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2);
            float targetYPos = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2);
            float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
            npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            float length2 = (new Vector2((targetXPos - NPC.Center.X), (targetYPos - NPC.Center.Y))).Length();

            float speedboost = Math.Min(4f, Math.Max(1f + (charge == true ? 0.5f : 0f), length2 / 500f));
            charge = false;

            if (NPC.ai[0] < 0)
            {
                NPC.ai[0] += 1;
                if (NPC.ai[0] < -9900)
                    NPC.velocity += new Vector2(0, 1f);
                if (NPC.ai[0] < -9750 && NPC.ai[0] > -9900)
                {
                    StartPhaseTwo();
                    NPC.velocity -= new Vector2(0, 0.96f);
                    NPC.velocity = new Vector2(NPC.velocity.X * 0.95f, NPC.velocity.Y);
                }

                if (NPC.ai[0] > -9200)
                    NPC.ai[0] = (phase == 0 ? 1 : 250);

                if (NPC.ai[0] > -9250)
                {
                    Vector2 moveto = new Vector2(targetXPos - NPC.Center.X, (NPC.Center.Y - 600f) - NPC.Center.Y);
                    moveto.Normalize();
                    NPC.velocity += moveto * 1.2f;
                    if (NPC.velocity.Length() > 15f)
                    {
                        NPC.velocity.Normalize();
                        NPC.velocity *= 15f;
                    }
                }
                else
                {
                    if (NPC.ai[0] > -9750)
                    {
                        NPC.velocity *= 0.94f;
                    }
                    if (NPC.ai[0] > -9720)
                    {
                        for (int q = 0; q < 4; q++)
                        {
                            int dust = Dust.NewDust(NPC.Center - new Vector2(8, 40), 16, 12, DustID.GemEmerald, Main.rand.Next(-100, 100) * 0.1f, Main.rand.Next(-100, -50) * 0.2f, 100, Color.DarkGreen, 2f);
                            Main.dust[dust].noGravity = true;

                            int num184 = Dust.NewDust(NPC.Center - new Vector2(8, 40), 16, 12, DustID.GemEmerald, Main.rand.Next(-100, 100) * 0.1f, Main.rand.Next(-100, -50) * 0.2f, 31, Color.DarkGreen, 2f);
                            Dust dust3 = Main.dust[num184];
                            dust3.alpha += Main.rand.Next(300);
                            dust3 = Main.dust[num184];
                            dust3.velocity *= 0.3f;
                            Dust dust24 = Main.dust[num184];
                            dust24.velocity.X = dust24.velocity.X + (float)Main.rand.Next(-10, 11) * 0.025f;
                            Dust dust25 = Main.dust[num184];
                            dust25.velocity.Y = dust25.velocity.Y - (0.4f + (float)Main.rand.Next(-3, 14) * 0.15f);
                            Main.dust[num184].fadeIn = 1.25f + (float)Main.rand.Next(20) * 0.15f;
                        }


                        if (NPC.ai[0] % 8 == 0)
                        {

                            NPC ply = Main.npc[(int)NPC.ai[3]];

                            if (!Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                            {
                                int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + Main.rand.Next(-8, 8), NPC.Center.Y - 40f, 0f, 0f, ProjectileID.DD2OgreSpit, 1, 0f, 0);
                                Main.projectile[num54].velocity = new Vector2(Main.rand.Next(-8, 8) * (Main.rand.Next(0, 2) == 0 ? 1 : -1), Main.rand.Next(-10, -3));
                                Main.projectile[num54].damage = (int)(50);
                                Main.projectile[num54].timeLeft = 400;
                                Main.projectile[num54].tileCollide = (phase == 0 ? false : true);
                                Main.projectile[num54].netUpdate = true;
                                //IdgProjectile.Sync(num54);
                                //IdgProjectile.AddOnHitBuff(num54,BuffID.Stinky,60*15);
                            }
                        }

                        if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[0] % 8 == 0 && phase == 1)
                        {
                            NPC ply = Main.npc[(int)NPC.ai[3]];

                            int him = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) + Main.rand.Next(-800, 800)), (int)(Main.player[NPC.target].position.Y - 700f), ModContent.NPCType<EvenMoreVileSpit>(), 0, 0f, 0f, 0f, 0f, 255);
                            //NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, mod.ProjectileType("EvenMoreVileSpit"));
                            Main.npc[him].velocity = new Vector2(Main.rand.Next(1, 6) * Main.rand.Next(0, 2) == 0 ? 1 : -1, Main.rand.Next(5, 10));
                            Main.npc[him].timeLeft = 200;
                            if (Main.rand.Next(0, 3) < 8)
                                Main.npc[him].ai[0] = 3f;
                            Main.npc[him].netUpdate = true;
                            //IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);
                        }
                    }
                    Vector2 capvelo = NPC.velocity;
                    capvelo.Normalize();
                    NPC.velocity = capvelo * Math.Min(NPC.velocity.Length(), 15f);
                }
            }
            else
            {
                if (collision)
                {
                    NPC.ai[0] += outofbounds == false ? (1) : 0;
                    NPC.velocity *= 0.95f;
                    if (NPC.soundDelay == 0)
                    {
                        NPC.soundDelay = 4 + (int)(length / 40f);
                        SoundEngine.PlaySound(SoundID.WormDig, NPC.position);
                    }

                    Vector2 moveto = new Vector2(targetXPos - NPC.Center.X, targetYPos - NPC.Center.Y);
                    if ((NPC.ai[0]) % 700 < 500 && belowground < 5 && !((NPC.ai[0] - 40f) % 400 > 360))
                    {
                        charge = true;
                        NPC.velocity = new Vector2(NPC.velocity.X, (NPC.velocity.Y * 0.8f) + 0.8f);
                    }
                    else
                    {
                        if (belowground > 2 && NPC.ai[0] % 30 == 0)
                        {
                            int rayloc = IDGHelper.RaycastDown((int)NPC.Center.X / 16, (int)(NPC.Center.Y - 1000f) / 16) * 16;
                            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, (float)rayloc - (10f), Main.rand.Next(-2, 2), 3, ModContent.ProjectileType<PoisonCloud>(), 1, 0f, 0);
                            Main.projectile[num54].damage = (int)(20);
                            Main.projectile[num54].timeLeft = 200;
                            Main.projectile[num54].velocity = new Vector2(0, 0);
                            Main.projectile[num54].netUpdate = true;
                        }
                    }
                    moveto.Normalize();
                    NPC.velocity += moveto * 0.5f * speedboost;
                    if ((NPC.ai[0] - 40f) % 400 > 360)
                    {
                        NPC.velocity += moveto * 0.9f;
                        moveto = new Vector2(((Main.player[NPC.target].velocity.X * 3f) + targetXPos) - NPC.Center.X, targetYPos - NPC.Center.Y - 400f);
                        moveto.Normalize();
                        NPC.velocity += moveto * 0.3f;
                    }
                    if (NPC.ai[0] % 400 > 250)
                    {
                        if (NPC.ai[0] % 30 == 0)
                            SoundEngine.PlaySound(SoundID.ZombieMoan, NPC.position);
                    }
                    if (NPC.ai[0] % 400 > 300)
                    {
                        NPC.velocity += new Vector2(0, 0.45f);
                    }

                    Vector2 capvelo = NPC.velocity;
                    capvelo.Normalize();
                    NPC.velocity = capvelo * Math.Min(Math.Max(NPC.velocity.Length(), 6f * speedboost), 25f * speedboost);

                    if (NPC.ai[0] > 1000)
                    {
                        NPC.ai[0] = -10000;
                    }
                }
                else
                {
                    NPC.velocity += new Vector2(0, 0.2f);
                    if ((Main.player[NPC.target].Center - NPC.Center).Length() > 1600)
                    {
                        NPC.velocity.X *= 0.99f;
                        NPC.timeLeft += 1;
                    }
                }
            }
            NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;

            return false;
        }


        public override void FindFrame(int frameHeight)
        {

            framevar = phase == 1 ? (0) : (NPC.ai[0] % 30 < 15 ? 1 : 2);
            NPC.frame.Y = framevar * NPC.height;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_4").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position - new Vector2(10, 6), NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position - new Vector2(-10, 6), NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_1").Type, 1f);

            }
        }
    }
    #endregion

    #region OphiopedeBody

    public class OphiopedeBody : ModNPC, ISinkyBoss
    {

        int framevar = 0;

        public override string Texture
        {
            get { return ("OphioidMod/NPCs/wormparts"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede");
            Main.npcFrameCount[NPC.type] = 7;

            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new(0)
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
        }
        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 30;
            NPC.defense = 15;
            NPC.lifeMax = 18000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.boss = false;
            AIType = NPCID.Wraith;
            AnimationType = 0;
            NPC.behindTiles = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            Music = MusicID.Boss2;
            NPC.value = 90000f;
            //NPC.buffImmune[BuffID.Daybreak] = true; NPC.buffImmune[BuffID.Frostburn] = true; NPC.buffImmune[BuffID.Poisoned] = true; NPC.buffImmune[BuffID.Venom] = true;
        }

        public override bool CheckActive()
        {
            return !Main.npc[(int)NPC.ai[3]].active;
        }


        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage *= (NPC.ai[1] > 99 ? 0.35 : 0.15) * (Main.expertMode ? 1 : 1.25);
            return true;
        }

        public override void UpdateLifeRegen(ref int damage)
        {
            if (Main.expertMode)
            {
                NPC.lifeRegen /= 10;
                damage /= 10;
            }
        }


        public override bool PreAI()
        {


            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC MyHead = Main.npc[(int)NPC.ai[2]];
                if (!MyHead.active || (MyHead.type != ModContent.NPCType<OphiopedeHead>() && MyHead.type != ModContent.NPCType<OphiopedeHead2>() && MyHead.type != ModContent.NPCType<OphiopedeBody>()))
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                }
                else
                {
                    if (NPC.CountNPCS(ModContent.NPCType<OphiopedeHead2>()) > 0)
                    {
                        NPC.defense = 75;
                        NPC.damage = 80;
                        NPC.dontTakeDamage = MyHead.dontTakeDamage;
                    }
                }
            }



            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];

            if (NPC.ai[2] < (double)Main.npc.Length)
            {
                NPC.TargetClosest(true);
                NPC.ai[0] += 1;


                if (NPC.ai[0] % 60 == 0 && Main.expertMode)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        NPC.buffTime[k] -= 60;
                    }
                }

                if (NPC.ai[0] == 150)
                    NPC.netUpdate = true;
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                int size = 60;
                float dirX = Main.npc[(int)NPC.ai[2]].position.X + (float)(size / 2) - npcCenter.X;
                float dirY = Main.npc[(int)NPC.ai[2]].position.Y + (float)(size / 2) - npcCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY) * 1.4f;
                float dist = (length - (float)NPC.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;


                float mylife = (float)(Main.npc[(int)NPC.ai[3]].life) / (float)(Main.npc[(int)NPC.ai[3]].lifeMax);
                if (mylife * 100 < NPC.ai[1] - 100 && NPC.ai[1] > 99)
                {
                    NPC.ai[1] = (int)Main.rand.Next(1, 7);
                    for (int i = 0; i < 5; i += 1)
                    {
                        NPC.HitEffect(0, 10.0);
                        Vector2 Vect = new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)); Vect.Normalize();
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vect, ModContent.Find<ModGore>("OphioidMod/gore_" + Main.rand.Next(5, 9)).Type, 1f);
                    }
                    NPC.netUpdate = true;
                }


                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.npc[(int)NPC.ai[3]].ai[0] > -9800 && Main.npc[(int)NPC.ai[3]].ai[0] < -5000)
                    {

                        if (NPC.ai[0] % 160 == 0)
                        {
                            if (!Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                            {
                                int him = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + (float)(NPC.width / 2) + NPC.velocity.X), (int)(NPC.position.Y + (float)(NPC.height / 2) + NPC.velocity.Y), ModContent.NPCType<EvenMoreVileSpit>(), 0, 0f, 0f, 0f, 0f, 255);
                                //NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, mod.ProjectileType("EvenMoreVileSpit"));
                                Main.npc[him].velocity = new Vector2(Main.rand.Next(10, 18) * (Main.rand.Next(0, 2) == 0 ? 1 : -1), Main.rand.Next(-4, 4));
                                Main.npc[him].timeLeft = 200;
                                Main.npc[him].netUpdate = true;
                                //IdgNPC.AddOnHitBuff(him, BuffID.Stinky, 60 * 15);
                            }
                        }

                        /*if (NPC.ai[0]%120==0){
                            NPC ply=Main.npc[(int)NPC.ai[3]];
                            if (!Collision.SolidCollision(NPC.position, NPC.width, NPC.height)){
                        int num54 = Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, Main.rand.Next(18,26)*Main.rand.Next(-2,2)>0 ? 1 : -1, Main.rand.Next(-10,2), ProjectileID.DD2OgreSpit, 1, 0f,0);
                        Main.projectile[num54].velocity=new Vector2(Main.rand.Next(12,22)*(NPC.Center.X<Main.player[NPC.target].Center.X ? 1 : -1), Main.rand.Next(-10,2));
                        Main.projectile[num54].damage=(int)(20);
                        Main.projectile[num54].timeLeft=200;
                        Main.projectile[num54].tileCollide=false;
                    }}*/


                    }
                }


                if ((Main.npc[(int)NPC.ai[3]].ai[0] - 100f) % 400 > 280 && Main.npc[(int)NPC.ai[2]].ai[0] > 0 && NPC.ai[0] % (Main.expertMode ? 75 : 115) == 0)
                {
                    int thattarget = 0;
                    Rectangle rectangle1 = new Rectangle((int)NPC.position.X, (int)NPC.position.Y - 600, NPC.width, NPC.height + 1200);
                    int maxDistance = 250;
                    bool playerCollision = false;
                    for (int index = 0; index < 255; ++index)
                    {
                        if (Main.player[index].active)
                        {
                            Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - maxDistance, (int)Main.player[index].position.Y - 32, maxDistance * 2, 64);
                            if (rectangle1.Intersects(rectangle2))
                            {
                                playerCollision = true;
                                thattarget = index;
                                break;
                            }
                        }
                    }
                    if (playerCollision && Collision.CanHitLine(new Vector2(NPC.Center.X, NPC.Center.Y), 8, 8, new Vector2(Main.player[thattarget].Center.X, Main.player[thattarget].Center.Y), 8, 8))
                    {
                        Player ply = Main.player[NPC.target];
                        int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(Main.rand.Next(-2, 2), Main.player[thattarget].Center.Y < NPC.Center.Y ? -8 : 8), ProjectileID.Stinger, 20, 0f);
                        Main.projectile[num54].damage = (int)(20);
                        Main.projectile[num54].timeLeft = 200;
                        Main.projectile[num54].netUpdate = true;

                        //IdgProjectile.Sync(num54);
                        //IdgProjectile.AddOnHitBuff(num54,BuffID.Stinky,60*15);

                    }

                }


            }

            return false;
        }

        public override void FindFrame(int frameHeight)
        {

            framevar = (int)NPC.ai[1] > 99 ? 0 : (int)NPC.ai[1];
            NPC.frame.Y = framevar * frameHeight;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0 && NPC.ai[1] < 7)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("OphioidMod/segment_2_gore_" + NPC.ai[1]).Type, 1f);
            }
        }
    }
    #endregion

    #region OphiopedeTail
    public class OphiopedeTail : OphiopedeBody
    {
        public enum MessageType : byte
        {
            OphioidMessage
        }

        public override string Texture
        {
            get { return ("OphioidMod/NPCs/wormmiscparts"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new(0)
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 70;
            NPC.height = 70;
            NPC.defense = 0;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            return true;
        }

        public override bool PreAI()
        {
            base.PreAI();

            if ((Main.npc[(int)NPC.ai[3]].ai[0] - 100f) % 400 > 280 && Main.npc[(int)NPC.ai[2]].ai[0] > 0 && NPC.ai[0] % (20) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                OphiopedeHead Head = Main.npc[(int)NPC.ai[3]].ModNPC as OphiopedeHead;
                if (Head.phase == 1)
                {
                    int thattarget = 0;
                    Rectangle rectangle1 = new Rectangle((int)NPC.position.X, (int)NPC.position.Y + 200, NPC.width, NPC.height + 1200);
                    int maxDistance = 450;
                    bool playerCollision = false;
                    for (int index = 0; index < 255; ++index)
                    {
                        if (Main.player[index].active)
                        {
                            Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - maxDistance, (int)Main.player[index].position.Y - 32, maxDistance * 2, 64);
                            if (rectangle1.Intersects(rectangle2))
                            {
                                playerCollision = true;
                                thattarget = index;
                                break;
                            }
                        }
                    }

                    if (playerCollision && Collision.CanHitLine(new Vector2(NPC.Center.X, NPC.Center.Y), 8, 8, new Vector2(Main.player[thattarget].Center.X, Main.player[thattarget].Center.Y), 8, 8))
                    {
                        Player ply = Main.player[NPC.target];
                        int him = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, (NPC.ai[0] % (40) == 0 ? NPCID.ToxicSludge : NPCID.SpikedJungleSlime), 0, 0f, 0f, 0f, 0f, 255);
                        Main.npc[him].damage *= 2;
                        Main.npc[him].defense *= 2;
                        Main.npc[him].lifeMax *= 3;
                        Main.npc[him].life = Main.npc[him].lifeMax - 100;
                        Main.npc[him].GetGlobalNPC<OphioidNPC>().fallthrough = (Main.expertMode == true ? 2000 : 1000);
                        Main.npc[him].lifeMax *= 3; Main.npc[him].life = Main.npc[him].lifeMax;
                        Main.npc[him].ai[0] = (float)(-1000 * Main.rand.Next(3));
                        Main.npc[him].ai[1] = 0f;
                        Main.npc[him].netUpdate = true;

                        //IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);

                        /*if (Main.netMode == 2)
                        {
                            NetMessage.SendData(23, -1, -1, null, him, 0f, 0f, 0f, 0, 0, 0);
                            ModPacket packet = Mod.GetPacket();
                            OphioidMod mymod = Mod as OphioidMod;

                            packet.Write((byte)MessageType.OphioidMessage);
                            packet.Write(him);
                            packet.Write(Main.expertMode == true ? 2000 : 1000);
                            packet.Send();
                        }*/
                    }
                }
            }
            return true;
        }

        public override void FindFrame(int frameHeight)
        {

            NPC.frame.Y = 3 * frameHeight;
        }
    }
	#endregion

	#region OphiopedeHead2
	[AutoloadBossHead]
    public class OphiopedeHead2 : OphiopedeHead
    {
        public override string Texture
        {
            get { return ("OphioidMod/NPCs/wormmiscparts"); }
        }

        public override void StartPhaseTwo()
        {
            if (NPC.life < (NPC.lifeMax * 0.50))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.boss = false;
                    NPC.active = false;
                    IDGHelper.Chat("The Ophiopede begins to metamorphosize!", 100, 225, 100);

                    int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
                    int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
                    int num663 = ModContent.NPCType<MetaOphiocoon>();

                    int num664 = NPC.NewNPC(NPC.GetSource_FromAI(), x, y, num663);
                    if (Main.netMode == NetmodeID.Server && num664 < 200)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }

                }
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede");
            Main.npcFrameCount[NPC.type] = 4;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
            {
                CustomTexturePath = "OphioidMod/NPCs/BestiaryOphiopede",
                PortraitPositionYOverride = 5f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
        {
            NPC.width = 70;
            NPC.height = 70;
            NPC.damage = 200;
            NPC.defense = 25;
            NPC.lifeMax = 72000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.boss = true;
            AIType = NPCID.Wraith;
            AnimationType = 0;
            NPC.behindTiles = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Centipede_Mod_-_Metamorphosis");
            NPC.value = 90000f;
            NPC.BossBar = ModContent.GetInstance<OphioidBossBar>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            int associatedNPCType = ModContent.NPCType<Ophiofly>();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("The second fight against Ophiopede.")
            });
        }
    }
	#endregion
}