using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

public class Word
{
    [XmlArray("Letters")]
    [XmlArrayItem("Letter")]
    public List<Letter> letters = new List<Letter>();
}
