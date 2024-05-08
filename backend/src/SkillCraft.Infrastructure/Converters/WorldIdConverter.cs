using SkillCraft.Domain.Worlds;

namespace SkillCraft.Infrastructure.Converters;

internal class WorldIdConverter : JsonConverter<WorldId>
{
  public override WorldId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return WorldId.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, WorldId worldId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(worldId.Value);
  }
}
