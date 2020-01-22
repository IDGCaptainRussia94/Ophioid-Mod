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
using Idglibrary;
using Idglibrary.Bases;


namespace Ophioid
{

    public class MetaOphiocoon : ModNPC
    {

        public override string Texture
        {
            get { return("Ophioid/cocoon");}
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiocoon");
        }

        public override void AI()
        {
        npc.ai[0]+=1;

        if (npc.ai[0]>10)
        {

        if (Main.netMode!=1){

            int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
            int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
            int num663 = mod.NPCType("Ophiocoon");

            int num664 = NPC.NewNPC(x, y, num663);
                    if (Main.netMode == 2 && num664 < 200)
                    {
                       NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }

        }
        npc.active = false;
        }

        }

        public override void SetDefaults()
        {
            npc.width = 96;
            npc.height = 256;
            npc.damage = 0;
            npc.defense = 75;
            npc.lifeMax = 2000000;
            npc.knockBackResist=0f;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            npc.boss=true;
            npc.dontTakeDamage=true;
            npc.immortal=true;
            aiType = -1;
            animationType = -1;
        }

    }

        [AutoloadBossHead]
    public class OphiopedeHead2 : OphiopedeHead
    {
        public override string Texture
        {
            get { return("Ophioid/wormmiscparts"); }
        }
        
        public override void StartPhaseTwo()
        {
        if (npc.life<(npc.lifeMax*0.50)){
        if (Main.netMode!=1){
        npc.boss=false;
        npc.active=false;
        Idglib.Chat("The Ophiopede begins to metamorphosize!",100,225,100);

            int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
            int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
            int num663 = mod.NPCType("MetaOphiocoon");

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

    public class OphSporeCloud : ModNPC
    {
        int mytimeisover=800;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophioid Spore Cloud");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override string Texture
        {
            get { return "Terraria/Projectile_" + ProjectileID.SporeCloud; }
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 24;
            npc.height = 24;
            npc.damage = 60;
            npc.defense = 0;
            npc.lifeMax = 400;
            npc.value = 0f;
            npc.noGravity = true;
            npc.knockBackResist=0f;
            npc.aiStyle = 5;
            aiType = NPCID.BeeSmall;
            animationType = NPCID.BeeSmall;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override bool CheckDead()
        {
        List<Projectile> projectile22=Idglib.Shattershots(npc.Center,npc.Center+npc.velocity,new Vector2(0,0),ProjectileID.SporeCloud,50,npc.velocity.Length()+10f,0,1,true,(float)Main.rand.Next(-100,100)*0.002f,false,240);
        IdgProjectile.Sync(projectile22[0].whoAmI);
        return true;
        }

        public override void AI()
        {
        mytimeisover-=1;
        npc.velocity/=1.15f;
        npc.velocity.Normalize();
        npc.velocity*=16f-(float)(mytimeisover*0.015f);
        if (mytimeisover<1)
        npc.StrikeNPCNoInteraction(9999, 0f, npc.direction, false, false, false);


        }


    }

        [AutoloadBossHead]
    public class Ophiofly : ModNPC
    {

        public bool poweredup=true;
        public int chargesleft=0;
        public int spawnminionsat=0;
        private Vector2[] oldPos = new Vector2[4];
        private float[] oldrot = new float[4];
        float leftright=0;
        int projectiletrack=0;
        bool noplayer=false;
        bool glowred=false;
        Player ply=null;


        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiofly");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 128;
            npc.height = 96;
            npc.damage = 150;
            npc.defense = 75;
            npc.lifeMax = 75000;
            npc.knockBackResist=0f;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            npc.boss=true;
            aiType = -1;
            animationType = -1;
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

public void resetattacks()
{

if (spawnminionsat==0)
spawnminionsat=(int)(npc.lifeMax*0.8);

if (Main.netMode!=1){
npc.ai[0]=0;
int[] pick = {0,1,2,2,3,4};
chargesleft=2;

npc.ai[1]=pick[Main.rand.Next(0,pick.Length)];
if (npc.life<spawnminionsat){
npc.ai[1]=5;
spawnminionsat-=(int)(npc.lifeMax*0.3);
}

npc.netUpdate=true;
}
}


public void ichorbeam()
{

    if (npc.ai[0]>300){

        if (npc.ai[0]==301){
        leftright=(ply.Center.X-npc.Center.X)>0 ? 1 : -1;
        }

        Vector2 plyloc=ply.Center+new Vector2(leftright*1200,-240);
        Vector2 plydist=(plyloc-npc.Center);
        Vector2 plynormal=plydist; plynormal.Normalize();
        npc.direction=(plynormal.X<0f).ToDirectionInt();

    if (npc.ai[0]>300 && npc.ai[0]<400){

        plyloc=ply.Center+new Vector2(-leftright*800,-320);
        plydist=(plyloc-npc.Center);
        plynormal=plydist; plynormal.Normalize();
        npc.velocity+=plynormal*0.4f;
        npc.direction=(plydist.X<0f).ToDirectionInt();
    }else{
        npc.velocity+=plynormal*0.8f;
    }

        npc.velocity/=1.025f;

        if (npc.ai[0]==400){
        List<Projectile> itz=Idglib.Shattershots(npc.Center,npc.Center+new Vector2(-1*npc.direction,4),new Vector2(0,0),mod.ProjectileType("Ophiobeamichor"),15,1f,0,1,true,0f,false,150);
        //Main.PlaySound(SoundID.Item33, npc.position);
        Main.projectile[projectiletrack].netUpdate=true;
        Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 104, 1f, 0f);
        projectiletrack=itz[0].whoAmI;
        }

        if (projectiletrack>0 && Main.projectile[projectiletrack].type==mod.ProjectileType("Ophiobeamichor")){
        Main.projectile[projectiletrack].position=npc.Center+new Vector2(-npc.direction*20,10);
        }

        if (npc.ai[0]==550){resetattacks(); projectiletrack=0;}


    if (npc.velocity.Length()>18f){npc.velocity.Normalize(); npc.velocity*=18f;}
    }


}

public void bodyslam()
{

    if (npc.ai[0]<300)
    return;

        Vector2 plyloc=ply.Center+new Vector2(0,-320);
        Vector2 plydist=(plyloc-npc.Center);
        Vector2 plynormal=plydist; plynormal.Normalize();

        int minTilePosX = (int)(npc.Center.X / 16.0) - 1;
        int minTilePosY = (int)((npc.Center.Y+32f) / 16.0) - 1;

            int whereisity;
            whereisity=Idglib.RaycastDown(minTilePosX+1,Math.Max(minTilePosY,0));



        npc.velocity/=1.01f;
        if (npc.ai[0]<500){
        npc.velocity/=1.025f;
        npc.velocity+=plynormal*1.0f;
        }else{
        if (npc.ai[0]<600){
        npc.ai[0]=501;
        npc.velocity.Y+=0.2f+Math.Abs(npc.velocity.Y/14f);
        if (npc.velocity.Y>32f)
        npc.velocity.Y=32f;
        if (whereisity-minTilePosY<2 && npc.Center.Y > ply.Center.Y){
        npc.ai[0]=600;
        npc.AddBuff(BuffID.BrokenArmor, 60*10, true);

        Main.PlaySound(SoundID.Item14,npc.Center);
        Main.PlaySound(SoundID.Item90,npc.Center);

        for (float num315 = -10; num315 < 10; num315 = num315 + 2f)
            {
                int num54 = Projectile.NewProjectile(npc.Center.X+Main.rand.Next(-8,8), npc.Center.Y-40f, 0f,0f, ProjectileID.DD2OgreSpit, 1, 0f,0);
                Main.projectile[num54].velocity=new Vector2(Main.rand.Next(-8,8)*(Main.rand.Next(0,2)==0 ? 1 : -1)+(num315/4f), Main.rand.Next(-15,-3));
                Main.projectile[num54].damage=(int)(50);
                Main.projectile[num54].timeLeft=800;
                Main.projectile[num54].tileCollide=true;
                Main.projectile[num54].netUpdate=true;
                IdgProjectile.Sync(num54);
                IdgProjectile.AddOnHitBuff(num54,BuffID.Stinky,60*15);
            }

        for (float num315 = -40; num315 < 40; num315 = num315 + 0.2f)
            {
                if (Main.rand.Next(0,100)<25){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                int num316 = Dust.NewDust(npc.Center+new Vector2(num315*3, -30), 0, 80, 184, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 4f-Math.Abs(num315)/15f);
                Main.dust[num316].noGravity = true;
                Dust dust3 = Main.dust[num316];
                dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat());
                dust3.velocity.X+=(float)num315/5f;
            }}

        }}
        }

        if (npc.ai[0]>599)
        npc.velocity=Vector2.Zero;

        if (npc.ai[0]==630){resetattacks(); projectiletrack=0;}


        if (Main.rand.Next(300,500)<npc.ai[0]){
        glowred=true; 
        }


}

public void makeminions()
{
npc.velocity/=1.01f;


                for (int a = 0; a < 20; a++){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                Vector2 vecr=randomcircle*512;
                vecr*=(1f-(300f/(npc.ai[0]%300)));

                    int num622 = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y)+vecr, 0, 0, 184, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[num622].velocity = randomcircle*-16f;

                    Main.dust[num622].noGravity = true;
                    Main.dust[num622].color=Main.hslToRgb((float)(Main.GlobalTime/5)%1, 0.9f, 1f);
                    Main.dust[num622].color.A=10;
                    Main.dust[num622].velocity.X = npc.velocity.X/3 + (Main.rand.Next(-50, 51) * 0.005f);
                    Main.dust[num622].velocity.Y = npc.velocity.Y/3 + (Main.rand.Next(-50, 51) * 0.005f);
                    Main.dust[num622].alpha = 100;;
                }


            if (Main.netMode != 1 && npc.ai[0]>100 && npc.ai[0]%50==0)
            {
            int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
            int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
            int num663 = mod.NPCType("FlyMinion");

            int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
            Main.npc[num664].ai[1]=npc.whoAmI;
                            if (Main.netMode == 2 && num664 < 200)
                            {
                                NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                            }
            }

        if (npc.ai[0]==300){resetattacks(); projectiletrack=0;}

}


public void sporeclouds()
{

        if (npc.ai[0]%40==0 && npc.ai[0]>150){
            if (Main.netMode!=1){
            int him=NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), mod.NPCType("OphSporeCloud"), 0, 0f, 0f, 0f, 0f, 255);
            Main.npc[him].velocity=new Vector2(Main.rand.Next(10,18)*(Main.rand.Next(0,2)==0 ? 1 : -1), Main.rand.Next(-4,4));
            Main.npc[him].netUpdate=true;
        }
        Main.PlaySound(SoundID.Item111,npc.Center);

        }

    if (npc.ai[0]<300)
    return;

        Vector2 plyloc=ply.Center+new Vector2(0,-240);
        Vector2 plydist=(plyloc-npc.Center);
        Vector2 plynormal=plydist; plynormal.Normalize();

        npc.velocity/=1.015f;
        npc.velocity+=plynormal*0.5f;

        npc.direction=(plydist.X<0f).ToDirectionInt();

        if (npc.ai[0]==450){resetattacks(); projectiletrack=0;}


}

public void feralbite()
{

        Vector2 plyloc=ply.Center+new Vector2(0,16);
        Vector2 plydist=(plyloc-npc.Center);
        Vector2 plynormal=plydist; plynormal.Normalize();
        npc.direction=(ply.Center.X-npc.Center.X<0f).ToDirectionInt();
        glowred=true;

    npc.velocity/=1.02f;

        if (npc.ai[0]>10 && npc.ai[0]<80 && npc.ai[0]%5==0 && Main.expertMode){
        List<Projectile> projectile22=Idglib.Shattershots(npc.Center,plyloc,new Vector2(0,-16),ProjectileID.Stinger,30,20f,0,1,true,(float)Main.rand.Next(-100,100)*0.004f,false,240);
        Projectile projectile2=projectile22[0];
        IdgProjectile.Sync(projectile2.whoAmI);
        Main.PlaySound(SoundID.Item42,npc.Center);

        for (int num315 = 1; num315 < 8; num315 = num315 + 1)
            {
                if (Main.rand.Next(0,100)<25){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                int num316 = Dust.NewDust(new Vector2(projectile2.position.X-1, projectile2.position.Y), projectile2.width, projectile2.height, 184, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 1.00f);
                Main.dust[num316].noGravity = true;
                Dust dust3 = Main.dust[num316];
                dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat());
                dust3.velocity.Normalize();
                dust3.velocity+=(projectile2.velocity*2f);
            }}

        }

    if (npc.ai[0]<200 && npc.ai[0]>80)
    npc.velocity+=plynormal*1.5f;

    if (npc.ai[0]<120)
    npc.velocity/=1.25f;
    if (npc.velocity.Length()>32f){npc.velocity.Normalize(); npc.velocity*=32f;}

        if (npc.ai[0]==240){
            chargesleft-=1;
            npc.ai[0]=60-(int)((float)npc.life/(float)npc.lifeMax)*100;
            npc.netUpdate=true;
            if (chargesleft<1){resetattacks(); projectiletrack=0;}
        }
}



public void sludgefield()
{

    if (npc.ai[0]>300){

        if (npc.ai[0]==301){
        leftright=(ply.Center.X-npc.Center.X)>0 ? 1 : -1;
        }

        Vector2 plyloc=ply.Center+new Vector2(0,-320);
        Vector2 plydist=(plyloc-npc.Center);
        Vector2 plynormal=plydist; plynormal.Normalize();
        if (npc.ai[0]<450)
        npc.direction=(ply.Center.X-npc.Center.X<0f).ToDirectionInt();

    if (npc.ai[0]<450){

        for (int num315 = 1; num315 < 8; num315 = num315 + 1)
            {
                if (Main.rand.Next(0,100)<25){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                int num316 = Dust.NewDust(npc.Center+new Vector2((-npc.direction*20)-12,12), 24, 24, 184, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 2.00f);
                Main.dust[num316].noGravity = true;
                Dust dust3 = Main.dust[num316];
                dust3.velocity = (randomcircle*1.5f*Main.rand.NextFloat());
            }}
        }
    if (npc.ai[0]>300 && npc.ai[0]<400){
        plyloc=ply.Center+new Vector2(-leftright*500,-240);
        plydist=(plyloc-npc.Center);
        plynormal=plydist; plynormal.Normalize();
        npc.velocity+=plynormal*0.25f;
        npc.direction=(plydist.X<0f).ToDirectionInt();
    }

        npc.velocity/=1.025f;
        npc.velocity+=plynormal*0.075f;

        if (npc.ai[0]==450){
        List<Projectile> itz=Idglib.Shattershots(npc.Center,npc.Center+new Vector2(-1*npc.direction,5),new Vector2(0,0),mod.ProjectileType("Ophiobeam"),80,1f,0,1,true,0f,false,140);
        Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 104, 1f, 0f);
        //Main.PlaySound(SoundID.Item33, npc.position);
        projectiletrack=itz[0].whoAmI;
        Main.projectile[projectiletrack].netUpdate=true;
        }if (npc.ai[0]>449){
        npc.velocity/=1.15f;
        }

        if (projectiletrack>0 && Main.projectile[projectiletrack].type==mod.ProjectileType("Ophiobeam")){
        Main.projectile[projectiletrack].position=npc.Center+new Vector2(-npc.direction*20,10);
        }

        if (npc.ai[0]==600){resetattacks(); projectiletrack=0;}


    if (npc.velocity.Length()>24f){npc.velocity.Normalize(); npc.velocity*=24f;}
    }


}

        public override void BossLoot(ref string name, ref int potionType)
        {
        name = "Ophioid";
        potionType=ItemID.GreaterHealingPotion;
        if (NPC.downedMoonlord)
        potionType=ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
                if (Main.rand.Next(0,100)<31)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Ophiopedetrophyitem"));
                if (Main.rand.Next(0,100)<31)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OphiopedeMask"));

                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SporeInfestedEgg"));

            List<int> types=new List<int>();
            types.Insert(types.Count,ItemID.SoulofMight); types.Insert(types.Count,ItemID.SoulofFright); types.Insert(types.Count,ItemID.SoulofSight); types.Insert(types.Count,ItemID.SoulofNight); types.Insert(types.Count,ItemID.SoulofLight); types.Insert(types.Count,ItemID.SoulofNight); types.Insert(types.Count,ItemID.SoulofLight);
            for (int f = 0; f < (Main.expertMode ? 200 : 120); f=f+1){
        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, types[Main.rand.Next(0,types.Count)]);
        }

            types=new List<int>();
            types.Insert(types.Count,ItemID.SoulofFlight); types.Insert(types.Count,ItemID.SoulofFlight); types.Insert(types.Count,ItemID.FragmentStardust); types.Insert(types.Count,ItemID.FragmentSolar); types.Insert(types.Count,ItemID.FragmentVortex); types.Insert(types.Count,ItemID.FragmentNebula);
            for (int f = 0; f < (Main.expertMode ? 100 : 50); f=f+1){
        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, types[Main.rand.Next(0,types.Count)]);
        }

        if (!OphioidWorld.downedOphiopede2){
        if (Main.netMode!=1)
        Idglib.Chat("The "+(WorldGen.crimson ? "Crimson" : "Corruption")+"'s abomination is no longer felt, Ophioid is defeated",100,225,100);
        OphioidWorld.downedOphiopede2=true;
        }
        }

        public override string Texture
        {
            get { return("Ophioid/ophioflyfinalform");}
        }

        public override void FindFrame(int frameHeight)
        {
        npc.frame.Y=((int)(npc.localAI[0]/30));
        npc.frame.Y%=2;
        npc.frame.Y*=frameHeight;
        }

        public override void AI()
        {
        npc.localAI[0]+=1;
        glowred=false;

        float anglgo=(float)Math.Pow(Math.Abs(npc.velocity.X*0.07f),0.8);
        npc.rotation=npc.rotation.AngleLerp(anglgo*(float)-npc.direction,Math.Abs(npc.velocity.X*0.015f));

            for (int k = oldPos.Length - 1; k > 0; k--)
            {
                oldPos[k] = oldPos[k - 1];
                oldrot[k] = oldrot[k - 1];
            }
            oldPos[0] = npc.Center;
            oldrot[0] = npc.rotation;

        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
        npc.TargetClosest(true);
        noplayer=true;
        if (npc.velocity.Y>0f)
        npc.velocity.Y/=1.15f;
        npc.velocity.Y-=0.15f;
        npc.velocity.X/=1.15f;
        return;
        }else{noplayer=false;}




        ply = Main.player[npc.target];

        npc.ai[0]+=1;

        if (npc.ai[1]==0)
        ichorbeam();
        if (npc.ai[1]==1)
        sludgefield();
        if (npc.ai[1]==2)
        feralbite();
        if (npc.ai[1]==3)
        bodyslam();
        if (npc.ai[1]==4)
        sporeclouds();
        if (npc.ai[1]==5)
        makeminions();

    if (npc.ai[0]<300 && npc.ai[1]!=2 && npc.ai[1]!=5){
        Vector2 plyloc=ply.Center-new Vector2(0,-16);
        Vector2 plydist=(plyloc-npc.Center);
        Vector2 plynormal=plydist; plynormal.Normalize();

        npc.velocity/=1.01f;
        npc.velocity+=plynormal*0.25f;

        if (npc.ai[0]%150==0 && npc.ai[0]>0){
        npc.velocity+=plynormal*20f;
        }
        if (npc.ai[0]>80 && npc.ai[0]<220 && npc.ai[0]%5==0){
        List<Projectile> itz=Idglib.Shattershots(npc.Center,npc.Center,new Vector2(0,16),ProjectileID.HornetStinger,30,15f,180,2,false,npc.ai[0]/35f,false,240);
        }

        npc.direction=(plydist.X<0f).ToDirectionInt();
        if (npc.velocity.Length()>16f){npc.velocity.Normalize(); npc.velocity*=16f;}
    }



}

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            if (npc.ai[1]==2)
            {
                player.AddBuff(BuffID.Rabies, 60*15, true);
            }
        }


        private SpriteEffects facing
        {
            get
            {
            return (npc.direction>0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            }


        }

            public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D texture = Main.npcTexture[mod.NPCType(this.GetType().Name)];
            Vector2 origin = new Vector2(texture.Width,texture.Height/5)/2f;
            if (glowred)
            lightColor=Color.Red;
            for (int k = oldPos.Length - 1; k >= 0; k -= 2)
            {
                float alpha = 1f - (float)(k + 1) / (float)(oldPos.Length + 2);
                spriteBatch.Draw(texture, oldPos[k] - Main.screenPosition,new Rectangle(0,npc.frame.Y,texture.Width,(texture.Height-1)/5), lightColor * alpha, oldrot[k], origin,new Vector2(1f,1f), facing, 0f);
            }

            spriteBatch.Draw(texture, npc.Center - Main.screenPosition,new Rectangle(0,npc.frame.Y,texture.Width,(texture.Height)/5), lightColor, npc.rotation, origin,new Vector2(1f,1f), facing, 0f);
            return false;
        }


            public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.npcTexture[mod.NPCType(this.GetType().Name)];
            //Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

            Vector2 origin = new Vector2(texture.Width,texture.Height/5)/2f;

            int wingframe=(int)(npc.localAI[0]/5f);
            wingframe%=3;wingframe+=2;
            spriteBatch.Draw(texture, npc.Center - Main.screenPosition,new Rectangle(0,wingframe*((texture.Height)/5),texture.Width,(texture.Height)/5), lightColor*0.75f, npc.rotation, origin,new Vector2(1f,1f), facing, 0f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life<1)
            {
            for(int i=1;i<8;i+=1){
            Vector2 Vect=new Vector2(Main.rand.Next(-2,2),Main.rand.Next(-2,2)); Vect.Normalize();
            Gore.NewGore(npc.Center, Vect, mod.GetGoreSlot("Gores/ophiofly_gore_"+i), 1f);
            }}
        }

}

        [AutoloadBossHead]
    public class Ophiocoon : ModNPC
    {

        Player ply;
        bool no2ndphase=false;
        float whichway=0f;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override bool CheckActive()
        {
            if (npc.life>0)
            no2ndphase=true;
            return true;
        }

        public override string Texture
        {
            get { return("Ophioid/cocoon");}
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiocoon");
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life<1)
            {
            Main.PlaySound(SoundID.NPCDeath1, npc.position);
            for (int a = 0; a < 500; a++){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                Vector2 vecr=randomcircle;

                    int num622 = Dust.NewDust(npc.position, npc.width, npc.height, 184, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[num622].velocity = randomcircle*new Vector2((float)Main.rand.Next(-1000,1000)/100f,(float)Main.rand.Next(-1000,1000)/100f);

                    Main.dust[num622].noGravity = true;
                    Main.dust[num622].color=Main.hslToRgb((float)(Main.GlobalTime/5)%1, 0.9f, 1f);
                    Main.dust[num622].color.A=10;
                    Main.dust[num622].alpha = 100;;
                }
            for(int i=1;i<6;i+=1){
            Vector2 Vect=new Vector2(Main.rand.Next(-2,2),Main.rand.Next(-2,2)); Vect.Normalize();
            Gore.NewGore(npc.Center, Vect, mod.GetGoreSlot("Gores/cocoon_gore_"+i), 1f);
            }}
        }

        public void falldown()
        {
        npc.velocity.Y+=0.25f;

            bool wallblocking = false;
            int y_top_edge = (int)(npc.position.Y - 16f) / 16;
            int y_bottom_edge = (int)(npc.position.Y + (float)npc.height + 16f) / 16;
            int x_left_edge = (int)(npc.position.X - 16f) / 16;
            int x_right_edge = (int)(npc.position.X + (float)npc.width + 16f) / 16;

            for (int x = x_left_edge; x <= x_right_edge; x++)
            {
                for (int y = y_top_edge; y <= y_bottom_edge; y++)
                {
                    if (Main.tile[x, y].nactive() && Main.tileSolid[(int)Main.tile[x, y].type] && !Main.tileSolidTop[(int)Main.tile[x, y].type])
                    {
                        wallblocking = true;
                        break;
                    }
                }
                if (wallblocking) break;
            }

            if (wallblocking==true)
            {

        npc.life = 0;
        npc.HitEffect(0, 10.0);

        if (Main.netMode!=1){
        Idglib.Chat("The Ophifly hatches from the Cocoon!",100,225,100);

            int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
            int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
            int num663 = mod.NPCType("Ophiofly");

            int num664 = NPC.NewNPC(x, y, num663);
                    if (Main.netMode == 2 && num664 < 200)
                    {
                       NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }

            }

        npc.active = false;
        }

        }

        public override void SetDefaults()
        {
            npc.width = 96;
            npc.height = 256;
            npc.damage = 0;
            npc.defense = 75;
            npc.lifeMax = 2000000;
            npc.knockBackResist=0f;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            npc.boss=true;
            npc.dontTakeDamage=true;
            npc.immortal=true;
            aiType = -1;
            animationType = -1;
        }

        public override void AI()
        {
        npc.velocity/=1.05f;
        npc.ai[0]+=1;
        //ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
        if (npc.ai[0]==25){
            if (Main.netMode != 1)
            {
            for(int i=0;i<7;i+=1)
            {
            int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
            int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
            int num663 = mod.NPCType("FlyMinionCacoon");

            //HalfVector2 half=new HalfVector2(Main.rand.Next(-120,120),Main.rand.Next(-380,240));

            int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
            Main.npc[num664].ai[0]=Main.rand.Next(0,3000);
            Main.npc[num664].ai[1]=npc.whoAmI;
            //Main.npc[num664].life = (int)(npc.life*0.007);
            //Main.npc[num664].lifeMax = (int)(npc.lifeMax*0.007);
            Main.npc[num664].netUpdate=true;
                        if (Main.netMode == 2 && num664 < 200)
                        {
                            NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                        }
            }
        }
        npc.life=1;
        npc.lifeMax=1;
        }

        if (npc.ai[1]<0){
        falldown();
        return;
        }

        if (npc.ai[0]>30){
        npc.ai[1]-=3;
        if (npc.ai[1]>10)
        npc.ai[1]=10;
        }

        int minTilePosX = (int)(npc.Center.X / 16.0) - 1;
        int minTilePosY = (int)((npc.Center.Y+32f) / 16.0) - 1;

        if (whichway==0f){
        whichway=minTilePosX>(int)(Main.maxTilesX/2) ? -1f : 1f;
        }


        if (minTilePosX<32 || minTilePosX > Main.maxTilesX-32){
        npc.active=false;
        return;
        }

        npc.ai[2]-=1;

        if (npc.ai[2]%10==0 && npc.ai[2]>0)
        {
            if (Main.netMode != 1)
            {
            int him=NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), mod.NPCType("EvenMoreVileSpit"), 0, 0f, 0f, 0f, 0f, 255);
            //NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.ProjectileType("EvenMoreVileSpit"));
            Main.npc[him].velocity=new Vector2(Main.rand.Next(5,18)*(Main.rand.Next(0,2)==0 ? 1 : -1), Main.rand.Next(-4,4));
            Main.npc[him].timeLeft = 200;
            Main.npc[him].damage=100;
            Main.npc[him].netUpdate=true;
            IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);


            }
        }

        if (npc.ai[0]>700){
        npc.ai[0]=50;
        npc.ai[2]=150;

            if (Main.netMode != 1 && NPC.CountNPCS(mod.NPCType("FlyMinion"))<5)
            {
            int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
            int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
            int num663 = mod.NPCType("FlyMinion");

            int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
            Main.npc[num664].ai[1]=npc.whoAmI;
                            if (Main.netMode == 2 && num664 < 200)
                            {
                                NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                            }
            }


        }


            int whereisity;
            whereisity=Idglib.RaycastDown(minTilePosX+1,Math.Max(minTilePosY,0));

            bool wallblocking = false;
            int y_top_edge = (int)(npc.position.Y - 16f) / 16;
            int y_bottom_edge = (int)(npc.position.Y + (float)npc.height + 16f) / 16;
            int x_left_edge = (int)(npc.position.X - 16f) / 16;
            int x_right_edge = (int)(npc.position.X + (float)npc.width + 16f) / 16;

            for (int x = x_left_edge; x <= x_right_edge; x++)
            {
                for (int y = y_top_edge; y <= y_bottom_edge; y++)
                {
                    if (Main.tile[x, y].nactive() && Main.tileSolid[(int)Main.tile[x, y].type] && !Main.tileSolidTop[(int)Main.tile[x, y].type])
                    {
                        wallblocking = true;
                        break;
                    }
                }
                if (wallblocking) break;
            }

        if (wallblocking){
        npc.velocity-=new Vector2(0,0.3f);
        }else{
        if (whereisity-minTilePosY>80)
        npc.velocity+=new Vector2(0,1.6f);
        npc.velocity+=new Vector2(0.4f*whichway,0);
        }
        if (npc.velocity.Length()>12f){
        npc.velocity.Normalize(); npc.velocity*=12;
        }





        npc.rotation=(npc.velocity.X/15f);

        }






}

    public class FlyMinion : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 24;
            npc.height = 24;
            npc.damage = 75;
            npc.defense = 15;
            npc.lifeMax = 2000;
            npc.value = 0f;
            npc.knockBackResist=0.5f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            aiType = -1;
            animationType = -1;
        }

        public override string Texture
        {
            get { return("Ophioid/baby_ophiofly_frames");}
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophioid Fly Minion");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void FindFrame(int frameHeight)
        {
        npc.frame.Y=((int)(Math.Abs(npc.ai[0]/4)));
        npc.frame.Y%=4;
        npc.frame.Y*=frameHeight;
        }

        public override void AI()
        {

        npc.ai[0]+=1;
        npc.velocity/=1.015f;
        NPC Master = Main.npc[(int)npc.ai[1]];
        if (!Master.active || npc.ai[1]<1)
        npc.active=false;

        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
        npc.TargetClosest(true);
        }
        Player ply = Main.player[npc.target];

        if (Main.rand.Next(0,800)<1 && npc.ai[0]>300 && Main.netMode!=1)
        {
        npc.ai[0]=(int)(-Main.rand.Next(500,1200));
        npc.netUpdate=true;
        }

        Vector2 masterloc=Master.Center-new Vector2(0,-96);
        if (npc.ai[0]<0 && !ply.dead)
        masterloc=ply.Center-new Vector2(0,0);
        Vector2 masterdist=(masterloc-npc.Center);
        Vector2 masternormal=masterdist; masternormal.Normalize();

        npc.velocity+=Vector2Extension.Rotate(masternormal,(float)Math.Sin(npc.ai[0]*0.02f)*0.8f)*0.4f;
        npc.spriteDirection=(npc.velocity.X>0f).ToDirectionInt();
        if (npc.velocity.Length()>14f){npc.velocity.Normalize(); npc.velocity*=14f;}

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
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 24;
            npc.height = 24;
            npc.damage = 100;
            npc.defense = 15;
            npc.lifeMax = 10000;
            npc.value = 0f;
            npc.knockBackResist=0.75f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            aiType = -1;
            animationType = -1;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override bool CheckActive()
        {
        NPC Master = Main.npc[(int)npc.ai[1]];
        if (!Master.active || npc.ai[1]<1 || Master.type!=mod.NPCType("Ophiocoon"))
            return true;
            else
            return false;
        }

        public override string Texture
        {
            get { return("Ophioid/baby_ophiofly_frames");}
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coocoon Carriers");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void AI()
        {
        npc.ai[0]+=1;
        npc.velocity/=1.025f;
        NPC Master = Main.npc[(int)npc.ai[1]];
        if (!Master.active || npc.ai[1]<1 || Master.type!=mod.NPCType("Ophiocoon")){
        nomaster=true;
        npc.boss=false;
        }

        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
        npc.TargetClosest(true);
        }
        Player ply = Main.player[npc.target];
        if (Master.ai[1]>-1 && Master.active && !nomaster)
        Master.ai[1]+=1;

        Vector2 gohere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(npc.ai[2]) }.ToVector2();
        Vector2 masterloc=new Vector2(0,-96)+gohere;
        if (!nomaster)
        masterloc+=Master.Center;
        if ((Master.ai[1]<0 && Master.active) || nomaster)
        masterloc=npc.Center+new Vector2(0,-96)+gohere;
        Vector2 masterdist=(masterloc-npc.Center);
        Vector2 masternormal=masterdist; masternormal.Normalize();

        if (!nomaster && masterdist.Length()>128f){
        Master.velocity-=masternormal*0.15f;
        npc.velocity+=masternormal*0.25f;
        }

        if (Main.rand.Next(0,200)<1 && npc.ai[2]==0 && Main.netMode!=1){
        HalfVector2 half=new HalfVector2(Main.rand.Next(-120,120),Main.rand.Next(-280,20));npc.ai[2]=ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
        npc.netUpdate=true;
        }

        if (npc.ai[0]%(Main.expertMode ? 450 : 600)==0)
        {

            if (Main.netMode != 1)
            {
            int him=NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), NPCID.VileSpit, 0, 0f, 0f, 0f, 0f, 255);
            //NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.ProjectileType("EvenMoreVileSpit"));
            Main.npc[him].velocity=new Vector2(Main.rand.Next(5,18)*(Main.rand.Next(0,2)==0 ? 1 : -1), Main.rand.Next(-4,4));
            Main.npc[him].timeLeft = 200;
            Main.npc[him].damage=100;
            Main.npc[him].netUpdate=true;
            IdgNPC.AddOnHitBuff(him,BuffID.Stinky,60*15);

            }

        }

        if (npc.ai[0]%7==0 && npc.ai[0]%730>700 && Main.expertMode)
        {
                    npc.TargetClosest(true);
                    npc.netUpdate=true;
                    Player target=Main.player[npc.target];
                    List<Projectile> itz2=Idglib.Shattershots(npc.Center,target.position,new Vector2(target.width/2,target.height/2),ProjectileID.PoisonSeedPlantera,20,10f,0,1,true,0f,false,300);
                    Main.PlaySound(SoundID.Item42,npc.Center);
        }

        npc.velocity+=Vector2Extension.Rotate(masternormal,(float)Math.Sin(npc.ai[0]*0.045f)*0.4f)*0.4f;
        npc.spriteDirection=(npc.velocity.X>0f).ToDirectionInt();
        if (npc.velocity.Length()>14f){npc.velocity.Normalize(); npc.velocity*=14f;}

        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
        if (!nomaster)
        Idglib.DrawTether("Ophioid/tether",Main.npc[(int)npc.ai[1]].Center,npc.Center);
        return true;
        }

    }


    public class Ophiobeamichor : ProjectileLaserBase
    {

        public float MoveDistance = 0f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 90;
            projectile.damage = 15;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ichorbeam");
        }

        public override string Texture
        {
            get { return "Ophioid/beam_1"; }
        }

        public override void AI()
        {
        //projectile.velocity+=new Vector2(projectile.velocity.X>0 ? 0.04f : -0.04f,0f);
        projectile.localAI[0]+=0.2f;
        base.AI();
        }

        public override void MoreAI(Vector2 dustspot)
        {

        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Ichor, 60*10, true);
            player.AddBuff(BuffID.Darkness, 60*15, true);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
        string[] lasers={"Ophioid/frame_1","Ophioid/frame2","Ophioid/frame_3"};
        Vector2 scale = new Vector2(MathHelper.Clamp((float)projectile.timeLeft/20,0f,1f),1f);
        Idglib.DrawTether(lasers[(int)projectile.localAI[0]%3],hitspot,projectile.Center,projectile.Opacity,scale.X,scale.Y);
        Texture2D captex=ModContent.GetTexture("Ophioid/ichor_cap");
        Main.spriteBatch.Draw(captex, projectile.Center - Main.screenPosition, null, lightColor*projectile.Opacity, (projectile.velocity).ToRotation()-((float)Math.PI/2f), new Vector2(captex.Width/2,captex.Height/2), new Vector2(scale.X,scale.Y), SpriteEffects.None, 0.0f);
        Main.spriteBatch.Draw(captex, hitspot - Main.screenPosition, null, lightColor*projectile.Opacity, projectile.velocity.ToRotation()+((float)Math.PI/2f), new Vector2(captex.Width/2,captex.Height/2), new Vector2(scale.X,scale.Y), SpriteEffects.None, 0.0f);

        return false;
        }

    }


    public class Ophiobeam : ProjectileLaserBase
    {

        public float MoveDistance = 0f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 90;
            projectile.damage = 40;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiobeam");
        }

        public override string Texture
        {
            get { return "Ophioid/beam_1"; }
        }

        public override void AI()
        {
        projectile.velocity+=new Vector2(projectile.velocity.X>0 ? 0.04f : -0.04f,0f);
        projectile.localAI[0]+=0.2f;
        projectile.ai[1]+=1;
        base.AI();
        }

        public override void MoreAI(Vector2 dustspot)
        {

        for (int num315 = 1; num315 < 8; num315 = num315 + 1)
            {
                if (Main.rand.Next(0,100)<25){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                int num316 = Dust.NewDust(new Vector2(projectile.position.X-1, projectile.position.Y), projectile.width, projectile.height, 184, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 2.00f);
                Main.dust[num316].noGravity = true;
                Dust dust3 = Main.dust[num316];
                dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat());
                dust3.velocity.Normalize();
                dust3.velocity+=(projectile.velocity*1f);
            }}

        for (int num315 = 1; num315 < 2; num315 = num315 + 1)
            {
                if (Main.rand.Next(0,100)<25){
                Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
                int num316 = Dust.NewDust(new Vector2(projectile.position.X-1, projectile.position.Y), projectile.width, projectile.height, 184, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 3.00f);
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
                int num316 = Dust.NewDust(new Vector2(dustspot.X-1, dustspot.Y)-new Vector2(projectile.width/2,projectile.height/2), projectile.width, projectile.height, 184, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 2.00f);
                Main.dust[num316].noGravity = true;
                Dust dust3 = Main.dust[num316];
                dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat());
            }}

            if (projectile.ai[1]%8==0){
            int num54 = Projectile.NewProjectile(dustspot.X,dustspot.Y, Main.rand.Next(-2,2), 3, mod.ProjectileType("PoisonCloud"), 1, 0f,0);
                Main.projectile[num54].damage=(int)(20);
                Main.projectile[num54].timeLeft=Main.expertMode ? 60*20 : 250;
                Main.projectile[num54].velocity=new Vector2(0,0);
                Main.projectile[num54].netUpdate=true;
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
        string[] lasers={"Ophioid/beam_1","Ophioid/beam_2","Ophioid/beam_3"};
        Vector2 scale = new Vector2(MathHelper.Clamp((float)projectile.timeLeft/20,0f,1f),1f);
        Idglib.DrawTether(lasers[(int)projectile.localAI[0]%3],hitspot,projectile.Center,projectile.Opacity,scale.X,scale.Y);
        Texture2D captex= ModContent.GetTexture("Ophioid/end_and_start");
        Main.spriteBatch.Draw(captex, projectile.Center - Main.screenPosition, null, lightColor*projectile.Opacity, (projectile.velocity).ToRotation()-((float)Math.PI/2f), new Vector2(captex.Width/2,captex.Height/2), new Vector2(scale.X,scale.Y), SpriteEffects.None, 0.0f);
        Main.spriteBatch.Draw(captex, hitspot - Main.screenPosition, null, lightColor*projectile.Opacity, projectile.velocity.ToRotation()+((float)Math.PI/2f), new Vector2(captex.Width/2,captex.Height/2), new Vector2(scale.X,scale.Y), SpriteEffects.None, 0.0f);

        return false;
        }

    }



}
