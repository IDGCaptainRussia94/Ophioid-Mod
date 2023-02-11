using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.GameContent.ObjectInteractions;
using OphioidMod.Items;
using Terraria.DataStructures;

namespace OphioidMod.Tiles
{
    public class OphiodBossTrophy : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            DustType = 7;
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Boss Trophy");
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int item = 0;
            switch (frameX / 54)
            {
                case 0:
                    item = ModContent.ItemType<Ophiopedetrophyitem>();
                    break;
            }
            if (item > 0)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, item);
            }
        }
    }
}