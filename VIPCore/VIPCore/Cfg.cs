using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace VIPCore;

public class Cfg
{
    private readonly VipCore _vipCore;

    public Cfg(VipCore vipCore)
    {
        _vipCore = vipCore;
    }
    
    public ConfigVipCoreSettings LoadVipSettingsConfig()
    {
        var configPath = Path.Combine(_vipCore.VipApi.CoreConfigDirectory, "vip_core.json");

        ConfigVipCoreSettings config;
        if (!File.Exists(configPath))
        {
            config = new ConfigVipCoreSettings
            {
                TimeMode = 0,
                ServerId = 0,
                UseCenterHtmlMenu = false,
                //DisplayUnavailableOptions = true,
                ReOpenMenuAfterItemClick = false,
                VipLogging = true,
                VipDescription = "";
                Connection = new VipDb
                {
                    Host = "HOST",
                    Database = "DATABASENAME",
                    User = "USER",
                    Password = "PASSWORD",
                    Port = 3306
                }
            };
            
            File.WriteAllText(configPath, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
            _vipCore.Logger.LogInformation("The configuration was successfully saved to a file: {path}", configPath);

            return config;
        }

        config = JsonSerializer.Deserialize<ConfigVipCoreSettings>(File.ReadAllText(configPath))!;

        return config;
    }

    public Config LoadConfig()
    {
        var configPath = Path.Combine(_vipCore.VipApi.CoreConfigDirectory, "vip.json");

        if (!File.Exists(configPath)) return CreateConfig(configPath);

        var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath))!;

        return config;
    }

    private Config CreateConfig(string configPath)
    {
        var config = new Config
        {
            Delay = 2.0f,
            Groups = new Dictionary<string, VipGroup>
            {
                {
                    "GROUP_NAME", new VipGroup
                    {
                        Values = new Dictionary<string, object>()
                    }
                }
            }
        };

        File.WriteAllText(configPath,
            JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));

        _vipCore.Logger.LogInformation("The configuration was successfully saved to a file: {path}", configPath);

        return config;
    }
}

public class Config
{
    public float Delay { get; init; }
    public Dictionary<string, VipGroup> Groups { get; init; } = null!;
}

public class VipGroup
{
    public Dictionary<string, object> Values { get; init; } = null!;
}

public class ConfigVipCoreSettings
{
    public int TimeMode { get; init; }
    public int ServerId { get; init; }
    public bool UseCenterHtmlMenu { get; init; }
    //public bool DisplayUnavailableOptions { get; init; }
    public bool ReOpenMenuAfterItemClick { get; init; }
    public bool VipLogging { get; init; }
    ppublic bool VipDescription { get; init; }
    public VipDb Connection { get; init; } = null!;
}

public class VipDb
{
    public required string Host { get; init; }
    public required string Database { get; init; }
    public required string User { get; init; }
    public required string Password { get; init; }
    public int Port { get; init; }
}
