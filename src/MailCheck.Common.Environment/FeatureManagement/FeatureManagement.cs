using System.Linq;

namespace MailCheck.Common.Environment.FeatureManagement
{
    public static class FeatureManagement
    {
        public static bool IsEnabled(string featureName)
        {
            string activeFeaturesValue = System.Environment.GetEnvironmentVariable("ACTIVE_FEATURES") ?? string.Empty;

            bool featureActive = activeFeaturesValue.Split(',').Select(x => x.Trim()).Contains(featureName);

            return featureActive;
        }
    }
}