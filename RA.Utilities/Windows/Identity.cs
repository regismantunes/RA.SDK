using System.Security.Principal;

namespace RA.Utilities.Windows
{
    public static class Identity
    {
        /// <summary>
        /// Gets the current user's Security Identifier (SID).
        /// </summary>
        public static string GetCurrentSID()
        {
            return WindowsIdentity.GetCurrent().User?.Value ?? string.Empty;
        }
    }
}