namespace BurnSystems.Logging.Console.DotNetFW
{
    public class WorkingMan
    {
        private readonly ILogger _logger = new ClassLogger(typeof(WorkingMan));

        public void Work()
        {
            _logger.Info("We are working.");
        }
    }
}