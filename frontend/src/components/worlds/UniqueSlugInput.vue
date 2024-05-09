<script setup lang="ts">
import { TarCheckbox } from "logitar-vue3-ui";
import { ref, watch } from "vue";
import { stringUtils } from "logitar-js";
import { useI18n } from "vue-i18n";

import AppInput from "@/components/shared/AppInput.vue";

const { t } = useI18n();

const props = withDefaults(
  defineProps<{
    defaultValue?: string;
    id?: string;
    label?: string;
    modelValue?: string;
    required?: boolean | string;
  }>(),
  {
    id: "unique-slug",
    label: "worlds.uniqueSlug",
  },
);

const custom = ref<boolean>(false);

const emit = defineEmits<{
  (e: "update:model-value", value?: string): void;
}>();

watch(custom, (custom) => {
  if (!custom) {
    emit("update:model-value", stringUtils.slugify(props.defaultValue));
  }
});
watch(
  () => props.defaultValue,
  (defaultValue) => {
    if (!custom.value) {
      emit("update:model-value", stringUtils.slugify(defaultValue));
    }
  },
);

// TODO(fpion): validation rules
</script>

<template>
  <AppInput
    :disabled="!custom"
    floating
    :id="id"
    :label="label"
    max="32"
    :model-value="modelValue"
    :placeholder="label"
    :required="required"
    @update:model-value="$emit('update:model-value', $event)"
  >
    <template #after>
      <TarCheckbox :id="`${id}_custom`" :label="t('custom')" v-model="custom" />
    </template>
  </AppInput>
</template>
