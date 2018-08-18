using FluentNHibernate.Mapping;
using Model.System;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class DocumentMap : ClassMap<Document>
    {
        public DocumentMap()
        {
            Id(n => n.Id);
            Map(n => n.Name);
            Map(n => n.Path);
            Map(n => n.ContentType);
        }
    }
}
