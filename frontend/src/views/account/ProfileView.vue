<script setup lang="ts">
import { onMounted } from "vue";

import { getProfile } from "@/api/account";
import type { UserProfile } from "@/types/account";

onMounted(async () => {
  const profile: UserProfile = await getProfile();
  console.log(profile);
}); // TODO(fpion): implement
</script>

<template>
  <main class="container">
    <h1>ProfileView</h1>
  </main>
</template>
