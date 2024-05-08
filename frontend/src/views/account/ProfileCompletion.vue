<script setup lang="ts">
import { TarProgress } from "logitar-vue3-ui";
import { computed, inject, ref } from "vue";
import { useI18n } from "vue-i18n";
import { useRoute, useRouter } from "vue-router";

import ProfileIdentification from "@/components/users/ProfileIdentification.vue";
import ProfilePersonal from "@/components/users/ProfilePersonal.vue";
import ProfileSecurity from "@/components/users/ProfileSecurity.vue";
import type {
  CompleteProfilePayload,
  Identification,
  JwtPayload,
  PersonalInformation,
  SecurityInformation,
  SignInPayload,
  SignInResponse,
} from "@/types/account";
import { decode } from "@/helpers/jwtUtils";
import { handleErrorKey } from "@/inject/App";
import { signIn } from "@/api/account";
import { useAccountStore } from "@/stores/account";

const account = useAccountStore();
const handleError = inject(handleErrorKey) as (e: unknown) => void;
const route = useRoute();
const router = useRouter();
const { locale, t } = useI18n();

type Step = "Identification" | "Personal" | "Security";

const isSubmitting = ref<boolean>(false);
const profile = ref<CompleteProfilePayload>({ token: "", multiFactorAuthenticationMode: "None", firstName: "", lastName: "", locale: "", timeZone: "" });
const step = ref<Step>("Identification");

const progress = computed<number>(() => {
  if (isSubmitting.value) {
    return 100.0;
  }
  switch (step.value) {
    case "Personal":
      return 66.67;
    case "Security":
      return 33.33;
    default:
      return 0;
  }
});
const token = computed<string>(() => route.params.token.toString());
const emailAddress = computed<string>(() => {
  const payload = decode(token.value) as JwtPayload;
  return payload.email ?? "";
});
const phoneNumber = computed<string | undefined>(() => {
  const payload = decode(token.value) as JwtPayload;
  return payload.phone_number || undefined;
});

function onIdentificationContinue(value: Identification): void {
  profile.value.firstName = value.firstName;
  profile.value.middleName = value.middleName || undefined;
  profile.value.lastName = value.lastName;
  step.value = "Security";
}
async function onPersonalContinue(value: PersonalInformation): Promise<void> {
  profile.value.birthdate = value.birthdate;
  profile.value.gender = value.gender;
  profile.value.locale = value.locale;
  profile.value.timeZone = value.timeZone;
  profile.value.token = token.value;
  isSubmitting.value = true;
  try {
    const payload: SignInPayload = {
      locale: locale.value,
      profile: profile.value,
    };
    const response: SignInResponse = await signIn(payload);
    if (!response.currentUser) {
      throw new Error("The 'currentUser' is required.");
    }
    account.signIn(response.currentUser);
    router.push({ name: "Profile" });
  } catch (e: unknown) {
    handleError(e);
  } finally {
    isSubmitting.value = false;
  }
}
function onSecurityContinue(value: SecurityInformation): void {
  profile.value.password = value.password;
  profile.value.multiFactorAuthenticationMode = value.multiFactorAuthenticationMode;
  step.value = "Personal";
}
</script>

<template>
  <main class="container">
    <h1>{{ t("users.profile.complete") }}</h1>
    <TarProgress :aria-label="t('users.profile.progress')" :value="progress" />
    <ProfileIdentification v-show="step === 'Identification'" @continue="onIdentificationContinue" />
    <ProfileSecurity
      v-show="step === 'Security'"
      :email-address="emailAddress"
      :phone-number="phoneNumber"
      @back="step = 'Identification'"
      @continue="onSecurityContinue"
    />
    <ProfilePersonal v-show="step === 'Personal'" :loading="isSubmitting" @back="step = 'Security'" @continue="onPersonalContinue" />
  </main>
</template>
