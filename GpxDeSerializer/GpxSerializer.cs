using GpxDeSerializer.Model;
using System.Xml.Serialization;

namespace GpxDeSerializer;

public class GpxSerializer
{
    public async Task Serialize(Gpx gpx, string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Gpx), "http://www.topografix.com/GPX/1/1");
        await Task.Run(() =>
        {
            using (FileStream reader = File.Create(filePath))
            {
                serializer.Serialize(reader, gpx);
            }
        });
    }
}