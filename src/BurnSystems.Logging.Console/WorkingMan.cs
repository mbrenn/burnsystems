namespace BurnSystems.Logging.Console
{
    public class WorkingMan
    {
        private readonly ILogger logger = new ClassLogger(typeof(WorkingMan));

        public void Work()
        {
            logger.Info("We are working.");
        }
    }
}