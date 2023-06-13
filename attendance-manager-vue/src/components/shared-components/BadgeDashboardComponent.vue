<template>
  <v-card width="400px" class="d-flex flex-column align-center ma-2">
    <v-badge :color="WARNING_AMBER_DARKEN_4" :content="role" bordered bottom overlap>
      <v-img
        :src="require(`@/assets/images/badges/${imagePath}`)"
        width="200px"
        height="100px"
        class="custom-image-display mt-3"
        contain
      ></v-img
    ></v-badge>
    <v-card-title>{{ title }}</v-card-title>
    <v-card-subtitle
      ><b>Requirements: </b>{{ description }}<br /><b
        >Number of achievements: </b
      >{{ occurrences }}/{{ maxOccurrences }}</v-card-subtitle
    >
    <v-progress-circular
      :rotate="360"
      :size="100"
      :width="15"
      :value="percentage"
      class="ma-5"
      :color="WARNING_AMBER_DARKEN_4"
    >
      {{ percentage }}%
    </v-progress-circular>
  </v-card>
</template>
  
  <style scoped>
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
</style>
  <script lang="ts">
import Vue from "vue";
import { Role } from "@/shared/enums";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";

export default Vue.extend({
  name: "BadgeDashboardComponent",
  props: {
    title: {
      type: String,
      required: true,
    },
    description: {
      type: String,
      required: true,
    },
    imagePath: {
      type: String,
      required: true,
    },
    role: {
      type: String,
      required: true,
    },
    occurrences: {
      type: Number,
      required: true,
    },
    maxOccurrences: {
      type: Number,
      required: true,
    },
    percentage: {
      type: Number,
      required: true,
    },
  },
  data: function () {
    return {
      Role,
      WARNING_AMBER_DARKEN_4,
    };
  },
});
</script>