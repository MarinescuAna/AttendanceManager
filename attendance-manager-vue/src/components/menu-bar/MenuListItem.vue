<template>
     <v-list-item
          router
          :to="goTo"
        >
        <v-list-item-avatar v-if="hasImage">
            <v-img :src="src"></v-img>
        </v-list-item-avatar>
          <v-list-item-icon v-if="!hasImage">
            <v-icon>{{ icon }}</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>{{ title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
</template>


<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  props: {
    title: String,
    src: String,
    goTo: String,
    icon: String,
    hasImage:  Boolean,
  },
});
</script>