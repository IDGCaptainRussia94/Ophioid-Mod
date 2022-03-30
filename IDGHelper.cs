using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Chat;

namespace OphioidMod
{
    class IDGHelper
    {
        public Color coloroverride = default;
        public static void DrawTether(string Tex, Vector2 Start, Vector2 End, Vector2 Screen, float Alpha = 1f, float scaleX = 1f, float scaleY = 1f, Color coloroverride = default)
        {
            DrawTether(ModContent.Request<Texture2D>(Tex).Value, Start, End, Screen, Alpha = 1f, scaleX, scaleY, coloroverride);
        }

        public static void DrawTether(Texture2D Tex, Vector2 Start, Vector2 End, Vector2 Screen, float Alpha = 1f, float scaleX = 1f, float scaleY = 1f, Color coloroverride = default)
        {

            Texture2D texture = Tex;
            if (Tex == null)
                return;

            Vector2 position = Start;
            Vector2 mountedCenter = End;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            float num1 = (float)(texture.Height * (scaleY));
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            Vector2 vector2_4 = mountedCenter - position;
            float keepgoing = vector2_4.Length();
            Vector2 vector2t = vector2_4;
            vector2t.Normalize();
            position -= vector2t * (num1 * 0.5f);
            int countup = 0;

            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if (keepgoing <= -1)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    keepgoing -= num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = new Color(color2.R, color2.G, color2.B);
                    //color2 = npc.GetAlpha(color2);
                    if (coloroverride != default)
                        color2 = coloroverride;
                    Main.spriteBatch.Draw(texture, position - Screen, new Rectangle(0, 0, texture.Width, (int)Math.Min(texture.Height, texture.Height + keepgoing)), color2 * (Alpha), rotation, origin, new Vector2(scaleX, scaleY), SpriteEffects.None, 0.0f);
                }
            }

        }

        public static List<Projectile> Shattershots(IEntitySource source,Vector2 here, Vector2 there, Vector2 widthheight, int type, int damage, float Speed, float spread, int count, bool centershot, float globalangularoffset, bool tilecollidez, int timeleft)
        {
            //if (Main.netMode!=1){
            List<Projectile> returns = new List<Projectile>();
            Vector2 vector8 = new Vector2(here.X + (0), here.Y + (0));
            float rotation = (float)Math.Atan2(vector8.Y - (there.Y + (widthheight.X * 0.5f)), vector8.X - (there.X + (widthheight.Y * 0.5f)));
            spread = spread * (0.0174f);
            float baseSpeed = (float)Math.Sqrt((float)((Math.Cos(rotation) * Speed) * -1) * (float)((Math.Cos(rotation) * Speed) * -1) + (float)((Math.Sin(rotation) * Speed) * -1) * (float)((Math.Sin(rotation) * Speed) * -1));
            double startAngle = Math.Atan2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
            double deltaAngle = spread / count;
            double offsetAngle;
            int i;
            for (i = 0; i < count; i++)
            {
                offsetAngle = (startAngle + globalangularoffset) + deltaAngle * i;
                double offsetAngle2 = (startAngle + globalangularoffset) - (deltaAngle * i);
                if (centershot == true || i > 0)
                {
                    int proj = Projectile.NewProjectile(source, vector8.X, vector8.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), type, damage, Speed, 0);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].tileCollide = tilecollidez;
                    Main.projectile[proj].timeLeft = timeleft;
                    Main.projectile[proj].netUpdate = true;
                    returns.Insert(returns.Count, Main.projectile[proj]);
                }
                if (i > 0)
                {
                    int proj2 = Projectile.NewProjectile(source, vector8.X, vector8.Y, baseSpeed * (float)Math.Sin(offsetAngle2), baseSpeed * (float)Math.Cos(offsetAngle2), type, damage, Speed, 0);
                    Main.projectile[proj2].friendly = false;
                    Main.projectile[proj2].hostile = true;
                    Main.projectile[proj2].tileCollide = tilecollidez;
                    Main.projectile[proj2].timeLeft = timeleft;
                    Main.projectile[proj2].netUpdate = true;
                    returns.Insert(returns.Count, Main.projectile[proj2]);
                }
            }


            return returns;
        }

        public static void Chat(string message, byte color1, byte color2, byte color3)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                string text = message;
                Main.NewText(text, color1, color3, color3);
            }
            else
            {
                NetworkText text = NetworkText.FromLiteral(message);
                ChatHelper.BroadcastChatMessage(text, new Color(color1, color2, color3));
            }
        }

        public static int RaycastDown(int x, int y)
        {
            while (!((Main.tile[x, y] != null && Main.tile[x, y].NactiveButWithABetterName() && (Main.tileSolid[(int)Main.tile[x, y].TileType] || Main.tileSolidTop[(int)Main.tile[x, y].TileType] && (int)Main.tile[x, y].TileFrameY == 0))))
            {
                y++;
            }
            return y;
        }

    }

    public class ProjectileLaserBase : ModProjectile
    {

        public string laserTexture = "Terraria/Images/Projectile_" + ProjectileID.RocketII;
        public string laserTextureEnd = "";
        public string laserTextureBeginning = "";
        public float MoveDistance = 0f;
        public float MaxDistance = 2200f;
        public float CollisionDistance = 0f;
        public float Distance
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        public Vector2 Hitspot
        {
            get { return Projectile.Center + (Projectile.velocity * Distance); }
        }

        public override string Texture
        {
            get { return "Terraria/Images/Projectile_" + ProjectileID.RocketII; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("This is a base, you shouldn't see this!");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = true;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public virtual void MoreAI(Vector2 dustspot)
        {

        }

        public float movementFactor
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {


            Vector2 start = Projectile.Center;
            if (Projectile.tileCollide)
            {
                for (Distance = MoveDistance; Distance <= MaxDistance; Distance += 5f)
                {
                    start = Projectile.Center + Projectile.velocity * Distance;

                    Lighting.AddLight(start,Color.White.ToVector3()*0.25f);

                    {
                        if ((!Collision.CanHit(Projectile.Center, 1, 1, start, 1, 1)) && Distance > CollisionDistance)
                        {
                            Distance -= 5f;
                            break;
                        }
                    }
                }
            }
            else
            {
                Distance = MaxDistance;
            }

            Projectile.position -= Projectile.velocity;

            MoreAI(Hitspot);


        }

        public override bool PreDraw(ref Color lightColor)
        {
            IDGHelper.DrawTether(laserTexture, Hitspot, Projectile.Center, Main.screenPosition, Projectile.Opacity);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            //if (AtMaxCharge)
            //{
            Player player = Main.player[Projectile.owner];
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Hitspot, 22, ref point);
            //}
            //return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

    }

}
