namespace RageshakeWebhookHandler.Consts;

public static class EnvVarsKeys
{
    public static readonly List<string> EnvVariablesKeys = new()
    {
        RageshakeDomainToBeReplaced,
        RageshakeDomain,
        MatrixNotifierUrl,
        MatrixNotifierMessageHeader,
        MatrixHomeserverUrl,
        MatrixHomeserverUser,
        MatrixHomeserverPasswd,
        MatrixHomeserverRoom
    };


    public static string RageshakeDomainToBeReplaced => "RAGESHAKE_DOMAIN_TO_BE_REPLACED";

    public static string RageshakeDomain => "RAGESHAKE_DOMAIN";

    public static string MatrixNotifierUrl => "MATRIX_NOTIFIER_URL";

    public static string MatrixNotifierMessageHeader => "MATRIX_NOTIFIER_MESSAGE_HEADER";

    public static string MatrixHomeserverUrl => "MATRIX_HOMESERVER_URL";

    public static string MatrixHomeserverUser => "MATRIX_HOMESERVER_USER";

    public static string MatrixHomeserverPasswd => "MATRIX_HOMESERVER_PASSWD";

    public static string MatrixHomeserverRoom => "MATRIX_HOMESERVER_ROOM";
}