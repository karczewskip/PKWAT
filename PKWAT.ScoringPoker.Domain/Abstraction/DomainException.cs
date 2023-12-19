namespace PKWAT.ScoringPoker.Domain.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception inner) : base(message, inner)
        {
        }

        public static void ThrowIf(bool condition, string message)
        {
            if (condition)
            {
                throw new DomainException(message);
            }
        }
    }
}
