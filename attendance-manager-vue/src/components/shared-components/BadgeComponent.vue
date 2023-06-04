<template>
  <v-tooltip v-if="!displayAsCustomBadge" bottom>
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
  <v-card width="400px" class="d-flex flex-column align-center ma-2" v-else>
    <v-img
      :src="require(`@/assets/images/badges/${badge.imagePath}`)"
      width="200px"
      height="100px"
      contain
    ></v-img>
    <v-card-title>{{ badge.title }}</v-card-title>
    <v-card-subtitle
      ><b>Description: </b>{{ badge.description }}<br />
      <b>Activity type: </b
      >{{ CourseType[badge.activityType] }}</v-card-subtitle
    >
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
import { BadgeViewModule } from "@/modules/document";
import { CourseType } from "@/shared/enums";

export default Vue.extend({
  name: "BadgeComponent",
  props: {
    badge: {
      required: true,
      type: Object as () => BadgeViewModule,
    },
    displayAsCustomBadge: {
      type: Boolean,
      default: false,
    },
  },
  data: function () {
    return {
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