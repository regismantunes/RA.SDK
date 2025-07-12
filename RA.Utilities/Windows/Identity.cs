using System.Security.Principal;

namespace RA.Utilities.Windows
{
    public static class Identity
    {
        public static string GetCurrentSID()
        {
            return WindowsIdentity.GetCurrent().User?.Value ?? string.Empty;
        }
    }
}