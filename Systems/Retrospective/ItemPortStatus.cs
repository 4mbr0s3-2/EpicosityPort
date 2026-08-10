using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Systems.Retrospective
{
    public class ItemPortStatus : GlobalItem
    {
        private static Dictionary<int, PortStatusID> itemPortStatuses;

        public static Dictionary<int, PortStatusID> ItemPortStatuses { get => itemPortStatuses; }

        public override void Load()
        {
            itemPortStatuses = [];
        }

        public override void Unload()
        {
            itemPortStatuses.Clear();
            itemPortStatuses = null;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem == null) return;
            if (item.ModItem.Mod != Mod) return;
            PortStatusID entry;
            if (!ItemPortStatuses.ContainsKey(item.type))
                entry = PortStatusID.Identical;
            else
                entry = ItemPortStatuses[item.type];
            tooltips.Add(new TooltipLine(Mod, "PortStatus", ""));
        }
    }
}
