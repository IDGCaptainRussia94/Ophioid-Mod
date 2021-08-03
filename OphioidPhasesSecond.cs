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
using Microsoft.Xna.Framework.Graphics.PackedVector;


namespace OphioidMod
{

   


    public class MetaOphiocoon : ModNPC, ISinkyBoss
    {

        public override string Texture
        {
            get { return("OphioidMod/cocoon");}
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiocoon");
        }

        public override void AI()
        {
        NPC.ai[0]+=1;

        if (NPC.ai[0]>10)
        {

        if (Main.netMode!=1){

            int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
            int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
            int num663 = ModContent.NPCType<Ophiocoon>();

            int num664 = NPC.NewNPC(x, y, num663);
                    if (Main.netMode == 2 && num664 < 200)
                    {
                       NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
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
            NPC.knockBackResist=0f;
            NPC.value = 0f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.boss=true;
            NPC.dontTakeDamage=true;
            NPC.immortal=true;
            AIType = -1;
            AnimationType = -1;
        }

    }

        [AutoloadBossHead]
    public class OphiopedeHead2 : OphiopedeHead
    {
        public override string Texture
        {
            get { return("OphioidMod/wormmiscparts"); }
        }
        
        public override void StartPhaseTwo()
        {
        if (NPC.life<(NPC.lifeMax*0.50)){
        if (Main.netMode!=1){
        NPC.boss=false;
        NPC.active=false;
        IDGHelper.Chat("The Ophiopede begins to metamorphosize!",100,225,100);

            int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
            int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
            int num663 = ModContent.NPCType<MetaOphiocoon>();

            int num664 = NPC.NewNPC(x, y, num663);
                    if (Main.netMode == 2 && num664 < 200)
                    {
                       NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }

        }}
    }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede");
            Main.npcFrameCount[NPC.type] = 4;
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
            NPC.boss=true;
            AIType = NPCID.Wraith;
            AnimationType = 0;
            NPC.behindTiles = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            Music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Centipede_Mod_-_Metamorphosis");
            NPC.value = 90000f;
        }
    }

    public class OphSporeCloud : ModNPC
    {
        int mytimeisover=800;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophioid Spore Cloud");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override string Texture
        {
            get { return "Terraria/Images/Projectile_" + ProjectileID.SporeCloud; }
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BeeSmall);
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 60;
            NPC.defense = 0;
            NPC.lifeMax = 400;
            NPC.value = 0f;
            NPC.noGravity = true;
            NPC.knockBackResist=0f;
            NPC.aiStyle = 5;
            AIType = NPCID.BeeSmall;
            AnimationType = NPCID.BeeSmall;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override bool CheckDead()
        {
        List<Projectile> projectile22= IDGHelper.Shattershots(NPC.GetProjectileSpawnSource(),NPC.Center,NPC.Center+NPC.velocity,new Vector2(0,0),ProjectileID.SporeCloud,50,NPC.velocity.Length()+10f,0,1,true,(float)Main.rand.Next(-100,100)*0.002f,false,240);
        //IdgProjectile.Sync(projectile22[0].whoAmI);
        return true;
        }

        public override void AI()
        {
        mytimeisover-=1;
        NPC.velocity/=1.15f;
        NPC.velocity.Normalize();
        NPC.velocity*=16f-(float)(mytimeisover*0.015f);
        if (mytimeisover<1)
        NPC.StrikeNPCNoInteraction(9999, 0f, NPC.direction, false, false, false);


        }


    }

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


        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiofly");
            Main.npcFrameCount[NPC.type] = 5;
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
            NPC.value = 0f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.boss = true;
            Music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Centipede_Mod_-_The_Fly");
            AIType = -1;
            AnimationType = -1;
            BossBag = ModContent.ItemType<TreasureBagOphioid>();
        }


        /*public override void SendExtraAI(BinaryWriter writer)   
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
        }*/

        public void Resetattacks()
        {

            if (spawnminionsat == 0)
                spawnminionsat = (int)(NPC.lifeMax * 0.8);

            if (Main.netMode != 1)
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
                    List<Projectile> itz = IDGHelper.Shattershots(NPC.GetProjectileSpawnSource(),NPC.Center, NPC.Center + new Vector2(-1 * NPC.direction, 4), new Vector2(0, 0), ModContent.ProjectileType<Ophiobeamichor>(), 15, 1f, 0, 1, true, 0f, false, 150);
                    //Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, NPC.position);
                    Main.projectile[projectiletrack].netUpdate = true;
                    Terraria.Audio.SoundEngine.PlaySound(29, (int)NPC.position.X, (int)NPC.position.Y, 104, 1f, 0f);
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

                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item90, NPC.Center);

                        for (float num315 = -10; num315 < 10; num315 = num315 + 2f)
                        {
                            int num54 = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(),NPC.Center.X + Main.rand.Next(-8, 8), NPC.Center.Y - 40f, 0f, 0f, ProjectileID.DD2OgreSpit, 1, 0f, 0);
                            Main.projectile[num54].velocity = new Vector2(Main.rand.Next(-8, 8) * (Main.rand.Next(0, 2) == 0 ? 1 : -1) + (num315 / 4f), Main.rand.Next(-15, -3));
                            Main.projectile[num54].damage = (int)(50);
                            Main.projectile[num54].timeLeft = 800;
                            Main.projectile[num54].tileCollide = true;
                            Main.projectile[num54].netUpdate = true;
                            //IdgProjectile.Sync(num54);
                            //IdgProjectile.AddOnHitBuff(num54, BuffID.Stinky, 60 * 15);
                        }

                        for (float num315 = -40; num315 < 40; num315 = num315 + 0.2f)
                        {
                            if (Main.rand.Next(0, 100) < 25)
                            {
                                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                                int num316 = Dust.NewDust(NPC.Center + new Vector2(num315 * 3, -30), 0, 80, 184, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 4f - Math.Abs(num315) / 15f);
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

                int num622 = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y) + vecr, 0, 0, 184, 0f, 0f, 100, default(Color), 3f);
                Main.dust[num622].velocity = randomcircle * -16f;

                Main.dust[num622].noGravity = true;
                Main.dust[num622].color = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5) % 1, 0.9f, 1f);
                Main.dust[num622].color.A = 10;
                Main.dust[num622].velocity.X = NPC.velocity.X / 3 + (Main.rand.Next(-50, 51) * 0.005f);
                Main.dust[num622].velocity.Y = NPC.velocity.Y / 3 + (Main.rand.Next(-50, 51) * 0.005f);
                Main.dust[num622].alpha = 100; ;
            }


            if (Main.netMode != 1 && NPC.ai[0] > 100 && NPC.ai[0] % 50 == 0)
            {
                int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
                int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
                int num663 = ModContent.NPCType<FlyMinion>();

                int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[num664].ai[1] = NPC.whoAmI;
                if (Main.netMode == 2 && num664 < 200)
                {
                    NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                }
            }

            if (NPC.ai[0] == 300) { Resetattacks(); projectiletrack = 0; }

        }


        public void Sporeclouds()
        {

            if (NPC.ai[0] % 40 == 0 && NPC.ai[0] > 150)
            {
                if (Main.netMode != 1)
                {
                    int him = NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2) + NPC.velocity.X), (int)(NPC.position.Y + (float)(NPC.height / 2) + NPC.velocity.Y), ModContent.NPCType<OphSporeCloud>(), 0, 0f, 0f, 0f, 0f, 255);
                    Main.npc[him].velocity = new Vector2(Main.rand.Next(10, 18) * (Main.rand.Next(0, 2) == 0 ? 1 : -1), Main.rand.Next(-4, 4));
                    Main.npc[him].netUpdate = true;
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item111, NPC.Center);

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
                List<Projectile> projectile22 = IDGHelper.Shattershots(NPC.GetProjectileSpawnSource(),NPC.Center, plyloc, new Vector2(0, -16), ProjectileID.Stinger, 30, 20f, 0, 1, true, (float)Main.rand.Next(-100, 100) * 0.004f, false, 240);
                Projectile projectile2 = projectile22[0];
                //IdgProjectile.Sync(projectile2.whoAmI);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, NPC.Center);

                for (int num315 = 1; num315 < 8; num315 = num315 + 1)
                {
                    if (Main.rand.Next(0, 100) < 25)
                    {
                        Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                        int num316 = Dust.NewDust(new Vector2(projectile2.position.X - 1, projectile2.position.Y), projectile2.width, projectile2.height, 184, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 1.00f);
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

                    for (int num315 = 1; num315 < 8; num315 = num315 + 1)
                    {
                        if (Main.rand.Next(0, 100) < 25)
                        {
                            Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                            int num316 = Dust.NewDust(NPC.Center + new Vector2((-NPC.direction * 20) - 12, 12), 24, 24, 184, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 2.00f);
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
                    List<Projectile> itz = IDGHelper.Shattershots(NPC.GetProjectileSpawnSource(),NPC.Center, NPC.Center + new Vector2(-1 * NPC.direction, 5), new Vector2(0, 0), ModContent.ProjectileType<Ophiobeam>(), 80, 1f, 0, 1, true, 0f, false, 140);
                    Terraria.Audio.SoundEngine.PlaySound(29, (int)NPC.position.X, (int)NPC.position.Y, 104, 1f, 0f);
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
                if (Main.netMode != 1)
                    IDGHelper.Chat("The " + (WorldGen.crimson ? "Crimson" : "Corruption") + "'s abomination is no longer felt, Ophioid is defeated", 100, 225, 100);
                OphioidWorld.downedOphiopede2 = true;
            }

            //if (Main.expertMode)
            //    NPC.DropBossBags();
        }

public override void ModifyNPCLoot(NPCLoot npcLoot)
{
            OphiopedeHead.DoThemDrops(npcLoot,true);

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
            get { return ("OphioidMod/ophioflyfinalform"); }
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
                    List<Projectile> itz = IDGHelper.Shattershots(NPC.GetProjectileSpawnSource(),NPC.Center, NPC.Center, new Vector2(0, 16), ProjectileID.HornetStinger, 30, 15f, 180, 2, false, NPC.ai[0] / 35f, false, 240);
                }

                NPC.direction = (plydist.X < 0f).ToDirectionInt();
                if (NPC.velocity.Length() > 16f) { NPC.velocity.Normalize(); NPC.velocity *= 16f; }
            }



        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            if (NPC.ai[1] == 2)
            {
                player.AddBuff(BuffID.Rabies, 60 * 15, true);
            }
        }


        private SpriteEffects facing
        {
            get
            {
                return (NPC.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            }


        }

        Texture2D MyTex => Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;

        public override bool PreDraw(SpriteBatch spriteBatch,Vector2 screenPos, Color lightColor)
        {

            Texture2D texture = MyTex;
            Vector2 origin = new Vector2(texture.Width, texture.Height / 5) / 2f;
            if (glowred)
                lightColor = Color.Red;
            for (int k = oldPos.Length - 1; k >= 0; k -= 2)
            {
                float alpha = 1f - (float)(k + 1) / (float)(oldPos.Length + 2);
                spriteBatch.Draw(texture, oldPos[k] - Main.screenPosition, new Rectangle(0, NPC.frame.Y, texture.Width, (texture.Height - 1) / 5), lightColor * alpha, oldrot[k], origin, new Vector2(1f, 1f), facing, 0f);
            }

            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle(0, NPC.frame.Y, texture.Width, (texture.Height) / 5), lightColor, NPC.rotation, origin, new Vector2(1f, 1f), facing, 0f);
            return false;
        }


        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D texture = MyTex;
            //Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

            Vector2 origin = new Vector2(texture.Width, texture.Height / 5) / 2f;

            int wingframe = (int)(NPC.localAI[0] / 5f);
            wingframe %= 3; wingframe += 2;
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle(0, wingframe * ((texture.Height) / 5), texture.Width, (texture.Height) / 5), lightColor * 0.75f, NPC.rotation, origin, new Vector2(1f, 1f), facing, 0f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                for (int i = 1; i < 8; i += 1)
                {
                    Vector2 Vect = new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2)); Vect.Normalize();
                    Gore.NewGore(NPC.Center, Vect, ModContent.Find<ModGore>("OphioidMod/ophiofly_gore_" + i).Type, 1f);
                }
            }
        }

    }

        [AutoloadBossHead]
    public class Ophiocoon : ModNPC,ISinkyBoss
    {

        Player ply;
        bool no2ndphase=false;
        float whichway=0f;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override bool CheckActive()
        {
            if (NPC.life>0)
            no2ndphase=true;
            return true;
        }

        public override string Texture
        {
            get { return("OphioidMod/cocoon");}
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiocoon");
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life<1)
            {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.position);
            for (int a = 0; a < 500; a++){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                Vector2 vecr=randomcircle;

                    int num622 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 184, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[num622].velocity = randomcircle*new Vector2((float)Main.rand.Next(-1000,1000)/100f,(float)Main.rand.Next(-1000,1000)/100f);

                    Main.dust[num622].noGravity = true;
                    Main.dust[num622].color=Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly/5)%1, 0.9f, 1f);
                    Main.dust[num622].color.A=10;
                    Main.dust[num622].alpha = 100;;
                }
            for(int i=1;i<6;i+=1){
            Vector2 Vect=new Vector2(Main.rand.Next(-2,2),Main.rand.Next(-2,2)); Vect.Normalize();
            Gore.NewGore(NPC.Center, Vect, ModContent.Find<ModGore>("OphioidMod/cocoon_gore_" + i).Type, 1f);
            }}
        }

        public void falldown()
        {
        NPC.velocity.Y+=0.25f;

            bool wallblocking = false;
            int y_top_edge = (int)(NPC.position.Y - 16f) / 16;
            int y_bottom_edge = (int)(NPC.position.Y + (float)NPC.height + 16f) / 16;
            int x_left_edge = (int)(NPC.position.X - 16f) / 16;
            int x_right_edge = (int)(NPC.position.X + (float)NPC.width + 16f) / 16;

            for (int x = x_left_edge; x <= x_right_edge; x++)
            {
                for (int y = y_top_edge; y <= y_bottom_edge; y++)
                {
                    if (Main.tile[x, y].NactiveButWithABetterName() && Main.tileSolid[(int)Main.tile[x, y].type] && !Main.tileSolidTop[(int)Main.tile[x, y].type])
                    {
                        wallblocking = true;
                        break;
                    }
                }
                if (wallblocking) break;
            }

            if (wallblocking==true)
            {

        NPC.life = 0;
        NPC.HitEffect(0, 10.0);

        if (Main.netMode!=1){
                    IDGHelper.Chat("The Ophifly hatches from the Cocoon!",100,225,100);

            int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
            int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
            int num663 = ModContent.NPCType<Ophiofly>();

            int num664 = NPC.NewNPC(x, y, num663);
                    if (Main.netMode == 2 && num664 < 200)
                    {
                       NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
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
            NPC.knockBackResist=0f;
            NPC.value = 0f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.boss=true;
            NPC.dontTakeDamage=true;
            NPC.immortal=true;
            Music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Centipede_Mod_-_Metamorphosis");
            AIType = -1;
            AnimationType = -1;
        }

        public override void AI()
        {
        NPC.velocity/=1.05f;
        NPC.ai[0]+=1;
        //ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
        if (NPC.ai[0]==25){
            if (Main.netMode != 1)
            {
            for(int i=0;i<7;i+=1)
            {
            int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
            int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
            int num663 = ModContent.NPCType<FlyMinionCacoon>();

            //HalfVector2 half=new HalfVector2(Main.rand.Next(-120,120),Main.rand.Next(-380,240));

            int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
            Main.npc[num664].ai[0]=Main.rand.Next(0,3000);
            Main.npc[num664].ai[1]=NPC.whoAmI;
            //Main.npc[num664].life = (int)(NPC.life*0.007);
            //Main.npc[num664].lifeMax = (int)(NPC.lifeMax*0.007);
            Main.npc[num664].netUpdate=true;
                        if (Main.netMode == 2 && num664 < 200)
                        {
                            NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                        }
            }
        }
        NPC.life=1;
        NPC.lifeMax=1;
        }

        if (NPC.ai[1]<0){
        falldown();
        return;
        }

        if (NPC.ai[0]>30){
        NPC.ai[1]-=3;
        if (NPC.ai[1]>10)
        NPC.ai[1]=10;
        }

        int minTilePosX = (int)(NPC.Center.X / 16.0) - 1;
        int minTilePosY = (int)((NPC.Center.Y+32f) / 16.0) - 1;

        if (whichway==0f){
        whichway=minTilePosX>(int)(Main.maxTilesX/2) ? -1f : 1f;
        }


        if (minTilePosX<32 || minTilePosX > Main.maxTilesX-32){
        NPC.active=false;
        return;
        }

        NPC.ai[2]-=1;

        if (NPC.ai[2]%10==0 && NPC.ai[2]>0)
        {
            if (Main.netMode != 1)
            {
            int him=NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2) + NPC.velocity.X), (int)(NPC.position.Y + (float)(NPC.height / 2) + NPC.velocity.Y), ModContent.NPCType<EvenMoreVileSpit>(), 0, 0f, 0f, 0f, 0f, 255);
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

                if (Main.netMode != 1 && NPC.CountNPCS(ModContent.NPCType<FlyMinion>()) < 5)
                {
                    int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
                    int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
                    int num663 = ModContent.NPCType<FlyMinion>();

                    int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                    Main.npc[num664].ai[1] = NPC.whoAmI;
                    if (Main.netMode == 2 && num664 < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }


            int whereisity;
            whereisity= IDGHelper.RaycastDown(minTilePosX+1,Math.Max(minTilePosY,0));

            bool wallblocking = false;
            int y_top_edge = (int)(NPC.position.Y - 16f) / 16;
            int y_bottom_edge = (int)(NPC.position.Y + (float)NPC.height + 16f) / 16;
            int x_left_edge = (int)(NPC.position.X - 16f) / 16;
            int x_right_edge = (int)(NPC.position.X + (float)NPC.width + 16f) / 16;

            for (int x = x_left_edge; x <= x_right_edge; x++)
            {
                for (int y = y_top_edge; y <= y_bottom_edge; y++)
                {
                    if (Main.tile[x, y].NactiveButWithABetterName() && Main.tileSolid[(int)Main.tile[x, y].type] && !Main.tileSolidTop[(int)Main.tile[x, y].type])
                    {
                        wallblocking = true;
                        break;
                    }
                }
                if (wallblocking) break;
            }

        if (wallblocking){
        NPC.velocity-=new Vector2(0,0.3f);
        }else{
        if (whereisity-minTilePosY>80)
        NPC.velocity+=new Vector2(0,1.6f);
        NPC.velocity+=new Vector2(0.4f*whichway,0);
        }
        if (NPC.velocity.Length()>12f){
        NPC.velocity.Normalize(); NPC.velocity*=12;
        }





        NPC.rotation=(NPC.velocity.X/15f);

        }






}

    public class FlyMinion : ModNPC, ISinkyBoss
    {
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BeeSmall);
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 75;
            NPC.defense = 15;
            NPC.lifeMax = 2000;
            NPC.value = 0f;
            NPC.knockBackResist=0.5f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            AIType = -1;
            AnimationType = -1;
        }

        public override bool CheckActive()
        {
            return !Main.npc[(int)NPC.ai[1]].active;
        }

        public override string Texture
        {
            get { return("OphioidMod/baby_ophiofly_frames");}
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophioid Fly Minion");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void FindFrame(int frameHeight)
        {
        NPC.frame.Y=((int)(Math.Abs(NPC.ai[0]/4)));
        NPC.frame.Y%=4;
        NPC.frame.Y*=frameHeight;
        }

        public override void AI()
        {
            bool nomaster = false;
        NPC.ai[0]+=1;
        NPC.velocity/=1.015f;
        NPC Master = Main.npc[(int)NPC.ai[1]];
            if (!Master.active || Master.boss==false)
            {
                nomaster = true;
            }

        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active){
        NPC.TargetClosest(true);
        }
        Player ply = Main.player[NPC.target];

        if (Main.rand.Next(0,800)<1 && NPC.ai[0]>300 && Main.netMode!=1)
        {
        NPC.ai[0]=(int)(-Main.rand.Next(500,1200));
        NPC.netUpdate=true;
        }

            Vector2 masterloc = new Vector2(NPC.Center.X, -160); ;
            if (!nomaster)
            {
                masterloc = Master.Center - new Vector2(0, -96);
                if (NPC.ai[0] < 0 && !ply.dead)
                    masterloc = ply.Center - new Vector2(0, 0);
            }
        Vector2 masterdist=(masterloc-NPC.Center);
        Vector2 masternormal=masterdist; masternormal.Normalize();

        NPC.velocity+=Vector2Extension.Rotate(masternormal,(float)Math.Sin(NPC.ai[0]*0.02f)*0.8f)*0.4f;
        NPC.spriteDirection=(NPC.velocity.X>0f).ToDirectionInt();
        if (NPC.velocity.Length()>14f){NPC.velocity.Normalize(); NPC.velocity*=14f;}

        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            if (Main.rand.Next(0,3)==0)
            {
                player.AddBuff(BuffID.Weak, 60*8, true);
            }
        }

    }

    public class FlyMinionCacoon : FlyMinion
    {
    bool nomaster=false;
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BeeSmall);
            NPC.width = 24;
            NPC.height = 24;
            NPC.damage = 100;
            NPC.defense = 15;
            NPC.lifeMax = 10000;
            NPC.value = 0f;
            NPC.knockBackResist=0.75f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            AIType = -1;
            AnimationType = -1;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override bool CheckActive()
        {
        NPC Master = Main.npc[(int)NPC.ai[1]];
        if (!Master.active || NPC.ai[1]<1 || Master.type!=ModContent.NPCType<Ophiocoon>())
            return true;
            else
            return false;
        }

        public override string Texture
        {
            get { return("OphioidMod/baby_ophiofly_frames");}
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coocoon Carriers");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void AI()
        {
        NPC.ai[0]+=1;
        NPC.velocity/=1.025f;
        NPC Master = Main.npc[(int)NPC.ai[1]];
        if (!Master.active || NPC.ai[1]<1 || Master.type!= ModContent.NPCType <Ophiocoon>()){
        nomaster=true;
        NPC.boss=false;
        }

        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active){
        NPC.TargetClosest(true);
        }
        Player ply = Main.player[NPC.target];
        if (Master.ai[1]>-1 && Master.active && !nomaster)
        Master.ai[1]+=1;

        Vector2 gohere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(NPC.ai[2]) }.ToVector2();
        Vector2 masterloc=new Vector2(0,-96)+gohere;
        if (!nomaster)
        masterloc+=Master.Center;
        if ((Master.ai[1]<0 && Master.active) || nomaster)
        masterloc=NPC.Center+new Vector2(0,-96)+gohere;
        Vector2 masterdist=(masterloc-NPC.Center);
        Vector2 masternormal=masterdist; masternormal.Normalize();

        if (!nomaster && masterdist.Length()>128f){
        Master.velocity-=masternormal*0.15f;
        NPC.velocity+=masternormal*0.25f;
        }

        if (Main.rand.Next(0,200)<1 && NPC.ai[2]==0 && Main.netMode!=1){
        HalfVector2 half=new HalfVector2(Main.rand.Next(-120,120),Main.rand.Next(-280,20));NPC.ai[2]=ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
        NPC.netUpdate=true;
        }

        if (NPC.ai[0]%(Main.expertMode ? 450 : 600)==0)
        {

            if (Main.netMode != 1)
            {
            int him=NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2) + NPC.velocity.X), (int)(NPC.position.Y + (float)(NPC.height / 2) + NPC.velocity.Y), NPCID.VileSpit, 0, 0f, 0f, 0f, 0f, 255);
            //NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, mod.ProjectileType("EvenMoreVileSpit"));
            Main.npc[him].velocity=new Vector2(Main.rand.Next(5,18)*(Main.rand.Next(0,2)==0 ? 1 : -1), Main.rand.Next(-4,4));
            Main.npc[him].timeLeft = 200;
            Main.npc[him].damage=100;
            Main.npc[him].netUpdate=true;

            //IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);
            }

        }

        if (NPC.ai[0]%7==0 && NPC.ai[0]%730>700 && Main.expertMode)
        {
                    NPC.TargetClosest(true);
                    NPC.netUpdate=true;
                    Player target=Main.player[NPC.target];
                    List<Projectile> itz2=IDGHelper.Shattershots(NPC.GetProjectileSpawnSource(),NPC.Center,target.position,new Vector2(target.width/2,target.height/2),ProjectileID.PoisonSeedPlantera,20,10f,0,1,true,0f,false,300);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42,NPC.Center);
        }

        NPC.velocity+=Vector2Extension.Rotate(masternormal,(float)Math.Sin(NPC.ai[0]*0.045f)*0.4f)*0.4f;
        NPC.spriteDirection=(NPC.velocity.X>0f).ToDirectionInt();
        if (NPC.velocity.Length()>14f){NPC.velocity.Normalize(); NPC.velocity*=14f;}

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
        if (!nomaster)
        IDGHelper.DrawTether("OphioidMod/tether", Main.npc[(int)NPC.ai[1]].Center,NPC.Center, screenPos);
        return true;
        }

    }


    public class Ophiobeamichor : Ophiobeam
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 90;
            Projectile.damage = 15;
            Projectile.tileCollide = true;
            MoveDistance = 0f;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ichorbeam");
        }

        public override string Texture
        {
            get { return "OphioidMod/beam_1"; }
        }

        public override void AI()
        {
            //projectile.velocity+=new Vector2(projectile.velocity.X>0 ? 0.04f : -0.04f,0f);
            //Projectile.localAI[0] += 0.2f;
            //Projectile.tileCollide = true;
            base.AI();
        }

        public override void MoreAI(Vector2 dustspot)
        {

        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Ichor, 60 * 10, true);
            player.AddBuff(BuffID.Darkness, 60 * 15, true);
        }
    }


    public class Ophiobeam : ProjectileLaserBase
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 90;
            Projectile.damage = 40;
            Projectile.tileCollide = true;
            MoveDistance = 0f;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiobeam");
        }

        public override string Texture
        {
            get { return "OphioidMod/beam_1"; }
        }

        public override void AI()
        {
            if (GetType() == typeof(Ophiobeam))
            {
                Projectile.velocity += new Vector2(Projectile.velocity.X > 0 ? 0.04f : -0.04f, 0f);
                Projectile.ai[1] += 1;
            }
            Projectile.tileCollide = true;
            Projectile.localAI[0] += 0.2f;
        base.AI();
        }

        public override void MoreAI(Vector2 dustspot)
        {

        for (int num315 = 1; num315 < 8; num315 = num315 + 1)
            {
                if (Main.rand.Next(0,100)<25){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                int num316 = Dust.NewDust(new Vector2(Projectile.position.X-1, Projectile.position.Y), Projectile.width, Projectile.height, 184, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 2.00f);
                Main.dust[num316].noGravity = true;
                Dust dust3 = Main.dust[num316];
                dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat());
                dust3.velocity.Normalize();
                dust3.velocity += (Projectile.velocity*1f);
            }}

        for (int num315 = 1; num315 < 2; num315 = num315 + 1)
            {
                if (Main.rand.Next(0,100)<25){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                int num316 = Dust.NewDust(new Vector2(Projectile.position.X-1, Projectile.position.Y), Projectile.width, Projectile.height, 184, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 3.00f);
                Main.dust[num316].noGravity = true;
                Dust dust3 = Main.dust[num316];
                dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat());
                dust3.velocity.Normalize();
                dust3.velocity*=(0.4f);
            }}

        for (int num315 = 1; num315 < 8; num315 = num315 + 1)
            {
                if (Main.rand.Next(0,100)<25){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                int num316 = Dust.NewDust(new Vector2(dustspot.X-1, dustspot.Y)-new Vector2(Projectile.width/2, Projectile.height/2), Projectile.width, Projectile.height, 184, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 2.00f);
                Main.dust[num316].noGravity = true;
                Dust dust3 = Main.dust[num316];
                dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat());
            }}

            if (Projectile.ai[1]%8==0){
            int num54 = Projectile.NewProjectile(Projectile.InheritSource(Projectile),dustspot.X,dustspot.Y, Main.rand.Next(-2,2), 3, ModContent.ProjectileType<PoisonCloud>(), 1, 0f,0);
                Main.projectile[num54].damage=(int)(20);
                Main.projectile[num54].timeLeft=Main.expertMode ? 60*20 : 250;
                Main.projectile[num54].velocity=new Vector2(0,0);
                Main.projectile[num54].netUpdate=true;
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            string[] lasers;
            lasers = GetType() == typeof(Ophiobeamichor) ? new string[] { "OphioidMod/frame_1", "OphioidMod/frame2", "OphioidMod/frame_3" } : new string[] { "OphioidMod/beam_1", "OphioidMod/beam_2", "OphioidMod/beam_3" };
            Vector2 scale = new Vector2(MathHelper.Clamp((float)Projectile.timeLeft / 20, 0f, 1f), 1f);
            IDGHelper.DrawTether(lasers[(int)Projectile.localAI[0] % 3], Hitspot, Projectile.Center, Main.screenPosition, Projectile.Opacity, scale.X, scale.Y,Color.White);
            Texture2D captex = ModContent.Request<Texture2D>(GetType() == typeof(Ophiobeamichor) ? "OphioidMod/ichor_cap" : "OphioidMod/end_and_start").Value;
            Main.spriteBatch.Draw(captex, Projectile.Center - Main.screenPosition, null, lightColor * Projectile.Opacity, (Projectile.velocity).ToRotation() - ((float)Math.PI / 2f), new Vector2(captex.Width / 2, captex.Height / 2), new Vector2(scale.X, scale.Y), SpriteEffects.None, 0.0f);
            Main.spriteBatch.Draw(captex, Hitspot - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.velocity.ToRotation() + ((float)Math.PI / 2f), new Vector2(captex.Width / 2, captex.Height / 2), new Vector2(scale.X, scale.Y), SpriteEffects.None, 0.0f);

            return false;
        }

        /*public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
        string[] lasers={"Ophioid/beam_1","Ophioid/beam_2","Ophioid/beam_3"};
        Vector2 scale = new Vector2(MathHelper.Clamp((float)projectile.timeLeft/20,0f,1f),1f);
        Idglib.DrawTether(lasers[(int)projectile.localAI[0]%3],hitspot,projectile.Center,projectile.Opacity,scale.X,scale.Y);
        Texture2D captex= ModContent.GetTexture("Ophioid/end_and_start");
        Main.spriteBatch.Draw(captex, projectile.Center - Main.screenPosition, null, lightColor*projectile.Opacity, (projectile.velocity).ToRotation()-((float)Math.PI/2f), new Vector2(captex.Width/2,captex.Height/2), new Vector2(scale.X,scale.Y), SpriteEffects.None, 0.0f);
        Main.spriteBatch.Draw(captex, hitspot - Main.screenPosition, null, lightColor*projectile.Opacity, projectile.velocity.ToRotation()+((float)Math.PI/2f), new Vector2(captex.Width/2,captex.Height/2), new Vector2(scale.X,scale.Y), SpriteEffects.None, 0.0f);

        return false;
        }*/

    }

    public class TreasureBagOphioid : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("Right click to open");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.expert = true;
            Item.rare = -12;
        }


        public override int BossBagNPC
        {
            get
            {
                return ModContent.NPCType<Ophiofly>();
            }
        }


        public override bool CanRightClick()
        {
            return true;
        }
        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();
            if (Main.rand.Next(0, 100) < 31)
                player.QuickSpawnItem(ModContent.ItemType<Ophiopedetrophyitem>());
            if (Main.rand.Next(0, 100) < 31)
                player.QuickSpawnItem(ModContent.ItemType<OphiopedeMask>());

            player.QuickSpawnItem(ModContent.ItemType <SporeInfestedEgg>());

            List<int> types = new List<int>();
            types.Insert(types.Count, ItemID.SoulofMight); types.Insert(types.Count, ItemID.SoulofFright); types.Insert(types.Count, ItemID.SoulofSight); types.Insert(types.Count, ItemID.SoulofNight); types.Insert(types.Count, ItemID.SoulofLight); types.Insert(types.Count, ItemID.SoulofNight); types.Insert(types.Count, ItemID.SoulofLight);
            for (int f = 0; f < (Main.expertMode ? 200 : 120); f = f + 1)
            {
                player.QuickSpawnItem(types[Main.rand.Next(0, types.Count)]);
            }

            types = new List<int>();
            types.Insert(types.Count, ItemID.SoulofFlight); types.Insert(types.Count, ItemID.SoulofFlight); types.Insert(types.Count, ItemID.FragmentStardust); types.Insert(types.Count, ItemID.FragmentSolar); types.Insert(types.Count, ItemID.FragmentVortex); types.Insert(types.Count, ItemID.FragmentNebula);
            for (int f = 0; f < (Main.expertMode ? 100 : 50); f = f + 1)
            {
                player.QuickSpawnItem(types[Main.rand.Next(0, types.Count)]);
            }

        }

    }


}
