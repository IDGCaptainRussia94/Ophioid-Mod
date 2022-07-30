using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using OphioidMod.NPCs;
using Terraria.GameContent.Creative;

namespace OphioidMod.Items
{
    public class Deadfungusbug : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dead Fungusbug");
            Tooltip.SetDefault("'A putrid stench comes from the thing you just made, it might attract something...' \nSummons Ophiopede in a Corruption world");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss && !WorldGen.crimson;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<OphiopedeHead>());
            SoundEngine.PlaySound(SoundID.Roar, player.position);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofNight, 1)
                .AddIngredient(ItemID.SoulofLight, 1)
                .AddIngredient(ItemID.SoulofFright, 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient(ItemID.CursedFlame, 3)
                .AddIngredient(ItemID.RottenChunk, 3)
                .AddTile(TileID.MythrilAnvil).Register();
        }
    }
    public class Infestedcompost : Deadfungusbug
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infested Compost");
            Tooltip.SetDefault("'An amalgamation of organic vileness \nSummons Ophiopede?");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss;
        }

        public override bool? UseItem(Player player)
        {
            //if (!OphioidWorld.downedOphiopede2 && Main.netMode!=1)
            IDGHelper.Chat("The air becomes stale and moist around you.", 100, 225, 100);


            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<OphiopedeHead2>());
            SoundEngine.PlaySound(SoundID.Roar, player.position);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeetleHusk, 4)
                .AddIngredient(ModContent.ItemType<Deadfungusbug>())
                .AddIngredient(ModContent.ItemType<Livingcarrion>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class Livingcarrion : Deadfungusbug
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Carrion");
            Tooltip.SetDefault("'A putrid stench comes from the thing you just made, it might attract something...' \nSummons Ophiopede in a Crimson world");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss && WorldGen.crimson;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofNight, 1)
                .AddIngredient(ItemID.SoulofLight, 1)
                .AddIngredient(ItemID.SoulofFright, 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient(ItemID.Ichor, 3)
                .AddIngredient(ItemID.Vertebrae, 3)
                .AddTile(TileID.MythrilAnvil).Register();
        }
    }
}