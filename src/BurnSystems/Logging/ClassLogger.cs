using System;

namespace BurnSystems.Logging
{
    public class ClassLogger : ILogger
    {
        private readonly string _category;

        public ClassLogger(Type type)
        {
            _category = type.FullName;
        }

        public void Log(LogMessage message)
        {
            if (string.IsNullOrEmpty(message.Category))
            {
                message.Category = _category;
            }
            else
            {
                message.Category += $" {_category}";
            }

            TheLog.Log(message);
        }
    }
}
