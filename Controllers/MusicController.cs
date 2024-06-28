    using Microsoft.AspNetCore.Mvc;
    using Umbraco.Cms.Api.Common.ViewModels.Pagination;
    using Umbraco.Cms.Core.Cache;
    using Umbraco.Cms.Core.Logging;
    using Umbraco.Cms.Core.Models;
    using Umbraco.Cms.Core.Models.PublishedContent;
    using Umbraco.Cms.Core.Routing;
    using Umbraco.Cms.Core.Services;
    using Umbraco.Cms.Core.Web;
    using Umbraco.Cms.Infrastructure.Persistence;
    using Umbraco.Cms.Web.Common;
    using Umbraco.Cms.Web.Common.Controllers;
    using Umbraco.Cms.Web.Website.Controllers;

    /*
    [Route("umbraco/api/[controller]")]
    public class MusicController : UmbracoApiController
    {
        private readonly IContentService _contentService;

        public MusicController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet("getsongs")]
        public IActionResult GetSongs()
        {
            // Assuming the root node with Songs is of document type alias "SongContainer"
            var rootContent = _contentService.GetRootContent().FirstOrDefault(c => c.ContentType.Alias == "SongContainer");
            if (rootContent == null)
            {
                return NotFound();
            }

            long totalSongs;
            var songs = _contentService.GetPagedChildren(rootContent.Id, 1, 100, out totalSongs)
                .Where(c => c.ContentType.Alias == "Song")
                .Select(song => new
            {
                song.Id,
                song.Name,
                Author = song.GetValue<string>("author") // Assuming the alias for author property is 'author'
            });

            var result = new
            {
                Total = totalSongs,
                Songs = songs
            };

            return Ok(result);
        }
    }
    */
    [Route("umbraco/api/musiccontroller")]
    public class MusicController : SurfaceController
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public MusicController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
            
        }
        


        private static readonly List<MyMusic> AllMusic = Enumerable.Range(1, 100)
            .Select(i => new MyMusic($"My Item #{i}"))
            .ToList();

        [HttpGet("getallitems")]
        public IActionResult GetAllItems(int skip = 0, int take = 10)
            => Ok(
                new PagedViewModel<MyMusic>
                {
                    Items = AllMusic.Skip(skip).Take(take),
                    Total = AllMusic.Count
                }
            );

        [HttpGet("get_music")]
        public IActionResult GetMusic()
        {
            //IContent content = Services.ContentService.GetById(1066);

            
            List<IPropertyCollection> propertyCollections = new List<IPropertyCollection>();

            for (int i = 1; i <= Services.ContentService.CountChildren(1065); i++)
            {
                IPropertyCollection properties = Services.ContentService.GetById(1065+i).Properties;
                 propertyCollections.Add(properties);
            }
            
            //IPropertyCollection properties = Services.ContentService.GetById(1066).Properties;

           // IUmbracoContext context = _umbracoContextAccessor.GetRequiredUmbracoContext();

            

            return Ok(propertyCollections);

        }


        public class MyMusic(string value)
        {
            public Guid Id { get; } = Guid.NewGuid();

            public string Value { get; set; } = value;
        }
    }