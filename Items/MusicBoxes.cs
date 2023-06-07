using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace OphioidMod.Items
{
	public class MusicBoxMetamorphosis : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Music Box (Ophiopede)");
			// Tooltip.SetDefault("Badassbunnyz - Metamorphosis");
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Centipede_Mod_-_Metamorphosis"), Item.type, ModContent.TileType<Tiles.MusicBoxMetamorphosis>());
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.MusicBoxMetamorphosis>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Orange;
			Item.value = 100000;
			Item.accessory = true;
		}
	}
	public class MusicBoxTheFly : MusicBoxMetamorphosis
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Music Box (Ophiofly)");
			// Tooltip.SetDefault("Badassbunnyz - The Fly");
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Centipede_Mod_-_The_Fly"), Item.type, ModContent.TileType<Tiles.MusicBoxTheFly>());
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<Tiles.MusicBoxTheFly>();
		}
	}
}
