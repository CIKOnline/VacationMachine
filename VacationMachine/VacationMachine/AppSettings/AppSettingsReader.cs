using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VacationMachine.AppSettings;

public class AppSettingsReader : IAppSettingsReader
{
    private const string DefaultAppSettingsFileName = "appsettings.json";
    private readonly object _readLock = new();
    private JObject _parsedAppSettingsFile;

    private string BaseDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private string AppSettingsPath => $"{BaseDirectory}/{DefaultAppSettingsFileName}";

    public T Read<T>()
    {
        lock (_readLock)
        {
            _parsedAppSettingsFile ??= JObject.Parse(File.ReadAllText(AppSettingsPath));

            string typeName = typeof(T).Name;

            JToken jsonToken = _parsedAppSettingsFile[typeName];

            if (jsonToken is null)
                throw new JsonReaderException($"Json file does not contain section '{typeName}'");

            var option = jsonToken.ToObject<T>();

            if (option is null)
                throw new JsonSerializationException(
                    $"'{typeName}' options could not be constructed from provided json definition");

            return option;
        }
    }
}