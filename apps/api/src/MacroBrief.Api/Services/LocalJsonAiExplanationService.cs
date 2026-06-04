public class LocalJsonAiExplanationService : InMemoryAiExplanationService
{
    private readonly JsonFileStore<AiExplanationAuditItem> _auditStore;

    public LocalJsonAiExplanationService(IHostEnvironment environment, LocalJsonStorageOptions options)
        : base(environment)
    {
        _auditStore = new JsonFileStore<AiExplanationAuditItem>(Path.Combine(options.DataDirectory, "ai-explanation-audit.json"));
    }

    protected override void AddAuditLog(AiExplanationAuditItem item)
    {
        _auditStore.Update(current =>
        {
            current.Add(item);
            return current;
        });
    }

    protected override IReadOnlyList<AiExplanationAuditItem> ReadAuditLogs()
    {
        return _auditStore.ReadAll();
    }
}
