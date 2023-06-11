<template>
  <v-layout wrap>
    <BadgeComponent
      v-for="badge in badges"
      :key="badge.badgeId"
      :badgePercentage="badge"
      :displayPercentage="true"
  /></v-layout>
</template>

<script lang="ts">
import BadgeComponent from "@/components/shared-components/BadgeComponent.vue";
import { BadgePercentageViewModule } from "@/modules/view-modules";
import BadgeService from "@/services/badge.service";
import Vue from "vue";
export default Vue.extend({
  name: "BadgePercentageComponent",
  components: { BadgeComponent },
  data: function () {
    return {
      badges: [] as BadgePercentageViewModule[],
    };
  },
  created: async function (): Promise<void> {
    this.badges = await BadgeService.getBadgesPercentageAsync();
  },
});
</script>