using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using OphioidMod.Buffs;
using OphioidMod.Projectiles;
using OphioidMod.Tiles;
using System.Collections.Generic;
using OphioidMod.NPCs;

namespace OphioidMod.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class OphiopedeMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede Mask");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.sellPrice(0, 0, 75, 0);
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
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<BabyFlyPet>();
            Item.buffType = ModContent.BuffType<BabyOphioflyBuff>();
            Item.sellPrice(0, 1, 0, 0);
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
            DisplayName.SetDefault("Ophiopede Trophy");
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
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("Right click to open");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.expert = true;
            Item.rare = -12;
        }


        public override int BossBagNPC
        {
            get
            {
                return ModContent.NPCType<Ophiofly>();
            }
        }


        public override bool CanRightClick()
        {
            return true;
        }
        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor(player.GetSource_OpenItem(Type));
            if (Main.rand.Next(0, 100) < 31)
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Ophiopedetrophyitem>());
            if (Main.rand.Next(0, 100) < 31)
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<OphiopedeMask>());

            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<SporeInfestedEgg>());

            List<int> types = new List<int>();
            types.Insert(types.Count, ItemID.SoulofMight); types.Insert(types.Count, ItemID.SoulofFright); types.Insert(types.Count, ItemID.SoulofSight); types.Insert(types.Count, ItemID.SoulofNight); types.Insert(types.Count, ItemID.SoulofLight); types.Insert(types.Count, ItemID.SoulofNight); types.Insert(types.Count, ItemID.SoulofLight);
            for (int f = 0; f < (Main.expertMode ? 200 : 120); f = f + 1)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), types[Main.rand.Next(0, types.Count)]);
            }

            types = new List<int>();
            types.Insert(types.Count, ItemID.SoulofFlight); types.Insert(types.Count, ItemID.SoulofFlight); types.Insert(types.Count, ItemID.FragmentStardust); types.Insert(types.Count, ItemID.FragmentSolar); types.Insert(types.Count, ItemID.FragmentVortex); types.Insert(types.Count, ItemID.FragmentNebula);
            for (int f = 0; f < (Main.expertMode ? 100 : 50); f = f + 1)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), types[Main.rand.Next(0, types.Count)]);
            }
        }
    }
}