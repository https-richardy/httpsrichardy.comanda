namespace Comanda.Orchestrator.Infrastructure.Policies;

public static class PollyPolicies
{
    public static AsyncPolicyWrap<Result<TResult>> CreatePolicy<TResult>(ILogger logger)
    {
        // rate Limiting restricts the number of requests to an external service,
        // protecting it from overload and ensuring fair usage.

        // more details: https://learn.microsoft.com/en-us/azure/architecture/patterns/rate-limiting-pattern
        var rateLimitPolicy = Policy.RateLimitAsync(
            numberOfExecutions: 5,
            perTimeSpan: TimeSpan.FromSeconds(1),
            maxBurst: 2
        );

        var rateLimitFallback = Policy<Result<TResult>>
           .Handle<RateLimitRejectedException>()
           .FallbackAsync(
               fallbackValue: Result<TResult>.Failure(CommonErrors.RateLimitExceeded),
               onFallbackAsync: (delegateResult, context) =>
               {
                   logger.LogWarning("Rate limit exceeded");
                   return Task.CompletedTask;
               }
           );

        var timeoutPolicy = Policy.TimeoutAsync<Result<TResult>>(TimeSpan.FromSeconds(20));
        var retryPolicy = Policy<Result<TResult>>
            .Handle<Exception>()
            .WaitAndRetryAsync(2, _ => TimeSpan.FromSeconds(2));

        // circuit Breaker prevents calling an external service after repeated failures,
        // protecting the system and giving time to recover.

        // more details: https://learn.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker
        var circuitBreakerPolicy = Policy.Handle<Exception>()
            .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking: 3, durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (exception, breakDelay) =>
                {
                    logger.LogInformation("Circuit Breaker opened for {Seconds}s due to: {Message}", breakDelay.TotalSeconds, exception.Message);
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit Breaker closed — back to normal");
                },
                onHalfOpen: () =>
                {
                    logger.LogInformation("Circuit Breaker half-open — testing next call");
                }
            );

        var fallbackPolicy = Policy<Result<TResult>>.Handle<Exception>()
            .FallbackAsync(
                fallbackAction: (cancellationToken) =>
                {
                    var result = Result<TResult>.Failure(CommonErrors.OperationFailed);
                    return Task.FromResult(result);
                }
            );

        return Policy.WrapAsync<Result<TResult>>(
            fallbackPolicy,
            retryPolicy,
            rateLimitFallback,
            circuitBreakerPolicy.AsAsyncPolicy<Result<TResult>>(),
            rateLimitPolicy.AsAsyncPolicy<Result<TResult>>(),
            timeoutPolicy
        );
    }

    public static AsyncPolicyWrap<Result> CreatePolicy(ILogger logger)
    {
        // rate Limiting restricts the number of requests to an external service,
        // protecting it from overload and ensuring fair usage.

        // more details: https://learn.microsoft.com/en-us/azure/architecture/patterns/rate-limiting-pattern
        var rateLimitPolicy = Policy.RateLimitAsync(
            numberOfExecutions: 5,
            perTimeSpan: TimeSpan.FromSeconds(1),
            maxBurst: 2
        );

        var rateLimitFallback = Policy<Result>
            .Handle<RateLimitRejectedException>()
            .FallbackAsync(
                fallbackValue: Result.Failure(CommonErrors.RateLimitExceeded),
                onFallbackAsync: (_, _) =>
                {
                    logger.LogWarning("Rate limit exceeded");
                    return Task.CompletedTask;
                }
            );

        var timeoutPolicy = Policy.TimeoutAsync<Result>(TimeSpan.FromSeconds(20));

        var retryPolicy = Policy<Result>
            .Handle<Exception>()
            .WaitAndRetryAsync(2, _ => TimeSpan.FromSeconds(2));

        // circuit Breaker prevents calling an external service after repeated failures,
        // protecting the system and giving time to recover.

        // more details: https://learn.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker
        var circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (exception, breakDelay) =>
                {
                    logger.LogInformation("Circuit Breaker opened for {Seconds}s due to: {Message}", breakDelay.TotalSeconds, exception.Message);
                },
                onReset: () => logger.LogInformation("Circuit Breaker closed — back to normal"),
                onHalfOpen: () => logger.LogInformation("Circuit Breaker half-open — testing next call")
            );

        var fallbackPolicy = Policy<Result>
            .Handle<Exception>()
            .FallbackAsync(
                fallbackAction: (_) =>
                {
                    return Task.FromResult(Result.Failure(CommonErrors.OperationFailed));
                }
            );

        return Policy.WrapAsync<Result>(
            fallbackPolicy,
            retryPolicy,
            rateLimitFallback,
            circuitBreakerPolicy.AsAsyncPolicy<Result>(),
            rateLimitPolicy.AsAsyncPolicy<Result>(),
            timeoutPolicy
        );
    }
}
