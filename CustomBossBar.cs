using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace OphioidMod
{
	// Shows basic boss bar code using a custom colored texture. It only does visual things, so for a more practical boss bar, see the other example (MinionBossBossBar)
	// To use this, in an NPCs SetDefaults, write:
	//  NPC.BossBar = ModContent.GetInstance<ExampleBossBar>();

	// Keep in mind that if the NPC has a boss head icon, it will automatically have the common boss health bar from vanilla. A ModBossBar is not mandatory for a boss.

	// You can make it so your NPC never shows a boss bar, such as Dungeon Guardian or Lunatic Cultist Clone:
	//  NPC.BossBar = Main.BigBossProgressBar.NeverValid;
	public class OphioidBossBar : ModBossBar
	{
		public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
		{
			return ModContent.Request<Texture2D>("OphioidMod/ophioflyfinalform_Head_Boss");
		}
	}
}