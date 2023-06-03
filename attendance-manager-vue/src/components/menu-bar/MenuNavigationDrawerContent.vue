<template>
  <div>
    <v-layout justify-center>
      <!--Display title-->
      <v-flex class="mx-5 mt-5" v-if="isLogged">
        <h3 class="text-uppercase">{{ name }}</h3>
        <h4>{{ email }}</h4>
        <h4>{{ code }}</h4>
      </v-flex>
      <v-flex class="mx-5 mt-5" v-else>
        <h3 class="text-uppercase">
          <span class="font-weight-light">Attendance</span>
          <span>Manager</span>
        </h3>
      </v-flex>
    </v-layout>
    <!--Login or Logout button-->
    <div class="ma-5" v-if="!isLogged">
      <v-btn
        @click="onRedirectToLogin"
        title="Login"
        class="light_button"
        block
      >
        SIGN IN
      </v-btn>
    </div>
    <div class="ma-5" v-else>
      <v-btn @click="logout" class="light_button" block> SIGN OUT </v-btn>
    </div>

    <v-divider></v-divider>

    <!--links-->
    <v-list>
      <v-list-item
        v-for="link in links"
        :key="link.title"
        :to="{ name: link.route }"
        router
      >
        <v-list-item-icon>
          <v-icon>{{ link.icon }}</v-icon>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title>{{ link.title }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>
  </div>
</template>

<script lang="ts">
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";
import Vue from "vue";
import { MenuChildModel, MenuItems } from "@/components/menu-bar/ItemList";
export default Vue.extend({
  name: "MenuNavigationDrawerContent",
  props: {
    isLogged: {
      type: Boolean,
      required: true,
    },
  },
  data: function () {
    return {
      name: "",
      code: "",
      email: "",
      // List with all the existent buttons for defined pages
      links: [] as MenuChildModel[],
    };
  },
  created: function(): void {
    this.setProperties();
  },
  watch: {
    isLogged: function (): void {
      this.setProperties();
    },
  },
  methods: {
    /**
     * Use this method in order to set the page properties
     */
    setProperties: function (): void {
      const data = AuthService.getDataFromToken();

      if (data === null) {
        this.links = MenuItems.getLinkListByRole(Role.All);
        this.$emit("login-changed", false);
      } else {
        this.$emit("login-changed", true);
        this.name = data.name;
        this.code = data.code;
        this.email = data.email;
        this.links = MenuItems.getLinkListByRole(Role[data.role]);
      }
    },

    /**
     * Use this method to logout
     */
    logout: function (): void {
      this.$emit("login-changed", false);
      this.links = MenuItems.getLinkListByRole(Role.All);
      AuthService.logout();
      this.onRedirectToLogin();
    },

    /**
     * Use this method to redirect the user to login page
     */
    onRedirectToLogin: function (): void {
      if (this.$route.name !== "login") {
        this.$router.push({ name: "login" });
      }
    },
  },
});
</script>