using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.ViewModels.Pagination;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;

namespace BackendJukeOn.Controllers;

[VersionedApiBackOfficeRoute("my/music")]
[ApiExplorerSettings(GroupName = "MusicBackendAPI")]
public class MusicBackendController : ManagementApiControllerBase
{
    private static readonly List<MyItem> AllItems = Enumerable.Range(1, 100)
        .Select(i => new MyItem($"My Item #{i}"))
        .ToList();
    
    
    
    [HttpGet]
    public IActionResult GetAllItems(int skip = 0, int take = 10)
        => Ok(
            new PagedViewModel<MyItem>
            {
                Items = AllItems.Skip(skip).Take(take),
                Total = AllItems.Count
            }
        );
}

public class MyItem(string value)
{
    public Guid Id { get; } = Guid.NewGuid();

    public string Value { get; set; } = value;
}

