using Microsoft.AspNetCore.Mvc;
using SkillCraft.Contracts.Search;

namespace SkillCraft.Models;

public record SearchModel
{
  protected const string DescendingKeyword = "desc";
  protected const char SortSeparator = '.';

  [FromQuery(Name = "ids")]
  public List<Guid>? Ids { get; set; }

  [FromQuery(Name = "search_operator")]
  public SearchOperator? SearchOperator { get; set; }

  [FromQuery(Name = "search_terms")]
  public List<string>? SearchTerms { get; set; }

  [FromQuery(Name = "sort")]
  public List<string>? Sort { get; set; }

  [FromQuery(Name = "skip")]
  public int? Skip { get; set; }

  [FromQuery(Name = "limit")]
  public int? Limit { get; set; }

  public SearchPayload ToPayload()
  {
    SearchPayload payload = new();
    Fill(payload);
    return payload;
  }
  protected void Fill(SearchPayload payload)
  {
    if (Ids != null)
    {
      payload.Ids = new List<Guid>(Ids);
    }

    if (SearchOperator != null || SearchTerms != null)
    {
      List<SearchTerm> terms = SearchTerms?.Select(value => new SearchTerm(value)).ToList() ?? [];
      payload.Search = new TextSearch(terms, SearchOperator ?? default);
    }

    if (Sort != null)
    {
      payload.Sort = new List<SortOption>(capacity: Sort.Count);
      foreach (string sort in Sort)
      {
        int index = sort.IndexOf(SortSeparator);
        if (index < 0)
        {
          payload.Sort.Add(new SortOption(sort));
        }
        else
        {
          bool isDescending = sort[..index].Trim().Equals(DescendingKeyword, StringComparison.OrdinalIgnoreCase);
          string field = sort[(index + 1)..];
          payload.Sort.Add(new SortOption(field, isDescending));
        }
      }
    }

    payload.Skip = Skip;
    payload.Limit = Limit;
  }
}
