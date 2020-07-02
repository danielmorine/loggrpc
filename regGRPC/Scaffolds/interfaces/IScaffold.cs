using System;

namespace Scaffolds.interfaces
{
    public interface IScaffold { }

    public interface IScaffold<T> : IScaffold where T : struct
    {
        DateTimeOffset CreatedDate { get; set; }
        T ID { get; set; }

    }   
}
