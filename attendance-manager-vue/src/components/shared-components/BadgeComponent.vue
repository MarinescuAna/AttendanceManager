<template>
  <v-tooltip v-if="!displayAsCustomBadge && !displayPercentage" bottom>
    <template v-slot:activator="{ on, attrs }">
      <v-layout
        class="badge-container"
        :class="!badge.isActive ? 'inactive' : ''"
        v-bind="attrs"
        v-on="on"
        align-center
        column
        ><v-badge
          :bordered="badge.isCustom"
          :color="badge.isCustom ? 'grey' : ''"
          :content="badge.isCustom ? 'Custom' : ''"
          bottom
          overlap
        >
          <v-img
            class="custom-image-display"
            :src="require(`@/assets/images/badges/${badge.imagePath}`)"
          ></v-img>
        </v-badge>
        <span class="title d-flex justify-center">
          {{ badge.title }}
        </span>
      </v-layout>
    </template>
    <span>{{ tooltip }}</span>
  </v-tooltip>
  <v-card
    width="400px"
    class="d-flex flex-column align-center ma-2"
    v-else-if="displayAsCustomBadge"
  >
  <v-badge
      color="grey"
      content="Custom"
      bottom
      overlap
      bordered
    >
    <v-img
      :src="require(`@/assets/images/badges/${badge.imagePath}`)"
      width="200px"
      height="100px"
      class="custom-image-display mt-3"
      contain
    ></v-img>
    </v-badge>
    <v-card-title>{{ badge.title }}</v-card-title>
    <v-card-subtitle
      ><b>Description: </b>{{ badge.description }}<br />
      <b>Activity type: </b
      >{{ CourseType[badge.activityType] }}</v-card-subtitle
    >
  </v-card>
  <v-card width="400px" class="d-flex flex-column align-center ma-2" v-else>
    <v-badge
      :bordered="badgePercentage.isCustom"
      :color="badgePercentage.isCustom ? 'grey' : ''"
      :content="badgePercentage.isCustom ? 'Custom' : ''"
      bottom
      overlap
    >
      <v-img
        :src="require(`@/assets/images/badges/${badgePercentage.imagePath}`)"
        width="200px"
        height="100px"
        class="custom-image-display mt-3"
        contain
      ></v-img
    ></v-badge>
    <v-card-title>{{ badgePercentage.title }}</v-card-title>
    <v-card-subtitle
      ><b>Requirements: </b>{{ badgePercentage.description }}</v-card-subtitle
    >
    <v-progress-circular
      :rotate="360"
      :size="100"
      :width="15"
      :value="badgePercentage.percentage"
      class="ma-5"
      :color="WARNING_AMBER_DARKEN_4"
    >
      {{ Number.parseFloat(badgePercentage.percentage.toFixed(2)) }}%
    </v-progress-circular>
  </v-card>
</template>

<style scoped>
.badge-container {
  width: 200px;
}
.custom-image-display {
  max-width: 130px;
  max-height: 130px;
  min-width: 130px;
  min-height: 130px;
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.7);
}
.title {
  font-size: 15px !important;
  font-family: cursive !important;
  font-weight: bold;
}
.inactive {
  opacity: 0.4;
}
</style>
<script lang="ts">
import Vue from "vue";
import { BadgePercentageViewModule, BadgeViewModule } from "@/modules/view-modules";
import { CourseType } from "@/shared/enums";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";

export default Vue.extend({
  name: "BadgeComponent",
  props: {
    badge: {
      type: Object as () => BadgeViewModule,
    },
    displayAsCustomBadge: {
      type: Boolean,
      default: false,
    },
    displayPercentage: {
      type: Boolean,
      default: false,
    },
    badgePercentage: {
      type: Object as () => BadgePercentageViewModule,
    },
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      CourseType,
    };
  },
  computed: {
    tooltip: function (): string {
      return this.badge.description === ""
        ? this.badge.title
        : this.badge.description;
    },
  },
});
</script>