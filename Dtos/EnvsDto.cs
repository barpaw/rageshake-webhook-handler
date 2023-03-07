namespace RageshakeWebhookHandler.Dtos;

public class EnvsDto
{
    public string? RageshakeDomainToBeReplaced { get; set; }
    public string? RageshakeDomain { get; set; }
    public string? MatrixNotifierUrl { get; set; }
    public string? MatrixNotifierMessageHeader { get; set; }
    public string? MatrixHomeserverUrl { get; set; }
    public string? MatrixHomeserverUser { get; set; }
    public string? MatrixHomeserverPasswd { get; set; }
    public string? MatrixHomeserverRoom { get; set; }
}