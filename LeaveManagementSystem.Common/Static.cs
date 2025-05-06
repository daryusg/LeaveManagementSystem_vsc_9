using System.Reflection;

namespace LeaveManagementSystem.Common
{
    public static class Misc
    {
        public static bool IsAzureEnv()
        {
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        }


        public static DateTime getAssemblyBuildDateTime()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
        }
    }
}
