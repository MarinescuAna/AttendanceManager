<template>
  <v-layout align-center column>
    <v-layout wrap>
      <h1 :class="isMobile ? 'my-1 ml-2' : 'my-12 ml-12'">
        <p :class="isMobile ? 'text-h6' : 'text-h3'">Welcome to</p>
        <span
          class="ml-12 text-uppercase"
          :class="isMobile ? 'text-h5' : 'text-h2'"
          >Attendance</span
        >
        <span
          class="font-weight-bold text-h2 text-uppercase"
          :class="isMobile ? 'text-h5' : 'text-h2'"
          >Manager</span
        >
      </h1>
      <v-img
        :width="isMobile ? '100px' : '400px'"
        :src="require('@/assets/images/home/home_1.svg')"
      ></v-img
    ></v-layout>
    <section
      :class="isMobile ? 'ma-2' : 'my-12 ma-12'"
      class="pa-12"
      v-if="!isLogged"
    >
      <h2 class="ml-12">Get Started</h2>
      <div class="d-flex flex-row ma-2" :class="isMobile ? 'flex-wrap' : ''">
        <p class="mx-4 mt-5 pa-3">
          We offer you a place where you can store all the attendance and
          students' involvement in class activities.
          <router-link :to="{ name: 'login' }"> Sign in </router-link> to access
          the attendance management system.
        </p>
        <v-img
          class="ma-5"
          :width="isMobile ? '50px' : '300px'"
          src="@/assets/images/home/home_login.svg"
        ></v-img>
      </div>
    </section>
    <section v-else>
      <h2 class="d-flex justify-center mt-12 mb-5">
        Welcome back {{ currentUserName }}!
      </h2>
      <v-layout
        :class="isMobile ? 'ma-1' : 'mx-6 mt-6'"
        class="mb-12"
        justify-center
        wrap
      >
        <v-card class="d-flex ma-4" v-for="link in links" :key="link.title">
          <v-img
            class="ma-5"
            :width="isMobile ? '50px' : '200px'"
            src="@/assets/images/home/home_cards.svg"
          ></v-img
          ><v-card-text
            class="d-flex flex-column"
            :style="{ width: isMobile ? '200px' : '300px' }"
          >
            <p class="text--primary" :class="isMobile ? 'text-h6' : 'text-h4'">
              {{ link.title }}
            </p>
            <div class="text--primary">
              {{ link.description }}
              <v-btn
                text
                color="teal accent-4"
                @click="$router.push({ name: link.route })"
              >
                Go to this page
              </v-btn>
            </div>
          </v-card-text>
        </v-card>
      </v-layout>
    </section>
    <section>
      <h2 class="d-flex justify-start pl-12 ml-12">Features</h2>
      <div class="d-flex flex-row" :class="isMobile ? 'flex-wrap' : ''">
        <p class="pa-12">
          Track attendance for different courses and activities. There are three
          types of ways to can add those involvements, and you can choose the
          one that perfectly fits your needs. So, in the end, you will have all
          the students' involvement in one place. The data are protected and
          students cannot view each other's involvement.
        </p>
        <v-img
          class="ma-6"
          :width="isMobile ? '50px' : '300px'"
          src="@/assets/images/home/home_secure.svg"
        ></v-img>
      </div>
      <div class="d-flex flex-row" :class="isMobile ? 'flex-wrap' : ''">
        <v-img
          class="ma-6"
          :width="isMobile ? '50px' : '300px'"
          src="@/assets/images/home/home_statistics.svg"
        ></v-img>
        <p class="pa-12">
          View involvements statistics and reports. Teachers can visualize the
          students' progress regarding their courses and can adjust their work
          according to students' progress because those statistics are computed
          from the beginning of the classes.
        </p>
      </div>
      <div class="d-flex flex-row" :class="isMobile ? 'flex-wrap' : ''">
        <p class="pa-12">
          Achieve rewards for each involvement. No matter if you are a teacher
          or student, you have some badges that can achieve. But as a teacher,
          you can define some custom rewards to improve the relationship with
          students and stimulate their interest in the course.
        </p>
        <v-img
          class="ma-6"
          :width="isMobile ? '50px' : '300px'"
          src="@/assets/images/home/home_rewards.svg"
        ></v-img>
      </div>
      <div class="d-flex flex-row mb-12" :class="isMobile ? 'flex-wrap' : ''">
        <v-img
          class="ma-6"
          :width="isMobile ? '50px' : '300px'"
          src="@/assets/images/home/home_notification.svg"
        ></v-img>
        <p class="pa-12">
          Receive notifications to be up-to-date with class activities, no
          matter whether you are a student or a teacher. All the changes to
          classes and their activity are notified, so that nobody can be left
          behind.
        </p>
      </div>
    </section>
    <footer>
      <v-divider class="ma-6 my-4 mt-12"></v-divider>
      <p>
        <span class="text-h6">Attendance<b>Manager</b></span>
        {{ new Date().toLocaleString() }}
      </p>
    </footer>
  </v-layout>
</template>

<script lang="ts">
import { MenuItems } from "@/components/menu-bar/ItemList";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";
import Vue from "vue";

interface ItemModel {
  title: string;
  route: string;
  description: string;
}

export default Vue.extend({
  name: "HomeView",
  data() {
    return {
      links: [] as ItemModel[],
      isLogged: false,
      reveal: false,
      model: {},
    };
  },
  computed: {
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.xs;
    },
    currentUserName: function (): string | undefined {
      return AuthService.getDataFromToken()?.name;
    },
  },
  created(): void {
    this.isLogged = AuthService.isLogged();

    if (this.isLogged) {
      const data = AuthService.getDataFromToken();

      if (data !== null) {
        MenuItems.getLinkListByRole(Role[data.role]).forEach((x) => {
          if (x.title !== "Home") {
            this.links.push({
              description: x.description,
              route: x.route,
              title: x.title,
            } as ItemModel);
          }
        });
      }
    }
  },
});
</script>
