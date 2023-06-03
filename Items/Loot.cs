using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using OphioidMod.Buffs;
using OphioidMod.Projectiles;
using OphioidMod.Tiles;
using OphioidMod.NPCs;
using Terraria.GameContent.ItemDropRules;

namespace OphioidMod.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class OphiopedeMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ophiopede Mask");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.vanity = true;
        }
    }

    public class SporeInfestedEgg : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spore Infested Egg");
            // Tooltip.SetDefault("'Looks like this egg didn't hatch yet to attack me...");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<BabyFlyPet>();
            Item.buffType = ModContent.BuffType<BabyOphioflyBuff>();
			Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
    public class Ophiopedetrophyitem : ModItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ophiopede Trophy");
            Item.ResearchUnlockCount = 1;
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
    public class TreasureBagOphioid : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Treasure Bag (Ophioid)");
            // Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            ItemID.Sets.BossBag[Type] = true; // This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
			Item.ResearchUnlockCount = 3;
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.expert = true;
            Item.rare = ItemRarityID.Expert;
        }

        //public override int BossBagNPC => ModContent.NPCType<Ophiofly>();

        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Ophiopedetrophyitem>(), 3));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<OphiopedeMask>(), 3));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SporeInfestedEgg>(), 1));

            // Expert Drop rates since the bag only drops in Expert Mode+
            itemLoot.Add(ItemDropRule.Common(ItemID.FragmentSolar, 1, 12, 22)); // Average of 17
            itemLoot.Add(ItemDropRule.Common(ItemID.FragmentVortex, 1, 12, 22));
            itemLoot.Add(ItemDropRule.Common(ItemID.FragmentNebula, 1, 12, 22));
            itemLoot.Add(ItemDropRule.Common(ItemID.FragmentStardust, 1, 12, 22));
            itemLoot.Add(ItemDropRule.Common(ItemID.SoulofLight, 1, 46, 66)); // Average of 56
            itemLoot.Add(ItemDropRule.Common(ItemID.SoulofNight, 1, 46, 66));
            itemLoot.Add(ItemDropRule.Common(ItemID.SoulofFlight, 1, 23, 43)); // Average of 33
            itemLoot.Add(ItemDropRule.Common(ItemID.SoulofSight, 1, 21, 35)); // Average of 28
            itemLoot.Add(ItemDropRule.Common(ItemID.SoulofMight, 1, 21, 35));
            itemLoot.Add(ItemDropRule.Common(ItemID.SoulofFright, 1, 21, 35));

            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<Ophiofly>()));
        }
    }
    public class OphioidLarva : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ophioid Larva");
            // Tooltip.SetDefault("'A little Ophiopede'");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.shoot = ModContent.ProjectileType<BabyOphiopedePet>();
            Item.buffType = ModContent.BuffType<BabyOphiopedeBuff>();
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}