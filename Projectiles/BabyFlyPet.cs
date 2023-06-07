using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OphioidMod.Projectiles
{
    public class BabyFlyPet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gross but cute Fly");
        }

        public override string Texture
        {
            get { return ("OphioidMod/Projectiles/baby_ophiofly_frames"); }
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyHornet);
            AIType = ProjectileID.BabyHornet;
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.hornet = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            OphioidPlayer modPlayer = player.GetModPlayer<OphioidPlayer>();
            if (player.dead)
            {
                modPlayer.PetBuff = false;
            }
            if (modPlayer.PetBuff)
            {
                Projectile.timeLeft = 2;
            }
        }
    }
}