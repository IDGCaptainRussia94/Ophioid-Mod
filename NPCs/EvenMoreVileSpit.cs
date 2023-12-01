using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace OphioidMod.NPCs
{
	public class EvenMoreVileSpit : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new()
			{
				Hide = true // Hides this NPC from the bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
		}

		public override void SetDefaults()
		{
			NPC.width = 16;
			NPC.height = 16;
			NPC.aiStyle = 9;
			NPC.damage = 65;
			NPC.defense = 0;
			NPC.lifeMax = 1;
			NPC.HitSound = null;
			NPC.DeathSound = SoundID.NPCDeath9;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
			NPC.scale = 0.9f;
			NPC.alpha = 80;
			NPC.aiStyle = -1;
		}

		public override string Texture
		{
			get { return "Terraria/Images/NPC_" + NPCID.VileSpit; }
		}

		public override void HitEffect(NPC.HitInfo hitInfo)
		{

			for (int num328 = 0; num328 < 20; num328 += 1)
			{
				int num329 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 2f), NPC.width, NPC.height, DustID.CorruptGibs, NPC.velocity.X * 1f, NPC.velocity.Y * 1f, 100, default(Color), 3f);
				Dust dust = Main.dust[num329];
				if (Main.rand.Next(15) < 12)
				{
					dust.scale *= 0.8f;
				}
				else
				{
					dust.scale *= 1.5f;
					dust.velocity *= 0.1f;
				}
				Main.dust[num329].noGravity = true;
			}
		}

		//public void NPCLoot(){
		//Effects();
		//}

		public override void AI()
		{
			NPC.TargetClosest(true);
			for (int num328 = 0; num328 < 2; num328 += 1)
			{
				int num329 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 2f), NPC.width, NPC.height, DustID.CorruptGibs, NPC.velocity.X * 0.3f, NPC.velocity.Y * 0.3f, 100, default(Color), 2f);
				Dust dust = Main.dust[num329];
				if (Main.rand.Next(15) < 12)
				{
					dust.scale *= 0.8f;
				}
				else
				{
					dust.scale *= 1.5f;
					dust.velocity *= 0.05f;
				}
				Main.dust[num329].noGravity = true;
			}

			if (NPC.ai[0] == 0 && Main.rand.Next(0, 10) < 5 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				float num125 = NPC.velocity.Length();
				Vector2 vector16 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num126 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector16.X;
				float num127 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector16.Y;
				float num128 = (float)Math.Sqrt((double)(num126 * num126 + num127 * num127));
				num128 = num125 / num128;
				NPC.velocity.X = num126 * num128;
				NPC.velocity.Y = num127 * num128;
				NPC.netUpdate = true;
			}
			NPC.ai[0] += 1;
			if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) || NPC.ai[0] > 200)
			{
				NPC.SimpleStrikeNPC(999, 0);
				NPC.HitEffect(0, 10.0);
			}

			if (NPC.collideX || NPC.collideY || NPC.Distance(Main.player[NPC.target].Center) < 20f)
			{
				NPC.SimpleStrikeNPC(9999, NPC.direction);
				NPC.HitEffect(0, 10.0);
			}
		}
	}
}