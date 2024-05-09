<script setup lang="ts">
import { TarButton, TarModal } from "logitar-vue3-ui";
import { computed, ref } from "vue";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";

import DisplayNameInput from "@/components/shared/DisplayNameInput.vue";
import UniqueSlugInput from "./UniqueSlugInput.vue";
import type { CreateWorldPayload, World } from "@/types/worlds";
import { createWorld } from "@/api/worlds";

const { t } = useI18n();

const props = withDefaults(
  defineProps<{
    close?: string;
    id?: string;
    title?: string;
  }>(),
  {
    close: "actions.close",
    id: "create-world",
    title: "worlds.title.create",
  },
);

const modalRef = ref<InstanceType<typeof TarModal> | null>(null);
const payload = ref<CreateWorldPayload>({ uniqueSlug: "" });

const modalId = computed<string>(() => `create-modal_${props.id}`);

function hide(): void {
  modalRef.value?.hide();
}

const emit = defineEmits<{
  (e: "created", value: World): void;
  (e: "error", value: unknown): void;
}>();

const { handleSubmit, isSubmitting } = useForm();
const onSubmit = handleSubmit(async () => {
  try {
    const world: World = await createWorld(payload.value);
    emit("created", world);
    hide();
  } catch (e: unknown) {
    emit("error", e);
  }
});
</script>

<template>
  <span>
    <TarButton icon="fas fa-plus" :text="t('actions.create')" variant="success" data-bs-toggle="modal" :data-bs-target="`#${modalId}`" />
    <TarModal :close="t(close)" :id="modalId" ref="modalRef" :title="t(title)">
      <form @submit.prevent="onSubmit">
        <DisplayNameInput v-model="payload.displayName" />
        <UniqueSlugInput :default-value="payload.displayName" required v-model="payload.uniqueSlug" />
      </form>
      <template #footer>
        <TarButton icon="fas fa-ban" :text="t('actions.cancel')" variant="secondary" @click="hide" />
        <TarButton
          :disabled="isSubmitting"
          icon="fas fa-plus"
          :loading="isSubmitting"
          :status="t('loading')"
          :text="t('actions.create')"
          variant="success"
          @click="onSubmit"
        />
      </template>
    </TarModal>
  </span>
</template>
