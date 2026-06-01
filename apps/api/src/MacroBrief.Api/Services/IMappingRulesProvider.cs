public interface IMappingRulesProvider
{
    IReadOnlyList<string> GetExposureTags(string symbol);
}
