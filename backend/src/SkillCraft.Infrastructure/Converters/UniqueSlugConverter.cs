using SkillCraft.Domain.Shared;

namespace SkillCraft.Infrastructure.Converters;

internal class UniqueSlugConverter : JsonConverter<UniqueSlugUnit>
{
  public override UniqueSlugUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return UniqueSlugUnit.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, UniqueSlugUnit worldId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(worldId.Value);
  }
}
