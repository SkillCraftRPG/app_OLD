<script setup lang="ts">
import type { SelectOption } from "logitar-vue3-ui";
import { arrayUtils } from "logitar-js";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

import AppSelect from "@/components/shared/AppSelect.vue";
import locales from "@/resources/locales.json";

const { orderBy } = arrayUtils;
const { availableLocales } = useI18n();

defineProps<{
  modelValue?: string;
  required?: boolean | string;
}>();

const options = computed<SelectOption[]>(() =>
  orderBy(
    locales.filter(({ code }) => availableLocales.includes(code)).map(({ code, nativeName }) => ({ value: code, text: nativeName })),
    "text",
  ),
);

defineEmits<{
  (e: "update:model-value", value?: string): void;
}>();
</script>

<template>
  <AppSelect
    floating
    id="locale"
    label="users.locale.label"
    :model-value="modelValue"
    :options="options"
    placeholder="users.locale.placeholder"
    :required="required"
    @update:model-value="$emit('update:model-value', $event)"
  />
</template>
