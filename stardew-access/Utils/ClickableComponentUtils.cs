using System.Collections;
using System.Reflection;
using StardewValley;
using StardewValley.Menus;

namespace stardew_access.Utils;

public partial class ClickableComponentUtils
{
    public static bool NarrateHoveredComponentUsingReflectionInMenu(IClickableMenu menu, bool skipAllClickableComponents = true)
    {
        if (menu is null) return false;

        var fields = menu.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        var ccFieldInfos = fields.Where(x => IsAcceptableField(x));
        if (ccFieldInfos.Count() == 0)
        {
            return false;
        }

#if DEBUG
        string fieldNamesList = string.Join('\n', ccFieldInfos.Select(x => $"{x.FieldType}\t{x.Name}"));
        Log.Debug($"[ClickableComponentUtils] Fields found to check and narrate, menu in question: {menu.GetType().FullName}\n{fieldNamesList}", once: true);
#endif

        int x = Game1.getMouseX(true), y = Game1.getMouseY(true);

        // First non generic fields should be checked as we can use the field names to detect common ui buttons

        foreach (var fieldInfo in ccFieldInfos)
        {
            if (fieldInfo.FieldType.IsGenericType) continue;

            ClickableComponent? cc = (ClickableComponent?)fieldInfo.GetValue(menu);
            if (!IsHovered(cc, x, y)) continue;

            CommonUIButton? commonButtonType = CommonUIButton.FromFieldInfo(fieldInfo);
            NarrateComponent(cc!, commonButtonType: commonButtonType);
            return true;
        }

        foreach (var fieldInfo in ccFieldInfos)
        {
            if (!fieldInfo.FieldType.IsGenericType) continue;

            // Cannot directly convert to List<T> as we don't exactly know what type is (although we do know that it'll either be `ClickableComponent` or one of it's derived class)
            // Ref: https://stackoverflow.com/a/14129466
            IList? ccList = (IList?)fieldInfo.GetValue(menu);
            if (ccList is null) continue;
            for (int j = 0; j < ccList.Count; j++)
            {
                object? cco = ccList[j];
                if (cco is not ClickableComponent cc) continue;
                if (!IsHovered(cc, x, y)) continue;

                NarrateComponent(cc!);
                return true;
            }
        }

        return false;
    }

    private static bool IsAcceptableField(FieldInfo x) => !(x.Name.Equals("currentlySnappedComponent") || x.Name.Equals("allClickableComponents"))
        && (x.FieldType.IsGenericType
                ? x.FieldType.GetGenericTypeDefinition() == typeof(List<>) && IsInstanceOfCC(x.FieldType.GetGenericArguments()[0])
                : IsInstanceOfCC(x.FieldType));

    private static bool IsInstanceOfCC(Type fieldType) => fieldType == typeof(ClickableComponent) || fieldType.IsSubclassOf(typeof(ClickableComponent));

    public static bool NarrateHoveredComponentFromList<T>(List<T> clickableComponents) where T : ClickableComponent
    {
        if (clickableComponents == null || clickableComponents.Count == 0) return false;

        int x = Game1.getMouseX(true), y = Game1.getMouseY(true);
        for (int i = 0; i < clickableComponents.Count; i++)
        {
            if (!IsHovered(clickableComponents[i], x, y)) continue;

            NarrateComponent(clickableComponents[i]);
            return true;
        }

        return false;
    }

    private static bool IsHovered(ClickableComponent? cc, int x, int y) => cc is not null && cc.visible && cc.bounds.Contains(x, y);

    public static void NarrateComponent(ClickableComponent clickableComponent, CommonUIButton? commonButtonType = null, bool screenReaderInterrupt = true)
    {
        /* v1.6.9
        if (clickableComponent.ScreenReaderIgnore) return;

        string toSpeak = commonButtonType is null
            ? string.IsNullOrWhiteSpace(clickableComponent.ScreenReaderText)
                ? string.IsNullOrWhiteSpace(clickableComponent.name) ? clickableComponent.label : clickableComponent.name
                : clickableComponent.ScreenReaderText
            : commonButtonType.Value;
        */

        string toSpeak = commonButtonType is null
            ? string.IsNullOrWhiteSpace(clickableComponent.name) ? clickableComponent.label : clickableComponent.name
            : commonButtonType.Value;

        MainClass.ScreenReader.SayWithMenuChecker(toSpeak, interrupt: screenReaderInterrupt);
    }
}
