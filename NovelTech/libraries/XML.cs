using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NovelTech.libraries
{
    public static class XML
    {
        public static string get_attribute_from_file(string filename, string element, string attribute)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            return doc.GetElementsByTagName(element)[0].Attributes[attribute].Value;
        }
    }
}
