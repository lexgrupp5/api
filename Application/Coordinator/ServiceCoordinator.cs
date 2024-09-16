namespace Service;

public class ServiceCoordinator(Lazy<IExampleService> exampleService) : IServiceCoordinator
{
    private readonly Lazy<IExampleService> _exampleService = exampleService;

    public IExampleService Examples => _exampleService.Value;   
}