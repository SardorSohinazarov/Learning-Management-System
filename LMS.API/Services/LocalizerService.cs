using System;
using System.Globalization;
using System.Linq;
using LMS.API.Context;
using Microsoft.Extensions.Caching.Memory;

namespace LMS.API.Services
{
    public class LocalizerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public LocalizerService(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public string this[string key] => GetLocalizedString(key);

        public string GetLocalizedString(string key)
        {
            var cacheKey = $"localized_{CultureInfo.CurrentCulture.Name}_{key}";

            return _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                var localizedStringObject = _context.LocalizedStrings.FirstOrDefault(x => x.Key == key);

                if (localizedStringObject is null)
                    return key;

                var currentCulture = CultureInfo.CurrentUICulture.Parent.Name;

                var localizedString = currentCulture.ToLower() switch
                {
                    "uz" => localizedStringObject.Uz,
                    "en" => localizedStringObject.En,
                    "ru" => localizedStringObject.Ru,
                    _ => key
                };

                entry.SlidingExpiration = TimeSpan.FromHours(1);

                return localizedString ?? key;
            });
        }
    }
}
