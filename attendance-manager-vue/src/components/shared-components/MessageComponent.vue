<template>
  <v-alert :style="{'background-color':backgroundColor,'color':color}" class="ma-2" prominent shaped>
    <v-icon
      class="ma-2 my-2"
      :style="{ 'font-size': iconSize,'color':color }"
      v-if="icon != ''"
      >{{ icon }}</v-icon
    >
    <span
      :style="{ 'font-size': fontSize, 'font-weight': fontWeight }"
      v-html="description"
    ></span
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
    backgroundColor: {
      type: String,
      default: "transparent",
    },
    color: {
      type: String,
      default: "black",
    },
    icon: {
      type: String,
      default: "",
    },
    iconSize: {
      type: String,
      default: "36px",
    },
    fontSize: {
      type: String,
      default: "16px",
    },
    fontWeight: {
      type: String,
      default: "",
    },
  },
});
</script>