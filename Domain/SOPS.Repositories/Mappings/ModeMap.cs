using FluentNHibernate.Mapping;
using Model.University;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class ModeMap : ClassMap<Mode>
    {
        public ModeMap()
        {
            Id(n => n.Id);
            Map(n => n.Name);
        }
    }
}
