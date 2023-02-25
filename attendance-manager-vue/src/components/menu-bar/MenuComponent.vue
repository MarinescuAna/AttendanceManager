<template>
  <div>
    <v-toolbar flat color="transparent">
      <v-app-bar-nav-icon
        @click.stop="drawerActivator = !drawerActivator"
      ></v-app-bar-nav-icon>
      <router-link :to="{ name: 'home' }" v-slot="{ navigate }">
        <v-toolbar-title
          class="text-uppercase font-weight-black"
          @click="navigate"
        >
          <span class="font-weight-light">Attendance</span>
          <span>Manager</span>
        </v-toolbar-title>
      </router-link>
    </v-toolbar>
    <v-navigation-drawer
      v-model="drawerActivator"
      class="blue-grey lighten-4 navigation-drawer-style"
      absolute
      temporary
      width="auto"
    >
    <v-row>
      <v-container justify="center">
        <!--Display title-->
        <v-container v-if="isLogged">
          <h3 class="text-uppercase">{{ name }}</h3>
          <h4>{{ email }}</h4>
          <h4>{{ code }}</h4>
        </v-container>
        <v-container v-else>
          <h3 class="text-uppercase">
            <span class="font-weight-light">Attendance</span>
            <span>Manager</span>
          </h3>
        </v-container>
      </v-container>
    </v-row>
      <!--Login or Logout button-->
      <v-container v-if="!isLogged">
        <v-btn
          @click="onRedirectToLogin"
          title="Login"
          class="orange lighten-3"
          block
        >
          SIGN IN
        </v-btn>
      </v-container>
      <v-container v-else>
        <v-btn @click="logout" class="orange lighten-3" block> SIGN OUT </v-btn>
      </v-container>

      <v-divider></v-divider>

      <!--links-->
      <v-list>
        <MenuItemListComponent
          v-for="link in links"
          :key="link.title"
          :item="link"
        />
      </v-list>
    </v-navigation-drawer>
  </div>
</template>

<style>
.navigation-drawer-style {
  min-width: 10%;
}
</style>

<script lang="ts">
import { Role } from "@/shared/enums";
import AuthService from "@/services/auth.service";
import Vue from "vue";
import { EventBus } from "@/main";
import { EVENT_BUS_ISLOGGED } from "@/shared/constants";
import MenuItemListComponent from "./MenuItemListComponent.vue";
import { MenuItems, MenuItemListModel } from "./ItemList";

export default Vue.extend({
  name: "MenuComponent",
  components: {
    MenuItemListComponent,
  },
  data() {
    return {
      // Username
      name: "",
      // Code
      code: "",
      // Email
      email: "",
      // Boolean for indicating is the user is logged or not
      isLogged: false,
      // List with all the existent buttons for defined pages
      links: [] as MenuItemListModel[],
      // Use this in order to activeate the drawer
      drawerActivator: false,
    };
  },
  created(): void {
    this.setProperties();
  },
  mounted: function () {
    /**
     * Emit an event using EventBus every time the user logs in to update the navbar
     */
    EventBus.$on(EVENT_BUS_ISLOGGED, () => {
      this.isLogged = true;
      this.setProperties();
    });
  },
  destroyed(): void {
    EventBus.$off(EVENT_BUS_ISLOGGED);
  },
  methods: {
    /**
     * Use this method in order to set the page properties
     */
    setProperties(): void {
      const data = AuthService.getDataFromToken();

      if (data === null) {
        this.links = MenuItems.getLinkListByRole(Role.All);
        this.isLogged = false;
      } else {
        this.isLogged = true;
        this.name = data.name;
        this.code = data.code;
        this.email = data.email;
        this.links = MenuItems.getLinkListByRole(Role[data.role]);
      }
    },

    /**
     * Use this method to logout
     */
    logout(): void {
      this.isLogged = false;
      this.links = MenuItems.getLinkListByRole(Role.All);
      AuthService.logout();
      this.onRedirectToLogin();
    },

    /**
     * Use this method to redirect the user to login page
     */
    onRedirectToLogin(): void {
      if (this.$route.name !== "login") {
        this.$router.push({ name: "login" });
      }
    },
  },
});
</script>