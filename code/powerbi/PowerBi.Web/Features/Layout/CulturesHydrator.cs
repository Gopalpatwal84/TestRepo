using System.Linq;
using System.Threading.Tasks;
using Acom.IO;
using Acom.Mvc;
using PowerBI.Entities;
using PowerBI.Web.Models.Pages;

namespace PowerBI.Web.Features.Layout
{
    public class CulturesHydrator : IHydrateModel<ViewMetadataModel>
    {
        private readonly IRepository<Culture> _culturesRepository;

        public CulturesHydrator(IRepository<Culture> culturesRepository)
        {
            _culturesRepository = culturesRepository;
        }

        public async Task Hydrate(ViewMetadataModel model)
        {
            model.Cultures = (await _culturesRepository.GetAsync()).ToArray();
        }
    }
}
