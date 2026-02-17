namespace Comanda.Orchestrator.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only http pipeline configuration with no business logic.")]
public static class HttpPipelineExtension
{
    public static void UseHttpPipeline(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseHeaderPropagation();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UsePrincipalMiddleware();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
