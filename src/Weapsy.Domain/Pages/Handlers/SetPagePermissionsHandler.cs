using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Pages.Commands;

namespace Weapsy.Domain.Pages.Handlers
{
    public class SetPagePermissionsHandler : ICommandHandler<SetPagePermissions>
    {
        private readonly IPageRepository _pageRepository;

        public SetPagePermissionsHandler(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public IEnumerable<IEvent> Handle(SetPagePermissions command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found.");

            page.SetPagePermissions(command);

            _pageRepository.Update(page);

            return page.Events;
        }
    }
}
