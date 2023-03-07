using RageshakeWebhookHandler.Consts;
using RageshakeWebhookHandler.Dtos;

namespace RageshakeWebhookHandler.Services;

public class EnvironmentService : IEnvironmentService
{
    private readonly ILogger<EnvironmentService> _logger;

    public EnvironmentService(ILogger<EnvironmentService> logger)
    {
        _logger = logger;
    }

    public EnvsDto GetEnvDto()
    {
        if (CheckIfEnvVariablesAreSetAndNotEmpty())
        {
            var envsDto = new EnvsDto();

            envsDto.RageshakeDomain = Environment.GetEnvironmentVariable(GetEnvValue(EnvVarsKeys.RageshakeDomain)) ??
                                      string.Empty;
            envsDto.RageshakeDomainToBeReplaced =
                Environment.GetEnvironmentVariable(GetEnvValue(EnvVarsKeys.RageshakeDomainToBeReplaced)) ??
                string.Empty;
            envsDto.MatrixHomeserverUser =
                Environment.GetEnvironmentVariable(GetEnvValue(EnvVarsKeys.MatrixHomeserverUser)) ?? string.Empty;
            envsDto.MatrixHomeserverPasswd =
                Environment.GetEnvironmentVariable(GetEnvValue(EnvVarsKeys.MatrixHomeserverPasswd)) ?? string.Empty;
            envsDto.MatrixHomeserverUrl =
                Environment.GetEnvironmentVariable(GetEnvValue(EnvVarsKeys.MatrixHomeserverUrl)) ?? string.Empty;
            envsDto.MatrixHomeserverRoom =
                Environment.GetEnvironmentVariable(GetEnvValue(EnvVarsKeys.MatrixHomeserverRoom)) ?? string.Empty;
            envsDto.MatrixNotifierUrl =
                Environment.GetEnvironmentVariable(GetEnvValue(EnvVarsKeys.MatrixNotifierUrl)) ?? string.Empty;
            envsDto.MatrixNotifierMessageHeader =
                Environment.GetEnvironmentVariable(GetEnvValue(EnvVarsKeys.MatrixNotifierMessageHeader)) ??
                string.Empty;

            return envsDto;
        }

        throw new Exception("Found empty env vars");
    }

    private bool CheckIfEnvVariablesAreSetAndNotEmpty()
    {
        var envs = EnvVarsKeys.EnvVariablesKeys;

        foreach (var env in envs)
            if (env.Length > 0)
            {
                _logger.LogWarning("Env: {Env} is empty", env);
                return false;
            }

        return true;
    }


    private string GetEnvValue(string envKey)
    {
        var envs = EnvVarsKeys.EnvVariablesKeys;

        foreach (var env in envs)
            if (env.Equals(envKey))
                return Environment.GetEnvironmentVariable(env) ?? string.Empty;

        return string.Empty;
    }
}