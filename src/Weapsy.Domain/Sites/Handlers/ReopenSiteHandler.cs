using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Sites.Commands;

namespace Weapsy.Domain.Sites.Handlers
{
    public class ReopenSiteHandler : ICommandHandler<ReopenSite>
    {
        private readonly ISiteRepository _siteRepository;

        public ReopenSiteHandler(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public IEnumerable<IEvent> Handle(ReopenSite command)
        {
            var site = _siteRepository.GetById(command.Id);

            if (site == null)
                throw new Exception("Site not found.");

            site.Reopen();

            _siteRepository.Update(site);

            return site.Events;
        }
    }
}
