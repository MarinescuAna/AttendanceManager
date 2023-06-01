<template>
  <div>
    <v-toolbar color="transparent" flat>
      <!--nav bar button for open the navigation drawer-->
      <v-app-bar-nav-icon
        @click.stop="drawerActivator = !drawerActivator"
      ></v-app-bar-nav-icon>
      <!--Attendance manager title-->
      <router-link :to="{ name: 'home' }" v-slot="{ navigate }">
        <v-toolbar-title
          class="text-uppercase font-weight-black"
          @click="navigate"
        >
          <span class="font-weight-light">Attendance</span>
          <span>Manager</span>
        </v-toolbar-title>
      </router-link>
      <!--Badge for notifications-->
      <v-spacer></v-spacer>
      <v-menu
        v-model="openNotificationDialog"
        v-if="isLogged"
        :close-on-content-click="false"
        :nudge-width="200"
        bottom
        offset-y
      >
        <template v-slot:activator="{ on, attrs }">
          <v-badge
            :content="messages"
            :value="messages"
            color="green"
            class="mx-2"
            bottom
            overlap
          >
            <v-btn
              class="orange lighten-3 black--text"
              elevation="3"
              v-bind="attrs"
              v-on="on"
              icon
              ><v-icon>
                {{ messages != 0 ? "mdi-bell-ring" : "mdi-bell" }}
              </v-icon></v-btn
            >
          </v-badge>
        </template>
        <NotificationMenuContent
          class="dialog-notification-menu"
          :notifications="notifications"
          @delete-notification="onRemoveNotification"
          @read-notification="onReadNotification"
        />
      </v-menu>
    </v-toolbar>
    <!--navigation drawer-->
    <v-navigation-drawer
      v-model="drawerActivator"
      class="blue-grey lighten-4 navigation-drawer-style"
      absolute
      temporary
      width="auto"
    >
      <MenuNavigationDrawerContent
        :isLogged="isLogged"
        @login-changed="onLoginChanged"
      />
    </v-navigation-drawer>
  </div>
</template>

<style scoped>
.navigation-drawer-style {
  min-width: 10% !important;
}
.dialog-notification-menu {
  max-width: 500px;
  min-height: 180px;
}
</style>

<script lang="ts">
import Vue from "vue";
import MenuNavigationDrawerContent from "./MenuNavigationDrawerContent.vue";
import NotificationMenuContent from "./NotificationMenuContent.vue";
import NotificationService from "@/services/notification.service";
import { NotificationViewModel } from "@/modules/notification/index";
import { EVENT_BUS_ISLOGGED } from "@/shared/constants";
import { EventBus } from "@/main";

export default Vue.extend({
  name: "MenuComponent",
  components: { MenuNavigationDrawerContent, NotificationMenuContent },
  data() {
    return {
      // Use this in order to activeate the drawer
      drawerActivator: false,
      openNotificationDialog: false,
      notifications: [] as NotificationViewModel[],
      isLogged: false,
    };
  },
  computed: {
    messages: function (): number {
      return this.notifications.filter((n) => !n.isRead).length;
    },
  },
  mounted: function () {
    /**
     * Emit an event using EventBus every time the user logs in to update the navbar
     */
    EventBus.$on(EVENT_BUS_ISLOGGED, () => {
      this.isLogged = true;
    });
  },
  destroyed(): void {
    EventBus.$off(EVENT_BUS_ISLOGGED);
  },
  created: async function (): Promise<void> {
    this.notifications =
      await NotificationService.getCurrentUserNotificationsAsync();
  },
  methods: {
    onRemoveNotification: function (notificationId): void {
      this.notifications = this.notifications.filter(
        (n) => n.notificationId != notificationId
      );
    },
    onReadNotification: function (notificationId): void {
      this.notifications.forEach((notification) => {
        if (notification.notificationId == notificationId) {
          notification.isRead = true;
        }
      });
    },
    onLoginChanged: function (value): void {
      this.isLogged = value;
    },
  },
});
</script>