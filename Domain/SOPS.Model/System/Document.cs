using System;
using System.Linq;

namespace Model.System
{
    public class Document
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Path { get; set; }
        public virtual string ContentType { get; set; }
    }
}
