using System.Reflection;
using stardew_access.Translation;

namespace stardew_access.Utils;

public sealed class CommonUIButton
{
    private string _id;
    private CommonUIButton(string name)
    {
        _id = name;
    }

    public override string ToString()
        => Translator.Instance.Translate($"common-ui-{_id}_button", translationCategory: TranslationCategory.Menu);

    public string Key => _id;
    public string Value => ToString();

    public static CommonUIButton? FromFieldInfo(FieldInfo? fieldInfo)
        => FromFieldName(fieldInfo?.Name);

    public static CommonUIButton? FromFieldName(string? fieldName)
    {
        if (string.IsNullOrWhiteSpace(fieldName)) return null;

        fieldName = fieldName.ToLower().Replace("_", "").Replace("button", "");

        return fieldName switch
        {
            "ok" => Ok,
            "done" => Done,
            "confirm" => Confirm,
            "cancel" => Cancel,
            "trashcan" => TrashCan,
            "dropiteminvisible" or "dropitem" => DropItem,
            "uparrow" or "up" or "scrollup" => ScrollUp,
            "downarrow" or "down" or "scrolldown" => ScrollDown,
            "nextpage" or "next" => NextPage,
            "previouspage" or "previous" or "prev" => PreviousPage,
            "upperrightclose" or "close" => CloseMenu,
            "back" => Back,
            "forward" => Forward,
            "organize" or "organizeinventory" => OrganizeInventory,
            "junimonoteicon" or "communitycenter" => CommunityCenter,
            _ => null,
        };
    }

    public static readonly CommonUIButton Ok = new("ok");
    public static readonly CommonUIButton Done = new("ok");
    public static readonly CommonUIButton Confirm = new("confirm");
    public static readonly CommonUIButton Cancel = new("cancel");
    public static readonly CommonUIButton TrashCan = new("trashcan");
    public static readonly CommonUIButton DropItem = new("drop_item");
    public static readonly CommonUIButton ScrollUp = new("scroll_up");
    public static readonly CommonUIButton ScrollDown = new("scroll_down");
    public static readonly CommonUIButton NextPage = new("next_page");
    public static readonly CommonUIButton PreviousPage = new("previous_page");
    public static readonly CommonUIButton CloseMenu = new("close_menu");
    public static readonly CommonUIButton Back = new("back");
    public static readonly CommonUIButton Forward = new("forward");
    public static readonly CommonUIButton OrganizeInventory = new("organize_inventory");
    public static readonly CommonUIButton CommunityCenter = new("community_center");
}
