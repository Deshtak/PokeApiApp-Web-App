using RestSharp;

public class PokeApiService
{
    private readonly RestClient _client;

    public PokeApiService()
    {
        _client = new RestClient("https://pokeapi.co/api/v2/");
    }

    // Método para obtener un solo Pokémon por nombre
    public async Task<Pokemon?> GetPokemonAsync(string name)
    {
        var request = new RestRequest($"pokemon/{name}", Method.Get);
        return await _client.GetAsync<Pokemon>(request);
    }
    
    

    // Método para obtener una página de Pokémon con solo su nombre y URL
    public async Task<PokemonListResponse> GetPokemonPageAsync(int page, int pageSize = 20)
    {
        int offset = (page - 1) * pageSize;
        var request = new RestRequest($"pokemon?limit={pageSize}&offset={offset}", Method.Get);
        return await _client.GetAsync<PokemonListResponse>(request) ?? new PokemonListResponse { Results = new List<PokemonListItem>() };
    }

    // Método para obtener los detalles completos de los Pokémon de una página
    public async Task<List<Pokemon>> GetPokemonDetailsPageAsync(int page, int pageSize = 20)
    {
        var pokemonPage = await GetPokemonPageAsync(page, pageSize);
        var tasks = pokemonPage.Results.Select(async item =>
        {
            var detailsRequest = new RestRequest(item.Url, Method.Get);
            return await _client.GetAsync<Pokemon>(detailsRequest);
        });

        var pokemonDetails = await Task.WhenAll(tasks);
        return pokemonDetails.Where(details => details != null).ToList()!;
    }
}

// Clases para representar la respuesta de la API
public class PokemonListResponse
{
    public int Count { get; set; }
    public List<PokemonListItem> Results { get; set; } = new List<PokemonListItem>();
}

public class PokemonListItem
{
    public string Name { get; set; }
    public string Url { get; set; }
}

// Clases para los detalles de cada Pokémon
public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TypeWrapper> Types { get; set; }
    public List<AbilityWrapper> Abilities { get; set; }
    public List<MoveWrapper> Moves { get; set; }
}



public class TypeWrapper
{
    public TypeInfo Type { get; set; }
}

public class TypeInfo
{
    public string Name { get; set; }
}

public class MoveWrapper
{
    public MoveInfo Move { get; set; }
}

public class MoveInfo
{
    public string Name { get; set; }
}

public class AbilityWrapper
{
    public AbilityInfo Ability { get; set; }
}

public class AbilityInfo
{
    public string Name { get; set; }
}
