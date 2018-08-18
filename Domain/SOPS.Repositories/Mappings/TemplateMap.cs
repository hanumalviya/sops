using FluentNHibernate.Mapping;
using Model.System;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class TemplateMap : ClassMap<Template>
    {
        public TemplateMap()
        {
            Id(n => n.Id);
            Map(n => n.Name);
            Map(n => n.FilePath);
            Map(n => n.ContentType);
            HasMany(n => n.Courses).Inverse();
        }
    }
}
