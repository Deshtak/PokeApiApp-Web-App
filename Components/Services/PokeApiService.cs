using System.Net.Http.Json;

public class PokeApiService
{
    private readonly HttpClient _httpClient;

    public PokeApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("PokeApi");
    }

    public async Task<Pokemon?> GetPokemonAsync(string name)
    {
        return await _httpClient.GetFromJsonAsync<Pokemon>($"pokemon/{name}");
    }
    
    
    // De aqui en adelante viene los id de especie
    
    
}

public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TypeWrapper> Types { get; set; }
    public List<AbilityWrapper> Abilities { get; set; } // Nueva propiedad
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
    public AbilityInfo Ability { get; set; } // Para almacenar la información de la habilidad
}

public class AbilityInfo
{
    public string Name { get; set; } // Nombre de la habilidad
}

// De aqui en adelante vienen las evoluciones y el id de especie
