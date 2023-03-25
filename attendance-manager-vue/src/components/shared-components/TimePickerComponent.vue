<template>
  <v-menu
    ref="menu"
    v-model="menu2"
    :close-on-content-click="false"
    :nudge-right="40"
    :return-value.sync="time"
    transition="scale-transition"
    offset-y
    max-width="290px"
    min-width="290px"
  >
    <template v-slot:activator="{ on, attrs }">
      <v-text-field
        v-model="time"
        label="Select time"
        prepend-icon="mdi-clock-time-four-outline"
        readonly
        v-bind="attrs"
        v-on="on"
      ></v-text-field>
    </template>
    <v-time-picker
      v-if="menu2"
      v-model="time"
      full-width
      @click:minute="onSubmit"
    ></v-time-picker>
  </v-menu>
</template>

<script lang="ts">
import Vue from "vue";
export default Vue.extend({
  name: "TimePickerComponent",
  data: function () {
    return {
      time: null,
      menu2: false,
    };
  },
  methods: {
    onSubmit: function (): void {
      this.$refs.menu.save(this.time);
      this.$emit("save", this.time);
    },
  },
});
</script>