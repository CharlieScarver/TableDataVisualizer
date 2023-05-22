namespace ListVisualizer.Model
{
    public class AppConfiguration
    {
        private static AppConfiguration singletonInstance = null!;

        private AppConfiguration() { }

        public string ConnectionString { get; set; } = string.Empty;

        public string TableName { get; set; } = string.Empty;

        public bool DeveloperMode { get; set; } = false;

        public static AppConfiguration GetAppConfiguration()
        {
            if (singletonInstance == null)
            {
                singletonInstance = new AppConfiguration();
            }

            return singletonInstance;
        }
    }
}
