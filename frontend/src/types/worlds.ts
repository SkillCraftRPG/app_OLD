import type { Aggregate } from "./aggregate";
import type { SearchPayload, SortOption } from "./search";

export type CreateWorldPayload = {
  uniqueSlug: string;
  displayName?: string;
  description?: string;
};

export type SearchWorldsPayload = SearchPayload & {
  sort?: WorldSortOption[];
};

export type World = Aggregate & {
  uniqueSlug: string;
  displayName?: string;
  description?: string;
};

export type WorldSort = "DisplayName" | "UniqueSlug" | "UpdatedOn";

export type WorldSortOption = SortOption & {
  field: WorldSort;
};
