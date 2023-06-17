<template>
  <v-menu
    ref="menu"
    transition="scale-transition"
    min-width="auto"
    :close-on-content-click="false"
    :return-value.sync="selectedDate"
    offset-y
  >
    <template v-slot:activator="{ on, attrs }">
      <v-text-field
        v-model="selectedDate"
        label="Select date"
        prepend-icon="mdi-calendar"
        v-bind="attrs"
        v-on="on"
        readonly
      ></v-text-field>
    </template>
    <v-date-picker v-model="selectedDate" @input="onSubmit" no-title scrollable>
    </v-date-picker>
  </v-menu>
</template>

<script lang="ts">
import Vue from "vue";
import { VMenu } from "vuetify/lib/components/VMenu";

export default Vue.extend({
  name: "DatePickerComponent",
  data: function () {
    return {
      selectedDate: '',
    };
  },
  props: {
    date: {
      type: String,
      default: '',
    },
  },
  created: function (): void {
    this.selectedDate = this.date;
  },
  methods: {
    onSubmit: function (): void {
      (this.$refs.menu as VMenu).save(this.selectedDate);
      this.$emit("save", this.selectedDate);
    },
  },
});
</script>