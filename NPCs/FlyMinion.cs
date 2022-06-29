using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.GameContent.Bestiary;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace OphioidMod.NPCs
{
    public class FlyMinion : ModNPC, ISinkyBoss
    {
        public override void SetDefaults()
        {
            //NPC.CloneDefaults(NPCID.BeeSmall);
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 75;
            NPC.defense = 15;
            NPC.lifeMax = 2000;
            NPC.value = 0f;
            NPC.knockBackResist = 0.5f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            AIType = -1;
            AnimationType = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override bool CheckActive()
        {
            return !Main.npc[(int)NPC.ai[1]].active;
        }

        public override string Texture
        {
            get { return ("OphioidMod/NPCs/FlyMinion"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophioid Fly Minion");
            Main.npcFrameCount[NPC.type] = 4;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            int associatedNPCType = ModContent.NPCType<FlyMinionCacoon>();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                    new FlavorTextBestiaryInfoElement("Small Ophioid flies")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = ((int)(Math.Abs(NPC.ai[0] / 4)));
            NPC.frame.Y %= 4;
            NPC.frame.Y *= frameHeight;
        }

        public override void AI()
        {
            bool nomaster = false;
            NPC.ai[0] += 1;
            NPC.velocity /= 1.015f;
            NPC Master = Main.npc[(int)NPC.ai[1]];
            if (!Master.active || Master.boss == false)
            {
                nomaster = true;
            }

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
            }
            Player ply = Main.player[NPC.target];

            if (Main.rand.Next(0, 800) < 1 && NPC.ai[0] > 300 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = (int)(-Main.rand.Next(500, 1200));
                NPC.netUpdate = true;
            }

            Vector2 masterloc = new Vector2(NPC.Center.X, -160); ;
            if (!nomaster)
            {
                masterloc = Master.Center - new Vector2(0, -96);
                if (NPC.ai[0] < 0 && !ply.dead)
                    masterloc = ply.Center - new Vector2(0, 0);
            }
            Vector2 masterdist = (masterloc - NPC.Center);
            Vector2 masternormal = masterdist; masternormal.Normalize();

            NPC.velocity += Vector2Extension.Rotate(masternormal, (float)Math.Sin(NPC.ai[0] * 0.02f) * 0.8f) * 0.4f;
            NPC.spriteDirection = (NPC.velocity.X > 0f).ToDirectionInt();
            if (NPC.velocity.Length() > 14f)
            {
                NPC.velocity.Normalize();
                NPC.velocity *= 14f;
            }
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            if (Main.rand.Next(0, 3) == 0)
            {
                player.AddBuff(BuffID.Weak, 60 * 8, true);
            }
        }

    }

    public class FlyMinionCacoon : FlyMinion
    {
        bool nomaster = false;
        public override void SetDefaults()
        {
            //NPC.CloneDefaults(NPCID.BeeSmall);
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 100;
            NPC.defense = 15;
            NPC.lifeMax = 10000;
            NPC.value = 0f;
            NPC.knockBackResist = 0.75f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            AIType = -1;
            AnimationType = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override bool CheckActive()
        {
            NPC Master = Main.npc[(int)NPC.ai[1]];
            if (!Master.active || NPC.ai[1] < 1 || Master.type != ModContent.NPCType<Ophiocoon>())
                return true;
            else
                return false;
        }

        public override string Texture
        {
            get { return ("OphioidMod/NPCs/FlyMinion"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coocoon Carriers");
            Main.npcFrameCount[NPC.type] = 4;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                    new FlavorTextBestiaryInfoElement("These small Ophioid flies carry the Ophiocoon to protect the Ophiofly")
            });
        }

        public override void AI()
        {
            NPC.ai[0] += 1;
            NPC.velocity /= 1.025f;
            NPC Master = Main.npc[(int)NPC.ai[1]];
            if (!Master.active || NPC.ai[1] < 1 || Master.type != ModContent.NPCType<Ophiocoon>())
            {
                nomaster = true;
                NPC.boss = false;
            }

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
            }
            Player ply = Main.player[NPC.target];
            if (Master.ai[1] > -1 && Master.active && !nomaster)
                Master.ai[1] += 1;

            Vector2 gohere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(NPC.ai[2]) }.ToVector2();
            Vector2 masterloc = new Vector2(0, -96) + gohere;
            if (!nomaster)
                masterloc += Master.Center;
            if ((Master.ai[1] < 0 && Master.active) || nomaster)
                masterloc = NPC.Center + new Vector2(0, -96) + gohere;

            Vector2 masterdist = (masterloc - NPC.Center);
            Vector2 masternormal = masterdist; masternormal.Normalize();

            if (!nomaster && masterdist.Length() > 128f)
            {
                Master.velocity -= masternormal * 0.15f;
                NPC.velocity += masternormal * 0.25f;
            }

            if (Main.rand.Next(0, 200) < 1 && NPC.ai[2] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                HalfVector2 half = new HalfVector2(Main.rand.Next(-120, 120), Main.rand.Next(-280, 20)); NPC.ai[2] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
                NPC.netUpdate = true;
            }

            if (NPC.ai[0] % (Main.expertMode ? 450 : 600) == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int him = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + (float)(NPC.width / 2) + NPC.velocity.X), (int)(NPC.position.Y + (float)(NPC.height / 2) + NPC.velocity.Y), NPCID.VileSpit, 0, 0f, 0f, 0f, 0f, 255);
                    //NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, mod.ProjectileType("EvenMoreVileSpit"));
                    Main.npc[him].velocity = new Vector2(Main.rand.Next(5, 18) * (Main.rand.Next(0, 2) == 0 ? 1 : -1), Main.rand.Next(-4, 4));
                    Main.npc[him].timeLeft = 200;
                    Main.npc[him].damage = 100;
                    Main.npc[him].netUpdate = true;

                    //IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);
                }
            }

            if (NPC.ai[0] % 7 == 0 && NPC.ai[0] % 730 > 700 && Main.expertMode)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                Player target = Main.player[NPC.target];
                List<Projectile> itz2 = IDGHelper.Shattershots(NPC.GetSource_FromThis(), NPC.Center, target.position, new Vector2(target.width / 2, target.height / 2), ProjectileID.PoisonSeedPlantera, 20, 10f, 0, 1, true, 0f, false, 300);
                SoundEngine.PlaySound(SoundID.Item42, NPC.Center);
            }

            NPC.velocity += Vector2Extension.Rotate(masternormal, (float)Math.Sin(NPC.ai[0] * 0.045f) * 0.4f) * 0.4f;
            NPC.spriteDirection = (NPC.velocity.X > 0f).ToDirectionInt();
            if (NPC.velocity.Length() > 14f)
            {
                NPC.velocity.Normalize(); NPC.velocity *= 14f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!nomaster)
                IDGHelper.DrawTether("OphioidMod/NPCs/tether", Main.npc[(int)NPC.ai[1]].Center, NPC.Center, screenPos);
            return true;
        }
    }
}