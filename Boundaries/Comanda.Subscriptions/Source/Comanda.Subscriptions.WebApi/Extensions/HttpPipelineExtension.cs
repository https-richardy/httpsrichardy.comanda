namespace Comanda.Subscriptions.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only http pipeline configuration with no business logic.")]
public static class HttpPipelineExtension
{
    public static void UseHttpPipeline(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}