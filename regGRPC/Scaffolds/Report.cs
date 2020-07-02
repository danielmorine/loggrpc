using Scaffolds.interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scaffolds
{
    public class Report : IScaffold<Guid>
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string ReportDescription { get; set; }
        public string ReportSource { get; set; }
        public byte LevelTypeID { get; set; }
        public int Events { get; set; }


        [NotMapped]
        public DateTimeOffset CreatedDate { get; set; }
        
        public virtual RegistrationProcess RegistrationProcess { get; set; }   
        public virtual LevelType LevelType { get; set; }
    }
}
