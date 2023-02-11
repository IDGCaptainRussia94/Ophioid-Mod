using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace OphioidMod.NPCs
{
    public class OphSporeCloud : ModNPC
    {
        int mytimeisover = 800;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ophioid Spore Cloud");
            Main.npcFrameCount[NPC.type] = 5;

            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new(0)
            {
                Hide = true // Hides this NPC from the bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
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
            NPC.knockBackResist = 0f;
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
            List<Projectile> projectile22 = IDGHelper.Shattershots(NPC.GetSource_FromThis(), NPC.Center, NPC.Center + NPC.velocity, new Vector2(0, 0), ProjectileID.SporeCloud, 50, NPC.velocity.Length() + 10f, 0, 1, true, (float)Main.rand.Next(-100, 100) * 0.002f, false, 240);
            IdgProjectile.Sync(projectile22[0].whoAmI);
            return true;
        }

        public override void AI()
        {
            mytimeisover -= 1;
            NPC.velocity /= 1.15f;
            NPC.velocity.Normalize();
            NPC.velocity *= 16f - (float)(mytimeisover * 0.015f);
            if (mytimeisover < 1)
                NPC.StrikeNPCNoInteraction(9999, 0f, NPC.direction, false, false, false);
        }
    }
}