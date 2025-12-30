using UnityEditor;

namespace EvieEngine.Editor
{
    public static class TogglePlayModeOptions
    {
        [MenuItem("Evie Engine/Поведение движка/Быстрая перезагрузка сцены/Включить")]
        public static void On()
        {
            EditorSettings.enterPlayModeOptionsEnabled = true;
            EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;
            EditorUtility.DisplayDialog("Быстрая перезагрузка сцены",
                "Теперь используется: Reload Scene ONLY (No Domain Reload)", "OK");
        }

        [MenuItem("Evie Engine/Поведение движка/Быстрая перезагрузка сцены/Выключить")]
        public static void Off()
        {
            EditorSettings.enterPlayModeOptionsEnabled = false;
            EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.None;
            EditorUtility.DisplayDialog("Быстрая перезагрузка сцены", "Теперь используется: Reload Scene AND Domain",
                "OK");
        }

        [MenuItem("Evie Engine/Поведение движка/Быстрая перезагрузка сцены/Информация")]
        public static void Info()
        {
            EditorSettings.enterPlayModeOptionsEnabled = false;
            EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.None;
            EditorUtility.DisplayDialog("Быстрая перезагрузка сцены",
                "Режим Reload Scene ONLY ускоряет запуск, но может вызывать баги:\n\n— Статические поля и события не сбрасываются\n— Синглтоны и кэш остаются от прошлого запуска\n— Поведение может отличаться от билда\n\nИспользуй только для тестов и верни стандартный режим перед сборкой.",
                "OK");
        }
    }
}