<template>
  <v-app class="grey lighten-5">
    <menu-component></menu-component>
    <v-container>
      <router-view></router-view>
    </v-container>
  </v-app>
</template>

<script lang="ts">
import Vue from "vue";
import MenuComponent from "./components/menu-bar/MenuComponent.vue";

export default Vue.extend({
  components: {
    MenuComponent
  }
});
</script>