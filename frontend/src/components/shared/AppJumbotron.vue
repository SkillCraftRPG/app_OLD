<script setup lang="ts">
import { useI18n } from "vue-i18n";

const { t } = useI18n();

defineProps<{
  title?: string;
}>();
</script>

<template>
  <div class="row vh-100 text-center align-items-center">
    <div class="col jumbotron">
      <slot name="title-override">
        <h1 v-if="title" class="display-4">{{ t(title) }}</h1>
      </slot>
      <slot></slot>
    </div>
  </div>
</template>

<style scoped>
@media (min-width: 576px) {
  .jumbotron {
    padding: 4rem 2rem;
  }
}

.jumbotron {
  padding: 2rem 1rem;
  margin-bottom: 2rem;
  background-color: #e9ecef;
  border-radius: 0.3rem;
  text-align: center;
}
</style>
