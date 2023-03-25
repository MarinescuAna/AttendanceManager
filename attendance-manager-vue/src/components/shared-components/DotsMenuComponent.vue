<template>
  <v-menu bottom right offset-y>
    <template v-slot:activator="{ on, attrs }">
      <v-btn icon v-bind="attrs" v-on="on">
        <v-icon>mdi-dots-vertical</v-icon>
      </v-btn>
    </template>
    <v-list>
      <v-list-item
        v-for="item in itemsMenu"
        :key="item.name"
        @click="onEvent(item.event)"
      >
        <v-list-item-title>{{ item.name }}</v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>
</template>
<script lang="ts">
import Vue from "vue";
export default Vue.extend({
  name: "DotsMenuComponent",
  data: function () {
    return {
      itemsMenu: [
        { name: "Change details", event: "onChange" },
        { name: "Delete", event: "onDelete" },
      ],
    };
  },
  methods: {
    onEvent: function (event: string): void {
      this.$emit(event);
    },
  },
});
</script>