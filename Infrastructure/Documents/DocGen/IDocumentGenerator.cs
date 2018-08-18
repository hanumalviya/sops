using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocGen
{
    public interface IDocumentGenerator
    {
        void Generate(Stream template, Stream document, Dictionary<string, string> fillData);
    }
}
