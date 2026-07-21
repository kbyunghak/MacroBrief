public interface IBetaStatusService
{
    BetaStatus GetStatus(int eventWindow = 200, int rollupDays = 7, int aiAuditWindow = 50);
}
