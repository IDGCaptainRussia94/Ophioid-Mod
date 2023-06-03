using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OphioidMod.Projectiles
{
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
            get { return "Terraria/Images/Item_" + ItemID.Mushroom; }
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
            //int q = 0;
            for (int q = 0; q < 4; q++)
            {

                int dust = Dust.NewDust(Projectile.position - new Vector2(100, 0), 200, 12, DustID.GemEmerald, 0f, Projectile.velocity.Y * 0.4f, 100, Color.DarkGreen, 1.5f);
                Main.dust[dust].noGravity = true;
            }

            Rectangle rectangle1 = new Rectangle((int)Projectile.position.X - 100, (int)Projectile.position.Y - 25, 200, 50);
            //int maxDistance = 50;
            //bool playerCollision = false;
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
}