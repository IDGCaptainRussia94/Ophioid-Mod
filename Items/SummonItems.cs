using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using OphioidMod.NPCs;

namespace OphioidMod.Items
{
    public class DeadFungusbug : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dead Fungusbug");
            // Tooltip.SetDefault("'A putrid stench comes from the thing you just made, it might attract something...' \nSummons Ophiopede in a Corruption world");
            Item.ResearchUnlockCount = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<LivingCarrion>();
		}
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss; // && !WorldGen.crimson;
        }

        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer)
			{
				// If the player using the item is the client
				// (explicitly excluded serverside here)
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = ModContent.NPCType<OphiopedeHead>();

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in OphiopedeHead
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
				}
			}

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
    public class InfestedCompost : DeadFungusbug
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Infested Compost");
            // Tooltip.SetDefault("'An amalgamation of organic vileness \nSummons Ophiopede?");
            Item.ResearchUnlockCount = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss;
        }

        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer)
			{
				// If the player using the item is the client
				// (explicitly excluded serverside here)
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				IDGHelper.Chat("The air becomes stale and moist around you.", 100, 225, 100);

				int type = ModContent.NPCType<OphiopedeHead2>();

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in OphiopedeHead2
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
				}
			}

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BeetleHusk, 4)
                .AddIngredient(ModContent.ItemType<DeadFungusbug>())
                .AddIngredient(ModContent.ItemType<LivingCarrion>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class LivingCarrion : DeadFungusbug
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Living Carrion");
            // Tooltip.SetDefault("'A putrid stench comes from the thing you just made, it might attract something...' \nSummons Ophiopede in a Crimson world");
            Item.ResearchUnlockCount = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<DeadFungusbug>();
		}

        public override bool CanUseItem(Player player)
        {
            return !OphioidWorld.OphioidBoss; // && WorldGen.crimson;
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