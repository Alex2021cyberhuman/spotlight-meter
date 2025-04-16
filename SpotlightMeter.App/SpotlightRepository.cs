using Blazored.LocalStorage;

namespace SpotlightMeter.App;

public class SpotlightRepository
{
    private const string Key = "SpotlightMeter.Companies";

    private readonly ILocalStorageService localStorageService;

    public SpotlightRepository(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
    }

    public async Task SaveLastCompany(Company company)
    {
        await SaveCompany(company);
        await SaveGenericAsync("last-company", "last-company", company);
    }

    public Task<Company?> GetLastCompanyAsync() => FindGenericAsync<Company>("last-company", "last-company");

    public Task SaveCompany(Company company) => SaveGenericAsync(nameof(Company), company.Id.ToString(), company);

    public IAsyncEnumerable<Company> GetCompaniesAsync() => SelectAllGenericAsync<Company>(nameof(Company));

    public Task SaveTargetAsync(SpotlightTarget target) => SaveGenericAsync(nameof(SpotlightTarget), target.Id.ToString(), target);

    public IAsyncEnumerable<SpotlightTarget> GetTargetsAsync() => SelectAllGenericAsync<SpotlightTarget>(nameof(SpotlightTarget));

    private async Task SaveGenericAsync<T>(string category, string key, T entity)
    {
        await localStorageService.SetItemAsync($"{category}:{key}", entity);
    }

    private async Task<T?> FindGenericAsync<T>(string category, string key)
    {
        return await localStorageService.GetItemAsync<T>($"{category}:{key}");
    }

    private async IAsyncEnumerable<T> SelectAllGenericAsync<T>(string category)
    {
        var keys = (await localStorageService.KeysAsync())
            .Where(key => key.StartsWith(category))
            .ToList();

        var tasks = keys.Select(key => localStorageService.GetItemAsync<T>(key));
        foreach (var chunk in tasks.Chunk(10))
        {
            foreach (var task in chunk)
            {
                var item = await task;
                if (item is { })
                    yield return item;
            }
        }
    }
}