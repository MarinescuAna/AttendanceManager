<template>
  <v-layout class="ma-2" v-if="rewards?.length > 0">
    <BadgeComponent
      v-for="badge in rewards"
      :key="badge.badgeId"
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
import RewardService from "@/services/reward.service";

export default Vue.extend({
  name: "RewardsComponent",
  data: function() {
    return {
      rewards: [] as BadgeViewModule[]
    }
  },
  components: {
    BadgeComponent,
  },
  created: async function():Promise<void> {
    this.rewards = await RewardService.getRewardsByReportIdAsync(storeHelper.documentStore.documentDetails.documentId);
  } 
});
</script>