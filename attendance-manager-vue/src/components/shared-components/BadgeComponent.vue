<template>
  <v-tooltip bottom>
    <template v-slot:activator="{ on, attrs }">
      <v-layout
        class="badge-container"
        :class="!badge.isActive ? 'inactive' : ''"
        v-bind="attrs"
        v-on="on"
        align-center
        justify-center
        column
      >
        <v-img
          class="image-display"
          :src="require(`@/assets/images/badges/${badge.imagePath}`)"
        ></v-img>
        <span class="title">
          {{ badge.title }}
        </span>
      </v-layout>
    </template>
    <span>{{ tooltip }}</span>
  </v-tooltip>
</template>

<style scoped>
.badge-container {
  width: 130px;
}
.image-display {
  max-width: 130px;
  max-height: 130px;
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
  text-align: center;
}
.inactive {
  opacity: 0.4;
}
</style>
<script lang="ts">
import Vue from "vue";
import { BadgeViewModule } from "@/modules/document";

export default Vue.extend({
  name: "BadgeComponent",
  props: {
    badge: {
      required: true,
      type: Object as () => BadgeViewModule,
    },
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