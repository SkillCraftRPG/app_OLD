<script setup lang="ts">
import { TarButton } from "logitar-vue3-ui";
import { useI18n } from "vue-i18n";

import AppJumbotron from "@/components/shared/AppJumbotron.vue";
import type { SentMessage } from "@/types/account";

const { t } = useI18n();

defineProps<{
  sentMessage: SentMessage;
}>();

function onClose(): void {
  window.close();
}
</script>

<template>
  <AppJumbotron v-if="sentMessage" title="users.authenticationLink.sent">
    <p>
      <font-awesome-icon :icon="`fas fa-${sentMessage.contactType === 'Email' ? 'at' : 'phone'}`" />
      {{ t(`users.authenticationLink.${sentMessage.contactType}`) }}
      <strong>{{ sentMessage.maskedContact }}</strong
      >.
    </p>
    <hr class="my-4" />
    <p>
      {{ t("users.authenticationLink.help") }} <br />
      {{ t("users.authenticationLink.confirmationNumber") }}
      <strong>{{ sentMessage.confirmationNumber }}</strong
      >.
    </p>
    <p class="lead">
      <TarButton icon="fas fa-times" size="large" :text="t('actions.close')" @click="onClose" />
    </p>
  </AppJumbotron>
</template>
