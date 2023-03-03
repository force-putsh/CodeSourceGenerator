using Microsoft.AspNetCore.Mvc;

namespace SourceGenerratorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaysController:ControllerBase
{
    
    private readonly IPays _pays;

    public PaysController(IPays pays)
    {
        _pays = pays;
    }

    [HttpGet]
    public IEnumerable<Personne> Get()
    {
        return _pays.Population();
    }
    
}