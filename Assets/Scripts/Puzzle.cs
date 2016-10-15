using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("Puzzle")]
public class Puzzle
{
    [XmlArray("Words")]
    [XmlArrayItem("Word")]
    public List<Word> words = new List<Word>();
}
