using EpicosityPort.Systems.Rhythm;
using EpicosityPort.Systems.UserInterface.AlphaMessage;
using EpicosityPort.Systems.UserInterface.Rhythm.Setup;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EpicosityPort
{
    //[Label("Epicosity Client Config")]
    public class EpicosityConfigClient : ModConfig
    {
        public static EpicosityConfigClient Instance; // tModLoader will handle loading/unloading through reflection
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("RhythmistSettings")]
        //[Label("4K: Upscroll")]
        public bool FKUpscroll;

        //[Label("4K & GH: Playfield above player?")]
        public bool FKGHPlayfieldAbovePlayer;

        //[Label("4K & GH: Track X position offset")]
        [DefaultValue(0)]
        [Range(-200, 200)]
        public int FKGHTrackXOffset;

        //[Label("4K & GH: Track Y position offset")]
        [DefaultValue(0)]
        [Range(-200, 200)]
        public int FKGHTrackYOffset;

        //[Label("4K: Track height")]
        [DefaultValue(300)]
        [Range(100, 300)]
        public int FKTrackHeight;

        //[Label("4K: Disable Anti-Mashing System")]
        //[Tooltip("Penalties are only counted for way-off hits.")]
        [DefaultValue(false)]
        public bool DisableAntiMashingSystem;

        //[Label("4K: Beatmap gameplay offset")]
        [DefaultValue(-3)]
        [Range(-20, 20)]
        //[Tooltip("How late (+) or early (-) notes on a beatmap are actually positioned (in 1/60ths of a second)")]
        public int FKBeatmapOffset;

        //[Label("4K: Beatmap visual offset")]
        [DefaultValue(0)]
        [Range(-20, 20)]
        //[Tooltip("How late (+) or early (-) notes on a beatmap are visually positioned (in 1/60ths of a second)")]
        public int FKVisualOffset;

        //[Label("4K: Scroll speed")]
        [DefaultValue(1)]
        [Range(0.1f, 5)]
        //[Tooltip("How fast notes scroll!")]
        public float ScrollSpeed;

        //[Label("4K & GH: Reduce note hit animation")]
        //[Tooltip("Minimize the animations for note hits if they're too distracting.")]
        [DefaultValue(false)]
        public bool ReduceNoteHitEffect;

        //[Label("4K & GH: Disable hit sounds")]
        //[Tooltip("Disable hit sounds if they're too distracting.")]
        [DefaultValue(false)]
        public bool DisableHitSounds;

        //[Label("Disable first-time rhythmist setup")]
        //[Tooltip("This is automatically enabled when finishing the rhythmist setup.")]
        [DefaultValue(false)]
        public bool DisableRhythmistSetup;

        //[Label("Replace copyrighted music with mediocre covers")]
        //[Tooltip("These covers will still match with the beatmaps!")]
        [DefaultValue(false)]
        public bool ReplaceCopyrightedMusic;

        [Header("Miscellaneous")]
        //[Label("Don't show unfinished disclaimer")]
        //[Tooltip("This is automatically enabled when first closing the disclaimer.")]
        [DefaultValue(false)]
        public bool DisableDisclaimer;

        //[Label("Fun: Guitar Hero speen mode")]
        [DefaultValue(false)]
        public bool GHSpeen;

        //[Label("Disable modified death messages")]
        [DefaultValue(false)]
        public bool DisableModifiedDeathMessages;

        //[Label("Disable modified game window titles")]
        [DefaultValue(false)]
        public bool DisableModifiedGameWindowTitles;

        [Header("Debug")]

        //[Label("Debug: Timing window for 4K and Guitar Hero weapons")]
        [DefaultValue(10)]
        public int TimingWindow;

        //[Label("Debug: Death message easter egg chance multiplier")]
        [DefaultValue(1)]
        public float EasterEggChanceMult;

        //[Label("Debug: Mashing Penalty Range")]
        [DefaultValue(5)]
        //[Tooltip("Way off range (from an OK) where hits break the combo.")]
        public int ExperimentalPenaltyRange;
        public override void OnChanged()
        {
            // RS: Now these directly reference this config
            //RhythmNotes.Instance.notesUpscroll = FKUpscroll;
            //RhythmNotes.Instance.playfieldAbovePlayer = FKGHPlayfieldAbovePlayer;
            //AlphaMessage.dontShowAgainConfig = DisableDisclaimer;
            //RhythmistSetup.Instance.dontShowAgainConfig = DisableRhythmistSetup;
        }
        public void Save()
        {
            typeof(ConfigManager).GetMethod("Save", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { Instance });
        }
    }
}
