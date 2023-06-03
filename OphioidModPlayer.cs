using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OphioidMod
{
    public class OphioidPlayer : ModPlayer
    {

        public bool PetBuff = false;
        public bool PetBuff2 = false;

        public override void ResetEffects()
        {
            PetBuff = false;
            PetBuff2 = false;
        }

		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
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
}