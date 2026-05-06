using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort.Content.Buffs.FreezingCold
{
	public class FreezingVisual : GlobalNPC
	{
		public override void PostAI(NPC npc)
		{
			int freezing = ModContent.BuffType<FreezingCold>();
			if (npc.HasBuff(freezing))
			{
				npc.velocity.X *= 0.7f;
			}
		}
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			int freezing = ModContent.BuffType<FreezingCold>();
			if (npc.HasBuff(freezing)) 
			{
				for (int buffIndex = 0; buffIndex < npc.buffType.Length; buffIndex++)
				{
					int buff = npc.buffType[buffIndex];
					if (buff == freezing)
					{
						float progress = npc.buffTime[buffIndex];
						if (progress > 180)
						{
							progress = 180;
						}
						progress /= 180f;
						float inverseProgress = 1f - progress;
						drawColor = new Color((byte)(0xA1 * progress + drawColor.R * inverseProgress), (byte)(0xFD * progress + drawColor.G * inverseProgress), (byte)(0xFF * progress + drawColor.B * inverseProgress));
						return;
					}
					continue;
				}
			}
		}
	}
}
