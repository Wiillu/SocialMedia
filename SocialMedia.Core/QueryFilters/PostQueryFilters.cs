using System;

namespace SocialMedia.Core.QueryFilters
{
    public class PostQueryFilters
    {//objeto que encapsula los parametros
        public int? UserId { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}
