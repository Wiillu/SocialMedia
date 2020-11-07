using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using System;

namespace SocialMedia.Infrastructure.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseuri;
        public UriService(string baseuri)
        {
            _baseuri = baseuri;
        }

        public Uri GetPostPaginationUri(PostQueryFilters filter, string actionUrl)
        {
            string baseUrl = $"{_baseuri}{actionUrl}";
            return new Uri(baseUrl);
        }
    }
}
