namespace Generator
{
    internal static class Application
    {

        public static void Run(ApplicationSettings settings)
        {
            var generator = new Generator();
            generator.Process(settings);
        }
    }
}
