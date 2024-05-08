<script setup lang="ts">
import { TarButton } from "logitar-vue3-ui";
import { useForm } from "vee-validate";
import { ref } from "vue";
import { useI18n } from "vue-i18n";

import PersonNameInput from "@/components/users/PersonNameInput.vue";
import type { Identification } from "@/types/account";

const { t } = useI18n();

const firstName = ref<string>("");
const lastName = ref<string>("");

const emit = defineEmits<{
  (e: "continue", value: Identification): void;
}>();

const { handleSubmit } = useForm();
const onSubmit = handleSubmit(() => {
  emit("continue", {
    firstName: firstName.value,
    lastName: lastName.value,
  });
});
</script>

<template>
  <div>
    <h3>{{ t("users.profile.identification.title") }}</h3>
    <p>
      <i><font-awesome-icon icon="fas fa-id-card" /> {{ t("users.profile.identification.lead") }}</i>
    </p>
    <form @submit="onSubmit">
      <div class="row">
        <PersonNameInput class="col-lg-6" required type="first" v-model="firstName" />
        <PersonNameInput class="col-lg-6" required type="last" v-model="lastName" />
      </div>
      <RouterLink :to="{ name: 'SignIn' }" class="btn btn-secondary me-1"><font-awesome-icon icon="fas fa-ban" /> {{ t("actions.cancel") }}</RouterLink>
      <TarButton class="ms-1" icon="fas fa-arrow-right" :text="t('actions.continue')" type="submit" />
    </form>
  </div>
</template>
