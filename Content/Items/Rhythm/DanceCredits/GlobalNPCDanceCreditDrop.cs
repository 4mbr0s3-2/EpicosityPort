using EpicosityPort.Systems.Rhythm;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Items.Rhythm.DanceCredits
{
    public class GlobalNPCDanceCreditDrop : GlobalNPC
    {
        public static GlobalNPCDanceCreditDrop SingletonInstance
        {
            get
            {
                return ModContent.GetInstance<GlobalNPCDanceCreditDrop>();
            }
        }
        public struct DanceCreditDropInfo
        {
            public DanceCreditDropInfo(int minStack, int maxStack, int itemType)
            {
                this.minStack = minStack;
                this.maxStack = maxStack;
                this.itemType = itemType;
            }
            public int minStack;
            public int maxStack;
            public int itemType;
        }
        public Dictionary<int, DanceCreditDropInfo> danceCreditDropInfo;
        public override void Load()
        {
            danceCreditDropInfo = new Dictionary<int, DanceCreditDropInfo>
            {
                { NPCID.KingSlime, new DanceCreditDropInfo(4, 6, ModContent.ItemType<CopperDanceCredit>()) },
                { NPCID.EyeofCthulhu, new DanceCreditDropInfo(4, 8, ModContent.ItemType<CopperDanceCredit>()) },
                { NPCID.QueenBee, new DanceCreditDropInfo(4, 10, ModContent.ItemType<SilverDanceCredit>()) },
                { NPCID.SkeletronHead, new DanceCreditDropInfo(6, 10, ModContent.ItemType<SilverDanceCredit>()) },
                { NPCID.WallofFlesh, new DanceCreditDropInfo(4, 8, ModContent.ItemType<GoldDanceCredit>()) },
                { NPCID.Retinazer, new DanceCreditDropInfo(4, 8, ModContent.ItemType<GoldDanceCredit>()) },
                { NPCID.Spazmatism, new DanceCreditDropInfo(4, 8, ModContent.ItemType<GoldDanceCredit>()) },
                { NPCID.TheDestroyer, new DanceCreditDropInfo(8, 12, ModContent.ItemType<GoldDanceCredit>()) },
                { NPCID.SkeletronPrime, new DanceCreditDropInfo(6, 10, ModContent.ItemType<GoldDanceCredit>()) },
                { NPCID.Plantera, new DanceCreditDropInfo(6, 10, ModContent.ItemType<PlatinumDanceCredit>()) },
                { NPCID.Golem, new DanceCreditDropInfo(8, 12, ModContent.ItemType<PlatinumDanceCredit>()) },
                { NPCID.DukeFishron, new DanceCreditDropInfo(10, 14, ModContent.ItemType<PlatinumDanceCredit>()) },
                { NPCID.CultistBoss, new DanceCreditDropInfo(12, 16, ModContent.ItemType<PlatinumDanceCredit>()) },
                { NPCID.MoonLordCore, new DanceCreditDropInfo(12, 32, ModContent.ItemType<TerraDanceCredit>()) }
            };
        }
        public int rhythmicDamage = 0;
        public int nonRhythmicDamage = 0;
        public override bool InstancePerEntity => true;
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            // Checks for rhythmic tag instead of inheritance because some rhythmic items may not inherit from the base class
            if (item.GetGlobalItem<RhythmicItemInfo>().rhythmic)
            {
                rhythmicDamage += damageDone;
            }
            else
            {
                nonRhythmicDamage += damageDone;
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.ModProjectile is RhythmicProjectile)
            {
                rhythmicDamage += damageDone;
            }
            else
            {
                nonRhythmicDamage += damageDone;
            }
        }
        public override void OnKill(NPC npc)
        {
            if (SingletonInstance.danceCreditDropInfo.ContainsKey(npc.type) && rhythmicDamage >= nonRhythmicDamage)
            {
                DanceCreditDropInfo danceCreditDrop = SingletonInstance.danceCreditDropInfo[npc.type];
                Item.NewItem(npc.GetSource_FromThis(), npc.getRect(), danceCreditDrop.itemType, Main.rand.Next(danceCreditDrop.minStack, danceCreditDrop.maxStack));
            }
        }
    }
}
