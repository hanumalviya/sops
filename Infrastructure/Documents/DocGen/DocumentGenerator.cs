using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocGen
{
    public class DocumentGenerator : IDocumentGenerator
    {
        public void Generate(Stream template, Stream output, Dictionary<string, string> fillData)
        {
            using (DocX docx = DocX.Load(template))
            {
                foreach (var item in fillData)
                {
                    docx.ReplaceText(item.Key, item.Value?? string.Empty);
                }

                docx.SaveAs(output);
            }
        }
    }
}
