using GpxDeSerializer.Model;
using System.Xml.Serialization;

namespace GpxDeSerializer;

public class GpxDeserializer
{
    public async Task<Gpx> DeSerialize(string filePath)
    {
        
        Gpx gpx = null;

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Gpx), "http://www.topografix.com/GPX/1/1");
            await Task.Run(() =>
            {
                using (FileStream reader = File.OpenRead(filePath))
                {
                    gpx = (Gpx)serializer.Deserialize(reader);
                }
            });
        }
        catch (Exception)
        {

        }

        if (gpx == null)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Gpx), "http://www.topografix.com/GPX/1/0");
                await Task.Run(() =>
                {
                    using (FileStream reader = File.OpenRead(filePath))
                    {
                        gpx = (Gpx)serializer.Deserialize(reader);
                    }
                });
            }
            catch (Exception)
            {
                throw;
            } 
        }

        return gpx;
    }
}