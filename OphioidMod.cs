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

namespace OphioidMod
{

 public static class Vector2Extension {
     public static Vector2 Rotate(this Vector2 v, float degrees) {
         float radians = (float)(degrees + 2.0* Math.PI);
         float sin = (float)Math.Sin(radians);
         float cos = (float)Math.Cos(radians);
         
         float tx = (float)v.X;
         float ty = (float)v.Y;
 
         return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
     }
 }

    public static class TileOneDotThree
    {
        public static bool NactiveButWithABetterName(this Tile tile)
        {
            return (tile.HasTile && !tile.IsActuated);
           // return (!tile.HasTile || tile.IsActuated);
        }
    }




    public interface ISinkyBoss
    {

    }

    public enum MusicPriority
    {
        None,
        BiomeLow,
        BiomeMedium,
        BiomeHigh,
        Environment,
        Event,
        BossLow,
        BossMedium,
        BossHigh
    }

    class OphioidMod : Mod
	{
		public static OphioidMod Instance;
        //public static Mod Idglib;
		public OphioidMod()
		{

		}

        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            Instance = null;
        }
        


		public override void PostSetupContent()
		{
            //Not in 1.4 yet

			if (ModLoader.TryGetMod("BossChecklist", out Mod bossList))
			{
                //bossList.Call("AddBossWithInfo", "Ophiopede", 9.05f, (Func<bool>)(() => OphioidWorld.downedOphiopede), string.Format("Use a [i:{0}] or [i:{1}] anywhere, anytime", ItemType("Deadfungusbug"), ItemType("Livingcarrion")));
                //bossList.Call("AddBossWithInfo", "Ophioid", 11.50f, (Func<bool>)(() => OphioidWorld.downedOphiopede2), string.Format("Use a [i:{0}] anywhere, anytime", ItemType("Infestedcompost")));
                bossList.Call("AddBoss", 11.05f, ModContent.NPCType<OphiopedeHead>(), this, "Ophiopede", (Func<bool>)(() => (OphioidWorld.downedOphiopede)), ModContent.ItemType<Deadfungusbug>(), new List<int>() {ModContent.ItemType<Ophiopedetrophyitem>(), ModContent.ItemType<OphiopedeMask>() }, new List<int>() {ItemID.SoulofFright, ItemID.SoulofLight, ItemID.SoulofMight, ItemID.SoulofNight, ItemID.SoulofSight, ModContent.ItemType<Livingcarrion>(), ModContent.ItemType<Deadfungusbug>() }, 
                    "Use a [i:" + ModContent.ItemType<Deadfungusbug>() + "] or [i:" + ModContent.ItemType<Livingcarrion>() + "] at any time", "Ophiopede tunnels away", "OphioidMod/BCLPede", "OphioidMod/icon_small");
                bossList.Call("AddBoss", 13.50f, ModContent.NPCType<Ophiofly>(), this, "Ophiopede & Ophiofly", (Func<bool>)(() => (OphioidWorld.downedOphiopede2)), ModContent.ItemType<Infestedcompost>(), new List<int>() { ModContent.ItemType<Ophiopedetrophyitem>(), ModContent.ItemType<OphiopedeMask>(), ModContent.ItemType<SporeInfestedEgg>() }, 
                    new List<int>() { ItemID.SoulofFright, ItemID.SoulofLight, ItemID.SoulofMight, ItemID.SoulofNight, ItemID.SoulofSight,ItemID.SoulofFlight, ItemID.FragmentSolar, ItemID.FragmentNebula, ItemID.FragmentVortex, ItemID.FragmentStardust }, 
                    "Use an [i:" + ModContent.ItemType<Infestedcompost>() + "] at any time after beating Ophiopede", "Ophioid slinks back into its hidden nest", "OphioidMod/BCLFly");


            }
            /*
            //Idglib = ModLoader.GetMod("Idglib");

            Mod yabhb = ModLoader.GetMod("FKBossHealthBar");
			if(yabhb != null)
			{
     			yabhb.Call("hbStart");
     			yabhb.Call("hbSetTexture",
         		GetTexture("healtbar_left"),
         		GetTexture("healtbar_frame"),
         		GetTexture("healtbar_right"),
         		GetTexture("healtbar_fill"));
    			yabhb.Call("hbSetMidBarOffset", -32, 12);
     			yabhb.Call("hbSetBossHeadCentre", 80, 32);
     			yabhb.Call("hbSetFillDecoOffsetSmall", 20);
     			yabhb.Call("hbFinishSingle", NPCType("OphiopedeHead"));

                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                GetTexture("healtbar_left"),
                GetTexture("healtbar_frame"),
                GetTexture("healtbar_right"),
                GetTexture("healtbar_fill"));
                yabhb.Call("hbSetMidBarOffset", -32, 12);
                yabhb.Call("hbSetBossHeadCentre", 80, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 20);
                yabhb.Call("hbFinishSingle", NPCType("OphiopedeHead2"));

                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                GetTexture("healtbar_left"),
                GetTexture("healtbar_frame"),
                GetTexture("healtbar_right"),
                GetTexture("healtbar_fill"));
                yabhb.Call("hbSetMidBarOffset", -32, 12);
                yabhb.Call("hbSetBossHeadCentre", 80, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 20);
                yabhb.Call("hbFinishSingle", NPCType("Ophiofly"));

                yabhb.Call("hbStart");
                yabhb.Call("hbSetTexture",
                GetTexture("healtbar_left"),
                GetTexture("healtbar_frame"),
                GetTexture("healtbar_right"),
                GetTexture("healtbar_fill"));
                yabhb.Call("hbSetMidBarOffset", -32, 12);
                yabhb.Call("hbSetBossHeadCentre", 80, 32);
                yabhb.Call("hbSetFillDecoOffsetSmall", 20);
                yabhb.Call("hbFinishMultiple",NPCType("FlyMinionCacoon"),NPCType("FlyMinionCacoon"));

			}
                        */
        }

        /*public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            if (NPC.CountNPCS(ModContent.NPCType<OphiopedeHead2>()) > 0)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Centipede_Mod_-_Metamorphosis");
                priority = MusicPriority.BossMedium;
            }
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType type = (MessageType)reader.ReadByte();

            if (type == MessageType.OphioidMessage && Main.netMode == 1)
            {
                int npcid = reader.ReadInt32();
                int time = reader.ReadInt32();
                Main.npc[npcid].GetGlobalNPC<OphioidNPC>().fallthrough = time;
            }
        }*/


    }

    public enum MessageType : byte
    {
    OphioidMessage
    }

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

    public class OphioidPlayer: ModPlayer
    {

    public bool PetBuff=false;

        public override void ResetEffects()
        {
            PetBuff = false;
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            /*IProjectileSource source = proj.GetProjectileSource_FromThis();
            Main.NewText(source.GetType().Name);
            if (source is ProjectileSource_NPC)
            {
                Main.NewText("test");
                Player.AddBuff(BuffID.MoonLeech,60*300);
            }*/
        }

    }

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

	    public class OphiopedeTail : OphiopedeBody
    {

            public enum MessageType : byte
    {
    OphioidMessage
    }

		public override string Texture
        {
            get { return("OphioidMod/wormmiscparts"); }
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ophiopede");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
            NPC.width = 70;
            NPC.height = 70;
            NPC.defense = 0;
        }

        public override bool StrikeNPC(ref double damage,int defense,ref float knockback,int hitDirection,ref bool crit)
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
                        int him = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, (NPC.ai[0] % (40) == 0 ? NPCID.ToxicSludge : NPCID.SpikedJungleSlime), 0, 0f, 0f, 0f, 0f, 255);
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

			NPC.frame.Y=3*frameHeight;
		}


    }

    public class TheSeeing : ModNPC
    {
        public override string Texture
        {
            get { return ("OphioidMod/TheSeeing"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Seeing");
            Main.npcFrameCount[NPC.type] = 1;
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
            NPC.value = 0f;
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

        public override bool CheckActive()
        {
            return !Main.npc[(int)NPC.ai[3]].active;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Darkness, 60 * 4, true);
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
            IDGHelper.DrawTether("OphioidMod/tether", Main.npc[(int)NPC.ai[3]].Center, NPC.Center, screenPos);
            return true;
        }
    }


    public class OphiopedeBody : ModNPC, ISinkyBoss
    {

    	int framevar=0;

		        public override string Texture
        {
            get { return("OphioidMod/wormparts"); }
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ophiopede");
			Main.npcFrameCount[NPC.type] = 7;
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
			NPC.value = 0f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.boss=false;
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


        public override bool StrikeNPC(ref double damage,int defense,ref float knockback,int hitDirection,ref bool crit)
		{
			damage*=(NPC.ai[1]>99 ? 0.35 : 0.15)*(Main.expertMode ? 1 : 1.25);
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
                        Gore.NewGore(NPC.Center, Vect, ModContent.Find<ModGore>("OphioidMod/gore_" + Main.rand.Next(5, 9)).Type, 1f);
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
                                int him = NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2) + NPC.velocity.X), (int)(NPC.position.Y + (float)(NPC.height / 2) + NPC.velocity.Y), ModContent.NPCType<EvenMoreVileSpit>(), 0, 0f, 0f, 0f, 0f, 255);
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
                        int num54 = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center, new Vector2(Main.rand.Next(-2, 2), Main.player[thattarget].Center.Y < NPC.Center.Y ? -8 : 8), ProjectileID.Stinger, 20, 0f);
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

			framevar=(int)NPC.ai[1]>99 ? 0 : (int)NPC.ai[1];
			NPC.frame.Y=framevar*frameHeight;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;   
        }

        public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 && NPC.ai[1]<7)
			{
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("OphioidMod/segment_2_gore_" + NPC.ai[1]).Type, 1f);
			}
		}

	}

	[AutoloadBossHead]
    public class OphiopedeHead : ModNPC, ISinkyBoss
    {

    	int framevar=0;
        bool charge=false;
    	public bool collision=false;
    	public int phase=0;

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
            get { return("OphioidMod/wormmiscparts"); }
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
			NPC.damage = 90;
			NPC.defense = 0;
			NPC.lifeMax = 18000;
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
			Music = MusicID.Boss2;
			NPC.value = Item.buyPrice(1, 25, 0, 0);
            NPC.BossBar = ModContent.GetInstance<OphioidBossBar>();
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
        potionType=ItemID.GreaterHealingPotion;
        if (NPC.downedMoonlord)
        potionType=ItemID.SuperHealingPotion;
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

            if (!OphioidWorld.downedOphiopede && Main.netMode!= NetmodeID.MultiplayerClient)
            IDGHelper.Chat("The infested worm is defeated, but you can still feel the presence of the "+(WorldGen.crimson ? "Crimson" : "Corruption")+"'s abomination",100,225,100);

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
                IItemDropRule bossitem1 = ItemDropRule.ByCondition(new CrimsonWorld(), ModContent.ItemType<Deadfungusbug>(), 1, 1, 1);
                npcLoot.Add(bossitem1);

                IItemDropRule bossitem2 = ItemDropRule.ByCondition(new CorruptionWorld(), ModContent.ItemType<Livingcarrion>(), 1, 1, 1);
                npcLoot.Add(bossitem2);
            }

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            DoThemDrops(npcLoot,false);

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

			int belowground=0;
			bool outofbounds=false;
			collision=false;
            int minTilePosX = (int)(NPC.position.X / 16.0) - 1;
            int maxTilePosX = minTilePosX+(int)((NPC.width) / 16.0) + 2;
            int minTilePosY = (int)(NPC.position.Y / 16.0) - 1;
            int maxTilePosY = minTilePosY+(int)((NPC.height) / 16.0) + 2;
            if (minTilePosX < 0){collision=true;
                minTilePosX = 0;
            }
            if (maxTilePosX > Main.maxTilesX){collision=true; outofbounds=true;
                maxTilePosX = Main.maxTilesX;
            }
            if (minTilePosY < 0){collision=true; outofbounds=true;
                minTilePosY = 0;
            }
            if (maxTilePosY > Main.maxTilesY){collision=true; outofbounds=true;
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
                        vector2.Y = (float)(j * 16);;
                        if (NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && (NPC.position.Y + NPC.height > (double)vector2.Y && NPC.position.Y < vector2.Y + 16.0))
                        {
                            collision = true;
                        }
                    }
                }
            }

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY-4; j < maxTilePosY-4; ++j)
                {
                    if (Main.tile[i, j] != null && (Main.tile[i, j].NactiveButWithABetterName() && (Main.tileSolid[(int)Main.tile[i, j].TileType] || Main.tileSolidTop[(int)Main.tile[i, j].TileType] && (int)Main.tile[i, j].TileFrameY == 0) || (int)Main.tile[i, j].LiquidAmount > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);;
                        if (NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && (NPC.position.Y + NPC.height > (double)vector2.Y && NPC.position.Y < vector2.Y + 16.0))
                        {
                            belowground +=1;
                        }
                    }
                }
            }

                NPC.TargetClosest(true);
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
            collision=false;
                if (NPC.Center.Y > Main.maxTilesY * 16)
                {
                    NPC.active = false;
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
            if (Main.rand.Next(0,30)==0)
            NPC.netUpdate=true;

            if (NPC.ai[0]==0){
            		int lastnpc=NPC.whoAmI;
            		int latest=lastnpc;
                    int randomWormLength = (Main.expertMode ? 40 : 35);
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latest = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y,ModContent.NPCType<OphiopedeBody>());
                        Main.npc[(int)latest].realLife = NPC.whoAmI;
                        Main.npc[(int)latest].ai[2] = lastnpc;
                        Main.npc[(int)latest].ai[3] = NPC.whoAmI;
                        Main.npc[(int)latest].ai[0] = Main.rand.Next(0,80);
                        Main.npc[(int)latest].ai[1] = Main.rand.Next(110,180);
                        lastnpc=latest;
                        //IdgNPC.AddOnHitBuff((int)latest,BuffID.Stinky,60*15);
                    }
                        latest = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType < OphiopedeTail>());
                        Main.npc[(int)latest].realLife = NPC.whoAmI;
                        Main.npc[(int)latest].ai[2] = lastnpc;
                        Main.npc[(int)latest].ai[3] = NPC.whoAmI;
                        Main.npc[(int)latest].ai[0] = Main.rand.Next(0,80);
                        Main.npc[(int)latest].ai[1] = 250;
                    NPC.ai[0] = 1;
                }
            NPC.netUpdate=true;

       	 }

       	 	if (phase==0 && NPC.life<NPC.lifeMax*(this.GetType().Name=="OphiopedeHead2" ? 0.75 : 0.5) && NPC.ai[0]>0){
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, NPC.position, 0);
       	 		phase=1;
           		if (Main.netMode != NetmodeID.MultiplayerClient)
            	{
            		NPC.ai[0]=1;
            		int lastnpc=NPC.whoAmI;
            		int latest=lastnpc;
                    int randomWormLength = Main.rand.Next(8, 8);
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latest = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType < TheSeeing>());
                        Main.npc[(int)latest].ai[2] = Main.rand.Next(0,360);
                        Main.npc[(int)latest].ai[3] = NPC.whoAmI;
                        Main.npc[(int)latest].ai[1] = Main.rand.Next(90,180);
                        Main.npc[(int)latest].ai[0] = Main.rand.Next(0,600);
                        Main.npc[(int)latest].lifeMax=(int)(NPC.lifeMax*0.15); Main.npc[(int)latest].life=NPC.life;
                        Main.npc[(int)latest].netUpdate=true;
                        lastnpc=latest;
                    }
                }

       	 	}


            if ((NPC.ai[0]-60f)%400>360){NPC.ai[0]+=1;
            collision=false;
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

            float length2 = (new Vector2((targetXPos-NPC.Center.X),(targetYPos-NPC.Center.Y))).Length();

            float speedboost=Math.Min(4f,Math.Max(1f+(charge==true ? 0.5f : 0f),length2/500f));
            charge=false;

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
                                int num54 = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center.X + Main.rand.Next(-8, 8), NPC.Center.Y - 40f, 0f, 0f, ProjectileID.DD2OgreSpit, 1, 0f, 0);
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

                            int him = NPC.NewNPC((int)(Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) + Main.rand.Next(-800, 800)), (int)(Main.player[NPC.target].position.Y - 700f), ModContent.NPCType<EvenMoreVileSpit>(), 0, 0f, 0f, 0f, 0f, 255);
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
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 1);
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
                            int num54 = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center.X, (float)rayloc - (10f), Main.rand.Next(-2, 2), 3, ModContent.ProjectileType<PoisonCloud>(), 1, 0f, 0);
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
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.ZombieMoan, (int)NPC.position.X, (int)NPC.position.Y, 1);
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

			framevar=phase==1 ? (0) : (NPC.ai[0]%30<15 ? 1 : 2);
			NPC.frame.Y=framevar*NPC.height;
		}

				 public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_3").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_4").Type, 1f);
				Gore.NewGore(NPC.position-new Vector2(10,6), NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_1").Type, 1f);
				Gore.NewGore(NPC.position-new Vector2(-10,6), NPC.velocity, ModContent.Find<ModGore>("OphioidMod/gore_1").Type, 1f);

			}
		}

	}

	public class EvenMoreVileSpit : ModNPC
	{

		public override void SetDefaults()
		{
				NPC.width = 16;
				NPC.height = 16;
				NPC.aiStyle = 9;
				NPC.damage = 65;
				NPC.defense = 0;
				NPC.lifeMax = 1;
				NPC.HitSound = null;
				NPC.DeathSound = SoundID.NPCDeath9;
				NPC.noGravity = true;
				NPC.noTileCollide = true;
				NPC.knockBackResist = 0f;
				NPC.scale = 0.9f;
				NPC.alpha = 80;
				NPC.aiStyle=-1;
		}

				public override string Texture
		{
			get { return "Terraria/Images/NPC_" + NPCID.VileSpit; }
		}


		public override void HitEffect(int hitDirection, double damage){

				for (int num328 = 0; num328 < 20; num328 += 1)
				{
					int num329 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 2f), NPC.width, NPC.height, DustID.CorruptGibs, NPC.velocity.X*1f, NPC.velocity.Y*1f, 100, default(Color), 3f);
					Dust dust = Main.dust[num329];
					if (Main.rand.Next(15) <12)
					{
						dust.scale *= 0.8f;
					}
					else
					{
						dust.scale *= 1.5f;
						dust.velocity *= 0.1f;
					}
						Main.dust[num329].noGravity = true;
				}
		}

				//public void NPCLoot(){
			//Effects();
				//}


		public override void AI()
		{
            NPC.TargetClosest(true);
				for (int num328 = 0; num328 < 2; num328 += 1)
				{
					int num329 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 2f), NPC.width, NPC.height, DustID.CorruptGibs, NPC.velocity.X*0.3f, NPC.velocity.Y*0.3f, 100, default(Color), 2f);
					Dust dust = Main.dust[num329];
					if (Main.rand.Next(15) <12)
					{
						dust.scale *= 0.8f;
					}
					else
					{
						dust.scale *= 1.5f;
						dust.velocity *= 0.05f;
					}
						Main.dust[num329].noGravity = true;
				}

				if (NPC.ai[0]==0 && Main.rand.Next(0,10)<5 && Main.netMode!= NetmodeID.MultiplayerClient)
            {
							float num125 = NPC.velocity.Length();
							Vector2 vector16 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
							float num126 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector16.X;
							float num127 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector16.Y;
							float num128 = (float)Math.Sqrt((double)(num126 * num126 + num127 * num127));
							num128 = num125 / num128;
							NPC.velocity.X = num126 * num128;
							NPC.velocity.Y = num127 * num128;
							NPC.netUpdate=true;
				}
           		 NPC.ai[0]+=1;
						if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) || NPC.ai[0]>200)
						{
							NPC.StrikeNPCNoInteraction(999, 0f, 0, false, false, false);
                            NPC.HitEffect(0, 10.0);
						}

						if (NPC.collideX || NPC.collideY || NPC.Distance(Main.player[NPC.target].Center) < 20f)
						{
							NPC.StrikeNPCNoInteraction(9999, 0f, NPC.direction, false, false, false);
                            NPC.HitEffect(0, 10.0);
						}



		}

	}


    public class PoisonCloud : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }

        public override string Texture
        {
            get { return "Terraria/Images/Item_" + 5; }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override void AI()
        {
            Projectile.velocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y * 0.95f);
            int q = 0;
            for (q = 0; q < 4; q++)
            {

                int dust = Dust.NewDust(Projectile.position - new Vector2(100, 0), 200, 12, DustID.GemEmerald, 0f, Projectile.velocity.Y * 0.4f, 100, Color.DarkGreen, 1.5f);
                Main.dust[dust].noGravity = true;
            }



            Rectangle rectangle1 = new Rectangle((int)Projectile.position.X - 100, (int)Projectile.position.Y - 25, 200, 50);
            int maxDistance = 50;
            bool playerCollision = false;
            for (int index = 0; index < 255; ++index)
            {
                if (Main.player[index].active)
                {
                    Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - Main.player[index].width, (int)Main.player[index].position.Y - Main.player[index].height, Main.player[index].height * 2, Main.player[index].width * 2);
                    if (rectangle1.Intersects(rectangle2))
                    {
                        Main.player[index].AddBuff(BuffID.Venom, 60 * 8, true);
                        Main.player[index].AddBuff(BuffID.OgreSpit, 60 * 5, true);
                        Main.player[index].AddBuff(BuffID.Stinky, 60 * 15, true);
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }

    }

    public class Infestedcompost : Deadfungusbug
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infested Compost");
            Tooltip.SetDefault("'An amalgamation of organic vileness \nSummons Ophiopede?");
        }

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss;
        }

        public override bool? UseItem(Player player)
        {
            //if (!OphioidWorld.downedOphiopede2 && Main.netMode!=1)
            IDGHelper.Chat("The air becomes stale and moist around you.",100,225,100);


            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<OphiopedeHead2>());
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
        
        public override void AddRecipes()
        {

            CreateRecipe(1).AddIngredient(ItemID.BeetleHusk,4).AddIngredient(ModContent.ItemType<Deadfungusbug>()).AddIngredient(ModContent.ItemType<Livingcarrion>()).AddTile(TileID.MythrilAnvil).Register();
        }
    }

    public class Deadfungusbug : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dead Fungusbug");
            Tooltip.SetDefault("'A putrid stench comes from the thing you just made, it might attract something...' \nSummons Ophiopede in a Corruption world");
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss && !WorldGen.crimson;
        }

        public override bool? UseItem(Player player)
        {
        NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<OphiopedeHead>());
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SoulofNight,1).
                AddIngredient(ItemID.SoulofLight, 1).
                AddIngredient(ItemID.SoulofFright, 1).
                AddIngredient(ItemID.SoulofSight, 1).
                AddIngredient(ItemID.SoulofMight, 1).
                AddIngredient(ItemID.ShadowScale, 5).
                AddIngredient(ItemID.CursedFlame, 3).
                AddIngredient(ItemID.RottenChunk, 3).
                AddTile(TileID.MythrilAnvil).Register();
        }
    }

        public class Livingcarrion : Deadfungusbug
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Carrion");
            Tooltip.SetDefault("'A putrid stench comes from the thing you just made, it might attract something...' \nSummons Ophiopede in a Crimson world");
        }

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss && WorldGen.crimson;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SoulofNight, 1).
                AddIngredient(ItemID.SoulofLight, 1).
                AddIngredient(ItemID.SoulofFright, 1).
                AddIngredient(ItemID.SoulofSight, 1).
                AddIngredient(ItemID.SoulofMight, 1).
                AddIngredient(ItemID.TissueSample, 5).
                AddIngredient(ItemID.Ichor, 3).
                AddIngredient(ItemID.Vertebrae, 3).
                AddTile(TileID.MythrilAnvil).Register();
        }
    }


    public class Ophiopedetrophyitem : ModItem
    {

         public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede Trophy");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 50000;
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<OphiodBossTrophy>();
            Item.placeStyle = 0;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    public class OphiopedeMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede Mask");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
        }
    }


    public class OphiodBossTrophy : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            DustType = 7;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Boss Trophy");
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override bool HasSmartInteract()
        {
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int item = 0;
            switch (frameX / 54)
            {
                case 0:
                    item = ModContent.ItemType<Ophiopedetrophyitem>();
                    break;
            }
            if (item > 0)
            {
                Item.NewItem(i * 16, j * 16, 48, 48, item);
            }
        }
    }

    public class SporeInfestedEgg : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spore Infested Egg");
            Tooltip.SetDefault("'Looks like this egg didn't hatch yet to attack me...");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<BabyFlyPet>();
            Item.buffType = ModContent.BuffType<BabyOphioflyBuff>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if(player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }

    }

    public class BabyOphioflyBuff: ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Ophioid Fly");
            Description.SetDefault("Gross but, oddly cute");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            OphioidPlayer modPlayer = player.GetModPlayer<OphioidPlayer>();

            modPlayer.PetBuff=true;

            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<BabyFlyPet>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex),player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<BabyFlyPet>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }

    }

    public class BabyFlyPet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gross but cute Fly");
        }

        public override string Texture
        {
            get { return("OphioidMod/baby_ophiofly_frames");}
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyHornet);
            AIType = ProjectileID.BabyHornet;
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            OphioidPlayer modPlayer = player.GetModPlayer<OphioidPlayer>();
            if(player.dead)
            {
                modPlayer.PetBuff = false;
            }
            if(modPlayer.PetBuff)
            {
                Projectile.timeLeft = 2;
            }
        }
    }

}
