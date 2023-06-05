<template>
    <v-app class="background-image">
      <menu-component></menu-component>
      <v-container >
        <router-view></router-view>
      </v-container>
    </v-app>
</template>

<style lang="scss" scoped>
.background-image {
  background-color: #CFD8DC;
}
</style>

<script lang="ts">
import Vue from "vue";
import MenuComponent from "./components/menu-bar/MenuComponent.vue";

export default Vue.extend({
  name: "App",
  components: {
    MenuComponent,
  }
});
</script>