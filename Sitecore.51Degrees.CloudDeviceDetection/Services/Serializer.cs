using System.Web.Script.Serialization;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services
{
    public interface ISerializer
    {
        T Deserialize<T>(string serializedData);
    }

    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string serializedData)
        {
            return new JavaScriptSerializer().Deserialize<T>(serializedData);
        }
    }
}
