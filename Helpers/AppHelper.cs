using System.Text.Json;
using System.Reflection;


namespace WebApi.Helpers
{

    public class AppHelper
    {

        public static string GetDateNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
        }
        public static string GetNuVersion()
        {

            string nuVersionMin = "1.0";
            string[] nums = Assembly.GetExecutingAssembly().GetName().Version!.ToString().Split('.');

            if (nums.Length >= 2)
            {
                nuVersionMin = string.Concat(nums[0], ".", nums[1]);
            }

            return nuVersionMin;

        }


        public static string GetApplicationCurrentPath()
        {
            return Directory.GetCurrentDirectory().Replace(@"\\", @"\");
        }

        public static string GetNmApplication()
        {
            var nmApplication = Assembly.GetExecutingAssembly().GetName().Name!.ToString();
            nmApplication = string.IsNullOrEmpty(nmApplication) ? string.Empty : nmApplication;
            return nmApplication;
        }

        public static string GenerateNuGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static string ToJSON(Object? obj)
        {
            string objJSON = string.Empty;
            try
            {
                objJSON = obj == null ? string.Empty : JsonSerializer.Serialize(obj);
            }
            catch (Exception)
            {
                objJSON = string.Empty;
            }
            return objJSON;
        }
    }
}