<template>
    <v-app class="background-image">
      <menu-component></menu-component>
      <v-container >
        <router-view></router-view>
      </v-container>
    </v-app>
</template>

<style>
.background-image {
  background: url('@/assets/images/background.jpg') no-repeat center center fixed !important;
  background-size: cover;
}
</style>

<script lang="ts">
import Vue from "vue";
import MenuComponent from "./components/menu-bar/MenuComponent.vue";

export default Vue.extend({
  name: "App",
  components: {
    MenuComponent,
  },
});
</script>