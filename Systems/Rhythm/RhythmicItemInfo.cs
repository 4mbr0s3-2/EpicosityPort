using EpicosityPort.Systems.UserInterface.Rhythm;
using EpicosityPort.Systems.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Utilities;
using static EpicosityPort.Systems.Rhythm.RhythmHandler;

namespace EpicosityPort.Systems.Rhythm
{
    //TODO: Maybe move everything to RhythmicWeaponBase? This takes up like 32 bytes for each item stack
    // RS: Why was I concerned about this
    // 32 bytes is NOTHING
    public class RhythmicItemInfo : GlobalItem
    {
        /// <summary>
        /// Whether or not the item is a rhythmist item.
        /// </summary>
        public bool rhythmic = false;
        /// <summary>
        /// Arrow-based: The default number of ticks between each note (or the crotchet). This should be set when making new items.
        /// </summary>
        public int defaultNoteRate = 60;
        /// <summary>
        /// The actual number of ticks between each note (or the crotchet). This should be set when using prefixes.
        /// </summary>
        public int noteRate = 60;
        /// <summary>
        /// The default value removed from the combo meter every tick. This should be set when making new items.
        /// </summary>
        public float defaultComboDepletionRate = 0.003f * 0.5f;
        /// <summary>
        /// The actual value removed from the combo meter every tick. This should be set when using prefixes.
        /// </summary>
        public float comboDepletionRate = 0.003f * 0.5f;
        /// <summary>
        /// The default value multiplied by the combo increment. This should be set when making new items.
        /// </summary>
        public float defaultComboIncrement = 0;
        /// <summary>
        /// The actual value multiplied by the combo increment. This should be set when using prefixes.
        /// </summary>
        public float comboIncrement = 0;
        /// <summary>
        /// Setting this to anything other than PrefixDependent would force the pattern regardless of the reforge of the item.
        /// GameplayType must be Arrow for this to count.
        /// </summary>
        public bool forcePattern = false;
        public FourKeyTrackPatternObject forcedArrowTrackPattern = null;
        public MouseTrackPatternObject forcedMouseTrackPattern = null;
        public GameplayType gameplayType = RhythmHandler.GameplayType.Note;
        public GameplayNoteDisplayStyle gameplayNoteDisplayStyle = GameplayNoteDisplayStyle.FourKey;

        public string fourKeySkin = "";

        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;
        public override void SetDefaults(Item item)
        {
            // Called after SetDefaults() in RhythmicWeaponBase
            noteRate = defaultNoteRate;
            comboDepletionRate = defaultComboDepletionRate;
            comboIncrement = defaultComboIncrement;
            if (rhythmic && item.UseSound == null)
            {
                item.UseSound = new SoundStyle($"{typeof(RhythmInterface).GetNamespaceAsPath()}/Sounds/RhythmicHit"); // Default sound
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (rhythmic)
            {
                tooltips.Find((tooltip1) => tooltip1.Name == "Damage").Text = Main.LocalPlayer.GetWeaponDamage(item) + " rhythmic damage";
                #region Display modifiers
                //if (trackSpeed != defaultTrackSpeed)
                //{
                //    double calculatedTrackSpeed = ((trackSpeed - defaultTrackSpeed) / defaultTrackSpeed) * 100f;
                //    tooltips.Add
                //    (
                //        new TooltipLine
                //        (
                //            mod,
                //            "TooltipTrackSpeed",
                //            (calculatedTrackSpeed > 0 ? "+" : "") + calculatedTrackSpeed + "% track speed"
                //        )
                //        {
                //            isModifier = true,
                //            isModifierBad = calculatedTrackSpeed > 0
                //        }
                //    );
                //}
                if (noteRate != defaultNoteRate)
                {
                    float calculatedNoteRate = (((float)noteRate - defaultNoteRate) / defaultNoteRate) * 100f;
                    tooltips.Add
                    (
                        new TooltipLine
                        (
                            Mod,
                            "TooltipArrowRate",
                            (calculatedNoteRate > 0 ? "+" : "") + calculatedNoteRate + "% note rate"
                        )
                        {
                            IsModifier = true,
                            IsModifierBad = calculatedNoteRate < 0
                        }
                    );
                }
                if (comboIncrement != defaultComboIncrement)
                {
                    float calculatedComboIncrement = (((float)comboIncrement - defaultComboIncrement) / defaultComboIncrement) * 100f;
                    tooltips.Add
                    (
                        new TooltipLine
                        (
                            Mod,
                            "TooltipComboIncrement",
                            (calculatedComboIncrement > 0 ? "+" : "") + calculatedComboIncrement + "% combo increment"
                        )
                        {
                            IsModifier = true,
                            IsModifierBad = calculatedComboIncrement < 0
                        }
                    );
                }
                if (comboDepletionRate != defaultComboDepletionRate)
                {
                    float calculatedComboDepletionRate = (((float)comboDepletionRate - defaultComboDepletionRate) / defaultComboDepletionRate) * 100f;
                    tooltips.Add
                    (
                        new TooltipLine
                        (
                            Mod,
                            "TooltipComboDepletionRate",
                            (calculatedComboDepletionRate > 0 ? "+" : "") + calculatedComboDepletionRate + "% combo depletion rate"
                        )
                        {
                            IsModifier = true,
                            IsModifierBad = calculatedComboDepletionRate > 0
                        }
                    );
                }
                #endregion
                #region Display full combo tooltip
                if (item.ModItem is RhythmicWeaponBase)
				{
                    RhythmicWeaponBase rhythmicWeapon = item.ModItem as RhythmicWeaponBase;
                    string comboBarBonus = rhythmicWeapon.ComboBarBonusText();
                    if (comboBarBonus != "")
					{
                        tooltips.Add
                        (
                            new TooltipLine
                            (
                                Mod,
                                "ComboBarBonus",
                                $"[c/FFFF00:Combo bar bonus:]\n{comboBarBonus}"
                            )
                        );
					}
                }
                #endregion
            }
        }
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            // TODO: More prefixes!
            RhythmicItemInfo itemInfo = item.GetGlobalItem<RhythmicItemInfo>();
            if (itemInfo.rhythmic)
            {
                if (itemInfo.forcedArrowTrackPattern != null || itemInfo.forcedMouseTrackPattern != null)
				{
                    return -1;
				}
                else
				{
                    if (rand.Next(0, 10) < 10)
					{
                        if (itemInfo.gameplayType == GameplayType.Note)
					    {
                            return RhythmNotes.Instance.prefixTypeToFourKeyTrackPattern.Keys.ToList()[rand.Next(RhythmNotes.Instance.prefixTypeToFourKeyTrackPattern.Count())];
                        }
                        else if (itemInfo.gameplayType == GameplayType.Note)
					    {
                            return RhythmMouse.Instance.prefixTypeToMouseTrackPattern.Keys.ToList()[rand.Next(RhythmMouse.Instance.prefixTypeToMouseTrackPattern.Count())];
                        }
					}
                    return base.ChoosePrefix(item, rand);
                }
            }
            else
            {
                return base.ChoosePrefix(item, rand);
            }
        }
    }
}
