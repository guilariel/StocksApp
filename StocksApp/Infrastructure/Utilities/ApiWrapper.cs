namespace StocksApp.Infrastructure.Utilities
{
    public class ApiWrapper
    {
        private readonly int _maxRetries;
        private readonly int _LimitOfrequestPerSecond;
        private readonly TimeSpan _delay;
        private readonly Queue<DateTime> _requestTimestamps = new();
        private readonly object _lock = new();
        public ApiWrapper(int maxRetries, int delayMs, int limitOfrequestPerSecond)
        {
            _maxRetries = maxRetries;
            _delay = TimeSpan.FromMilliseconds(delayMs);
            _LimitOfrequestPerSecond = limitOfrequestPerSecond;
        }
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    await EnsureRateLimitAsync();
                    return await action();
                }
                catch (Exception ex)
                {
                    attempt++;
                    if (attempt == _maxRetries)
                        throw new Exception($"Max retries reached. Last error: {ex.Message}");

                    await Task.Delay(_delay);
                }
            }
        }
        private async Task EnsureRateLimitAsync()
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan waitTime = TimeSpan.Zero;

            lock (_lock)
            {
                while (_requestTimestamps.Count > 0 && now - _requestTimestamps.Peek() > TimeSpan.FromSeconds(1))
                    _requestTimestamps.Dequeue();

                if (_requestTimestamps.Count >= _LimitOfrequestPerSecond)
                    waitTime = TimeSpan.FromSeconds(1) - (now - _requestTimestamps.Peek());

                _requestTimestamps.Enqueue(DateTime.UtcNow);
            }

            if (waitTime > TimeSpan.Zero)
                await Task.Delay(waitTime);
        }
    }
}
