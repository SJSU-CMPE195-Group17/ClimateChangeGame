using System.IO;
using System.Xml.Serialization;

public static class XmlOperation
{    
    public static void Serialize(object item, string path)
    {
        XmlSerializer serializer = new XmlSerializer(item.GetType());
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, item);
        writer.Close();
    }

    public static ResourcesContainer Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ResourcesContainer));
        StreamReader reader = new StreamReader(path);
        ResourcesContainer deserialized = serializer.Deserialize(reader.BaseStream) as ResourcesContainer;
        reader.Close();
        return deserialized;
    }


}