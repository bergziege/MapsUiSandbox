using System.Xml.Serialization;

namespace GpxDeSerializer.Model;

// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(Gpx));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (Gpx)serializer.Deserialize(reader);
// }

[XmlRoot(ElementName = "link")]
public class Link
{

    [XmlElement(ElementName = "text")]
    public List<string> Text { get; set; }

    [XmlAttribute(AttributeName = "href")]
    public string Href { get; set; }
}

[XmlRoot(ElementName = "metadata")]
public class Metadata
{

    [XmlElement(ElementName = "name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "link")]
    public Link Link { get; set; }

    [XmlElement(ElementName = "time")]
    public DateTime Time { get; set; }
}

[XmlRoot(ElementName = "wpt")]
public class Wpt
{

    [XmlElement(ElementName = "ele")]
    public double Ele { get; set; }

    [XmlElement(ElementName = "name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "type")]
    public string Type { get; set; }

    [XmlAttribute(AttributeName = "lat")]
    public double Lat { get; set; }

    [XmlAttribute(AttributeName = "lon")]
    public double Lon { get; set; }

    //[XmlText]
    //public string Text { get; set; }
}

[XmlRoot(ElementName = "trkpt")]
public class Trkpt
{

    [XmlElement(ElementName = "ele")]
    public double Ele { get; set; }

    //[XmlElement(ElementName = "time")]
    //public DateTime Time { get; set; }

    [XmlAttribute(AttributeName = "lat")]
    public double Lat { get; set; }

    [XmlAttribute(AttributeName = "lon")]
    public double Lon { get; set; }

    //[XmlText]
    //public string Text { get; set; }
}

[XmlRoot(ElementName = "trkseg")]
public class Trkseg
{

    [XmlElement(ElementName = "trkpt")]
    public List<Trkpt> Trkpt { get; set; }
}

[XmlRoot(ElementName = "trk")]
public class Trk
{

    [XmlElement(ElementName = "name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "trkseg")]
    public Trkseg Trkseg { get; set; }
}

[XmlRoot(ElementName = "gpx")]
public class Gpx
{

    [XmlElement(ElementName = "metadata")]
    public Metadata Metadata { get; set; }

    [XmlElement(ElementName = "wpt")]
    public List<Wpt> Wpt { get; set; }

    [XmlElement(ElementName = "trk")]
    public Trk Trk { get; set; }

    [XmlAttribute(AttributeName = "creator")]
    public string Creator { get; set; } = "Segment Planer";

    [XmlAttribute(AttributeName = "version")]
    public string Version { get; set; } = "1.1";

    [XmlAttribute(AttributeName = "schemaLocation")]
    public string SchemaLocation { get; set; } //= "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/11.xsd";

    [XmlAttribute(AttributeName = "xmlns")]
    public string Xmlns { get; set; } //= "http://www.topografix.com/GPX/1/1";

    [XmlAttribute(AttributeName = "xsi")]
    public string Xsi { get; set; } //= "http://www.w3.org/2001/XMLSchema-instance";

    //[XmlText]
    //public string Text { get; set; }
}
