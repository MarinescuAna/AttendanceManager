<template>
  <div>
    <h4 v-if="title !== ''">{{ title }}</h4>
    <p>{{ description }}</p>
  </div>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  name: "ToastificationComponent",
  props: {
    description: {
      type: String,
      required: true,
    },
    title: {
      type: String,
      default: "",
    },
  },
});
</script>