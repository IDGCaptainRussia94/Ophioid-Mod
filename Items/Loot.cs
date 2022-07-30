using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using OphioidMod.Buffs;
using OphioidMod.Projectiles;
using OphioidMod.Tiles;
using System.Collections.Generic;
using OphioidMod.NPCs;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

namespace OphioidMod.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class OphiopedeMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophiopede Mask");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            DisplayName.SetDefault("Treasure Bag (Ophioid)");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            ItemID.Sets.BossBag[Type] = true; // This set is one that every boss bag should have, it, for example, lets our boss bag drop dev armor..
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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

        // Below is code for the visuals

        public override Color? GetAlpha(Color lightColor)
        {
            // Makes sure the dropped bag is always visible
            return Color.Lerp(lightColor, Color.White, 0.4f);
        }

        public override void PostUpdate()
        {
            // Spawn some light and dust when dropped in the world
            Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.4f);

            if (Item.timeSinceItemSpawned % 12 == 0)
            {
                Vector2 center = Item.Center + new Vector2(0f, Item.height * -0.1f);

                // This creates a randomly rotated vector of length 1, which gets it's components multiplied by the parameters
                Vector2 direction = Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f);
                float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                Dust dust = Dust.NewDustPerfect(center + direction * distance, DustID.SilverFlame, velocity);
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 0;
            }
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            // Draw the periodic glow effect behind the item when dropped in the world (hence PreDrawInWorld)
            Texture2D texture = TextureAssets.Item[Item.type].Value;

            Rectangle frame;

            if (Main.itemAnimations[Item.type] != null)
            {
                // In case this item is animated, this picks the correct frame
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
            }
            else
            {
                frame = texture.Frame();
            }

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(90, 70, 255, 50), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(140, 120, 255, 77), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
    public class OphioidLarva : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ophioid Larva");
            Tooltip.SetDefault("'A little Ophiopede'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.shoot = ModContent.ProjectileType<BabyOphiopedePet>();
            Item.buffType = ModContent.BuffType<BabyOphiopedeBuff>();
            Item.sellPrice(0, 5, 0, 0);
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