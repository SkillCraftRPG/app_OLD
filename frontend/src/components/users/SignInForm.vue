<script setup lang="ts">
import { TarButton } from "logitar-vue3-ui";
import { ref } from "vue";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";

import EmailAddressInput from "@/components/users/EmailAddressInput.vue";
import PasswordInput from "@/components/users/PasswordInput.vue";
import type { SignInPayload, SignInResponse } from "@/types/account";
import { signIn } from "@/api/account";

const { locale, t } = useI18n();

const emailAddress = ref<string>("");
const isPasswordRequired = ref<boolean>(false);
const password = ref<string>("");

const emit = defineEmits<{
  (e: "error", value: unknown): void;
  (e: "response", value: SignInResponse): void;
}>();

const { handleSubmit, isSubmitting } = useForm();
const onSubmit = handleSubmit(async () => {
  try {
    const payload: SignInPayload = {
      locale: locale.value,
      credentials: {
        emailAddress: emailAddress.value,
        password: password.value || undefined,
      },
    };
    const response: SignInResponse = await signIn(payload);
    if (response.isPasswordRequired) {
      isPasswordRequired.value = true;
    } else {
      emit("response", response);
    }
  } catch (e: unknown) {
    emit("error", e);
  }
});
</script>

<template>
  <div>
    <h1>{{ t("users.signIn.title") }}</h1>
    <form @submit.prevent="onSubmit">
      <EmailAddressInput :disabled="isPasswordRequired" required v-model="emailAddress" />
      <PasswordInput v-if="isPasswordRequired" required v-model="password" />
      <TarButton
        :disabled="isSubmitting"
        icon="fas fa-arrow-right-to-bracket"
        :loading="isSubmitting"
        :status="t('loading')"
        :text="t('users.signIn.submit')"
        type="submit"
      />
    </form>
  </div>
</template>
