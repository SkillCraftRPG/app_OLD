<script setup lang="ts">
import { useI18n } from "vue-i18n";

import AppJumbotron from "@/components/shared/AppJumbotron.vue";

const { t } = useI18n();
</script>

<template>
  <main class="container">
    <AppJumbotron>
      <template #title-override>
        <h1 class="display-4">404</h1>
      </template>
      <p class="lead">{{ t("notFound.lead") }}</p>
      <hr class="my-4" />
      <p>{{ t("notFound.help") }}</p>
      <p class="lead">
        <RouterLink :to="{ name: 'Home' }" class="btn btn-primary btn-lg" role="button">
          <font-awesome-icon icon="fas fa-home" /> {{ t("notFound.link") }}
        </RouterLink>
      </p>
    </AppJumbotron>
  </main>
</template>
