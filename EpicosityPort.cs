using EpicosityPort.Systems.Network;
using EpicosityPort.Systems.UserInterface.VanillaOverrides;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace EpicosityPort
{
    public class EpicosityPort : Mod
    {
        //[Flags]
        //public enum CompatibilityMods
        //{
        //    CalamityMod = 1,
        //    ThoriumMod = 2,
        //    TerrariaOverhaul = 4,
        //    CalamityModMusic = 8,
        //    JoostMod = 16,
        //    Macrocosm = 32,
        //}

        //public static CompatibilityMods loadedCompatibilityMods = 0;
        //public int HasOneShotGame = 0;
        //public static bool CalamityTupacMenu = false;


        //public static List<MethodInfo> LoadMethods;
        //public static List<MethodInfo> UnloadMethods;

        public static EpicosityPort Instance
        {
            get; private set;
        }

        // RS: It's been 5 years since I wrote this. I mean, it can't be *that* bad, right? -- 4mbr0s3 2
        private static string noteToEveryone = "Most of the code in this mod is old and scuffed and should have had better implementations. Expect unnecessarily static fields, huge if-statement nests, and other terrible practices. --4mbr0s3 2";
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModNetHandler.Instance.HandlePacket(reader, whoAmI);
        }
        //public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)/* tModPorter Note: Removed. Use ModSystem.ModifyInterfaceLayers */
        //{
        //InterfaceHandler.ModifyInterfaceLayers(layers);
        //}
        public EpicosityPort()
        {
            ContentAutoloadingEnabled = true;
            GoreAutoloadingEnabled = true;
            MusicAutoloadingEnabled = true;
            BackgroundAutoloadingEnabled = true;

            Instance = this;
        }
        public override void PostSetupContent()
        {
            // Boss Checklist support
            //ModSupport.BossChecklist.PostSetupContent(this);
            //ModSupport.Census.PostSetupContent(this);

            //foreach (string mod in Enum.GetNames(typeof(CompatibilityMods)))
            //{
            //    if (ModLoader.GetMod(mod) != null)
            //    {
            //        loadedCompatibilityMods |= (CompatibilityMods)Enum.Parse(typeof(CompatibilityMods), mod);
            //    }
            //}

            IL_Main.DrawMenu += MainMenuOverride.HookMenuSplash;

            //if (Main.netMode != NetmodeID.Server)
            //{
            //    if (ModLoader.GetMod("CalamityModMusic") != null && Main.rand.NextBool(10) && !loadedCompatibilityMods.HasFlag(CompatibilityMods.TerrariaOverhaul))
            //    {
            //        CalamityTupacMenu = true;
            //        Terraria.IL_Main.UpdateAudio += ILHooks.Main_UpdateAudio_CalamityEasterEgg;
            //    }
            //}
            MainMenuOverride.SetMainMenuSplashText();
        }
        //public void UnloadReflection()
        //{
        //    // These methods have already been net-checked
        //    //foreach (MethodInfo method in UnloadMethods)
        //    //{
        //    //    method.Invoke(null, new object[0]);
        //    //}
        //    //LoadMethods = null;
        //    //UnloadMethods = null;
        //}

        // RS: Maybe scanning for scattered static methods throughout the assembly and randomly calling them to load and unload... isn't a good idea?
        //public void LoadReflection()
        //{
        //    bool CheckMethodNetmode(LoadManagerAttribute memoryManage)
        //    {
        //        bool addMethod = false;
        //        if (Main.dedServ || Main.netMode == NetmodeID.Server)
        //        {
        //            addMethod = memoryManage.netMode.HasFlag(LoadManagerAttribute.ManageNetmode.Server);
        //        }
        //        else
        //        {
        //            addMethod = memoryManage.netMode.HasFlag(LoadManagerAttribute.ManageNetmode.Client);
        //        }
        //        return addMethod;
        //    }

        //    // Initalize lists
        //    LoadMethods = new List<MethodInfo>();
        //    UnloadMethods = new List<MethodInfo>();

        //    Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
        //    foreach (Type type in allTypes)
        //    {
        //        MethodInfo[] methodInfos = type.GetMethods();
        //        foreach (MethodInfo methodInfo in methodInfos)
        //        {
        //            IEnumerable<Attribute> attributes = methodInfo.GetCustomAttributes().Where((attribute) => attribute is LoadManagerAttribute);
        //            foreach (LoadManagerAttribute memoryManage in attributes)
        //            {
        //                if (CheckMethodNetmode(memoryManage))
        //                {
        //                    if (memoryManage.type == LoadManagerAttribute.ManageType.Load)
        //                    {
        //                        LoadMethods.Add(methodInfo);
        //                    }
        //                    else
        //                    {
        //                        UnloadMethods.Add(methodInfo);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    foreach (MethodInfo method in LoadMethods)
        //    {
        //        method.Invoke(null, new object[0]);
        //    }
        //}
        public override void Load()
        {



            // RS: DUDE WHAT WAS I THINKING 😂😂😂
            // Exclusive in-game mod content for owning another game is crazy

            //if (!Main.dedServ)
            //{
            //    if (FileUtilities.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/Oneshot/save.dat", false))
            //    {
            //        HasOneShotGame = 1;
            //    }
            //    if (FileUtilities.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/My Games/Oneshot/BigPortal/keyR.txt", false))
            //    {
            //        HasOneShotGame = 2;
            //    }
            // Also on by default, but doesn't draw until inventory conditions are met
            // InterfaceHandler.LoadInterfaces();
            // FilterLoader.LoadFilters();
            //}

            InitializeHooks();

            //ManualLoadItems();
        }
        public override void Unload()
        {
            //UnloadReflection();
            //inst = null; // Sayonara
        }
        //private void ManualLoadItems()
        //{
        //    //AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Bonus_NATWM"), ModContent.ItemType<NATWMMusicBox>(), ModContent.TileType<NATWMMusicBoxTile>());
        //    //AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/ChildrenOfTheRuins"), ModContent.ItemType<COTRMusicBox>(), ModContent.TileType<COTRMusicBoxTile>());
        //    LoadCrossModItems();
        //}
        public void LoadCrossModItems()
        {
            //if (ModLoader.GetMod("EpicosityThorium") != null)
            //{
            //	LoadThoriumItems();
            //}
            //else
            //{
            //LoadItemsNormally();
            //}
        }

        //public void LoadThoriumItems()
        //{

        //}

        //public void LoadItemsNormally()
        //{
        //    //AddItem("StarGuitar", new StarGuitar());
        //}
        //public override void MidUpdateNPCGore()/* tModPorter Note: Removed. Use ModSystem.PostUpdateNPCs or ModSystem.PreUpdateGores */
        //{
        //    //if (Main.netMode == NetmodeID.Server)
        //    //{
        //    //    RhythmBeatmapHandler.inst?.Update();
        //    //}
        //}
        void InitializeHooks()
        {
            //ILHooks.SetupHooks();
        }
        public void UpdateMusicAccessories(ref int music, ref SceneEffectPriority priority)
        {
            Player player = Main.player[Main.myPlayer];
            //if (player.armor[10].type == ItemID.ClownHat && player.armor[11].type == ItemID.ClownShirt && player.armor[12].type == ItemID.ClownPants && player.mount.Active && player.mount._type == ModContent.MountType<ClownBallMount>())
            //{
            //    music = GetSoundSlot(SoundType.Music, "Sounds/Music/CircusThemeSong");
            //    priority = SceneEffectPriority.Event;
            //}
            //else if (player.mount.Active && player.mount._type == ModContent.MountType<NyanCatMount>() && player.HeldItem.type == ItemID.Meowmere)
            //{
            //    music = GetSoundSlot(SoundType.Music, "Sounds/Music/Nyanyanyanyanyanyanya");
            //    priority = SceneEffectPriority.Event;
            //}
            //else if (player.HeldItem.type == ModContent.ItemType<StarGuitar>() && player.GetModPlayer<ModPlayerStarGuitarAnimation>().enabledBrodyquest)
            //{
            //    music = GetSoundSlot(SoundType.Music, "Sounds/Music/BRODYQUEST");
            //    priority = SceneEffectPriority.Environment;
            //}
        }
        public void UpdateMusicEvents(ref int music, ref SceneEffectPriority priority)
        {
            //if (TheUltimateShowdownWorld.showdownInvasionUp && (Main.invasionX == (double)Main.spawnTileX))
            //{
            //    music = GetSoundSlot(SoundType.Music, "Sounds/Music/TheUltimateShowdownInvasion");
            //    if (Main.hardMode)
            //    {
            //        music = GetSoundSlot(SoundType.Music, "Sounds/Music/TheUltimateShowdownInvasionLyrics");
            //    }
            //    priority = SceneEffectPriority.BossHigh;
            //}
            //else if (DummyInvasionWorld.dummyInvasionUp && (Main.invasionX == (double)Main.spawnTileX))
            //{
            //    music = GetSoundSlot(SoundType.Music, "Sounds/Music/Dummy");
            //    priority = SceneEffectPriority.Event;
            //}
            //else if (BarrelInvasionWorld.barrelInvasionUp && (Main.invasionX == (double)Main.spawnTileX))
            //{
            //    music = GetSoundSlot(SoundType.Music, "Sounds/Music/BarrelInvasion");
            //    priority = SceneEffectPriority.Event;
            //}
        }
        //public override void UpdateMusic(ref int music, ref SceneEffectPriority priority)/* tModPorter Note: Removed. Use ModSceneEffect.Music and .Priority, aswell as ModSceneEffect.IsSceneEffectActive */
        //{
        //    #region Rhythm
        //    if (!(Main.netMode == NetmodeID.SinglePlayer && !Main.hasFocus))
        //    {
        //        RhythmBeatmapHandler.inst?.Update();
        //        if (RhythmBeatmapHandler.inst?.TrackActive == true)
        //        {
        //            music = 0;
        //            priority = SceneEffectPriority.BossHigh;
        //        }
        //    }
        //    #endregion
        //    UpdateMusicEvents(ref music, ref priority);
        //    #region Environmental Events
        //    if (ChocolateRainManager.IsRainingChocolate && (double)Main.screenPosition.Y < Main.worldSurface * 16.0)
        //    {
        //        music = GetSoundSlot(SoundType.Music, "Sounds/Music/ChocolateRain");
        //        priority = SceneEffectPriority.Environment;
        //    }
        //    #endregion
        //    #region Local player dependent
        //    // Circus music
        //    if (Main.myPlayer < 0)
        //    {
        //        return;
        //    }
        //    UpdateMusicAccessories(ref music, ref priority);
        //    foreach (NPC npc in Main.npc)
        //    {
        //        if (npc.type == ModContent.NPCType<DancingPallbearers>() && Main.LocalPlayer.respawnTimer > 0)
        //        {
        //            music = 0;
        //            priority = SceneEffectPriority.BossMedium;
        //            return;
        //        }
        //    }
        //    if (MainMenuUI.deathMessage == "GAME OVER IWBTG" && Main.LocalPlayer.respawnTimer > 0)
        //    {
        //        music = 0;
        //        priority = SceneEffectPriority.BossMedium;
        //    }
        //    int geddanSoundSlot = GetSoundSlot(SoundType.Music, "Sounds/Music/Geddan");
        //    Player player = Main.player[Main.myPlayer];
        //    if (player.HeldItem.type == ModContent.ItemType<Geddan>() && player.GetModPlayer<ModPlayerGeddan>().geddanTime >= 0)
        //    {
        //        priority = SceneEffectPriority.BossHigh;
        //        music = geddanSoundSlot;
        //        for (int m = 1; m < Main.music.Length; m++)
        //        {
        //            if (m != music)
        //            {
        //                Main.musicFade[m] -= 0.08f; // Speed up fade out
        //            }
        //            else
        //            {
        //                Main.musicFade[m] += 0.08f; // Speed up fade in
        //            }
        //        }
        //    }

        //    #endregion
        //}
        //public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)/* tModPorter Note: Removed. Use ModSystem.ModifyTransformMatrix */
        //{
        //    int vertigo = ModContent.BuffType<Vertigo>();
        //    if (vertigo < Main.LocalPlayer.buffImmune.Length)
        //    {
        //        bool hasVertigo = Main.LocalPlayer.HasBuff(vertigo);
        //        if (hasVertigo)
        //        {
        //            Vector2 originalZoom = Transform.Zoom;
        //            Transform.Zoom = originalZoom * (float)(Math.Sin(Main.GlobalTimeWrappedHourly) * 0.1f + 1.1);
        //            Matrix modifiedMatrix = Transform.TransformationMatrix * Matrix.CreateRotationZ((float)(Math.Cos(Main.GlobalTimeWrappedHourly) * 0.1f));
        //            FieldInfo info = Transform.GetType().GetField("_transformationMatrix", BindingFlags.Instance | BindingFlags.NonPublic);
        //            info?.SetValue(Transform, modifiedMatrix);
        //        }
        //    }

        //    if (RhythmBeatmapHandler.inst?.TrackActive == true && RhythmBeatmapHandler.inst.conductor != null)
        //    {
        //        Transform.Zoom *= (float)RhythmBeatmapHandler.inst.conductor.zoomPumper;
        //    }
        //}
    }
}