<script setup lang="ts">
import { TarButton } from "logitar-vue3-ui";
import { inject, ref } from "vue";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";
import { useRoute, useRouter } from "vue-router";

import EmailAddressInput from "@/components/users/EmailAddressInput.vue";
import PasswordInput from "@/components/users/PasswordInput.vue";
import type { Credentials, SignInPayload, SignInResponse } from "@/types/account";
import { handleErrorKey } from "@/inject/App";
import { signIn } from "@/api/account";
import { useAccountStore } from "@/stores/account";

const account = useAccountStore();
const handleError = inject(handleErrorKey) as (e: unknown) => void;
const route = useRoute();
const router = useRouter();
const { locale, t } = useI18n();

const credentials = ref<Credentials>({ emailAddress: "" });
const isPasswordRequired = ref<boolean>(false);

const { handleSubmit, isSubmitting } = useForm();
const onSubmit = handleSubmit(async () => {
  try {
    // invalidCredentials.value = false;
    const payload: SignInPayload = {
      locale: locale.value,
      credentials: credentials.value,
    };
    const response: SignInResponse = await signIn(payload);
    if (response.currentUser) {
      account.signIn(response.currentUser);
      const redirect: string | undefined = route.query.redirect?.toString();
      router.push(redirect ?? { name: "Profile" });
    } else {
      isPasswordRequired.value = response.isPasswordRequired;
      console.log(response); // TODO(fpion): implement
    }
  } catch (e: unknown) {
    // const { data, status } = e as ApiError;
    // if (status === 400 && (data as Error)?.code === "InvalidCredentials") {
    //   invalidCredentials.value = true;
    //   password.value = "";
    //   passwordRef.value?.focus();
    // } else {
    handleError(e);
    // }
  }
});
</script>

<template>
  <main class="container">
    <h1>{{ t("users.signIn.title") }}</h1>
    <form @submit.prevent="onSubmit">
      <EmailAddressInput :disabled="isPasswordRequired" required v-model="credentials.emailAddress" />
      <PasswordInput v-if="isPasswordRequired" required v-model="credentials.password" />
      <TarButton
        :disabled="isSubmitting"
        icon="fas fa-arrow-right-to-bracket"
        :loading="isSubmitting"
        :status="t('loading')"
        :text="t('users.signIn.submit')"
        type="submit"
      />
    </form>
  </main>
</template>
