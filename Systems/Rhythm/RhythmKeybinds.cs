using Terraria.ModLoader;

namespace EpicosityPort.Systems.Rhythm
{
    public class RhythmKeybinds
    {
        public ModKeybind downKey;
        public ModKeybind leftKey;
        public ModKeybind rightKey;
        public ModKeybind strumKey;
        public ModKeybind strumKey2;
        public ModKeybind targetSwitchLeft;
        public ModKeybind targetSwitchRight;

        public ModKeybind upKey;

        //public override void Load()
        //{
        //    LoadRhythmHotKeys(Mod);
        //}
        public void LoadRhythmHotKeys(Mod mod)
        {
            leftKey = KeybindLoader.RegisterKeybind(mod, "Rhythm Button 1", "Left");
            upKey = KeybindLoader.RegisterKeybind(mod, "Rhythm Button 2", "Up");
            downKey = KeybindLoader.RegisterKeybind(mod, "Rhythm Button 3", "Down");
            rightKey = KeybindLoader.RegisterKeybind(mod, "Rhythm Button 4", "Right");
            targetSwitchLeft = KeybindLoader.RegisterKeybind(mod, "Rhythm Switch Target (Left)", "LeftAlt");
            targetSwitchRight = KeybindLoader.RegisterKeybind(mod, "Rhythm Switch Target (Right)", "RightAlt");
            strumKey = KeybindLoader.RegisterKeybind(mod, "Rhythm Strum", "Space");
            strumKey2 = KeybindLoader.RegisterKeybind(mod, "Rhythm Strum (Secondary)", "Space");
        }

        //public void UnloadRhythmHotKeys()
        //{
        //    leftKey = null;
        //    upKey = null;
        //    downKey = null;
        //    rightKey = null;
        //    targetSwitchLeft = null;
        //    targetSwitchRight = null;
        //    strumKey = null;
        //    strumKey2 = null;
        //}
    }
}