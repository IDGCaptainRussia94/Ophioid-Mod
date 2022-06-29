using OphioidMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OphioidMod.Buffs
{
    public class BabyOphioflyBuff : ModBuff
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

            modPlayer.PetBuff = true;

            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<BabyFlyPet>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<BabyFlyPet>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
    public class BabyOphiopedeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Ophioid Larva");
            Description.SetDefault("Gross but, oddly cute");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            OphioidPlayer modPlayer = player.GetModPlayer<OphioidPlayer>();

            modPlayer.PetBuff2 = true;

            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<BabyOphiopedePet>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<BabyOphiopedePet>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}