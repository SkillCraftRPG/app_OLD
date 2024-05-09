using SkillCraft.Contracts.Worlds;

namespace SkillCraft.Models.Worlds;

public record SearchWorldsModel : SearchModel
{
  public new SearchWorldsPayload ToPayload()
  {
    SearchWorldsPayload payload = new();
    Fill(payload);

    if (Sort != null)
    {
      payload.Sort = new List<WorldSortOption>(capacity: Sort.Count);
      foreach (string sort in Sort)
      {
        int index = sort.IndexOf(SortSeparator);
        if (index < 0)
        {
          if (Enum.TryParse(sort, out WorldSort field))
          {
            payload.Sort.Add(new WorldSortOption(field));
          }
        }
        else
        {
          bool isDescending = sort[..index].Trim().Equals(DescendingKeyword, StringComparison.OrdinalIgnoreCase);
          if (Enum.TryParse(sort[(index + 1)..], out WorldSort field))
          {
            payload.Sort.Add(new WorldSortOption(field, isDescending));
          }
        }
      }
    }

    return payload;
  }
}
