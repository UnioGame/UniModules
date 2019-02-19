using TMPro;

namespace UniModule.UnityTools.UiViews
{
    public static class UiExtension
    {

        public static bool SetText(this TextMeshProUGUI field, string text)
        {
            if (!field) return false;
            if (string.Equals(field.text, text))
                return false;

            field.text = text;

            return true;
        }

    }
}
