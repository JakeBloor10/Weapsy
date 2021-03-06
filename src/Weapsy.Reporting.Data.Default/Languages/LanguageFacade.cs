﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Infrastructure.Caching;
using Weapsy.Domain.Languages;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Default.Languages
{
    public class LanguageFacade : ILanguageFacade
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public LanguageFacade(ILanguageRepository languageRepository, 
            ICacheManager cacheManager, 
            IMapper mapper)
        {
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public IEnumerable<LanguageInfo> GetAllActive(Guid siteId)
        {
            return _cacheManager.Get(string.Format(CacheKeys.LanguagesCacheKey, siteId), () =>
            {
                var languages = _languageRepository.GetAll(siteId);
                return _mapper.Map<IEnumerable<LanguageInfo>>(languages);
            });
        }

        public IEnumerable<LanguageAdminModel> GetAllForAdmin(Guid siteId)
        {
            var languages = _languageRepository.GetAll(siteId);
            return _mapper.Map<IEnumerable<LanguageAdminModel>>(languages);
        }

        public LanguageAdminModel GetForAdmin(Guid siteId, Guid id)
        {
            var language = _languageRepository.GetById(siteId, id);
            return language == null ? null : _mapper.Map<LanguageAdminModel>(language);
        }
    }
}