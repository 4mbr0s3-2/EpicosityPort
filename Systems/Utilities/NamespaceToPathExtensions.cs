using Terraria.Audio;

namespace EpicosityPort.Systems.Utilities
{
    internal static class NamespaceToPathExtensions
    {
        public static string GetQualifiedNameAsPath(this System.Type type)
        {
            return type.FullName.Replace(".", "/");
        }
        public static string GetNamespaceAsPath(this System.Type type)
        {
            return type.Namespace.Replace(".", "/");
        }
        public static SoundStyle GetSound(this System.Type type, string name)
        {
            return new SoundStyle($"{GetNamespaceAsPath(type)}/{name}");
        }
    }
}
