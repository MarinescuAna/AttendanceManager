<template>
  <div>
    <v-toolbar color="transparent" flat fixed>
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
        v-if="isLogged && !isAdmin"
        :close-on-content-click="false"
        :nudge-width="200"
        max-height="650px"
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
              class="light_button black--text"
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
      class="blue_grey_4 navigation-drawer-style"
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
import { EVENT_BUS_ISLOGGED } from "@/shared/constants";
import { EventBus } from "@/main";
import { Role } from "@/shared/enums";
import AuthService from "@/services/auth.service";
import { NotificationViewModel } from "@/modules/view-modules";

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
    isAdmin: function (): boolean {
      return AuthService.getDataFromToken()?.role == Role[0];
    },
  },
  watch: {
    isLogged: async function (): Promise<void> {
      if (this.isLogged) {
        this.notifications =
          await NotificationService.getCurrentUserNotificationsAsync();
      } else {
        this.notifications = [];
      }
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
      // Sort the notifications array
      this.notifications.sort(this._compareNotifications);
    },
    onLoginChanged: function (value): void {
      this.isLogged = value;
    },
    // Custom comparison function for sorting
    _compareNotifications: function (
      a: NotificationViewModel,
      b: NotificationViewModel
    ): number {
      // Sort by isRead (ascending)
      if (a.isRead === b.isRead) {
        // Sort by createdOn (descending)
        return (
          new Date(b.createdOn).getTime() - new Date(a.createdOn).getTime()
        );
      }
      return a.isRead ? 1 : -1;
    },
  },
});
</script>