using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubberduck.Model
{
    public class SearchViewModel
    {
        public SearchViewModel() { }
        public SearchViewModel(string query)
        {
            Query = query;
        }

        public string Query { get; set; }
    }

    public class SearchResultsViewModel : SearchViewModel
    {
        public SearchResultsViewModel() 
            : this(string.Empty)
        {
        }

        public SearchResultsViewModel(string query) 
            : base(query)
        {
            Results = Enumerable.Empty<SearchResultViewModel>();
        }

        public IEnumerable<SearchResultViewModel> Results { get; set; }
    }

    public class SearchResultViewModel
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
    }
}
