import type { SearchResults } from "@/types/search";
import type { CreateWorldPayload, SearchWorldsPayload, World } from "@/types/worlds";
import { UrlBuilder, type IUrlBuilder } from "@/helpers/urlUtils";
import { get, post } from ".";

function createUrlBuilder(id?: string): IUrlBuilder {
  if (id) {
    return new UrlBuilder({ path: "/worlds/{id}" }).setParameter("id", id);
  }
  return new UrlBuilder({ path: "/worlds" });
}

export async function createWorld(payload: CreateWorldPayload): Promise<World> {
  return (await post<CreateWorldPayload, World>(createUrlBuilder().buildRelative(), payload)).data;
}

export async function readWorld(id: string): Promise<World> {
  return (await get<World>(createUrlBuilder(id).buildRelative())).data;
}

export async function searchWorlds(payload: SearchWorldsPayload): Promise<SearchResults<World>> {
  const url: string = createUrlBuilder()
    .setQuery("ids", payload.ids)
    .setQuery(
      "search_terms",
      payload.search.terms.map(({ value }) => value),
    )
    .setQuery("search_operator", payload.search.operator)
    .setQuery(
      "sort",
      payload.sort.map(({ field, isDescending }) => (isDescending ? `DESC.${field}` : field)),
    )
    .setQuery("skip", payload.skip.toString())
    .setQuery("limit", payload.limit.toString())
    .buildRelative();
  return (await get<SearchResults<World>>(url)).data;
}
