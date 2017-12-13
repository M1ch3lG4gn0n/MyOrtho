using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyOrthoOrtho.Controllers
{
    public class XmlHelper
    {
        private XmlDocument document = new XmlDocument();
        private XmlElement root;

        public XmlHelper(bool withHeader)
        {
            if (withHeader)
            {
                XmlDeclaration xmlDeclaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                root = document.DocumentElement;
                document.InsertBefore(xmlDeclaration, root);
            }
        }

        public XmlElement AddToRoot(string name, string value)
        {
            XmlElement node = document.CreateElement(string.Empty, name, string.Empty);
            if (!string.IsNullOrEmpty(value))
            {
                XmlText textNode = document.CreateTextNode(value);
                node.AppendChild(textNode);
            }
            document.AppendChild(node);
            return node;
        }

        public XmlElement AppendToNode(XmlElement node, string name, string value)
        {
            XmlElement childNode = document.CreateElement(string.Empty, name, string.Empty);
            if (!string.IsNullOrEmpty(value))
            {
                XmlText textValue = document.CreateTextNode(value);
                childNode.AppendChild(textValue);
            }
            node.AppendChild(childNode);
            return childNode;
        }

        public void Save(string path)
        {
            document.Save(path);
        }
        
    }
}
