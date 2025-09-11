public interface IConfigLoader
{
    bool TryLoadConfig<T>(string configName, out T config);
}
