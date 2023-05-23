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
        
        var rageshakeDomain = Environment.GetEnvironmentVariable("RAGESHAKE_DOMAIN");
        var rageshakeDomainToBeReplaced =
            Environment.GetEnvironmentVariable("RAGESHAKE_DOMAIN_TO_BE_REPLACED");
        var matrixHomeserverUser =
            Environment.GetEnvironmentVariable("MATRIX_HOMESERVER_USER");
        var matrixHomeserverPasswd =
            Environment.GetEnvironmentVariable("MATRIX_HOMESERVER_PASSWD");
        var matrixHomeserverDeviceId =
            Environment.GetEnvironmentVariable("MATRIX_HOMESERVER_DEVICEID");
        var matrixHomeserverUrl =
            Environment.GetEnvironmentVariable("MATRIX_HOMESERVER_URL");
        var matrixHomeserverRoom =
            Environment.GetEnvironmentVariable("MATRIX_HOMESERVER_ROOM");
        var matrixNotifierUrl =
            Environment.GetEnvironmentVariable("MATRIX_NOTIFIER_URL");
        var matrixNotifierMessageHeader =
            Environment.GetEnvironmentVariable("MATRIX_NOTIFIER_MESSAGE_HEADER");

        var rageshakeDomainCondition = rageshakeDomain is not null && rageshakeDomain.Length > 0;
        var rageshakeDomainToBeReplacedCondition = rageshakeDomainToBeReplaced is not null && rageshakeDomainToBeReplaced.Length > 0;
        var matrixHomeserverUserCondition = matrixHomeserverUser is not null && matrixHomeserverUser.Length > 0;
        var matrixHomeserverPasswdCondition = matrixHomeserverPasswd is not null && matrixHomeserverPasswd.Length > 0;
        var matrixHomeserverDeviceIdCondition = matrixHomeserverDeviceId is not null && matrixHomeserverDeviceId.Length > 0;
        var matrixHomeserverUrlCondition = matrixHomeserverUrl is not null && matrixHomeserverUrl.Length > 0;
        var matrixHomeserverRoomCondition = matrixHomeserverRoom is not null && matrixHomeserverRoom.Length > 0;
        var matrixNotifierUrlCondition = matrixNotifierUrl is not null && matrixNotifierUrl.Length > 0;
        var matrixNotifierMessageHeaderCondition = matrixNotifierMessageHeader is not null && matrixNotifierMessageHeader.Length > 0;

        if (rageshakeDomainCondition && rageshakeDomainToBeReplacedCondition && matrixHomeserverUserCondition &&
            matrixHomeserverPasswdCondition && matrixHomeserverDeviceIdCondition && matrixHomeserverUrlCondition && matrixHomeserverRoomCondition &&
            matrixNotifierUrlCondition && matrixNotifierMessageHeaderCondition)
        {
            var envsDto = new EnvsDto();

            envsDto.RageshakeDomain = rageshakeDomain;
            envsDto.RageshakeDomainToBeReplaced = rageshakeDomainToBeReplaced;
            envsDto.MatrixHomeserverUser = matrixHomeserverUser;
            envsDto.MatrixHomeserverPasswd = matrixHomeserverPasswd;
            envsDto.MatrixHomeserverDeviceId = matrixHomeserverDeviceId;
            envsDto.MatrixHomeserverUrl = matrixHomeserverUrl;
            envsDto.MatrixHomeserverRoom = matrixHomeserverRoom;
            envsDto.MatrixNotifierUrl = matrixNotifierUrl;
            envsDto.MatrixNotifierMessageHeader = matrixNotifierMessageHeader;
        
            return envsDto;
        }
        else
        {
            throw new Exception("One or more env vars is/are null/empty");
        }

    }
    
}