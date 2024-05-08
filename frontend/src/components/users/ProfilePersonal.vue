<script setup lang="ts">
import { TarButton } from "logitar-vue3-ui";
import { ref } from "vue";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";

import BirthdateInput from "./BirthdateInput.vue";
import GenderSelect from "./GenderSelect.vue";
import LocaleSelect from "./LocaleSelect.vue";
import TimeZoneSelect from "./TimeZoneSelect.vue";
import type { PersonalInformation } from "@/types/account";

const { t } = useI18n();

defineProps<{
  loading?: boolean;
}>();

const birthdate = ref<Date>();
const gender = ref<string>("");
const locale = ref<string>("fr");
const timeZone = ref<string>("America/Montreal");

const emit = defineEmits<{
  (e: "back"): void;
  (e: "continue", value: PersonalInformation): void;
}>();

const { handleSubmit } = useForm();
const onSubmit = handleSubmit(() => {
  emit("continue", {
    birthdate: birthdate.value,
    gender: gender.value,
    locale: locale.value,
    timeZone: timeZone.value,
  });
});
</script>

<template>
  <div>
    <h3>{{ t("users.profile.personal.title") }}</h3>
    <p>
      <i><font-awesome-icon icon="fas fa-user" /> {{ t("users.profile.personal.lead") }}</i>
    </p>
    <form @submit="onSubmit">
      <div class="row">
        <LocaleSelect class="col-lg-6" required v-model="locale" />
        <TimeZoneSelect class="col-lg-6" required v-model="timeZone" />
      </div>
      <div class="row">
        <BirthdateInput class="col-lg-6" v-model="birthdate" />
        <GenderSelect class="col-lg-6" v-model="gender" />
      </div>
      <TarButton class="me-1" icon="fas fa-arrow-left" :text="t('actions.back')" variant="secondary" @click="$emit('back')" />
      <TarButton
        class="ms-1"
        :disabled="loading"
        icon="fas fa-check"
        :loading="loading"
        :status="t('loading')"
        :text="t('actions.complete')"
        type="submit"
        variant="success"
      />
    </form>
  </div>
</template>
