namespace PKWAT.ScoringPoker.Domain.Abstraction
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEntity<out T>
    {
        T Id { get; }
    }
}
