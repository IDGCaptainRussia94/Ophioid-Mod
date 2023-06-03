using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OphioidMod.Projectiles
{
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
            // DisplayName.SetDefault("Ophiobeam");
        }

        public override string Texture
        {
            get { return "OphioidMod/Projectiles/beam_1"; }
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

            for (int num315 = 1; num315 < 8; num315++)
            {
                if (Main.rand.Next(0, 100) < 25)
                {
                    Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                    int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ScourgeOfTheCorruptor, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 2.00f);
                    Main.dust[num316].noGravity = true;
                    Dust dust3 = Main.dust[num316];
                    dust3.velocity = (randomcircle * 2.5f * Main.rand.NextFloat());
                    dust3.velocity.Normalize();
                    dust3.velocity += (Projectile.velocity * 1f);
                }
            }

            for (int num315 = 1; num315 < 2; num315++)
            {
                if (Main.rand.Next(0, 100) < 25)
                {
                    Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                    int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ScourgeOfTheCorruptor, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 3.00f);
                    Main.dust[num316].noGravity = true;
                    Dust dust3 = Main.dust[num316];
                    dust3.velocity = (randomcircle * 2.5f * Main.rand.NextFloat());
                    dust3.velocity.Normalize();
                    dust3.velocity *= (0.4f);
                }
            }

            for (int num315 = 1; num315 < 8; num315++)
            {
                if (Main.rand.Next(0, 100) < 25)
                {
                    Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                    int num316 = Dust.NewDust(new Vector2(dustspot.X - 1, dustspot.Y) - new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.width, Projectile.height, DustID.ScourgeOfTheCorruptor, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 2.00f);
                    Main.dust[num316].noGravity = true;
                    Dust dust3 = Main.dust[num316];
                    dust3.velocity = (randomcircle * 2.5f * Main.rand.NextFloat());
                }
            }

            if (Projectile.ai[1] % 8 == 0)
            {
                int num54 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), dustspot.X, dustspot.Y, Main.rand.Next(-2, 2), 3, ModContent.ProjectileType<PoisonCloud>(), 1, 0f, 0);
                Main.projectile[num54].damage = (int)(20);
                Main.projectile[num54].timeLeft = Main.expertMode ? 60 * 20 : 250;
                Main.projectile[num54].velocity = new Vector2(0, 0);
                Main.projectile[num54].netUpdate = true;
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            string[] lasers;
            lasers = GetType() == typeof(Ophiobeamichor) ? new string[] { "OphioidMod/Projectiles/frame_1", "OphioidMod/Projectiles/frame2", "OphioidMod/Projectiles/frame_3" } : new string[] { "OphioidMod/Projectiles/beam_1", "OphioidMod/Projectiles/beam_2", "OphioidMod/Projectiles/beam_3" };
            Vector2 scale = new Vector2(MathHelper.Clamp((float)Projectile.timeLeft / 20, 0f, 1f), 1f);
            IDGHelper.DrawTether(lasers[(int)Projectile.localAI[0] % 3], Hitspot, Projectile.Center, Main.screenPosition, Projectile.Opacity, scale.X, scale.Y, Color.White);
            Texture2D captex = ModContent.Request<Texture2D>(GetType() == typeof(Ophiobeamichor) ? "OphioidMod/Projectiles/ichor_cap" : "OphioidMod/Projectiles/end_and_start").Value;
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
            // DisplayName.SetDefault("Ichorbeam");
        }

        public override string Texture
        {
            get { return "OphioidMod/Projectiles/beam_1"; }
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

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.Ichor, 60 * 10, true);
			target.AddBuff(BuffID.Darkness, 60 * 15, true);
        }
    }
}