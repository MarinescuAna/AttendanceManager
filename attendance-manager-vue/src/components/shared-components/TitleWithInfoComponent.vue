<template>
  <v-flex class="one-row">
    <span :class="h_Text+'-text'">{{ title }}</span>
    <v-tooltip top>
      <template v-slot:activator="{ on, attrs }">
        <v-icon v-bind="attrs" v-on="on" class="ma-1">mdi-alert-circle</v-icon>
      </template>
      <span v-html="description"></span>
    </v-tooltip>
  </v-flex>
</template>

<style scoped>
.one-row {
  display: -webkit-box;
}
</style>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  name: "TitleWithInfoComponent",
  props: {
    description: {
      type: String,
      required: true,
    },
    title: {
      type: String,
      required: true,
    },
    h_Text:{
      type: String,
      default: 'h3'
    }
  },
});
</script>