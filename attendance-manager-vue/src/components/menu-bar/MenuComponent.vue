<template>
  <div>
    <v-toolbar flat>
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
      class="blue-grey lighten-4"
      absolute
      temporary
      width-max="20%"
      width="auto"
    >
      <v-list>
        <v-container justify="center" v-if="isLogged">
          <h3 class="text-uppercase">{{ name }}</h3>
          <h4>{{ email }}</h4>
          <h4>{{ code }}</h4>
        </v-container>
        <MenuItemComponent :item="signInItem" v-if="!isLogged" />
        <v-list-item router @click="logout" v-if="isLogged">
          <v-list-item-icon>
            <v-icon>mdi-exit-to-app</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>Sign Out</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
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
import AuthService from "@/services/auth.service";
import Vue from "vue";
import { EventBus } from "@/main";
import { EVENT_BUS_ISLOGGED } from "@/shared/constants";
import MenuItemListComponent from "./MenuItemListComponent.vue";
import MenuItemComponent from "./MenuItemComponent.vue";
import { links, MenuChildModel, MenuItemListModel } from "./ItemList";

export default Vue.extend({
  name: "MenuComponent",
  components: {
    MenuItemListComponent,
    MenuItemComponent,
  },
  data() {
    return {
      // Sign In button details
      signInItem: {
        icon: "mdi-login",
        route: "login",
        title: "Sign In",
      } as MenuChildModel,
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
  /**
   * Check if the user is logged and display the proper buttons in the navbar
   */
  created(): void {
    this.isLogged = AuthService.isLogged();
    if (this.isLogged) {
      this.setProperties();
    }
  },
  mounted: function () {
    /**
     * Emit an event using EventBus every time the user logs in to update the navbar
     */
    EventBus.$on(EVENT_BUS_ISLOGGED, () => {
      this.isLogged = true;
      this.setProperties();
      EventBus.$off(EVENT_BUS_ISLOGGED);
    });
  },
  methods: {
    /**
     * Use this method to initialize the list of links
     */
    filterLinksByRole(role: Role): void {
      links.forEach((cr) => {
        if (cr.role.toString() == Role[role] || cr.role == Role.All) {
          let newLink = cr;
          if (cr.children.length > 0) {
            newLink.children = cr.children.filter(
              (child) => child.role.toString() == Role[role]
            );
          }
          this.links.push(newLink);
        }
      });
    },

    /**
     * Use this method in order to set the page properties
     */
    setProperties(): void {
      const data = AuthService.getDataFromToken();

      if (data === null) {
        this.filterLinksByRole(Role.All);
      } else {
        this.name = data.name;
        this.code = data.code;
        this.email = data.email;
        this.filterLinksByRole(data.role);
      }
    },

    /**
     * Use this method to logout
     */
    logout(): void {
      this.isLogged = false;
      this.links = [];
      AuthService.logout();
      this.$router.push({ name: "home" });
    },
  },
});
</script>