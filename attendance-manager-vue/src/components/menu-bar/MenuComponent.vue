<template>
  <div>
    <v-toolbar short flat>
      <v-app-bar-nav-icon
        class="grey--text"
        @click.stop="drawer = !drawer"
      ></v-app-bar-nav-icon>
      <v-toolbar-title class="text-uppercase font-weight-black">
        <span class="font-weight-light">Attendance</span>
        <span>Manager</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn text color="font-weight-black" to="/login" v-if="!isLogged">
        <span>Sign In</span>
        <v-icon>mdi-login</v-icon>
      </v-btn>
      <v-btn text color="font-weight-black" @click="logout" v-if="isLogged">
        <span>Sign Out</span>
        <v-icon>mdi-exit-to-app</v-icon>
      </v-btn>
    </v-toolbar>
    <v-navigation-drawer
      v-model="drawer"
      class="blue-grey lighten-4"
      absolute
      temporary
    >
      <v-list>
        <v-list-item v-if="isLogged">
          <v-list-item-avatar>
            <v-img :src="src"></v-img>
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title class="font-weight-black text-h5">{{ name }}</v-list-item-title>
            <v-list-item-title class="text-h6">{{ code }}</v-list-item-title> 
          </v-list-item-content>
        </v-list-item>

        <v-divider v-if="isLogged"></v-divider>
        <v-list-item
          v-for="link in links"
          :key="link.text"
          router
          :to="link.route"
        >
          <v-list-item-icon>
            <v-icon>{{ link.icon }}</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>{{ link.text }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
  </div>
</template>

<script lang="ts">
import { Role } from "@/shared/enums";
import UserService from "@/services/auth.service";
import Vue from "vue";

export default Vue.extend({
  data() {
    return {
      name: "",
      role: Role.NoRole,
      code: '',
      drawer: false,
      src: "",
      isLogged: false,
      links: [
        {
          icon: "mdi-information-variant",
          text: "About",
          route: "/about",
        },
      ],
    };
  },
  created(): void {
    this.isLogged = UserService.isLogged();
    if(this.isLogged)
    {
      const data = UserService.getDataFromToken();
      this.name = data.name;
      this.role = data.role;
      this.code = data.code;
      this.src = `./images/${this.role}-profile.jpg`;
    }
  },
  methods:{
    logout(): void {
      UserService.logout();
    }
  }
});
</script>