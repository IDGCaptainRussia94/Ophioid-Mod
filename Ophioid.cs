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

	class Ophioid: Mod
	{
		public static Ophioid Instance;
        //public static Mod Idglib;
		public Ophioid()
		{

			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
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
			Mod bossList = ModLoader.GetMod("BossChecklist");
			if (bossList != null)
			{
				bossList.Call("AddBossWithInfo", "Ophiopede", 9.05f, (Func<bool>)(() => OphioidWorld.downedOphiopede), string.Format("Use a [i:{0}] or [i:{1}] anywhere, anytime", ItemType("Deadfungusbug"), ItemType("Livingcarrion")));
                bossList.Call("AddBossWithInfo", "Ophioid", 11.50f, (Func<bool>)(() => OphioidWorld.downedOphiopede2), string.Format("Use a [i:{0}] anywhere, anytime", ItemType("Infestedcompost")));
			}

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
		}

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            if (NPC.CountNPCS(NPCType("OphiopedeHead2")) > 0)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Centipede_Mod_-_Metamorphosis");
                priority = MusicPriority.BossMedium;
            }
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType type = (MessageType)reader.ReadByte();

            if (type==MessageType.OphioidMessage && Main.netMode==1){
            int npcid=reader.ReadInt32();
            int time=reader.ReadInt32();
            Main.npc[npcid].GetGlobalNPC<OphioidNPC>().fallthrough = time;
            }


    }

    public enum MessageType : byte
    {
    OphioidMessage
    }


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

       public int fallthrough=-10;

        public override void AI(NPC npc)
        {
        if (fallthrough>-5){fallthrough-=1;
        if (fallthrough>-1 && fallthrough<1){fallthrough=1;
           if (npc.velocity.Y>0){fallthrough=-5000;
        npc.noTileCollide=true;
        npc.netUpdate=true;
        }}
        }}

    }

    public class OphioidPlayer: ModPlayer
    {
    public bool PetBuff=false;

        public override void ResetEffects()
        {
            PetBuff = false;
        }

    }

    public class OphioidWorld : ModWorld
    {
        #region vars
        public static bool downedOphiopede = false;
        public static bool downedOphiopede2 = false;
        #endregion

        static public bool OphioidBoss
        {
            get {return IdgNPC.FindNPCsMultitype(new ushort[] {(ushort)Ophioid.Instance.NPCType("OphiopedeHead"),(ushort)Ophioid.Instance.NPCType("Ophiofly"),(ushort)Ophioid.Instance.NPCType("Ophiocoon"),(ushort)Ophioid.Instance.NPCType("OphiopedeHead2")}).Count>0;}
        }

        public override void Initialize()
        {
            downedOphiopede = false;
            downedOphiopede2 = false;
        }

                public override TagCompound Save()
        {
            TagCompound Ophioidsavedata = new TagCompound();
            Ophioidsavedata["downedOphiopede"] = downedOphiopede;            
            Ophioidsavedata["downedOphiopede2"] = downedOphiopede2;            

            return Ophioidsavedata;
        }

        public override void Load(TagCompound tag)
        {
            var Ophioidsavedata = tag.GetList<string>("Ophioidsavedata");
            downedOphiopede = tag.GetBool("downedOphiopede");
            downedOphiopede2 = tag.GetBool("downedOphiopede2");
        }

                public override void NetSend(BinaryWriter writer)
        {
            BitsByte bossdeaths = new BitsByte();
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
            get { return("Ophioid/wormmiscparts"); }
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ophiopede");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 70;
			npc.height = 70;
			npc.defense = 0;
		}

        public override bool StrikeNPC(ref double damage,int defense,ref float knockback,int hitDirection,ref bool crit)
        {
            return true;
        }

        public override bool PreAI()
        {
        base.PreAI();

            if ((Main.npc[(int)npc.ai[3]].ai[0]-100f)%400>280 && Main.npc[(int)npc.ai[2]].ai[0]>0 && npc.ai[0]%(20)==0 && Main.netMode!=1)
            {
             OphiopedeHead Head = Main.npc[(int)npc.ai[3]].modNPC as OphiopedeHead;
                if (Head.phase==1){
                int thattarget=0;
                Rectangle rectangle1 = new Rectangle((int)npc.position.X, (int)npc.position.Y+200, npc.width, npc.height+1200);
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
                            thattarget=index;
                            break;
                        }
                    }
                }

                if (playerCollision && Collision.CanHitLine(new Vector2(npc.Center.X, npc.Center.Y), 8, 8, new Vector2(Main.player[thattarget].Center.X, Main.player[thattarget].Center.Y), 8, 8)){
                Player ply=Main.player[npc.target];
            int him=NPC.NewNPC((int)npc.Center.X,(int)npc.Center.Y, (npc.ai[0]%(40)==0 ? NPCID.ToxicSludge : NPCID.SpikedJungleSlime), 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[him].damage*=2;
                Main.npc[him].defense*=2;
                Main.npc[him].lifeMax*=3;
                Main.npc[him].life=Main.npc[him].lifeMax-100;
                Main.npc[him].GetGlobalNPC<OphioidNPC>().fallthrough = (Main.expertMode==true ? 2000 : 1000);
                Main.npc[him].lifeMax*=3; Main.npc[him].life=Main.npc[him].lifeMax;
                Main.npc[him].ai[0] = (float)(-1000 * Main.rand.Next(3));
                Main.npc[him].ai[1] = 0f;
                Main.npc[him].netUpdate=true;
                IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);
                    if (Main.netMode==2){
                    NetMessage.SendData(23, -1, -1, null, him, 0f, 0f, 0f, 0, 0, 0);
                    ModPacket packet = mod.GetPacket();
                    Ophioid mymod = mod as Ophioid;
                    packet.Write((byte)MessageType.OphioidMessage);
                    packet.Write(him);
                    packet.Write(Main.expertMode==true ? 2000 : 1000);
                    packet.Send();
                }}
             }
            }

        return true;
        }

		public override void FindFrame(int frameHeight)
		{

			npc.frame.Y=3*frameHeight;
		}


    }

        public class TheSeeing : ModNPC
    {


		    public override string Texture
        {
            get { return("Ophioid/TheSeeing"); }
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Seeing");
			Main.npcFrameCount[npc.type] = 1;
		}
		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 32;
			npc.damage = 60;
			npc.defense = 15;
			npc.lifeMax = 3000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 0f;
			npc.knockBackResist = 0.3f;
			npc.aiStyle = -1;
			npc.boss=false;
			aiType = NPCID.Wraith;
			animationType = 0;
            npc.behindTiles = true;
            npc.noTileCollide = true;
			npc.noGravity = true;
			music = MusicID.Boss2;
			npc.value = 90000f;
		}

		public override bool CheckActive()
        {
            return !Main.npc[(int)npc.ai[3]].active;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Darkness, 60*4, true);
        }



		public override void AI()
		{

            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)npc.ai[3]].active)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                }
            }

        npc.TargetClosest(true);
        Player ply=Main.player[npc.target];
         float ownerspeed=0.25f+Main.npc[(int)npc.ai[3]].velocity.Length()/25f;
        	npc.ai[0]+=1;
        	if (npc.ai[0]%300==0 && Main.netMode!=1){
        	npc.ai[2]=npc.ai[2]+Main.rand.Next(-30,30);
            npc.ai[1]=Main.rand.Next(90,180);
        	npc.netUpdate=true;
			}
      		Vector2 capvelo=npc.velocity;
            capvelo.Normalize();
            Vector2 diff=new Vector2(Main.npc[(int)npc.ai[3]].Center.X-npc.Center.X,Main.npc[(int)npc.ai[3]].Center.Y-npc.Center.Y);
            if (diff.Length()>npc.ai[1]){
            Vector2 newdif=diff; newdif.Normalize();
            npc.velocity+=(newdif*((diff.Length()-160f)/50f))*(1.4f*ownerspeed);
        }

            if (ply.active){
            Vector2 diff4=new Vector2(ply.Center.X-npc.Center.X,ply.Center.Y-npc.Center.Y);
        	}
			double angle=2.0* Math.PI * ((npc.ai[2]+((float)(Main.npc[(int)npc.ai[3]].rotation/Math.PI)*360f))/360f);
            Vector2 diff3=new Vector2((float)Math.Cos(angle)*5f,(float)Math.Sin(angle)*5f);//new Vector2(ply.Center.X-npc.Center.X,ply.Center.Y-npc.Center.Y);
            diff3.Normalize();
            npc.velocity+=(diff3)*(1.8f*ownerspeed);

        npc.rotation = (float)Math.Atan2((double)diff3.Y, (double)diff3.X) + 1.57f;  
        npc.velocity*=0.93f;
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
        Idglib.DrawTether("Ophioid/tether",Main.npc[(int)npc.ai[3]].Center,npc.Center);
            return true;
        }

    }


    public class OphiopedeBody : ModNPC
    {

    	int framevar=0;

		        public override string Texture
        {
            get { return("Ophioid/wormparts"); }
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ophiopede");
			Main.npcFrameCount[npc.type] = 7;
		}
		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 60;
			npc.damage = 30;
			npc.defense = 15;
			npc.lifeMax = 18000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 0f;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.boss=false;
			aiType = NPCID.Wraith;
			animationType = 0;
            npc.behindTiles = true;
            npc.noTileCollide = true;
			npc.noGravity = true;
			music = MusicID.Boss2;
			npc.value = 90000f;
            //npc.buffImmune[BuffID.Daybreak] = true; npc.buffImmune[BuffID.Frostburn] = true; npc.buffImmune[BuffID.Poisoned] = true; npc.buffImmune[BuffID.Venom] = true;
		}

		public override bool CheckActive()
        {
            return !Main.npc[(int)npc.ai[3]].active;
        }


        public override bool StrikeNPC(ref double damage,int defense,ref float knockback,int hitDirection,ref bool crit)
		{
			damage*=(npc.ai[1]>99 ? 0.35 : 0.15)*(Main.expertMode ? 1 : 1.25);
			return true;
		}

        public override void UpdateLifeRegen(ref int damage)
        {
            if (Main.expertMode)
            {
                npc.lifeRegen /= 10;
                damage /= 10;
            }
       }


		public override bool PreAI()
		{


            if (Main.netMode != 1)
            {
                NPC MyHead = Main.npc[(int)npc.ai[2]];
                if (!MyHead.active || (MyHead.type!=mod.NPCType("OphiopedeHead") && MyHead.type!=mod.NPCType("OphiopedeHead2") && MyHead.type!=mod.NPCType("OphiopedeBody")))
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                }else{
                    if (NPC.CountNPCS(mod.NPCType("OphiopedeHead2"))>0)
                    {
                        npc.defense = 75;
                        npc.damage = 80;
                        npc.dontTakeDamage = MyHead.dontTakeDamage;
                    }
                }
            }



                if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];

            if (npc.ai[2] < (double)Main.npc.Length)
            {
            npc.TargetClosest(true);
            npc.ai[0]+=1;     


            if (npc.ai[0]%60==0 && Main.expertMode){
                for (int k = 0; k < 5; k++)
                {
                 npc.buffTime[k]-=60;
                }
            }

            if (npc.ai[0]==150)
            npc.netUpdate=true;
                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
               int size=60;
                float dirX = Main.npc[(int)npc.ai[2]].position.X + (float)(size / 2) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[2]].position.Y + (float)(size / 2) - npcCenter.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY)*1.4f;
                float dist = (length - (float)npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;


                float mylife=(float)(Main.npc[(int)npc.ai[3]].life)/(float)(Main.npc[(int)npc.ai[3]].lifeMax);
            if (mylife*100<npc.ai[1]-100 && npc.ai[1]>99){
            npc.ai[1]=(int)Main.rand.Next(1,7);
            for(int i=0;i<5;i+=1){
            npc.HitEffect(0, 10.0);
            Vector2 Vect=new Vector2(Main.rand.Next(-2,2),Main.rand.Next(-2,2)); Vect.Normalize();
            Gore.NewGore(npc.Center, Vect, mod.GetGoreSlot("Gores/gore_"+Main.rand.Next(5,9)), 1f);
            }
            npc.netUpdate=true;
        	}


            if (Main.netMode != 1)
            {
            if (Main.npc[(int)npc.ai[3]].ai[0]>-9800 && Main.npc[(int)npc.ai[3]].ai[0]<-5000){

            if (npc.ai[0]%160==0){
            if (!Collision.SolidCollision(npc.position, npc.width, npc.height)){
            int him=NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), mod.NPCType("EvenMoreVileSpit"), 0, 0f, 0f, 0f, 0f, 255);
            //NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.ProjectileType("EvenMoreVileSpit"));
            Main.npc[him].velocity=new Vector2(Main.rand.Next(10,18)*(Main.rand.Next(0,2)==0 ? 1 : -1), Main.rand.Next(-4,4));
            Main.npc[him].timeLeft = 200;
            Main.npc[him].netUpdate=true;
            IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);
            }}

            	/*if (npc.ai[0]%120==0){
            		NPC ply=Main.npc[(int)npc.ai[3]];
            		if (!Collision.SolidCollision(npc.position, npc.width, npc.height)){
                int num54 = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(18,26)*Main.rand.Next(-2,2)>0 ? 1 : -1, Main.rand.Next(-10,2), ProjectileID.DD2OgreSpit, 1, 0f,0);
                Main.projectile[num54].velocity=new Vector2(Main.rand.Next(12,22)*(npc.Center.X<Main.player[npc.target].Center.X ? 1 : -1), Main.rand.Next(-10,2));
                Main.projectile[num54].damage=(int)(20);
                Main.projectile[num54].timeLeft=200;
                Main.projectile[num54].tileCollide=false;
            }}*/


            }}


                if ((Main.npc[(int)npc.ai[3]].ai[0]-100f)%400>280 && Main.npc[(int)npc.ai[2]].ai[0]>0 && npc.ai[0]%(Main.expertMode ? 75 : 115)==0){
            	int thattarget=0;
                Rectangle rectangle1 = new Rectangle((int)npc.position.X, (int)npc.position.Y-600, npc.width, npc.height+1200);
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
                            thattarget=index;
                            break;
                        }
                    }
                }
                if (playerCollision && Collision.CanHitLine(new Vector2(npc.Center.X, npc.Center.Y), 8, 8, new Vector2(Main.player[thattarget].Center.X, Main.player[thattarget].Center.Y), 8, 8)){
                Player ply=Main.player[npc.target];
                int num54 = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-2,2), Main.player[thattarget].Center.Y<npc.Center.Y ? -8 : 8, ProjectileID.Stinger, 1, 0f,0,0,0);
                Main.projectile[num54].damage=(int)(20);
                Main.projectile[num54].timeLeft=200;
                Main.projectile[num54].netUpdate=true;
                IdgProjectile.Sync(num54);
                IdgProjectile.AddOnHitBuff(num54,BuffID.Stinky,60*15);
				}

            	}


            }

		return false;
		}

		public override void FindFrame(int frameHeight)
		{

			framevar=(int)npc.ai[1]>99 ? 0 : (int)npc.ai[1];
			npc.frame.Y=framevar*frameHeight;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;   
        }

        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && npc.ai[1]<7)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/segment_2_gore_"+npc.ai[1]), 1f);
			}
		}

	}

	[AutoloadBossHead]
    public class OphiopedeHead : ModNPC
    {

    	int framevar=0;
        bool charge=false;
    	public bool collision=false;
    	public int phase=0;

	        public int RaycastDown(int x, int y)
        {
            while (!((Main.tile[x, y] != null && (Main.tile[x, y].nactive() && (Main.tileSolid[(int)Main.tile[x, y].type] || Main.tileSolidTop[(int)Main.tile[x, y].type] && (int)Main.tile[x, y].frameY == 0)))))
            {
                y++;
            }
            return y;
        }

        public virtual void StartPhaseTwo()
        {
        //null
        }

        public override bool CheckActive()
        {
            return (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active);
        }

        public override string Texture
        {
            get { return("Ophioid/wormmiscparts"); }
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
			npc.damage = 90;
			npc.defense = 0;
			npc.lifeMax = 18000;
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
			npc.value = Item.buyPrice(1, 25, 0, 0);
		}

        public override void BossLoot(ref string name, ref int potionType)
        {
        potionType=ItemID.GreaterHealingPotion;
        if (NPC.downedMoonlord)
        potionType=ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
                if (Main.rand.Next(0,100)<11)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Ophiopedetrophyitem"));
                if (Main.rand.Next(0,100)<11)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OphiopedeMask"));
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, WorldGen.crimson ? mod.ItemType("Deadfungusbug") : mod.ItemType("Livingcarrion"));

            List<int> types=new List<int>();
            types.Insert(types.Count,ItemID.SoulofMight); types.Insert(types.Count,ItemID.SoulofFright); types.Insert(types.Count,ItemID.SoulofSight); types.Insert(types.Count,ItemID.SoulofNight); types.Insert(types.Count,ItemID.SoulofLight); types.Insert(types.Count,ItemID.SoulofNight); types.Insert(types.Count,ItemID.SoulofLight);
            for (int f = 0; f < (Main.expertMode ? 80 : 50); f=f+1){
        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, types[Main.rand.Next(0,types.Count)]);
        }

        if (!OphioidWorld.downedOphiopede && Main.netMode!=1)
        Idglib.Chat("The infested worm is defeated, but you can still feel the presence of the "+(WorldGen.crimson ? "Crimson" : "Corruption")+"'s abomination",100,225,100);
        OphioidWorld.downedOphiopede=true;
        }

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

			public override bool PreAI()
		{

            if (GetType() == typeof(OphiopedeHead2))
            {
                if (npc.life < (int)(npc.lifeMax * 0.05f))
                {
                    npc.dontTakeDamage = true;
                    npc.life = (int)(npc.lifeMax * 0.05f);
                }
            }

			int belowground=0;
			bool outofbounds=false;
			collision=false;
            int minTilePosX = (int)(npc.position.X / 16.0) - 1;
            int maxTilePosX = minTilePosX+(int)((npc.width) / 16.0) + 2;
            int minTilePosY = (int)(npc.position.Y / 16.0) - 1;
            int maxTilePosY = minTilePosY+(int)((npc.height) / 16.0) + 2;
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
                    if (Main.tile[i, j] != null && (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0) || (int)Main.tile[i, j].liquid > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);;
                        if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16.0 && (npc.position.Y + npc.height > (double)vector2.Y && npc.position.Y < vector2.Y + 16.0))
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
                    if (Main.tile[i, j] != null && (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0) || (int)Main.tile[i, j].liquid > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);;
                        if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16.0 && (npc.position.Y + npc.height > (double)vector2.Y && npc.position.Y < vector2.Y + 16.0))
                        {
                            belowground +=1;
                        }
                    }
                }
            }

                npc.TargetClosest(true);
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
            collision=false;
                if (npc.Center.Y > Main.maxTilesY * 16)
                {
                    npc.active = false;
                }
            }

            if (Main.netMode != 1)
            {
            if (Main.rand.Next(0,30)==0)
            npc.netUpdate=true;

            if (npc.ai[0]==0){
            		int lastnpc=npc.whoAmI;
            		int latest=lastnpc;
                    int randomWormLength = (Main.expertMode ? 40 : 35);
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latest = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OphiopedeBody"));
                        Main.npc[(int)latest].realLife = npc.whoAmI;
                        Main.npc[(int)latest].ai[2] = lastnpc;
                        Main.npc[(int)latest].ai[3] = npc.whoAmI;
                        Main.npc[(int)latest].ai[0] = Main.rand.Next(0,80);
                        Main.npc[(int)latest].ai[1] = Main.rand.Next(110,180);
                        lastnpc=latest;
                        IdgNPC.AddOnHitBuff((int)latest,BuffID.Stinky,60*15);
                    }
                        latest = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OphiopedeTail"));
                        Main.npc[(int)latest].realLife = npc.whoAmI;
                        Main.npc[(int)latest].ai[2] = lastnpc;
                        Main.npc[(int)latest].ai[3] = npc.whoAmI;
                        Main.npc[(int)latest].ai[0] = Main.rand.Next(0,80);
                        Main.npc[(int)latest].ai[1] = 250;
                    npc.ai[0] = 1;
                }
            npc.netUpdate=true;

       	 }

       	 	if (phase==0 && npc.life<npc.lifeMax*(this.GetType().Name=="OphiopedeHead2" ? 0.75 : 0.5) && npc.ai[0]>0){
                Main.PlaySound(SoundID.Roar, npc.position, 0);
       	 		phase=1;
           		if (Main.netMode != 1)
            	{
            		npc.ai[0]=1;
            		int lastnpc=npc.whoAmI;
            		int latest=lastnpc;
                    int randomWormLength = Main.rand.Next(8, 8);
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latest = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TheSeeing"));
                        Main.npc[(int)latest].ai[2] = Main.rand.Next(0,360);
                        Main.npc[(int)latest].ai[3] = npc.whoAmI;
                        Main.npc[(int)latest].ai[1] = Main.rand.Next(90,180);
                        Main.npc[(int)latest].ai[0] = Main.rand.Next(0,600);
                        Main.npc[(int)latest].lifeMax=(int)(npc.lifeMax*0.15); Main.npc[(int)latest].life=npc.life;
                        Main.npc[(int)latest].netUpdate=true;
                        lastnpc=latest;
                    }
                }

       	 	}


            if ((npc.ai[0]-60f)%400>360){npc.ai[0]+=1;
            collision=false;
        }

            Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
            float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);
            float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
            npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            float length2 = (new Vector2((targetXPos-npc.Center.X),(targetYPos-npc.Center.Y))).Length();

            float speedboost=Math.Min(4f,Math.Max(1f+(charge==true ? 0.5f : 0f),length2/500f));
            charge=false;

            	if (npc.ai[0]<0){
            	npc.ai[0]+=1;
            	if (npc.ai[0]<-9900)
            	npc.velocity+=new Vector2(0,1f);
            	if (npc.ai[0]<-9750 && npc.ai[0]>-9900){
                StartPhaseTwo();
            	npc.velocity-=new Vector2(0,0.96f);
            	npc.velocity=new Vector2(npc.velocity.X*0.95f,npc.velocity.Y);
           		 }

            	if (npc.ai[0]>-9200)
            	npc.ai[0]=(phase==0 ? 1 : 250);

            	if (npc.ai[0]>-9250){
			Vector2 moveto=new Vector2(targetXPos-npc.Center.X,(npc.Center.Y-600f)-npc.Center.Y);
			moveto.Normalize();
			npc.velocity+=moveto*1.2f;
			if (npc.velocity.Length()>15f){
			npc.velocity.Normalize();
			npc.velocity*=15f;
			}
            	}else{
            	if (npc.ai[0]>-9750){
            	npc.velocity*=0.94f;
            	}


            	if (npc.ai[0]>-9720){

            	for (int q = 0; q < 4; q++)
				{

					int dust = Dust.NewDust(npc.Center-new Vector2(8,40), 16, 12, 89, Main.rand.Next(-100,100)*0.1f, Main.rand.Next(-100,-50)*0.2f, 100, Color.DarkGreen, 2f);
					Main.dust[dust].noGravity = true;
					//Main.dust[dust].velocity *= 1.8f;
					//Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);

						int num184 = Dust.NewDust(npc.Center-new Vector2(8,40), 16, 12, 89, Main.rand.Next(-100,100)*0.1f, Main.rand.Next(-100,-50)*0.2f, 31, Color.DarkGreen, 2f);
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


				if (npc.ai[0]%8==0){

            		NPC ply=Main.npc[(int)npc.ai[3]];

            		if (!Collision.SolidCollision(npc.position, npc.width, npc.height)){
                int num54 = Projectile.NewProjectile(npc.Center.X+Main.rand.Next(-8,8), npc.Center.Y-40f, 0f,0f, ProjectileID.DD2OgreSpit, 1, 0f,0);
                Main.projectile[num54].velocity=new Vector2(Main.rand.Next(-8,8)*(Main.rand.Next(0,2)==0 ? 1 : -1), Main.rand.Next(-10,-3));
                Main.projectile[num54].damage=(int)(50);
                Main.projectile[num54].timeLeft=400;
                Main.projectile[num54].tileCollide=(phase==0 ? false : true);
                Main.projectile[num54].netUpdate=true;
                IdgProjectile.Sync(num54);
                IdgProjectile.AddOnHitBuff(num54,BuffID.Stinky,60*15);
            }}

            		if (Main.netMode!=1 && npc.ai[0]%8==0 && phase==1){

            		NPC ply=Main.npc[(int)npc.ai[3]];

            int him=NPC.NewNPC((int)(Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) + Main.rand.Next(-800,800)), (int)(Main.player[npc.target].position.Y - 700f), mod.NPCType("EvenMoreVileSpit"), 0, 0f, 0f, 0f, 0f, 255);
            //NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.ProjectileType("EvenMoreVileSpit"));
            Main.npc[him].velocity=new Vector2(Main.rand.Next(1,6)*Main.rand.Next(0,2)==0 ? 1 : -1, Main.rand.Next(5,10));
            Main.npc[him].timeLeft = 200;
            if (Main.rand.Next(0,3)<8)
            Main.npc[him].ai[0]=3f;
            Main.npc[him].netUpdate=true;
            IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);
            	}
            }

      		Vector2 capvelo=npc.velocity;
            capvelo.Normalize();
            npc.velocity=capvelo*Math.Min(npc.velocity.Length(),15f);

            	}
            	}else{


                if (collision){
                npc.ai[0]+=outofbounds==false ? (1) : 0;
      			npc.velocity*=0.95f;
      			if (npc.soundDelay==0)
                {
      			npc.soundDelay=4+(int)(length/40f);
                Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1);
      			}

      			Vector2 moveto=new Vector2(targetXPos-npc.Center.X,targetYPos-npc.Center.Y);
      			if ((npc.ai[0])%700<500 && belowground<5 && !((npc.ai[0]-40f)%400>360)){
                charge=true;
      			npc.velocity=new Vector2(npc.velocity.X,(npc.velocity.Y*0.8f)+0.8f);
      			}else{
      			if (belowground>2 && npc.ai[0]%30==0){
      			int rayloc=RaycastDown((int)npc.Center.X/16,(int)(npc.Center.Y-1000f)/16)*16;
                int num54 = Projectile.NewProjectile(npc.Center.X,(float)rayloc-(10f), Main.rand.Next(-2,2), 3, mod.ProjectileType("PoisonCloud"), 1, 0f,0);
                Main.projectile[num54].damage=(int)(20);
                Main.projectile[num54].timeLeft=200;
				Main.projectile[num54].velocity=new Vector2(0,0);
				Main.projectile[num54].netUpdate=true;
      			}}
      			moveto.Normalize();
      			npc.velocity+=moveto*0.5f*speedboost;
      			if ((npc.ai[0]-40f)%400>360){
      			npc.velocity+=moveto*0.9f;
      			moveto=new Vector2(((Main.player[npc.target].velocity.X*3f)+targetXPos)-npc.Center.X,targetYPos-npc.Center.Y-400f);
      			moveto.Normalize();
      			npc.velocity+=moveto*0.3f;
      			}
      			if (npc.ai[0]%400>250){
				if (npc.ai[0]%30==0)
				Main.PlaySound(14, (int)npc.position.X, (int)npc.position.Y, 1);
      			}
      			if (npc.ai[0]%400>300){
				npc.velocity+=new Vector2(0,0.45f);
      			}

      		Vector2 capvelo=npc.velocity;
            capvelo.Normalize();
            npc.velocity=capvelo*Math.Min(Math.Max(npc.velocity.Length(),6f*speedboost),25f*speedboost);

            if (npc.ai[0]>1000){
            npc.ai[0]=-10000;
            }


      			}else{
      			npc.velocity+=new Vector2(0,0.2f);
                    if ((Main.player[npc.target].Center - npc.Center).Length() > 1600)
                    {
                        npc.velocity.X *= 0.99f;
                        npc.timeLeft += 1;
                    }


      			}
      			
			}

      			npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + 1.57f;  


		return false;
		}


		public override void FindFrame(int frameHeight)
		{

			framevar=phase==1 ? (0) : (npc.ai[0]%30<15 ? 1 : 2);
			npc.frame.Y=framevar*npc.height;
		}

				 public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_4"), 1f);
				Gore.NewGore(npc.position-new Vector2(10,6), npc.velocity, mod.GetGoreSlot("Gores/gore_1"), 1f);
				Gore.NewGore(npc.position-new Vector2(-10,6), npc.velocity, mod.GetGoreSlot("Gores/gore_1"), 1f);

			}
		}

	}

	public class EvenMoreVileSpit : ModNPC
	{

		public override void SetDefaults()
		{
				npc.width = 16;
				npc.height = 16;
				npc.aiStyle = 9;
				npc.damage = 65;
				npc.defense = 0;
				npc.lifeMax = 1;
				npc.HitSound = null;
				npc.DeathSound = SoundID.NPCDeath9;
				npc.noGravity = true;
				npc.noTileCollide = true;
				npc.knockBackResist = 0f;
				npc.scale = 0.9f;
				npc.alpha = 80;
				npc.aiStyle=-1;
		}

				public override string Texture
		{
			get { return "Terraria/Npc_" + NPCID.VileSpit; }
		}


		public override void HitEffect(int hitDirection, double damage){

				for (int num328 = 0; num328 < 20; num328 += 1)
				{
					int num329 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 18, npc.velocity.X*1f, npc.velocity.Y*1f, 100, default(Color), 3f);
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
            npc.TargetClosest(true);
				for (int num328 = 0; num328 < 2; num328 += 1)
				{
					int num329 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 18, npc.velocity.X*0.3f, npc.velocity.Y*0.3f, 100, default(Color), 2f);
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

				if (npc.ai[0]==0 && Main.rand.Next(0,10)<5 && Main.netMode!=1){
							float num125 = npc.velocity.Length();
							Vector2 vector16 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
							float num126 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector16.X;
							float num127 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector16.Y;
							float num128 = (float)Math.Sqrt((double)(num126 * num126 + num127 * num127));
							num128 = num125 / num128;
							npc.velocity.X = num126 * num128;
							npc.velocity.Y = num127 * num128;
							npc.netUpdate=true;
				}
           		 npc.ai[0]+=1;
						if (Collision.SolidCollision(npc.position, npc.width, npc.height) || npc.ai[0]>200)
						{
							npc.StrikeNPCNoInteraction(999, 0f, 0, false, false, false);
                            npc.HitEffect(0, 10.0);
						}

						if (npc.collideX || npc.collideY || npc.Distance(Main.player[npc.target].Center) < 20f)
						{
							npc.StrikeNPCNoInteraction(9999, 0f, npc.direction, false, false, false);
                            npc.HitEffect(0, 10.0);
						}



		}

	}


	public class PoisonCloud : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			//projectile.aiStyle = 1;
			projectile.friendly = false;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.tileCollide=false;
		}

				public override string Texture
		{
			get { return "Terraria/Item_" + 5; }
		}

	public override bool? CanHitNPC(NPC target){
		return false;
	}
	public override bool CanHitPlayer(Player target){
		return false;
	}
	public override void AI()
	{
		projectile.velocity=new Vector2(projectile.velocity.X,projectile.velocity.Y*0.95f);
		int q=0;
			for (q = 0; q < 4; q++)
				{

					int dust = Dust.NewDust(projectile.position-new Vector2(100,0), 200, 12, 89, 0f, projectile.velocity.Y * 0.4f, 100, Color.DarkGreen, 1.5f);
					Main.dust[dust].noGravity = true;
					//Main.dust[dust].velocity *= 1.8f;
					//Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}



                Rectangle rectangle1 = new Rectangle((int)projectile.position.X-100, (int)projectile.position.Y-25, 200, 50);
                int maxDistance = 50;
                bool playerCollision = false;
                for (int index = 0; index < 255; ++index)
                {
                    if (Main.player[index].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - Main.player[index].width, (int)Main.player[index].position.Y - Main.player[index].height, Main.player[index].height * 2, Main.player[index].width*2);
                        if (rectangle1.Intersects(rectangle2))
                        {
						Main.player[index].AddBuff(BuffID.Venom, 60*8, true);
						Main.player[index].AddBuff(BuffID.OgreSpit, 60*5, true);                       
                        Main.player[index].AddBuff(BuffID.Stinky, 60*15, true);
                        }
                    }
                }



	}

public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{
return false;
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

        public override bool UseItem(Player player)
        {
            if (!OphioidWorld.downedOphiopede2 && Main.netMode!=1)
            Idglib.Chat("The air becomes stale and moist around you.",100,225,100);
            //Idglib.Chat("The air becomes stale and moist around you.",30,180,15);
            //NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 800, mod.NPCType("OphiopedeHead2"));
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("OphiopedeHead2"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(2218,4);
            recipe.AddIngredient(null,"Deadfungusbug",1);
            recipe.AddIngredient(null,"Livingcarrion",1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
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
            item.width = 12;
            item.height = 12;
            item.maxStack = 99;
            item.rare = 3;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            item.UseSound = SoundID.Item44;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss && !WorldGen.crimson;
        }

        public override bool UseItem(Player player)
        {
            if (!OphioidWorld.downedOphiopede && Main.netMode!=1)
            Idglib.Chat("The air becomes stale and moist around you.",100,225,100);
            //Idglib.Chat("The air becomes stale and moist around you.",30,180,;
            //NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 800, mod.NPCType("OphiopedeHead"));
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("OphiopedeHead"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight,1);
            recipe.AddIngredient(ItemID.SoulofLight,1);
            recipe.AddIngredient(ItemID.SoulofFright,1);
            recipe.AddIngredient(ItemID.SoulofSight,1);
            recipe.AddIngredient(ItemID.SoulofMight,1);
            recipe.AddIngredient(ItemID.ShadowScale,5);
            recipe.AddIngredient(ItemID.CursedFlame,3);
            recipe.AddIngredient(ItemID.RottenChunk,5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight,1);
            recipe.AddIngredient(ItemID.SoulofLight,1);
            recipe.AddIngredient(ItemID.SoulofFright,1);
            recipe.AddIngredient(ItemID.SoulofSight,1);
            recipe.AddIngredient(ItemID.SoulofMight,1);
            recipe.AddIngredient(ItemID.TissueSample,5);
            recipe.AddIngredient(ItemID.Ichor,3);
            recipe.AddIngredient(ItemID.Vertebrae,5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
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
            item.width = 30;
            item.height = 30;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 50000;
            item.rare = 1;
            item.createTile = mod.TileType("OphiodBossTrophy");
            item.placeStyle = 0;
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
            item.width = 20;
            item.height = 26;
            item.rare = 1;
        }
    }


    public class OphiodBossTrophy : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            dustType = 7;
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Boss Trophy");
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int item = 0;
            switch (frameX / 54)
            {
                case 0:
                    item = mod.ItemType("Ophiopedetrophyitem");
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
            item.CloneDefaults(ItemID.ZephyrFish);
            item.rare = 10;
            item.shoot = mod.ProjectileType("BabyFlyPet");
            item.buffType = mod.BuffType("BabyOphioflyBuff");
        }

        public override void UseStyle(Player player)
        {
            if(player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }

    }

    public class BabyOphioflyBuff: ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Baby Ophioid Fly");
            Description.SetDefault("Gross but, oddly cute");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            OphioidPlayer modPlayer = (OphioidPlayer)player.GetModPlayer(mod, "OphioidPlayer");
            modPlayer.PetBuff=true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("BabyFlyPet")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("BabyFlyPet"), 0, 0f, player.whoAmI, 0f, 0f);
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
            get { return("Ophioid/baby_ophiofly_frames");}
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyHornet);
            aiType = ProjectileID.BabyHornet;
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            OphioidPlayer modPlayer = (OphioidPlayer)player.GetModPlayer(mod, "OphioidPlayer");
            if(player.dead)
            {
                modPlayer.PetBuff = false;
            }
            if(modPlayer.PetBuff)
            {
                projectile.timeLeft = 2;
            }
        }
    }

}
