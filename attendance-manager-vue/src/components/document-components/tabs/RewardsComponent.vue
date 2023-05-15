<template>
  <v-layout class="ma-2" v-if="rewards?.length > 0">
    <BadgeComponent
      v-for="badge in rewards"
      :key="badge.title"
      :badge="badge"
    />
  </v-layout>
  <v-layout v-else>
    <h1>You have no rewards!</h1>
  </v-layout>
</template>

<script lang="ts">
import { BadgeViewModule } from "@/modules/document";
import BadgeComponent from "@/components/shared-components/BadgeComponent.vue";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
export default Vue.extend({
  name: "RewardsComponent",
  components: {
    BadgeComponent,
  },
  computed: {
    rewards: function (): BadgeViewModule[] {
      return storeHelper.documentStore.rewards;
    },
  },
});
</script>