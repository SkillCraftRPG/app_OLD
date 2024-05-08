<script setup lang="ts">
import { TarButton } from "logitar-vue3-ui";
import { ref } from "vue";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";

import EmailAddressInput from "./EmailAddressInput.vue";
import PasswordInput from "./PasswordInput.vue";
import PhoneNumberInput from "./PhoneNumberInput.vue";
import type { MultiFactorAuthenticationMode, SecurityInformation } from "@/types/account";
import type { PasswordSettings } from "@/types/settings";

const passwordSettings: PasswordSettings = {
  minimumLength: 8,
  uniqueCharacters: 8,
  requireNonAlphanumeric: true,
  requireLowercase: true,
  requireUppercase: true,
  requireDigit: true,
};
const { t } = useI18n();

defineProps<{
  emailAddress: string;
  phoneNumber?: string;
}>();

const multiFactorAuthenticationMode = ref<MultiFactorAuthenticationMode>("None");
const newPassword = ref<string>("");
const passwordConfirmation = ref<string>("");
const usePassword = ref<boolean>(false);

function setUsePassword(value: boolean): void {
  usePassword.value = value;
  if (!value) {
    newPassword.value = "";
    passwordConfirmation.value = "";
    multiFactorAuthenticationMode.value = "None";
  }
}

const emit = defineEmits<{
  (e: "back"): void;
  (e: "continue", value: SecurityInformation): void;
}>();

const { handleSubmit } = useForm();
const onSubmit = handleSubmit(() => {
  emit("continue", {
    password: usePassword.value ? newPassword.value : undefined,
    multiFactorAuthenticationMode: usePassword.value ? multiFactorAuthenticationMode.value : "None",
  });
});
</script>

<template>
  <div>
    <h3>{{ t("users.profile.security.title") }}</h3>
    <p>
      <i><font-awesome-icon icon="fas fa-lock" /> {{ t("users.profile.security.lead") }}</i>
    </p>
    <form @submit="onSubmit">
      <div class="row">
        <EmailAddressInput :class="{ 'col-lg-6': Boolean(phoneNumber) }" disabled :model-value="emailAddress" show-status="never" />
        <PhoneNumberInput v-if="phoneNumber" class="col-lg-6" disabled :model-value="phoneNumber" show-status="never" />
      </div>
      <div class="mb-3">
        <input class="btn-check" id="password-less" name="usePassword" type="radio" value="false" :checked="!usePassword" @change="setUsePassword(false)" />
        <label class="btn btn-outline-primary me-1" for="password-less">
          <font-awesome-icon icon="fas fa-envelope" /> {{ t("users.profile.security.password.less.label") }}
        </label>
        <input class="btn-check" id="with-password" name="usePassword" type="radio" value="true" :checked="usePassword" @change="setUsePassword(true)" />
        <label class="btn btn-outline-primary mx-1" for="with-password">
          <font-awesome-icon icon="fas fa-key" /> {{ t("users.profile.security.password.with.label") }}
        </label>
        <span class="ms-1">
          <font-awesome-icon icon="fas fa-circle-info" /> {{ t(`users.profile.security.password.${usePassword ? "with" : "less"}.help`) }}
        </span>
      </div>
      <div v-if="usePassword">
        <h5>{{ t("users.password.label") }}</h5>
        <div class="row">
          <PasswordInput
            class="col-lg-6"
            id="new-password"
            label="users.password.new"
            :required="usePassword"
            :settings="passwordSettings"
            v-model="newPassword"
          />
          <PasswordInput
            class="col-lg-6"
            :confirm="{ value: newPassword, label: 'users.password.new' }"
            id="password-confirmation"
            label="users.password.confirm"
            :required="usePassword"
            v-model="passwordConfirmation"
          />
        </div>
        <h5>{{ t("users.profile.security.multiFactorAuthentication.label") }}</h5>
        <div class="mb-3">
          <input class="btn-check" id="mfa-none" name="multiFactorAuthenticationMode" type="radio" value="None" v-model="multiFactorAuthenticationMode" />
          <label class="btn btn-outline-primary me-1" for="mfa-none">
            <font-awesome-icon icon="fas fa-times" /> {{ t("users.profile.security.multiFactorAuthentication.None.label") }}
          </label>
          <input class="btn-check" id="mfa-email" name="multiFactorAuthenticationMode" type="radio" value="Email" v-model="multiFactorAuthenticationMode" />
          <label class="btn btn-outline-primary mx-1" for="mfa-email">
            <font-awesome-icon icon="fas fa-at" /> {{ t("users.profile.security.multiFactorAuthentication.Email.label") }}
          </label>
          <input
            class="btn-check"
            disabled
            id="mfa-phone"
            name="multiFactorAuthenticationMode"
            type="radio"
            value="Phone"
            v-model="multiFactorAuthenticationMode"
          />
          <label class="btn btn-outline-primary mx-1" for="mfa-phone">
            <font-awesome-icon icon="fas fa-phone" /> {{ t("users.profile.security.multiFactorAuthentication.Phone.label") }}
          </label>
          <span class="ms-1">
            <font-awesome-icon icon="fas fa-circle-info" /> {{ t(`users.profile.security.multiFactorAuthentication.${multiFactorAuthenticationMode}.help`) }}
          </span>
        </div>
      </div>
      <TarButton class="me-1" icon="fas fa-arrow-left" :text="t('actions.back')" variant="secondary" @click="$emit('back')" />
      <TarButton class="ms-1" icon="fas fa-arrow-right" :text="t('actions.continue')" type="submit" />
    </form>
  </div>
</template>
