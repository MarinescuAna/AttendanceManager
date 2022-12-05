<template>
  <v-card>
    <UsersTableComponent />
  </v-card>
</template>

<script lang="ts">
import Vue from "vue";
import UsersTableComponent from "@/components/user/UsersTableComponent.vue";

export default Vue.extend({
  components: {
    UsersTableComponent,
  },
});
</script>
