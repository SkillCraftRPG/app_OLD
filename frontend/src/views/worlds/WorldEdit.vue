<script setup lang="ts">
import { computed, inject, onMounted, ref } from "vue";
import { useForm } from "vee-validate";
import { useRoute, useRouter } from "vue-router";

import AppBackButton from "@/components/shared/AppBackButton.vue";
import DescriptionTextarea from "@/components/shared/DescriptionTextarea.vue";
import DisplayNameInput from "@/components/shared/DisplayNameInput.vue";
import StatusDetail from "@/components/shared/StatusDetail.vue";
import type { ApiError } from "@/types/api";
import type { World } from "@/types/worlds";
import { readWorld } from "@/api/worlds";
import { handleErrorKey } from "@/inject/App";

const handleError = inject(handleErrorKey) as (e: unknown) => void;
const route = useRoute();
const router = useRouter();

const description = ref<string>("");
const displayName = ref<string>("");
const hasLoaded = ref<boolean>(false);
const uniqueSlug = ref<string>("");
const world = ref<World>();

const hasChanges = computed<boolean>(() => displayName.value !== (world.value?.displayName ?? "") || description.value !== (world.value?.description ?? ""));

function setModel(model: World): void {
  world.value = model;
  description.value = model.description ?? "";
  displayName.value = model.displayName ?? "";
  uniqueSlug.value = model.uniqueSlug;
}

const { handleSubmit } = useForm();
const onSubmit = handleSubmit(async () => {
  try {
    // if (world.value) {
    //   const updatedWorld = await replaceWorld(
    //     world.value.id,
    //     {
    //       isDone: isDone.value,
    //       displayName: displayName.value,
    //       description: description.value,
    //     },
    //     world.value.version,
    //   );
    //   setModel(updatedWorld);
    //   toasts.success("worlds.updated");
    // } else {
    //   const createdWorld = await createWorld({
    //     displayName: displayName.value,
    //     description: description.value,
    //   });
    //   setModel(createdWorld);
    //   toasts.success("worlds.created");
    //   router.replace({ name: "WorldEdit", params: { id: createdWorld.id } });
    // } // TODO(fpion): implement
  } catch (e: unknown) {
    handleError(e);
  }
});

onMounted(async () => {
  try {
    const id: string = route.params.id.toString();
    const world: World = await readWorld(id);
    setModel(world);
  } catch (e: unknown) {
    const { status } = e as ApiError;
    if (status === 404) {
      router.push({ path: "/not-found" });
    } else {
      handleError(e);
    }
  }
  hasLoaded.value = true;
});
</script>

<template>
  <main class="container">
    <template v-if="world">
      <h1>{{ world.displayName ? `${world.displayName} (${world.uniqueSlug})` : world.uniqueSlug }}</h1>
      <StatusDetail :aggregate="world" />
      <form @submit.prevent="onSubmit">
        <div class="mb-3">
          <AppBackButton :has-changes="hasChanges" />
        </div>
        <DisplayNameInput v-model="displayName" />
        <DescriptionTextarea v-model="description" />
      </form>
    </template>
  </main>
</template>
