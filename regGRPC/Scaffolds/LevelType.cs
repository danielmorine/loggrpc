using Scaffolds.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scaffolds
{
    public class LevelType : IScaffold<byte>
    {
        [NotMapped]
        public DateTimeOffset CreatedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public byte ID { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
    }
}
