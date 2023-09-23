namespace PFL.Utils
{
    public class MenuHelper
    {
        public static string IsActive(string menuUrl, string controllerName, string actionName)
        {

            bool isActive = menuUrl.Replace("/", "").ToLower() == controllerName.ToLower() + actionName.ToLower().Replace("index", "");

            return isActive ? "active" : "";
        }
    }
}