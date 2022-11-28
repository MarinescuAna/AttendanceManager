<template>
  <v-list-item router :to="item.route">
    <v-list-item-icon>
      <v-icon>{{ item.icon }}</v-icon>
    </v-list-item-icon>
    <v-list-item-content>
      <v-list-item-title>{{ item.title }}</v-list-item-title>
    </v-list-item-content>
  </v-list-item>
</template>
  
  
  <script lang="ts">
import Vue from "vue";
import { MenuChildModel } from "../menu-bar/ItemList";

export default Vue.extend({
  props: {
    // Child details
    item: Object as () => MenuChildModel
  }
});
</script>