using Scaffolds.interfaces;
using System;

namespace Scaffolds
{
    public class RegistrationProcess : IScaffold<Guid>
    {
        public DateTimeOffset CreatedDate { get; set; }
        public Guid ID { get; set; }
        public bool IsActive { get; set; }
        public Guid ReportID { get; set; }
        public Guid OwnerID { get; set; }
        public byte EnvironmentTypeID { get; set; }

        public virtual Report Report { get; set; }
        public EnvironmentType EnvironmentType { get; set; } 
    }
}
