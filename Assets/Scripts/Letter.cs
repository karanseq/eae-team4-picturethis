using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class Letter
{
    [XmlAttribute("index")]
    public int index;
    [XmlAttribute("value")]
    public string value;
}
