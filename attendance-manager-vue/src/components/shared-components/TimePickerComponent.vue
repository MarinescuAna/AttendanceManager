<template>
  <v-menu
    ref="menu"
    transition="scale-transition"
    max-width="290px"
    min-width="290px"
    :nudge-right="40"
    :close-on-content-click="false"
    :return-value.sync="timeSelected"
    offset-y
  >
    <template v-slot:activator="{ on, attrs }">
      <v-text-field
        v-model="timeSelected"
        label="Select time"
        prepend-icon="mdi-clock-time-four-outline"
        v-bind="attrs"
        color="black"
        v-on="on"
        readonly
      ></v-text-field>
    </template>
    <v-time-picker
      v-model="timeSelected"
      format="24hr"
      @click:minute="onSubmit"
      full-width
      color="black"
    ></v-time-picker>
  </v-menu>
</template>

<script lang="ts">
import Vue from "vue";
import { VMenu } from 'vuetify/lib/components/VMenu';

export default Vue.extend({
  name: "TimePickerComponent",
  data: function () {
    return {
      timeSelected: '',
    };
  },
  props:{
    time: {
      type: String,
      default: ''
    }
  },
  created: function(): void{
    this.timeSelected = this.time;
  },
  methods: {
    onSubmit: function (): void {
      (this.$refs.menu as VMenu).save(this.timeSelected);
      this.$emit("save", this.timeSelected);
    },
  },
});
</script>