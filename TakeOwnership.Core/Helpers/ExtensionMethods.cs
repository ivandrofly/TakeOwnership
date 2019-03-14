using System.Security.AccessControl;

namespace TakeOwnership.Core.Helpers
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Remove all access of previous owner of type <see cref="SecurityIdentifier"/>
        /// </summary>
        /// <typeparam name="TIdentifier">The parameter can be of typeof <see cref="System.Security.Principal.NTAccount"/> or <see cref="System.Security.Principal.SecurityIdentifier"/> , <see cref=""/></typeparam>
        public static void PurgeAllAccess<TIdentifier>(this ObjectSecurity refId)
        {
            var oldOwner = refId.GetOwner(typeof(TIdentifier));
            refId.PurgeAccessRules(oldOwner);
        }
    }
}
