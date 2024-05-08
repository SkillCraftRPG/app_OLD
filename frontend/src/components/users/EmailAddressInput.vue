<script setup lang="ts">
import AppInput from "@/components/shared/AppInput.vue";
import type { ShowStatus } from "@/types/validation";

defineProps<{
  disabled?: boolean | string;
  modelValue?: string;
  required?: boolean | string;
  showStatus?: ShowStatus;
}>();

defineEmits<{
  (e: "update:model-value", value?: string): void;
}>();
</script>

<template>
  <AppInput
    :disabled="disabled"
    floating
    id="email-address"
    label="users.email.address"
    max="255"
    :model-value="modelValue"
    placeholder="users.email.address"
    :required="required"
    :show-status="showStatus"
    type="email"
    @update:model-value="$emit('update:model-value', $event)"
  />
</template>
