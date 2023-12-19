namespace PKWAT.ScoringPoker.Domain.Abstraction
{
    using System;

    internal static class ObjectExtensions
    {
        public static Type GetUnproxiedType(this object obj)
        {
            const string EFCoreProxyPrefix = "Castle.Proxies.";
            const string NHibernateProxyPostfix = "Proxy";

            var type = obj.GetType();
            var typeString = type.ToString();

            if (typeString.Contains(EFCoreProxyPrefix) || typeString.EndsWith(NHibernateProxyPostfix))
            {
                return type.BaseType;
            }

            return type;
        }
    }
}
