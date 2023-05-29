<template>
  <v-alert :color="color" class="ma-2" prominent shaped>
    <v-icon class="ma-2 my-2 icon" v-if="icon != ''">{{ icon }}</v-icon>
    <span class="text-style" v-html="description"></span
  ></v-alert>
</template>

<style scoped>
.text-style {
  font-weight: bold;
  font-size: 20px;
}

.icon {
  font-size: 36px;
}
</style>

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