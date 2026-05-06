using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Buffs.StickySlimed
{
	public class StickySlimedVisual : GlobalNPC
	{
		public override void PostAI(NPC npc)
		{
			int stickySlimeDebuff = ModContent.BuffType<StickySlimed>();
			if (npc.HasBuff(stickySlimeDebuff))
			{
				npc.velocity.X *= 0.7f;
			}
		}
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			int stickySlimeDebuff = ModContent.BuffType<StickySlimed>();
			if (npc.HasBuff(stickySlimeDebuff)) 
			{
				for (int buffIndex = 0; buffIndex < npc.buffType.Length; buffIndex++)
				{
					int buff = npc.buffType[buffIndex];
					if (buff == stickySlimeDebuff)
					{
						float progress = npc.buffTime[buffIndex];
						if (progress > 180)
						{
							progress = 180;
						}
						progress /= 180f;
						float inverseProgress = 1f - progress;
						drawColor = new Color((byte)(0x52 * progress + drawColor.R * inverseProgress), (byte)(0x88 * progress + drawColor.G * inverseProgress), (byte)(0xFE * progress + drawColor.B * inverseProgress));
						return;
					}
					continue;
				}
			}
		}
	}
}
