using Microsoft.Extensions.Configuration;


public class Env
{

    private static IConfiguration? configuration;

    private static bool hasConfiguration;
 
    public static void SetConfiguration(IConfiguration configuration)
    {
        Env.configuration = configuration;
        Env.hasConfiguration = Env.configuration is IConfiguration;
    }

    /// <summary>
    /// String de conexão com o banco de dados utilizada por este servico.
    /// </summary>
    public static string GetConnectionString
    {
        get
        {
            return Env.GetConfigurationValue<string>("ConnectionStrings:DefaultConnection");
        }
    }

    /// <summary>
    /// Pega um limitante para a busca de páginas
    /// </summary>
    public static int GetMaxPageLimitForSearch
    {
        get
        {
            return Env.GetConfigurationValue<int>("MaxPageLimit") > 0 ? Env.GetConfigurationValue<int>("MaxPageLimit") : 2;
        }
    }

    /// <summary>
    /// Pega a palavra chave para busca 
    /// </summary>
    public static string GetKeyWord
    {
        get
        {
            return !String.IsNullOrEmpty(Env.GetConfigurationValue<string>("Keyword"))  ? Env.GetConfigurationValue<string>("Keyword") : "RPA";
        }
    }


    private static T GetConfigurationValue<T>(string key)
    {
        if (Env.hasConfiguration)
        {
            return Env.configuration.GetValue<T>(key);
        }

        T? t = default;

        return t;
    }
}