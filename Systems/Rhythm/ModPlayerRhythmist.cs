using EpicosityPort.Systems.UserInterface.Rhythm;
using EpicosityPort.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace EpicosityPort.Systems.Rhythm
{
	public class ModPlayerRhythmist : ModPlayer
	{
		/// <summary>The value removed from each arrow's time every tick.</summary>
		public double trackSpeed = 0.5;

		/// <summary>The number of ticks between each note.</summary>
		public int noteRate = 60;

		/// <summary>The value removed from the combo meter every tick.</summary>
		public float comboDepletionRate = 0.003f * 0.5f;

		public int bodyFramePose = 0;
		public int poseAnimationTimer = 0;
		public int bodyDirectionPose = 0;

		public float comboMeter = 0;
		public int combo = 0;

		public override void PostUpdate()
		{
			if (Main.LocalPlayer.HeldItem.type != ItemID.None)
			{
				if (RhythmHandler.Instance.toggleDoubleTapCooldown > 0)
				{
                    RhythmHandler.Instance.toggleDoubleTapCooldown -= 1;
				}
				if (Main.LocalPlayer.HeldItem.GetGlobalItem<RhythmicItemInfo>().rhythmic && Main.mouseRight && !RhythmHandler.Instance.lastMouseRight && !Main.LocalPlayer.mouseInterface)
				{
					// Toggle targeting
					//RhythmHandler.Instance.lockedTarget = !RhythmHandler.Instance.lockedTarget;
					RhythmHandler.Instance.autoAiming = !RhythmHandler.Instance.autoAiming;
                    bool hasLocked = false;
					if (RhythmHandler.Instance.toggleDoubleTapCooldown > 0)
					{
						hasLocked = true;
                        RhythmHandler.Instance.LockTarget(RhythmHandler.Instance.autoAimTargetWhoAmI);
					}
                    RhythmHandler.Instance.toggleDoubleTapCooldown = 10;
					if (!hasLocked)
					{
						if (RhythmHandler.Instance.autoAiming)
						{
                            SoundEngine.PlaySound(new SoundStyle($"{typeof(RhythmInterface).GetNamespaceAsPath()}/Sounds/TargetEnable"));
						}
						else
						{
                            SoundEngine.PlaySound(new SoundStyle($"{typeof(RhythmInterface).GetNamespaceAsPath()}/Sounds/TargetDisable"));
                            RhythmHandler.Instance.lockedTargetIndex = -1;
                            //RhythmHandler.Instance.uiSound.Volume = Main.soundVolume;
                            //RhythmHandler.Instance.uiSound.Play();
						}
					}
				}
                RhythmHandler.Instance.lastMouseRight = Main.mouseRight; // TODO: move these somewhere more appropriate probably
			}

			// Combo meter called outside update function of UI so it can drain in the background
			// RS: I think there's better reasons why it shouldn't be in the UI update method
			comboMeter -= comboDepletionRate;

			if (comboMeter < 0)
			{
				comboMeter = 0;
			}
			if (comboMeter > 1)
			{
				comboMeter = 1;
			}
			if (poseAnimationTimer > 0)
			{
				poseAnimationTimer--;
				if (bodyFramePose >= 0)
				{
					Player.bodyFrame.Y = Player.bodyFrame.Height * bodyFramePose;
				}
				if (bodyDirectionPose != 0)
				{
					Player.ChangeDir(bodyDirectionPose);
				}
				if (poseAnimationTimer == 0)
				{
					bodyDirectionPose = 0;
				}
			}
		}
	}
}
