<template>
  <div>
    <v-toolbar short flat>
      <v-toolbar-title class="text-uppercase font-weight-black ml-12">
        <span class="font-weight-light">Attendance</span>
        <span>Manager</span>
      </v-toolbar-title>
    </v-toolbar>
    <v-navigation-drawer
      expand-on-hover
      class="blue-grey lighten-4"
      absolute
      permanent
    >
      <v-list>
        <MenuListItem
          hasImage="true"
          :src="src"
          v-if="!isLogged"
        />
        <MenuListItem
          hasImage="false"
          icon="mdi-login"
          goTo="/login"
          title="Sign In"
          v-if="!isLogged"
        />
        <MenuListItem
          hasImage="false"
          icon="mdi-exit-to-app"
          title="Sign Out"
          @click="logout"
          v-if="isLogged"
        />
        <v-divider></v-divider>
        <MenuListItem
          v-for="link in links"
          :key="link.text"
          :hasImage="false"
          :icon="link.icon"
          :goTo="link.route"
          :title="link.text"
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
import MenuListItem from "./MenuListItem.vue";

export default Vue.extend({
  components: {
    MenuListItem,
  },
  data() {
    return {
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
      /**
       *  This array contains all the routes and buttons available according to the user state (logged or not)
       *  Item:
       *  - icon
       *  - text
       *  - route
       */
      links: [
        {
          icon: "mdi-home",
          text: "Home",
          route: "/",
        },
        {
          icon: "mdi-information-variant",
          text: "About",
          route: "/about",
        },
      ],
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