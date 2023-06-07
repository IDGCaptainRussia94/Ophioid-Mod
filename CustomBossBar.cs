using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
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
			return ModContent.Request<Texture2D>("OphioidMod/NPCs/ophioflyfinalform_Head_Boss");
		}

		public override bool PreDraw(SpriteBatch spriteBatch, NPC npc, ref BossBarDrawParams drawParams)
		{
			if (npc.type == ModContent.NPCType<NPCs.MetaOphiocoon>() || npc.type == ModContent.NPCType<NPCs.Ophiocoon>())
			{
				// Don't show the HP on the Ophiocoons.
				drawParams.ShowText = false;
			}
			return true;
		}

		public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax)
		{
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (npc.type == ModContent.NPCType<NPCs.OphiopedeHead2>())
			{
				// If the Ophiopede is stalling to transform to the second phase, make the boss bar have a shield.
				if (npc.life <= (int)(npc.lifeMax * 0.05f))
				{
					shield = npc.life;
					shieldMax = npc.lifeMax;
				}
			}
			if (npc.type == ModContent.NPCType<NPCs.MetaOphiocoon>() || npc.type == ModContent.NPCType<NPCs.Ophiocoon>())
			{
				// Give the Ophiocoons shields.
				shield = 1f;
				shieldMax = 1f;
			}
			if (npc.type == ModContent.NPCType<NPCs.Ophiofly>())
			{
				// Remove the shields once Ophiofly spawns.
				shield = 0f;
				shieldMax = 0f;
			}
			return null;
		}
	}
}