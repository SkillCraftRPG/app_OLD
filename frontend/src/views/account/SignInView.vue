<script setup lang="ts">
import { inject, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import { useRoute, useRouter } from "vue-router";

import AuthenticationLinkSent from "@/components/users/AuthenticationLinkSent.vue";
import SignInForm from "@/components/users/SignInForm.vue";
import type { SignInPayload, SignInResponse } from "@/types/account";
import { handleErrorKey } from "@/inject/App";
import { signIn } from "@/api/account";
import { useAccountStore } from "@/stores/account";

const account = useAccountStore();
const handleError = inject(handleErrorKey) as (e: unknown) => void;
const route = useRoute();
const router = useRouter();
const { locale } = useI18n();

const hasLoaded = ref<boolean>(false);
const response = ref<SignInResponse>();

function setResponse(value: SignInResponse): void {
  if (value.currentUser) {
    account.signIn(value.currentUser);
    const redirect: string | undefined = route.query.redirect?.toString();
    router.push(redirect ?? { name: "Profile" });
  } else if (value.profileCompletionToken) {
    router.push({ name: "CompleteProfile", params: { token: value.profileCompletionToken } });
  } else {
    response.value = value;
  }
}

onMounted(async () => {
  try {
    const authenticationToken: string | undefined = route.query.token?.toString();
    if (authenticationToken) {
      const payload: SignInPayload = {
        locale: locale.value,
        authenticationToken,
      };
      const response: SignInResponse = await signIn(payload);
      setResponse(response);
    }
  } catch (e: unknown) {
    console.error(e);
    router.push({ name: "SignIn" });
  } finally {
    hasLoaded.value = true;
  }
});
</script>

<template>
  <main class="container">
    <template v-if="response">
      <AuthenticationLinkSent v-if="response.authenticationLinkSentTo" :sent-message="response.authenticationLinkSentTo" />
    </template>
    <SignInForm v-else-if="hasLoaded" @error="handleError" @response="setResponse" />
  </main>
</template>
