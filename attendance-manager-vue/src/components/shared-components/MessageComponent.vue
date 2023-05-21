<template>
  <v-alert :color="color" class="ma-2" prominent shaped
    > <v-icon class="ma-2" v-if="icon!=''">{{ icon }}</v-icon><span v-html="description"></span
  ></v-alert>
</template>

<script lang="ts">
import Vue from "vue";
export default Vue.extend({
  name: "MessageComponent",
  props: {
    description: {
      type: String,
      required: true,
    },
    color: {
      type: String,
      default: "grey",
    },
    icon: {
      type: String,
      default: "",
    },
  },
});
</script>