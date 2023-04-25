using System.Globalization;
using System.Threading.Tasks;
using LMS.API.Context;
using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<string> GetLocalizedString(string key)
        {
            var cacheKey = $"localized_{CultureInfo.CurrentCulture.Name}_{key}";

            if(_memoryCache.TryGetValue(cacheKey,out string cacheValue))
            {
                return cacheValue;
            }

            var localizedStringObject = await _context.LocalizedStrings.FirstOrDefaultAsync(x => x.Key == key);
            
            if(localizedStringObject is null)
                return key;

            var currentCulture = CultureInfo.CurrentUICulture.Parent.Name;

            var localizedString = currentCulture.ToLower() switch
            {
                "uz" => localizedStringObject.Uz,
                "en" => localizedStringObject.En,
                "ru" => localizedStringObject.Ru,
                _ => key
            };

            if(localizedString is not null)
                _memoryCache.Set(cacheKey, localizedString);

            return localizedString ?? key;
        }
    }
}
