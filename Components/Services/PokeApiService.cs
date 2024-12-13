public class PokeApiService
{
    private readonly HttpClient _httpClient;

    public PokeApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("PokeApi");
    }

    // Método para obtener un solo Pokémon por nombre
    public async Task<Pokemon?> GetPokemonAsync(string name)
    {
        return await _httpClient.GetFromJsonAsync<Pokemon>($"pokemon/{name}");
    }

    // Método para obtener todos los Pokémon con solo su nombre y URL
    public async Task<List<PokemonListItem>> GetAllPokemonAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<PokemonListResponse>("pokemon?limit=100000&offset=0");
        return response?.Results ?? new List<PokemonListItem>();
    }

    // Método para obtener los detalles completos de todos los Pokémon
    public async Task<List<Pokemon>> GetAllPokemonDetailsAsync()
    {
        var response = await GetAllPokemonAsync(); // Obtener la lista básica de Pokémon
        var pokemonDetails = new List<Pokemon>();

        foreach (var pokemon in response)
        {
            var details = await _httpClient.GetFromJsonAsync<Pokemon>(pokemon.Url); // Obtener detalles de cada Pokémon usando su URL
            if (details != null)
                pokemonDetails.Add(details); // Agregar los detalles a la lista
        }

        return pokemonDetails;
    }

    // Método para obtener Pokémon por página (limitado a 20 por página)
    public async Task<PokemonListResponse> GetPokemonPageAsync(int page)
    {
        int limit = 20; // Número de Pokémon por página
        int offset = (page - 1) * limit; // Desplazamiento según la página

        var response = await _httpClient.GetFromJsonAsync<PokemonListResponse>($"pokemon?limit={limit}&offset={offset}");
        return response;
    }

    // Método para obtener los detalles completos de los Pokémon por página
    public async Task<List<Pokemon>> GetPokemonDetailsPageAsync(int page)
    {
        var response = await GetPokemonPageAsync(page); // Obtener la lista básica de Pokémon para la página actual
        var pokemonDetails = new List<Pokemon>();

        foreach (var pokemon in response.Results)
        {
            var details = await _httpClient.GetFromJsonAsync<Pokemon>(pokemon.Url); // Obtener detalles de cada Pokémon usando su URL
            if (details != null)
                pokemonDetails.Add(details); // Agregar los detalles a la lista
        }

        return pokemonDetails;
    }
}

// Clases para representar la respuesta de la API
public class PokemonListResponse
{
    public List<PokemonListItem> Results { get; set; }
    public int Count { get; set; } // Total de Pokémon
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
}

public class TypeWrapper
{
    public TypeInfo Type { get; set; }
}

public class TypeInfo
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
