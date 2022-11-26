<template>
  <div>
    <v-toolbar flat>
      <v-toolbar-title class="text-uppercase font-weight-black ml-12">
        <span class="font-weight-light">Attendance</span>
        <span>Manager</span>
      </v-toolbar-title>
    </v-toolbar>
    <v-navigation-drawer
      expand-on-hover
      class="blue-grey lighten-4"
      absolute
      mini-variant.sync
      permanent
    >
      <v-list>
        {{ name }}
        <!-- <MenuItemComponent :item="signInItem" v-if="!isLogged" />
        <MenuItemComponent
          :item:="signOutItem"
          @click="logout"
          v-if="isLogged"
        /> -->
        <v-divider></v-divider>
        <MenuItemListComponent
          v-for="link in links"
          :key="link.title"
          :item="link"
        />
      </v-list>
    </v-navigation-drawer>
  </div>
</template>

<script lang="ts">
import { Role } from "@/shared/enums";
import UserService from "@/services/auth.service";
import Vue from "vue";
import { EventBus } from "@/main";
import { EVENT_BUS_ISLOGGED } from "@/shared/constants";
import MenuItemListComponent from "./MenuItemListComponent.vue";
// import MenuItemComponent from "./MenuItemComponent.vue";
import { links, MenuChildModel } from "./ItemList";

export default Vue.extend({
  components: {
    MenuItemListComponent,
    // MenuItemComponent,
  },
  data() {
    return {
      // Sign In button details
      signInItem: {
        icon: "mdi-login",
        role: Role.NoRole,
        route: "/login",
        title: "Sign In",
      } as MenuChildModel,
      // Sign Out button details
      signOutItem: {
        icon: "mdi-exit-to-app",
        role: Role.NoRole,
        route: "",
        title: "Sign Out",
      } as MenuChildModel,
      // Username
      name: "",
      // Role
      role: Role.NoRole,
      // Code
      code: "",
      // Image
      src: "",
      // Boolean for indicating is the user is logged or not
      isLogged: false,
      // List with all the existent buttons for defined pages 
      links: links,
    };
  },
  /**
   * Check if the user is logged and display the proper buttons in the navbar
   */
  created(): void {
    this.isLogged = UserService.isLogged();
    if (this.isLogged) {
      this.setUserData();
    } else {
      this.src = "./images/logo.jpg";
    }
  },
  mounted: function () {
    /**
     * Emit an event using EventBus every time the user logs in to update the navbar
     */
    EventBus.$on(EVENT_BUS_ISLOGGED, () => {
      this.isLogged = true;
      this.setUserData();
      EventBus.$off(EVENT_BUS_ISLOGGED);
    });
  },
  methods: {
    /**
     * Use this method to update the user information: name, role, code and image
     */
    setUserData(): void {
      const data = UserService.getDataFromToken();
      this.name = data.name;
      this.role = data.role;
      this.code = data.code;
      this.src = `./images/${this.role}-profile.jpg`;
    },

    /**
     * Use this method to logout
     */
    logout(): void {
      this.isLogged = false;
      this.src = "./images/logo.jpg";
      UserService.logout();
    },
  },
});
</script>