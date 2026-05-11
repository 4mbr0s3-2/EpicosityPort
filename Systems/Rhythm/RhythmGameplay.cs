using EpicosityPort.Systems.Network;
using EpicosityPort.Systems.UserInterface;
using EpicosityPort.Systems.UserInterface.Rhythm;
using EpicosityPort.Systems.UserInterface.Rhythm.Metronome;
using EpicosityPort.Systems.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace EpicosityPort.Systems.Rhythm;

// RS: Dude what a GODCLASS
// RhythmHandler is split into a whopping 6 files
// I need to clean this up immediately
public class RhythmHandler : ModSystem
{
    public static RhythmHandler Instance
    {
        get { return ModContent.GetInstance<RhythmHandler>(); }
    }
    public RhythmKeybinds Keybinds
    {
        get; private set;
    }
    public RhythmInterface Interface
    {
        get; private set;
    }
    public RhythmHandler()
    {
        Keybinds = new RhythmKeybinds();
        Interface = new RhythmInterface();
    }
    public int lockedTargetIndex = -1;
    public int autoAimTargetWhoAmI = -1;
    public int autoAimTargetOrderedNumber = 0;
    public bool autoAiming = false;
    public bool IsAutoAiming
    {
        get
        {
            return lockedTargetIndex >= 0 || autoAiming;
        }
    }

    public SoundEffectInstance uiSound;
    /// <summary>Minimum distance the mouse has to move from the start point in swipe gameplay.</summary>
    //public const int MIN_DIST_FROM_START = 100;
    private const int MAXIMUM_HIT_OBJECTS = 3000;
    public const float PENALTY_TICKS = 50f;
    public float playfieldHeight { get { return EpicosityConfigClient.Instance.FKTrackHeight; } }
    public Vector2 targetPosition = Vector2.Zero;
    public bool mouseTargeting = false;
    //public static float rotationFromMouseStartPositionToCursor = 0;

    public int toggleDoubleTapCooldown = 0;
    public int toggleDoubleClickCooldown = 0;

    public float crosshairToggleAnimation = 0;
    public float crosshairRotation = 0;

    public FourKeyTrackPatternObject fKTrackPattern;
    public MouseTrackPatternObject mouseTrackPattern;

    public int patternArrowIndex = 0;

    // RS: Gross!! This should be put somewhere more closed and transient and not next to the keybinds at the bottom lol
    // To be fair, everything here is based on disassembled/decompiled Terraria code soooooooo
    public SpriteScene chart = new SpriteScene();


    // RS: Gonna move these
    //public ModKeybind upKey;
    //public ModKeybind downKey;
    //public ModKeybind leftKey;
    //public ModKeybind rightKey;
    //public ModKeybind targetSwitchLeft;
    //public ModKeybind targetSwitchRight;
    //public ModKeybind strumKey;
    //public ModKeybind strumKey2;

    public bool[] heldDown;
    public bool[] prevHeldDown;
    public bool lastMouseLeftRelease = false;
    public bool lastMouseLeft = false;
    public bool lastMouseRight = false;

    public bool calibrationMode = false;
    public List<float> calibrationList = new List<float>();

    /// <summary>
    /// The timer used for spawning notes based on note rate. It is reset every time the weapon is unequipped.
    /// </summary>
    public long noteTimer = 0;
    public readonly static Dictionary<Judgement, Color> JudgementColors = new Dictionary<Judgement, Color>()
    {
        [Judgement.Miss] = new Color(255, 4, 0),
        [Judgement.WayOff] = new Color(255, 107, 0),
        [Judgement.Decent] = new Color(204, 29, 255),
        [Judgement.Good] = new Color(12, 255, 0),
        [Judgement.Great] = new Color(255, 230, 14),
        [Judgement.Perfect] = new Color(0, 203, 255),
        [Judgement.Epic] = new Color(255, 255, 255),
    };
    public enum Judgement
    {
        Miss,
        WayOff,
        Decent,
        Good,
        Great,
        Perfect,
        Epic
    }

    public enum GameplayNoteDisplayStyle
    {
        FourKey,
        GuitarHero,
        Taiko
    }
    public enum GameplayType
    {
        Note,
        Swipe,
        Miscellaneous,
    }
    //[LoadManager(LoadManagerAttribute.ManageType.Load)]
    //public static void Load()
    //{
    //	inst = new RhythmHandler(); 
    //	inst.LoadRhythmHotKeys(Epicosity.inst);
    //	inst.LoadTrackPatterns();
    //}
    //[LoadManager(LoadManagerAttribute.ManageType.Unload)]
    //public static void Unload()
    //{
    //	inst = null;
    //}
    public override void OnModLoad()
    {
        Keybinds = new RhythmKeybinds();
        Keybinds.LoadRhythmHotKeys(Mod);
    }
    public override void OnModUnload()
    {
        Interface.Dispose();
    }
    public static GameplayType gameplayType = GameplayType.Miscellaneous;
    /// <summary>Applies only for note gameplay.</summary>
    public static GameplayNoteDisplayStyle gameplayNoteDisplayStyle = GameplayNoteDisplayStyle.FourKey;
    private static int maplessCrotchetCounter;

    public static ModPlayerRhythmist GetLocalRhythmPlayer()
    {
        return Main.LocalPlayer.GetModPlayer<ModPlayerRhythmist>();
    }
    public void LockTarget(int npcIndex)
    {
        //uiSound = ModContent.GetSound($"{typeof(Epicosity).Name}/Sounds/Item/Rhythmic/TargetLock").CreateInstance();
        //uiSound.Volume = Main.soundVolume;
        //uiSound.Play();
        SoundEngine.PlaySound(new SoundStyle($"{typeof(RhythmInterface).GetNamespaceAsPath()}/Sounds/TargetLock"));

        lockedTargetIndex = npcIndex; // Locking target
    }

    public void SetCalibrationMode(bool on)
    {
        calibrationMode = on;
    }

    public void Spawn4KNoteWithTrackPattern()
    {
        fKTrackPattern.GetNote(ref patternArrowIndex, ref chart);
    }
    public void SpawnMouseNoteWithTrackPattern()
    {
        mouseTrackPattern.GetArrow(ref patternArrowIndex, ref chart);
    }

    public void Update4KHitting(GameTime gt, ModPlayerRhythmist dancePlayer, ref bool noMiss)
    {
        bool[] successfullyHitColumns = new bool[] { false, false, false, false };
        bool disableAntiMash = EpicosityConfigClient.Instance.DisableAntiMashingSystem;
        bool[] justPressed = new bool[] {
            heldDown[0] && !prevHeldDown[0],
            heldDown[1] && !prevHeldDown[1],
            heldDown[2] && !prevHeldDown[2],
            heldDown[3] && !prevHeldDown[3],
        };

        List<ISprite> orderedNoteList = chart.sprites.OrderBy((chartNote) => (chartNote as RhythmNote).time).ToList();

        #region For each note...
        for (int noteIndex = 0; noteIndex < orderedNoteList.Count; noteIndex++)
        {
            RhythmNote note = orderedNoteList[noteIndex] as RhythmNote;
            note.Update(gt, dancePlayer, justPressed, ref successfullyHitColumns);
        }
        #endregion
        // Disable if penalty is handled in method
        if (!disableAntiMash)
        {
            for (int x = 0; x < 4; x++)
            {
                if (justPressed[x])
                {
                    UserInterface.Rhythm.ComboBar.ComboBarInterface.Instance.holdComboBarTimer = 120;
                    if (!successfullyHitColumns[x]) // Check if a note has been hit in that column; otherwise, it's a miss
                    {
                        MissPenalty(dancePlayer);
                    }
                }
            }
        }
    }
    // RS: What a MESS
    // 4K behavior gets delegated to RhythmNote.Update() (as shown above) but GH behavior is right here
    public void UpdateGuitarHeroHitting(ModPlayerRhythmist dancePlayer, ref bool noMiss)
    {
        noMiss = true;
        bool[] successfulColumns = new bool[] { false, false, false, false };
        List<RhythmNote> orderedNoteList = chart.sprites.Cast<RhythmNote>().OrderBy((chartNote) => chartNote.time).ToList();
        bool strum = Keybinds.strumKey.JustPressed || Keybinds.strumKey2.JustPressed;
        for (int noteIndex = 0; noteIndex < orderedNoteList.Count; noteIndex++)
        {
            int timingWindow = EpicosityConfigClient.Instance.TimingWindow;
            RhythmNote note = orderedNoteList[noteIndex];
            // Attempt to get the notes closest to the control arrows
            if (!note.initialHit)
            {
                //if (note.beatmap && RhythmBeatmapHandler.inst?.conductor != null)
                //{
                //	note.time = ((note.beatmappedTime - RhythmBeatmapHandler.inst.conductor.syncedSongPosition) * 60f + EpicosityConfigClient.Instance.FKBeatmapOffset) * dancePlayer.trackSpeed;
                //}
                //else
                {
                    note.time -= dancePlayer.trackSpeed;
                    //}
                } // Actual chart movement
                if (Math.Abs(note.time) <= timingWindow && !note.initialHit && strum && heldDown[note.column] && !successfulColumns[note.column]) // Hit range
                {
                    successfulColumns[note.column] = true; // Note has been hit on that column
                                                           //dancePlayer.bodyFramePose = note.bodyFrame;
                    dancePlayer.bodyFramePose = 4; // Since this is for Guitar Hero, we assume that we always have a strum bar...
                    dancePlayer.bodyDirectionPose = note.bodyDirection;
                    dancePlayer.poseAnimationTimer = 10;
                    note.initialHit = true;
                    RhythmHandler.Instance.Interface.RhythmUIStateInstance.AddFireEffect(note.column);
                    if (!note.hold)
                    {
                        note.destroy = true;
                        NoteHit((float)note.time);
                    }
                }
                // RS: This is not a good idea
                // Check if hold note is still being held down, subtract from holdTime
                if (note.initialHit && note.hold && heldDown[note.column])
                {
                    note.holdTime -= dancePlayer.trackSpeed;
                }
                if (note.hold && note.initialHit)
                {
                    // If the player has let go and the hold time is in the timing window, it's a hit!
                    if (Keybinds.strumKey.JustReleased)
                    {
                        note.destroy = true;
                        if (Math.Abs(note.holdTime) < timingWindow)
                        {
                            NoteHit((float)note.holdTime, default, 1.2f);
                        }
                        else
                        {
                            MissPenalty(dancePlayer);
                        }
                    }
                }
                if (note.destroy && note.timeSinceDestroyed >= 20)
                {
                    chart.Remove(note);
                }
                if (note.time <= -30)
                {
                    MissPenalty(dancePlayer, note.beatmap, 20);
                    chart.Remove(note);
                    continue;
                }
            }
            bool successMismatch = false;
            for (int column = 0; column < 4; column++)
            {
                if (heldDown[column] != successfulColumns[column])
                {
                    successMismatch = true;
                }
            }
            //if (strum && successMismatch)
            //{
            //	MissPenalty(dancePlayer);
            //for (int column = 0; column < 4; column++)
            //{
            //    Main.NewText(successfulColumns[0]);
            //    Main.NewText(heldDown[0]);
            //}
            //}
        }
    }
    //[Obsolete]
    //public void UpdateSwipeHitting(ModPlayerRhythmist dancePlayer, ref bool noMiss)
    //{
    //	// If left-click, save position and calculate the direction
    //	if (Main.mouseLeft && !lastMouseLeft)
    //	{
    //		mouseStartPositionScreen = Main.MouseScreen;
    //	}
    //	if (Main.mouseLeft)
    //	{
    //		Vector2 mouseStartToEnd = (Main.MouseScreen - mouseStartPositionScreen);
    //		rotationFromMouseStartPositionToCursor = mouseStartToEnd.SafeNormalize(Vector2.One).ToRotation();
    //	}
    //	for (int noteIndex = 0; noteIndex < chart.Count; noteIndex++)
    //	{
    //		RhythmNote note = chart[noteIndex];
    //		if (!note.initialHit)
    //		{
    //			note.animationTime += 1f / dancePlayer.noteRate;
    //			note.animationTime = Math.Min(note.animationTime, 1);
    //			note.time = (1f - Easings.EaseInOutCubic(note.animationTime)) * 60; // Actual chart movement
    //		}
    //		float difference = (note.direction.ToRotationVector2() - (rotationFromMouseStartPositionToCursor + MathHelper.Pi / 2).ToRotationVector2()).Length();
    //		if (Main.mouseLeftRelease && !lastMouseLeftRelease && Main.hasFocus)
    //		{
    //			Vector2 mouseStartToEnd = (Main.MouseScreen - mouseStartPositionScreen);
    //			if (Math.Abs(note.time) <= 10 && difference <= 0.5f && !note.initialHit && mouseStartToEnd.Length() > 10) // Hit range
    //			{
    //				dancePlayer.bodyFramePose = note.bodyFrame;
    //				dancePlayer.bodyDirectionPose = note.bodyDirection;
    //				dancePlayer.poseAnimationTimer = 10;
    //				note.initialHit = true;
    //				note.hitPosition = mouseStartPositionScreen + Main.screenPosition;
    //				noMiss = true;
    //				NoteHit((float)(difference * 20), note.legacySoundStyle);
    //			}
    //			else
    //			{
    //				note.initialHit = true;
    //			}
    //		}
    //		if (note.initialHit && note.timeSinceDestroyed >= 20)
    //		{
    //			chart.Remove(note);
    //		}
    //	}
    //}

    public int CreateRhythmistProjectileWithEffects(Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0, float ai1 = 0)
    {
        return CreateRhythmistProjectileWithEffects(Main.LocalPlayer, position, velocity, Type, Damage, KnockBack, Owner, ai0, ai1);
    }
    public int CreateRhythmistProjectileWithEffects(Player player, Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0, float ai1 = 0)
    {
        int projIndex = Projectile.NewProjectile
        (
            player.GetSource_FromThis(),
            position,
            velocity,
            Type,
            Damage,
            KnockBack,
            Owner,
            ai0,
            ai1
        );
        // Give projectile information about target
        if (Main.projectile[projIndex].ModProjectile is RhythmicProjectile)
        {
            RhythmicProjectile rhythmistProjectile = Main.projectile[projIndex].ModProjectile as RhythmicProjectile;

            // RS: IDK why this was for 4K specifically
            // It really should both be 4K and GH

            // Set projectile target
            //if (gameplayType == GameplayType.Note)
            //{
            if (IsAutoAiming)
            {
                rhythmistProjectile.targetNPCWhoAmI = autoAimTargetWhoAmI;
            }
            //}
            rhythmistProjectile.targetPosition = targetPosition;

            // Add effects to projectile based on accessories
            SetRhythmistProjectileEffects(player, Main.projectile[projIndex].ModProjectile as RhythmicProjectile);
            Main.projectile[projIndex].netUpdate2 = true;
        }
        return projIndex;
    }

    /// <summary>
    /// Applies effects to projectiles based on the player's accessories.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="projectile"></param>
    public void SetRhythmistProjectileEffects(Player player, RhythmicProjectile rhythmistProjectile)
    {
        ModPlayerRhythmistAccessories rhythmistAccessoriesPlayer = player.GetModPlayer<ModPlayerRhythmistAccessories>();
        if (rhythmistAccessoriesPlayer.hasFlamingAccessory)
        {
            rhythmistProjectile.effects |= RhythmicProjectile.RhythmistProjectileEffects.Flaming;
        }
        if (rhythmistAccessoriesPlayer.hasFreezingAccessory) // We are not making these mutually exclusive because it's funny
        {
            rhythmistProjectile.effects |= RhythmicProjectile.RhythmistProjectileEffects.Freezing;
        }
    }

    public void OnShootProjectileEffects(Player player, float comboMeter, Judgement accuracy)
    {
        ModPlayerRhythmistAccessories acc = player.GetModPlayer<ModPlayerRhythmistAccessories>();
        if (acc.hasPacemaker)
        {
            int health = (int)accuracy;
            if (health > 0)
            {
                player.statLife += health;
                player.HealEffect(health);
            }
        }
    }
    public void IncrementLocalPlayerCombo(Item item)
    {
        var p = GetLocalRhythmPlayer();
        p.comboMeter += 0.1f * (Main.LocalPlayer.GetModPlayer<ModPlayerRhythmicPlayerStats>().rhythmicComboIncrementMult + item.GetGlobalItem<RhythmicItemInfo>().comboIncrement);
        p.combo++;
    }


    //public void NoteHit(float accuracy = 0, LegacySoundStyle legacySoundStyle = null, float holdDamageMult = 1)

    public void NoteHit(float accuracy = 0, SoundStyle? soundStyle = null, float holdDamageMult = 1)
    {
        // The combo meter should still count here if there's an accessory that depends on it while the player is dead
        RhythmicWeaponBase rhythmicWeaponBase = Main.LocalPlayer.HeldItem.ModItem as RhythmicWeaponBase;
        IncrementLocalPlayerCombo(Main.LocalPlayer.HeldItem);
        UserInterface.Rhythm.ComboBar.ComboBarInterface.Instance.arrowScale = 0.2f;

        if (Main.LocalPlayer.dead)
        {
            return;
        }
        if (Main.LocalPlayer.HeldItem.GetGlobalItem<RhythmicItemInfo>().rhythmic)
        {
            Judgement accuracyType;

            // RS: What are we doing 💔 (this is probably fine)
            if (Math.Abs(accuracy) >= 6)
            {
                accuracyType = Judgement.WayOff;
            }
            else if (Math.Abs(accuracy) >= 5)
            {
                accuracyType = Judgement.Decent;
            }
            else if (Math.Abs(accuracy) >= 4)
            {
                accuracyType = Judgement.Good;
            }
            else if (Math.Abs(accuracy) >= 3)
            {
                accuracyType = Judgement.Great;
            }
            else if (Math.Abs(accuracy) >= 1)
            {
                accuracyType = Judgement.Perfect;
            }
            else
            {
                accuracyType = Judgement.Epic;
            }

            Interface.RhythmUIStateInstance.SetAccuracy(accuracyType);

            UserInterface.Rhythm.ComboBar.ComboBarInterface.Instance.comboBarUIState.AddMarker(accuracy, JudgementColors[accuracyType]);

            if (calibrationMode)
            {
                var earlyLate = accuracy > 0 ? "early" : "late";
                Main.NewText($"Deviation (in frames): {Math.Abs(accuracy)} {earlyLate}");
                calibrationList.Add(accuracy);
                return;
            }

            Vector2 velocity = Vector2.Zero;
            if (gameplayType == GameplayType.Note)
            {
                velocity = targetPosition - Main.LocalPlayer.Center;
            }
            else if (gameplayType == GameplayType.Swipe)
            {
                velocity = RhythmMouse.Instance.mouseStartPositionScreen + Main.screenPosition - Main.LocalPlayer.Center;
            }
            if (velocity != Vector2.Zero) velocity.Normalize();
            velocity *= Main.LocalPlayer.HeldItem.shootSpeed;

            // Damage multiplier
            float damageMult = 1;
            if (accuracyType == Judgement.Great)
            {
                damageMult = 1.5f;
            }
            if (accuracyType == Judgement.Perfect)
            {
                damageMult = 2;
            }
            // Extra damage from hold notes
            damageMult *= holdDamageMult;
            int rhythmicDamage = Main.LocalPlayer.GetWeaponDamage(Main.LocalPlayer.HeldItem);

            if (!(Main.LocalPlayer.HeldItem.ModItem is RhythmicWeaponBase))
            {
                return;
            }
            // Make projectile
            var localRhythmPlayer = GetLocalRhythmPlayer();
            //int calculatedTotalDamage = (int)((rhythmicDamage * 0.8f * localRhythmPlayer.comboMeter + rhythmicDamage * 0.2f) * damageMult);
            int calculatedTotalDamage = (int)((rhythmicDamage * 0.8f * localRhythmPlayer.comboMeter + rhythmicDamage * 0.2f) * damageMult);
            float calculatedKnockback = Main.LocalPlayer.HeldItem.knockBack * localRhythmPlayer.comboMeter;
            // Overridden shoot method
            if (!EpicosityConfigClient.Instance.DisableHitSounds)
            {
                if (soundStyle != null)
                {
                    // RS: For some reason, despite the null check above,
                    // this is still treated as a nullable
                    // and I'm too lazy to figure out why
                    // so we explicit cast
                    SoundUtil.PlaySoundOnAllClients((SoundStyle)soundStyle, Main.LocalPlayer.Center);
                }
                else
                {
                    if (Main.LocalPlayer.HeldItem.UseSound != null)
                        SoundUtil.PlaySoundOnAllClients((SoundStyle)Main.LocalPlayer.HeldItem.UseSound, Main.LocalPlayer.Center);
                }
            }
            OnShootProjectileEffects(Main.LocalPlayer, localRhythmPlayer.comboMeter, accuracyType);
            if (localRhythmPlayer.comboMeter >= 1)
            {
                UserInterface.Rhythm.ComboBar.ComboBarInterface.Instance.fullComboAnimation = 1;
            }
            if
            (
                rhythmicWeaponBase.RhythmShoot
                (
                    velocity,
                    calculatedTotalDamage,
                    calculatedKnockback,
                    localRhythmPlayer.comboMeter,
                    targetPosition,
                    autoAimTargetWhoAmI
                )
            )
            {
                return;
            }
            // Do combo meter FC animation regardless of whether or not the weapon has an FC ability
            int projIndex = CreateRhythmistProjectileWithEffects
            (
                Main.LocalPlayer.Center,
                velocity,
                Main.LocalPlayer.HeldItem.shoot,
                calculatedTotalDamage,
                calculatedKnockback,
                Main.LocalPlayer.whoAmI,
                0f,
                0f
            );
            //// Give projectile information about target
            //if (Main.projectile[projIndex].ModProjectile is RhythmicProjectile)
            //{
            //	RhythmicProjectile rhythmistProjectile = Main.projectile[projIndex].ModProjectile as RhythmicProjectile;
            //	if (gameplayType == GameplayType.Note)
            //	{
            //		if (lockedTarget)
            //		{
            //			rhythmistProjectile.targetNPCWhoAmI = targetWhoAmI;
            //		}
            //	}
            //	rhythmistProjectile.targetPosition = targetPosition;
            //	Main.projectile[projIndex].netUpdate2 = true;
            //}

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // TODO: Check if it works on multiplayer
                // Send to server the animation and combo meter details to send to other clients
                ModPlayerRhythmist dancePlayer = GetLocalRhythmPlayer();
                ModNetHandler.GetPacketHandler<RhythmPlayerSyncComboBarHandler>().SendSyncAll(-1, Main.myPlayer, dancePlayer.comboDepletionRate, dancePlayer.comboMeter, dancePlayer.bodyFramePose, dancePlayer.poseAnimationTimer, dancePlayer.bodyDirectionPose);
            }
        }
    }
    public void UpdateRhythmGameplay(GameTime gameTime)
    {
        if (gameplayType == GameplayType.Note)
        {

            if (toggleDoubleClickCooldown > 0) toggleDoubleClickCooldown--;
            if (Main.mouseLeft && !lastMouseLeft)
            {
                if (toggleDoubleClickCooldown > 0)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && npc.getRect().Contains(Main.MouseWorld.ToPoint()))
                        {
                            LockTarget(npc.whoAmI);
                            break;
                        }
                    }
                }
                toggleDoubleClickCooldown = 10;
            }
            heldDown = new bool[]
            {
                        Keybinds.leftKey.Current,
                        Keybinds.upKey.Current,
                        Keybinds.downKey.Current,
                        Keybinds.rightKey.Current,
            };
        }
        else if (gameplayType == GameplayType.Swipe)
        {
            targetPosition = Main.MouseWorld;
        }
        ModPlayerRhythmist dancePlayer = GetLocalRhythmPlayer();
        // Animate fretboard
        if (Main.LocalPlayer.GetModPlayer<ModPlayerRhythmistAccessories>().fretboardType != "")
        {
            Interface.chartAnimationTime += (float)Main.LocalPlayer.GetModPlayer<ModPlayerRhythmist>().trackSpeed;
            ModPlayerRhythmist rhythmist = Main.LocalPlayer.GetModPlayer<ModPlayerRhythmist>();
        }
        // Spawn arrow at right time
        if (dancePlayer.noteRate > 0)
        {
            //Main.NewText(arrowNoteTime);
            //if (!RhythmBeatmapHandler.inst.TrackActive)
            //{
            noteTimer += 1;
            if (gameplayType == GameplayType.Note)
            {
                if ((int)(noteTimer - (60 / (float)dancePlayer.trackSpeed)) % dancePlayer.noteRate == 0)
                {
                    MetronomeInterface.Instance.MetronomeUIStateInstance?.metronome?.Beat(maplessCrotchetCounter == 0);
                    maplessCrotchetCounter++;
                    maplessCrotchetCounter %= 4;
                }
                if ((int)noteTimer % dancePlayer.noteRate == 0)
                {
                    Instance.Interface.chartAnimationTime = 0; // This ensures that the fretboard lines align
                    Spawn4KNoteWithTrackPattern();
                }
            }
            else if (gameplayType == GameplayType.Swipe)
            {
                if (chart.sprites.Where((note) => !(note as RhythmNote).initialHit).Count() == 0) SpawnMouseNoteWithTrackPattern();
            }
            //}
            //else
            //{
            //	noteTimer = (long)(RhythmBeatmapHandler.inst.conductor.normalizedSongPosition * 60);
            //	chartAnimationTime = (float)RhythmBeatmapHandler.inst.conductor.normalizedSongPosition;
            //	chartAnimationTime %= (float)RhythmBeatmapHandler.inst.conductor.crotchetSeconds;
            //	chartAnimationTime *= 60 * (float)Main.LocalPlayer.GetModPlayer<ModPlayerRhythmist>().trackSpeed;
            //	HandleSpawningBeatmap();
            //}
        }
        bool noMiss = false;
        if (gameplayType == GameplayType.Note)
        {
            if (gameplayNoteDisplayStyle == GameplayNoteDisplayStyle.GuitarHero)
            {
                UpdateGuitarHeroHitting(dancePlayer, ref noMiss);
            }
            else
            {
                Update4KHitting(gameTime, dancePlayer, ref noMiss);
            }
            prevHeldDown = heldDown;
        }
        // OBSOLETE!!!
        //else if (gameplayType == GameplayType.Swipe)
        //{
        //	HandleSwipeHitting(dancePlayer, ref noTrip);
        //	if (Main.mouseLeftRelease && !lastMouseLeftRelease && Main.hasFocus)
        //	{
        //		ComboBar.ComboBarInterface.holdComboBarTimer = 120;
        //		if (!noTrip)
        //		{
        //			MissPenalty(dancePlayer);
        //		}
        //	}
        //}
        lastMouseLeftRelease = Main.mouseLeftRelease;
        lastMouseLeft = Main.mouseLeft;
        if (chart.sprites.Count > MAXIMUM_HIT_OBJECTS) // Maximum
        {
            chart.sprites.RemoveAt(0);
        }
    }
    public bool SwitchTarget(int indexOffset, IEnumerable<NPC> sorted, bool silent = false)
    {
        if (sorted.Count() <= 0) return false;
        lockedTargetIndex = -1;
        if (!silent)
            SoundEngine.PlaySound(new SoundStyle($"{typeof(RhythmInterface).GetNamespaceAsPath()}/Sounds/TargetSwitch"));

        bool foundEnemyInRange = false;

        for (int npcIndex = 0; npcIndex < sorted.Count(); npcIndex++)
        {
            NPC npc = sorted.ElementAt(npcIndex);
            if (npc.whoAmI == autoAimTargetWhoAmI)
            {
                foundEnemyInRange = true;
                npcIndex += indexOffset; // Go up based on order from player
                npcIndex += sorted.Count(); // Prevent it from going negative
                npcIndex %= sorted.Count();
                autoAimTargetWhoAmI = sorted.ElementAt(npcIndex).whoAmI;
                break;
            }
        }
        // If an enemy hasn't been selected yet
        if (!foundEnemyInRange || autoAimTargetWhoAmI == -1)
        {
            autoAimTargetWhoAmI = sorted.First().whoAmI;
        }
        return true;
    }
    public void PlayComboHitSound()
    {
        if (EpicosityConfigClient.Instance.DisableHitSounds)
            return;
        SoundUtil.PlaySoundOnAllClients(new SoundStyle($"{typeof(RhythmInterface).GetNamespaceAsPath()}/Sounds/FullComboHit"), Main.LocalPlayer.Center);
    }
    public IEnumerable<NPC> UpdateTargetPosition()
    {
        // RS: You love to see it
        float closestDistance = 1000;

        // Sort nearest NPCs
        IEnumerable<NPC> sorted = GetNearestTargetableNPCs(closestDistance);
        #region Dance target switch function
        bool pressedRight = Instance.Keybinds.targetSwitchRight.JustPressed;
        bool pressedLeft = Instance.Keybinds.targetSwitchLeft.JustPressed;
        if ((pressedRight || pressedLeft) && Main.hasFocus && IsAutoAiming)
        {
            int off = 0;
            if (pressedLeft) off = -1;
            if (pressedRight) off = 1;
            SwitchTarget(off, sorted);
        }
        if (autoAimTargetWhoAmI > -1 && !Main.npc[autoAimTargetWhoAmI].active)
        {
            if (!SwitchTarget(Main.LocalPlayer.direction, sorted, true)) autoAimTargetWhoAmI = -1; // Automatically switch
        }
        #endregion
        if (lockedTargetIndex > -1)
        {
            if (Main.npc[lockedTargetIndex].active)
            {
                autoAimTargetWhoAmI = lockedTargetIndex;
            }
            else
            {
                lockedTargetIndex = -1;
            }
        }
        // Dance target switch code
        // Target if targetIndex is valid
        if (IsAutoAiming && autoAimTargetWhoAmI >= 0) // This makes sure that the cursor doesn't move much
        {
            targetPosition = Main.npc[autoAimTargetWhoAmI].Center;
            crosshairRotation += 0.04f;
        }
        else
        {
            PlayerInput.SetZoom_World();
            Vector2 mouseRelative = Main.MouseWorld;
            targetPosition = mouseRelative;
            crosshairRotation = 0;
        }
        return sorted;
    }

    private IEnumerable<NPC> GetNearestTargetableNPCs(float closestDistance)
    {
        bool InRange(NPC npc, float dist)
        {
            return (Main.LocalPlayer.Center - npc.Center).Length() < dist;
        }
        // Sort nearest NPCs
        IEnumerable<NPC> sorted = Main.npc
            .Where(npc => npc.active
                    && !npc.friendly
                    && InRange(npc, closestDistance))
            .OrderBy(npc => npc.position.X);
        //.OrderByDescending(npc => npc.lifeMax);
        return sorted;
    }

    public void MissPenalty(ModPlayerRhythmist dancePlayer, bool hurt = false, int damage = 3)
    {
        dancePlayer.combo = 0;
        Instance.Interface.RhythmUIStateInstance.SetAccuracy(Judgement.Miss);
        UserInterface.Rhythm.ComboBar.ComboBarInterface.Instance.shake = 5;
        dancePlayer.comboMeter -= PENALTY_TICKS * dancePlayer.comboDepletionRate; // TODO: Customizable / stat?
        if (hurt)
        {
            SoundEngine.PlaySound(SoundID.Item16);
            Player player = dancePlayer.Player;
            player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(NetworkText.FromKey($"Mods.{EpicosityPort.Instance.Name}.DeathMessages.RhythmMiss", [player.name])),
                damage,
                0,
                default,
                default,
                default,
                false,
                default,
                default,
                2);
        }
    }
}
