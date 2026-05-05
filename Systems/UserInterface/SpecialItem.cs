using Terraria.UI;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using System;

namespace EpicosityPort.Systems.UserInterface
{
	/// <summary>
	/// Use this for items that have special functionality, such as interfaces or animations.
	/// </summary>
	// RS: Ah yes "SpecialItem" is very descriptive
	public abstract class SpecialItem : ModItem
	{
		/// <summary>
		/// Set this to the UIState the common item interface would be set to.
		/// </summary>
		public static UIState ItemUIState;
		/// <summary>
		/// Set the ItemUIState here and activate it.
		/// </summary>
		[Obsolete]
		public virtual void InitializeUIState()
		{
		}
		public virtual void SetStaticSpecialDefaults()
		{
		}
		public sealed override void SetStaticDefaults()
		{
			if (Main.netMode != NetmodeID.Server) // Interface should not be called on server
			{
				SetStaticSpecialDefaults();
			}
		}
		/// <summary>
		/// Allows for custom functionality for when the player is holding the item. Called in PreUpdate hook.
		/// </summary>
		public virtual void PreUpdatePlayer(Player player)
		{
		}
		/// <summary>
		/// Called whenever the special item is unequipped. Called for all clients in a server.
		/// </summary>
		/// <param name="player"></param>
		public virtual void UnequipItem(Player player)
		{
		}
		/// <summary>
		/// Called whenever the special item is equipped. Called for all clients in a server.
		/// </summary>
		/// <param name="player"></param>
		public virtual void EquipItem(Player player)
		{
		}
		/// <summary>
		/// Allows for custom functionality for when the player is holding the item. Called in PostUpdate hook.
		/// </summary>
		public virtual void PostUpdatePlayer(Player player)
		{
		}
	}
}
