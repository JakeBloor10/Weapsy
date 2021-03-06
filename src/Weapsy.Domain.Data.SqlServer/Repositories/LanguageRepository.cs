﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Languages;
using LanguageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Language;

namespace Weapsy.Domain.Data.SqlServer.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly IWeapsyDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public LanguageRepository(IWeapsyDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Language GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<LanguageDbEntity>()
                    .FirstOrDefault(x => x.Id == id && x.Status != LanguageStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;                
            }
        }

        public Language GetById(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<LanguageDbEntity>()
                    .FirstOrDefault(x => x.SiteId == siteId && x.Id == id && x.Status != LanguageStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;
            }
        }

        public Language GetByName(Guid siteId, string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<LanguageDbEntity>()
                    .FirstOrDefault(x => x.SiteId == siteId && x.Name == name && x.Status != LanguageStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;
            }
        }

        public Language GetByCultureName(Guid siteId, string cultureName)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<LanguageDbEntity>()
                    .FirstOrDefault(x => x.SiteId == siteId && x.CultureName == cultureName && x.Status != LanguageStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;
            }
        }

        public Language GetByUrl(Guid siteId, string url)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<LanguageDbEntity>()
                    .FirstOrDefault(x => x.SiteId == siteId && x.Url == url && x.Status != LanguageStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;
            }
        }

        public ICollection<Language> GetAll(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Set<LanguageDbEntity>()
                    .Where(x =>
                        x.SiteId == siteId &&
                        x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToList();
                return _mapper.Map<ICollection<Language>>(dbEntities);
            }
        }

        public int GetLanguagesCount(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Set<LanguageDbEntity>().Count(x => x.SiteId == siteId && x.Status != LanguageStatus.Deleted);
            }            
        }

        public int GetActiveLanguagesCount(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Set<LanguageDbEntity>().Count(x => x.SiteId == siteId && x.Status == LanguageStatus.Active);
            }
        }

        public IEnumerable<Guid> GetLanguagesIdList(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var languages =
                    context.Set<LanguageDbEntity>()
                        .Where(x => x.SiteId == siteId && x.Status != LanguageStatus.Deleted)
                        .ToList();

                return languages.Select(x => x.Id);
            }            
        }

        public void Create(Language language)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<LanguageDbEntity>(language);
                context.Add(dbEntity);
                context.SaveChanges();
            }
        }

        public async Task CreateAsync(Language language)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<LanguageDbEntity>(language);
                context.Add(dbEntity);
                await context.SaveChangesAsync();
            }
        }

        public void Update(Language language)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<LanguageDbEntity>(language);
                context.Update(dbEntity);
                context.SaveChanges();                
            }
        }

        public void Update(IEnumerable<Language> languages)
        {
            using (var context = _dbContextFactory.Create())
            {
                foreach (var language in languages)
                {
                    var dbEntity = _mapper.Map<LanguageDbEntity>(language);
                    context.Update(dbEntity);
                }
                context.SaveChanges();
            }
        }
    }
}
