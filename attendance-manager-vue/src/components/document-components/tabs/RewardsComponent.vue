<template>
  <v-layout class="ma-2" v-if="rewards?.length > 0" column>
    <v-flex class="pa-5">
      <h2 class="ma-3">Achieved rewards</h2>
      <v-layout v-if="achievedRewards.length > 0" wrap>
        <BadgeComponent
          v-for="badge in achievedRewards"
          :key="badge.badgeId"
          :badge="badge"
        />
      </v-layout>
      <MessageComponent
        description="You have no rewards achieved by now!"
        :color="'#FF6F00'"
        icon="mdi-information"
        fontSize="20px"
        fontWeight="bold"
        v-else
      />
    </v-flex>
    <v-flex class="pa-5">
      <h2 class="ma-3">Unachieved rewards</h2>
      <v-layout v-if="unachievedRewards.length > 0" wrap>
        <BadgeComponent
          v-for="badge in unachievedRewards"
          :key="badge.badgeId"
          :badge="badge"
        />
      </v-layout>
      <MessageComponent
        description="Great job! You achieved all the rewards."
        color="green"
        fontSize="20px"
        fontWeight="bold"
        v-else
      />
    </v-flex>
  </v-layout>
  <v-layout v-else>
    <MessageComponent
      description="You have no rewards defined in the system for your role that can be achieved."
      color="blue"
      icon="mdi-information"
      fontSize="20px"
      fontWeight="bold"
    />
  </v-layout>
</template>

<script lang="ts">
import { BadgeViewModule } from "@/modules/document";
import BadgeComponent from "@/components/shared-components/BadgeComponent.vue";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import RewardService from "@/services/reward.service";
import MessageComponent from "@/components/shared-components/MessageComponent.vue";

export default Vue.extend({
  name: "RewardsComponent",
  data: function () {
    return {
      rewards: [] as BadgeViewModule[],
    };
  },
  components: {
    BadgeComponent,
    MessageComponent,
  },
  computed: {
    achievedRewards: function (): BadgeViewModule[] {
      return this.rewards.filter((s) => s.isActive);
    },
    unachievedRewards: function (): BadgeViewModule[] {
      return this.rewards.filter((s) => !s.isActive);
    },
  },
  created: async function (): Promise<void> {
    this.rewards = await RewardService.getRewardsByReportIdAsync(
      storeHelper.documentStore.documentDetails.documentId
    );
  },
});
</script>