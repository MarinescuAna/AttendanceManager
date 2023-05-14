<template>
  <v-card width="150" class="ma-2">
    <v-img 
      width="150"
      height="100"
      max-height="100"
      max-width="150"
      :src="require(`@/assets/images/badges/${badge.imagePath}`)"
    ></v-img>
    <v-card-subtitle> 
        {{ badge.title }}
    </v-card-subtitle>
  </v-card>
</template>

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
});
</script>